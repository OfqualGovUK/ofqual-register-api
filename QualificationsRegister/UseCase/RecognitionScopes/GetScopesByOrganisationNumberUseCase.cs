using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterFrontend.Models.APIModels;
using Ofqual.Common.RegisterFrontend.RegisterAPI;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using static Ofqual.Common.RegisterAPI.Constants.ScopesConstants;

namespace Ofqual.Common.RegisterAPI.UseCase.RecognitionScopes

{
    public partial class GetScopesByOrganisationNumberUseCase : IGetScopesByOrganisationNumberUseCase
    {

        [GeneratedRegex(@"^\d+$")]
        private static partial Regex OrgNumberRegex();
        private readonly IRegisterDb _registerDb;
        private readonly ILogger _logger;
        private readonly IRefDataAPIClient _refDataAPIClient;

        public GetScopesByOrganisationNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb, IRefDataAPIClient refDataAPIClient)
        {
            _logger = loggerFactory.CreateLogger<GetScopesByOrganisationNumberUseCase>();
            _registerDb = registerDb;
            _refDataAPIClient = refDataAPIClient;
        }

        public async Task<RecognitionScope> GetScopesByOrganisationNumber(string? recognitionNumber)
        {
            _logger.LogInformation("Getting Recognition scopes for Organisation");

            if (string.IsNullOrEmpty(recognitionNumber))
            {
                throw new BadRequestException("Organisation number is null or empty");
            }

            string numberNoRN, numberRN;

            //match and add RN to the org number if required
            if (OrgNumberRegex().IsMatch(recognitionNumber))
            {
                numberNoRN = recognitionNumber!;
                numberRN = $"RN{recognitionNumber}";
            }
            else if (recognitionNumber!.Substring(0, 2).Equals("rn", StringComparison.InvariantCultureIgnoreCase))
            {
                numberNoRN = recognitionNumber[2..];
                numberRN = recognitionNumber;
            }
            else
                throw new BadRequestException("Please provide a valid organisation number");

            var number = string.IsNullOrEmpty(numberRN) ? numberNoRN : numberRN;

            List<QualificationType> types = await _refDataAPIClient.GetQualificationTypesAsync();
            List<Level> levels = await _refDataAPIClient.GetLevelsAsync();

            //Other (Not Applicable) scopes should be at the last of the list
            types.RemoveAt(types.FindIndex(e => e.Description == NOT_APPLICABLE));
            types.Insert(types.Count, new QualificationType { Description = NOT_APPLICABLE });

            var allScopes = _registerDb.GetRecognitionScope(number);

            var responseScopes = ProcessRecognitionScopes(types, levels, allScopes);

            return responseScopes;
        }

        //format and structure the scopes from types > levels > recog
        private static RecognitionScope ProcessRecognitionScopes(List<QualificationType> types, List<Level> levels, List<DbRecognitionScope>? allScopes)
        {
            var responseScopes = new RecognitionScope
            {
                Inclusions = [],
                Exclusions = []
            };

            if (allScopes != null && allScopes.Count != 0)
            {
                foreach (var type in types)
                {
                    var includedScopeType = new ScopeType
                    {
                        Type = GetTypeName(type.Description),
                        Levels = []
                    };

                    foreach (var level in levels)
                    {
                        var includedScopeLevel = new ScopeLevel
                        {
                            Level = level.LevelDescription == ENTRY_LEVEL ? $"{level.LevelDescription} - {level.SubLevelDescription}" : level.LevelDescription,
                            Recognitions = []
                        };

                        var scopes = level.LevelDescription == ENTRY_LEVEL ? allScopes.Where(e => e.Level == level.LevelDescription && e.Type == type.Description && e.SubLevel == level.SubLevelDescription) : allScopes.Where(e => e.Level == level.LevelDescription && e.Type == type.Description);

                        foreach (var scope in scopes.Where(e => e.InclusionExclusion == true))
                        {
                            //@Phil McAllister dt. 03 May 2024 - remove starting 0's (if any) from the SSA
                            var recog = scope.SSA!.StartsWith('0') ? scope.SSA[1..] : scope.SSA;

                            //@Phil McAllister dt. 03 May 2024 - Where your qualification type == 'End-Point Assessment', pull your data from apprenticeship standard code and apprenticeship standard name 
                            recog = type.Description == END_POINT_ASSESSMENT ? $"{scope.ApprenticeshipStandardReferenceNumber} - {scope.ApprenticeshipStandardTitle}" : recog;

                            recog = type.Description == TECHNICAL_QUALIFICATION ? $"{scope.TechnicalQualificationSubject}" : recog;

                            includedScopeLevel.Recognitions.Add(recog);
                        }

                        if (includedScopeLevel.Recognitions.Count != 0)
                        {
                            includedScopeType.Levels.Add(includedScopeLevel);
                        }
                    }

                    if (includedScopeType.Levels.Count != 0)
                    {
                        responseScopes.Inclusions.Add(includedScopeType);
                    }
                }

                //no need to structure the Excluded scopes
                responseScopes.Exclusions = allScopes.Where(e => e.QualificationDescription != NOT_APPLICABLE).Select(e => e.QualificationDescription).ToList();
            }

            return responseScopes;
        }

        private static string GetTypeName(string description)
        {
            return description switch
            {
                ENGLISH_FOR_SPEAKERS_OF_OTHER_LANGUAGES => ENGLISH_FOR_SPEAKERS_OF_OTHER_LANGUAGES_RENAME,
                GCE_A => GCE_A_RENAME,
                GCE_AS => GCE_AS_RENAME,
                END_POINT_ASSESSMENT => ENDPOINT_ASSESSMENT_RENAME,
                TECHNICAL_QUALIFICATION => TECHNICAL_QUALIFICATION_RENAME,
                NOT_APPLICABLE => NOT_APPLICABLE_RENAME,
                _ => description,
            };
        }
    }
}

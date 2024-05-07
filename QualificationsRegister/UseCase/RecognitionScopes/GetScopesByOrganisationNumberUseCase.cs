using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterFrontend.Models.APIModels;
using Ofqual.Common.RegisterFrontend.RegisterAPI;
using System.Text.RegularExpressions;

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

            var allScopes = _registerDb.GetRecognitionScope(number);

            var responseScopes = ProcessRecognitionScopes(types, levels, allScopes);

            return responseScopes;
        }

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
                        Type = type.Description,
                        Levels = []
                    };

                    var excludedScopeType = new ScopeType
                    {
                        Type = type.Description,
                        Levels = []
                    };

                    foreach (var level in levels)
                    {
                        var includedScopeLevel = new ScopeLevel
                        {
                            Level = level.LevelDescription == "Entry Level" ? $"{level.LevelDescription} - {level.SubLevelDescription}" : level.LevelDescription,
                            Recognitions = []
                        };

                        var excludedScopeLevel = new ScopeLevel
                        {
                            Level = level.LevelDescription == "Entry Level" ? $"{level.LevelDescription} - {level.SubLevelDescription}" : level.LevelDescription,
                            Recognitions = []
                        };

                        var scopes = level.LevelDescription == "Entry Level" ? allScopes.Where(e => e.Level == level.LevelDescription && e.Type == type.Description && e.SubLevel == level.SubLevelDescription) : allScopes.Where(e => e.Level == level.LevelDescription && e.Type == type.Description);

                        foreach (var scope in scopes)
                        {
                            //@Phil McAllister dt. 03 May 2024 - remove starting 0's (if any) from the SSA
                            var recog = scope.SSA!.StartsWith('0') ? scope.SSA[1..] : scope.SSA;

                            //@Phil McAllister dt. 03 May 2024 - Where your qualification type == 'End-Point Assessment', pull your data from apprenticeship standard code and apprenticeship standard name 
                            recog = type.Description == "End-Point Assessment" ? $"{scope.ApprenticeshipStandardReferenceNumber} - {scope.ApprenticeshipStandardTitle}" : recog;

                            recog = type.Description == "Technical Qualification" ? $"{scope.TechnicalQualificationSubject}" : recog;

                            if (scope.InclusionExclusion)//Included
                            {
                                includedScopeLevel.Recognitions.Add(recog);
                            }
                            else if (!scope.InclusionExclusion)//Excluded
                            {
                                excludedScopeLevel.Recognitions.Add(recog);
                            }
                        }

                        if (includedScopeLevel.Recognitions.Count != 0)
                        {
                            includedScopeType.Levels.Add(includedScopeLevel);
                        }

                        if (excludedScopeLevel.Recognitions.Count != 0)
                        {
                            excludedScopeType.Levels.Add(excludedScopeLevel);
                        }

                    }


                    if (includedScopeType.Levels.Count != 0)
                    {
                        responseScopes.Inclusions.Add(includedScopeType);
                    }

                }

                responseScopes.Exclusions = allScopes.Where(e => e.QualificationDescription != "Not Applicable").Select(e => e.QualificationDescription).ToList();
            }

            return responseScopes;
        }
    }
}

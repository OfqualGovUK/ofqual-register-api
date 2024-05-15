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

            //Other (Not Applicable) scopes should be at the last of the list
            types.RemoveAt(types.FindIndex(e => e.Description == "Not Applicable"));
            types.Insert(types.Count, new QualificationType { Description = "Not Applicable" });

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
                        Type = GetTypeName(type.Description),
                        Levels = []
                    };

                    foreach (var level in levels)
                    {
                        var includedScopeLevel = new ScopeLevel
                        {
                            Level = level.LevelDescription == "Entry Level" ? $"{level.LevelDescription} - {level.SubLevelDescription}" : level.LevelDescription,
                            Recognitions = []
                        };

                        var scopes = level.LevelDescription == "Entry Level" ? allScopes.Where(e => e.Level == level.LevelDescription && e.Type == type.Description && e.SubLevel == level.SubLevelDescription) : allScopes.Where(e => e.Level == level.LevelDescription && e.Type == type.Description);

                        foreach (var scope in scopes.Where(e => e.InclusionExclusion == true))
                        {
                            //@Phil McAllister dt. 03 May 2024 - remove starting 0's (if any) from the SSA
                            var recog = scope.SSA!.StartsWith('0') ? scope.SSA[1..] : scope.SSA;

                            //@Phil McAllister dt. 03 May 2024 - Where your qualification type == 'End-Point Assessment', pull your data from apprenticeship standard code and apprenticeship standard name 
                            recog = type.Description == "End-Point Assessment" ? $"{scope.ApprenticeshipStandardReferenceNumber} - {scope.ApprenticeshipStandardTitle}" : recog;

                            recog = type.Description == "Technical Qualification" ? $"{scope.TechnicalQualificationSubject}" : recog;

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

                ////Other (Not Applicable) scopes should be at the last of the list
                //var otherIndex = responseScopes.Inclusions.FindIndex(e => e.Type == "Other");
                //if (otherIndex > 0)
                //{
                //    var other = responseScopes.Inclusions.Where(e => e.Type == "Other").FirstOrDefault();

                //}

                responseScopes.Exclusions = allScopes.Where(e => e.QualificationDescription != "Not Applicable").Select(e => e.QualificationDescription).ToList();
            }

            return responseScopes;
        }

        private static string GetTypeName(string description)
        {
            return description switch
            {
                "English For Speakers of Other Languages" => "English For Speakers of Other Languages (ESOL)",
                "GCE A Level" => "A level (GCE)",
                "GCE AS Level" => "AS level (GCE)",
                "End-Point Assessment" => "End-point Assessment (Apprenticeship EPA)",
                "Technical Qualification" => "Technical Qualification (part of T Levels)",
                "Not Applicable" => "Other",
                _ => description,
            };
        }
    }
}

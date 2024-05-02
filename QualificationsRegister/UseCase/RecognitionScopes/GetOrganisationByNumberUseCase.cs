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
                            Level = level.LevelDescription,
                            Recognitions = []
                        };

                        var excludedScopeLevel = new ScopeLevel
                        {
                            Level = level.LevelDescription,
                            Recognitions = []
                        };

                        var scopes = allScopes.Where(e => e.Level == level.LevelDescription && e.Type == type.Description);

                        if (scopes.Any())
                        {
                            foreach (var scope in scopes)
                            {
                                if (scope.InclusionExclusion && scope.SSA != null)//Included
                                {
                                    includedScopeLevel.Recognitions.Add(scope.SSA);
                                }
                                else if (!scope.InclusionExclusion && scope.SSA != null)//Included
                                {
                                    excludedScopeLevel.Recognitions.Add(scope.SSA);
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
                    }


                    if (includedScopeType.Levels.Count != 0)
                    {
                        responseScopes.Inclusions.Add(includedScopeType);
                    }

                    if (excludedScopeType.Levels.Count != 0)
                    {
                        responseScopes.Exclusions.Add(excludedScopeType);
                    }
                }
            }

            return responseScopes;
        }
    }
}

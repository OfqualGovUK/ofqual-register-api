# Ofqual Register of Regulated Qualifications API 

The Ofqual Register of Regulated Qualifications API allows users to programmatically access details of qualifications and awarding organisations regulated by Ofqual and CCEA Regulation.
## Provider 
[The Office of Qualifications and Examinations Regulation](https://www.gov.uk/government/organisations/ofqual)

## About this project
This project is an Azure Functions app built using .NET 8 and managed through Azure API Management

#### Libraries
- Microsoft.Data.SqlClient v5.1.5
- Microsoft.EntityFrameworkCore v8.0.2
- Refit 7.0

## Architecture
![api](https://github.com/OfqualGovUK/ofqual-register-api/blob/main/API_Arch.jpg?raw=true)

## Environment Variables
Variables set in the Function Apps config on Azure

- **APIMgmtUrl** - URL for the API management (used for the HealthCheck function)
- **MDDBConnString** - Connection string for the Database (MDDB)
- **QualificationsPagingLimit** - Number of Qualifications to return from the API for the List Qualificaions function
- **RefDataAPIUrl** = URL for the Ref Data API to fetch Scopes data (Qualification Types and Levels)
- **AzureWebJobsStorage**: Connection string for where the web jobs should be stored; can be on dev storage when ran locally, but should have a proper string for when deployed to Azure
- **FUNCTIONS_WORKER_RUNTIME** and **FUNCTIONS_EXTENSION_WORKER**: Values used as part of running Azure Functions properly

Use the following in a local.settings.json. The values should not be used in deployed environments; use appropriate connection strings and URLs depending upon the deployed environment (dev/prod)

```json

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=True",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "FUNCTIONS_EXTENSION_WORKER": "~4",
    "RefDataAPIUrl": "<URL>",
    "MDDBConnString": "Server=localhost,1433;Initial Catalog=ofqds-dev-md-sql01;Persist Security Info=False;User ID=<USERNAME>;Password=<PASSWORD>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;",
    "QualificationsPagingLimit": 10000,
    "APIMgmtURL": "<URL>"
  }
}

```

## Functions
- Health Check

### Qualifications
- List Qualifications
- Get Qualification by Qualification Number

### Organisations
- List Organisations
- Get Organisation by Reference Number
- Get Scopes of Recognition for Organisation Reference Number

### Qualifications Private (includes non public facing information)

- List Qualifications 
- Get Qualification by Qualification Number

## API Documentation

### Postman

Project files include a Postman collection by the name `Ofqual Register API.postman_collection.json`,

Postman environment for the variables are in `Dev.postman_environment.json` for the following variables:
- **APIMgmgt_URL**: URL for the API endpoint
- **APISubKey**: API key for the non public functions which gets appeneded to the `Gov` postman folder REST calls headers



### Health Check

<details>
 <summary><code>GET</code> <code><b>/HealthCheck</b></code></summary>

####
Checks the status of the API 

##### Responses

> Returns `Status: 200 OK` or any error code depending on the status of the API

##### Example Requests

> ```javascript
>  /HealthCheck
> ```


 </details>

### Organisations Public

<details>
 <summary><code>GET</code> <code><b>api/Organisations/{recognitionNumber}</b></code> </summary>

####
Retrieves an individual Organisation by the Recognition Number

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | recognitionNumber      |  required | string   | Recognition number for the Organisation record in the RN format eg. RN5353 |  Allowed without the RN prefix eg. 5353|

##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Organisation JSON                     | JSON object for the Organisation                                    |
> | `404`         | `application/json`                | `{"message":"Not found"}`  | The organisation could not be found for the recognition number provided                            |

##### Example Requests

> ```javascript
>  api/Organisations/RN5353
> ```

> ```javascript
>  api/Organisations/5353
> ```


##### Example response

```
 {
    "name": "Example Organization", // String
    "recognitionNumber": "RN5353", // String
    "legalName": "Legal Organization Name", // String
    "acronym": "EO", // String
    "ofqualOrganisationStatus": null, // Nullable String
    "cceaOrganisationStatus": null, // Nullable String
    "ofqualRecognisedOn": null, // Nullable DateTime
    "ofqualRecognisedTo": null, // Nullable Date
    "ofqualSurrenderedOn": null, // Nullable DateTime
    "ofqualWithdrawnOn": null, // Nullable DateTime
    "cceaRecognisedOn": null, // Nullable DateTime
    "cceaRecognisedTo": null, // Nullable Date
    "cceaSurrenderedOn": null, // Nullable DateTime
    "cceaWithdrawnOn": null, // Nullable DateTime
    "contactEmail": "contact@example.org", // String
    "website": "https://example.org", // String
    "phoneNumber": "+1 123-456-7890", // String
    "feesUrl": "https://example.org/fees", // String
    "addressLine1": "123 Main Street", // String
    "addressLine2": "Suite 456", // String
    "addressCity": "Cityville", // String
    "addressCounty": "Countyshire", // String
    "addressCountry": "United Kingdom", // String
    "addressPostCode": "AB12 3CD", // String
    "lastUpdatedDate": "2024-02-20T09:07:22Z" // DateTime
}
```

 </details>

 
<details>
 <summary><code>GET</code> <code><b>api/Organisations?search={search}&page={page}&limit={limit}</b></code> </summary>


####
Retrieves a list of organisations along with with the paging metadata ordered by Organisation name, Legal Name, then Acronym.

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | search      |  optional | string   | Search term that matches within the organisation name or ‘known as’/acronym field of one or more records| if not provided, all organisations are returned
> | page      |  optional | int   | Page number for the current set of search results|if not provided, defaults to page # 1
> | limit      |  optional | int   | Number of organisation records to return for the search | if not provided returns the whole list


##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Organisation List response JSON                     | Paging metadata with list of Organisation records                                    |
> | `400`         | `application/json`                | `{"message": {error description}}`  | Parameters provided are not correct / data not supported                            |


 
##### Example Requests

> ```javascript
>  api/Organisations?search=Chartered&page=1&limit=10
> ```

> ```javascript
>  api/Organisations?page=5&limit=15
> ```

> ```javascript
>  api/Organisations/
> ```

##### Example Response

```
 {
    "count": 27,
    "currentPage": 1,
    "limit": 15,
    "results": [
        {
            "name": "Example Organization", // String
            "recognitionNumber": "ABC123", // String
            "legalName": "Legal Organization Name", // String
            "acronym": "EO", // String
            "ofqualOrganisationStatus": null, // Nullable String
            "cceaOrganisationStatus": null, // Nullable String
            "ofqualRecognisedOn": null, // Nullable DateTime
            "ofqualRecognisedTo": null, // Nullable Date
            "ofqualSurrenderedOn": null, // Nullable DateTime
            "ofqualWithdrawnOn": null, // Nullable DateTime
            "cceaRecognisedOn": null, // Nullable DateTime
            "cceaRecognisedTo": null, // Nullable Date
            "cceaSurrenderedOn": null, // Nullable DateTime
            "cceaWithdrawnOn": null, // Nullable DateTime
            "contactEmail": "contact@example.org", // String
            "website": "https://example.org", // String
            "phoneNumber": "+1 123-456-7890", // String
            "feesUrl": "https://example.org/fees", // String
            "addressLine1": "123 Main Street", // String
            "addressLine2": "Suite 456", // String
            "addressCity": "Cityville", // String
            "addressCounty": "Countyshire", // String
            "addressCountry": "United Kingdom", // String
            "addressPostCode": "AB12 3CD", // String
            "lastUpdatedDate": "2024-02-20T09:07:22Z" // DateTime
        },
        ....
    ]
 }
 ```
 </details>


### Qualifications Public

<details>
 <summary><code>GET</code> <code><b>api/Qualifications/{qualificationNumber}</b></code> </summary>

####
Retrieves an individual Qualification by the Qualification Number

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | qualificationNumber      |  required | string   | Qulification number for the qualification record eg. 100/0512/2 |  Allowed without obliques eg. 10005122|

##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Qualification JSON                     | JSON object for the Qualification                                    |
> | `404`         | `application/json`                | `{"message":"Not found"}`  | The qualification could not be found for the qualification number provided                            |

##### Example Requests

> ```javascript
>  api/Qualifications/100/0512/2
> ```

> ```javascript
>  api/Qualifications/10005122
> ```


##### Example response

```
 {
    "qualificationNumber": "100/0512/2", // String
    "qualificationNumberNoObliques": "10005122", // Nullable String
    "title": "Example Qualification", // String
    "status": null, // Nullable String
    "organisationName": "Example Organization", // String
    "organisationAcronym": "EO", // String
    "organisationRecognitionNumber": "XYZ456", // String
    "type": null, // Nullable String
    "ssa": null, // Nullable String
    "level": null, // Nullable String
    "subLevel": null, // Nullable String
    "eqfLevel": null, // Nullable String
    "gradingType": null, // Nullable String
    "gradingScale": null, // Nullable String
    "totalCredits": null, // Nullable Integer
    "tqt": null, // Nullable Integer
    "glh": null, // Nullable Integer
    "minimumGLH": null, // Nullable Integer
    "maximumGLH": null, // Nullable Integer
    "regulationStartDate": "2024-02-20T09:21:37Z", // DateTime
    "operationalStartDate": "2024-02-20T09:21:37Z", // DateTime
    "operationalEndDate": null, // Nullable DateTime
    "certificationEndDate": null, // Nullable DateTime
    "reviewDate": null, // Nullable DateTime
    "offeredInEngland": null, // Nullable Boolean
    "offeredInNorthernIreland": null, // Nullable Boolean
    "offeredInternationally": null, // Nullable Boolean
    "specialism": null, // Nullable String
    "pathways": null, // Nullable String
    "assessmentMethods": null, // Nullable String
    "approvedForDELFundedProgramme": null, // Nullable Boolean
    "linkToSpecification": null, // Nullable String
    "apprenticeshipStandardReferenceNumber": null, // Nullable String
    "apprenticeshipStandardTitle": null, // Nullable String
    "regulatedByNorthernIreland": null, // Nullable Boolean
    "niDiscountCode": null, // Nullable String
    "gceSizeEquivalence": null, // Nullable Decimal
    "gcseSizeEquivalence": null, // Nullable Decimal
    "entitlementFrameworkDesignation": null, // Nullable String
    "lastUpdatedDate": null, // Nullable DateTime
    "organisationId": 789, // Integer
    "levelId": null, // Nullable Integer
    "typeId": null, // Nullable Integer
    "ssaId": null, // Nullable Integer
    "gradingTypeId": null, // Nullable Integer
    "gradingScaleId": null // Nullable Integer
}
```

 </details>

 
<details>
 <summary><code>GET</code> <code><b>api/qualifications?title={title}&page={page}&limit={limit}&assessmentMethods={assessmentMethods}&gradingTypes={gradingTypes}&awardingOrganisations={awardingOrganisations}&availability={availability}&qualificationTypes={qualificationTypes}&qualificationLevels={qualificationLevels}&nationalAvailability={nationalAvailability}&sectorSubjectAreas={sectorSubjectAreas}&minTotalQualificationTime={minTotalQualificationTime}&maxTotalQualificationTime={maxTotalQualificationTime}&minGuidedLearninghours={minGuidedLearninghours}&maxGuidedLearninghours={maxGuidedLearninghours}</b></code> </summary>


####
Retrieves a list of qualifications along with with the paging metadata ordered by the qualification number.

##### Parameters

> | name      |  type     | data type               | description | example                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | title      |  optional | string   | Search term that matches within the qualification title | title=biology
> | assessmentMethods       |  optional | string array (comma separated)  | The type of assessment methods used within the qualification | assessmentMethods=Coursework,E-assessment
> | gradingTypes       |  optional | string array (comma separated)    | The grading types used for the qualification | gradingTypes=Graded,Pass/Fail
> | awardingOrganisations       |  optional | string  | The name of the awarding organisation(s) that offers the qualification. Note that for this parameter, comma separated values are not used to search on multiple organisations due to some organisations containing commas; instead, you can list this argument multiple times to search for multiple organisations inclusively. | awardingOrganisations=BCS, The Chartered Institute for IT&awardingOrganisations=1st4sport
> | availability       |  optional | string array (comma separated)    | Availability of the qualification | availability=Available to learners,No longer awarded
> | qualificationTypes       |  optional | string array (comma separated)    | The type of qualification offered | qualificationTypes=Project,Technical Qualification,QCF
> | qualificationLevels       |  optional | string array (comma separated)    | The qualification level | qualificationLevels=Level 7,Level 4,Level 1
> | qualificationSubLevels       |  optional | string array (comma separated)    | The qualification sublevel where relevant | qualificationSubLevels=Entry 3,None
> | nationalAvailability       |  optional | string array (comma separated)    | What countries the qualification is offered in | nationalAvailability=England,Northern Ireland,Internationally
> | sectorSubjectAreas       |  optional | string array (comma separated)    | The subject sector area this qualification belongs to | sectorSubjectAreas=Politics,Science
> | minTotalQualificationTime       |  optional | int   | The minimum required time on the qualification in hours  | minTotalQualificationTime=1
> | maxTotalQualificationTime       |  optional | int   | The maximum amount of time on the qualification in hours | maxTotalQualificationTime=20
> | minGuidedLearninghours       |  optional | int   | The minimum amount of guided learning hours to complete the qualification | minGuidedLearninghours=1
> | maxGuidedLearninghours       |  optional | int   | The maximum amount of guided learning hours to complete the qualification | maxGuidedLearninghours=20
> | page      |  optional | int   | Page number for the current set of search results|if not provided, defaults to page # 1
> | limit      |  optional | int   | Number of organisation records to return for the search | if not provided defaults to 15. This is set via the `QualificationsPagingLimit` environment variable in Azure

Parameters are all applied as AND when multiple filters are set. For example, if there are 3 Qualification records as such: 

> | Qualification         | Qualification Level       | Sector Subject Areas|
> |-----------------------|--------------------------------|-----------------------------------|
> | Q1    | Level 1   | Direct Learning support   |
> | Q2    | Level 2   | Retailing and wholesaling     |
> | Q3    | Level 2   | Direct Learning support   |

And the query parameters contain `qualificationLevels = Level 2` and `sectorSubjectAreas = Direct Learning support`, only Q3 will be returned. 

##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Qualifications List response JSON                     | Paging metadata with list of Qualification records                                    |
> | `400`         | `application/json`                | `{"message": {error description}}`  | Parameters provided are not correct / data not supported                            |

##### Example Requests

> ```javascript
>  api/Qualifications?search=Chartered&assessmentMethods=Coursework,E-assessment&awardingOrganisations=Trinity College London&awardingOrganisations=ABE&page=1&limit=10
> ```

> ```javascript
>  api/Qualifications?page=5&limit=15
> ```

> ```javascript
>  api/Qualifications/
> ```

##### Example Response

```
 {
    "Count": 44787,
    "CurrentPage": 6,
    "Limit": 20,
    "Results": [
        {
            "QualificationNumber": "100/0102/5",
            "QualificationNumberNoObliques": "10001025",
            "Title": "AQA Advanced GCE in English Language A",
            "Status": "No longer awarded",
            "OrganisationName": "AQA Education DEMO2.2",
            "OrganisationAcronym": "AQA Education",
            "OrganisationRecognitionNumber": "RN5196",
            "Type": "GCE A Level",
            "SSA": "Languages, literature and culture of the British Isles",
            "Level": "Level 2",
            "SubLevel": "None",
            "EQFLevel": "Level 3",
            "GradingType": "Graded",
            "GradingScale": "A/B/C/D/E",
            "TotalCredits": 0,
            "TQT": null,
            "GLH": null,
            "MinimumGLH": 360,
            "MaximumGLH": 360,
            "RegulationStartDate": "2000-09-01T00:00:00",
            "OperationalStartDate": "2000-09-01T00:00:00",
            "OperationalEndDate": "2009-08-31T00:00:00",
            "CertificationEndDate": "2010-08-31T00:00:00",
            "ReviewDate": "2005-09-01T00:00:00",
            "OfferedInEngland": true,
            "OfferedInNorthernIreland": false,
            "OfferedInternationally": null,
            "Specialism": null,
            "Pathways": null,
            "AssessmentMethods": null,
            "ApprovedForDELFundedProgramme": null,
            "LinkToSpecification": null,
            "ApprenticeshipStandardReferenceNumber": null,
            "ApprenticeshipStandardTitle": null,
            "RegulatedByNorthernIreland": false,
            "NIDiscountCode": null,
            "GCESizeEquivalence": null,
            "GCSESizeEquivalence": null,
            "EntitlementFrameworkDesignation": null,
            "LastUpdatedDate": "2021-09-20T15:17:09.427"
        },
        ....
    ]
 }
 ```
 </details>



### Scopes of Recognition

<details>
 <summary><code>GET</code> <code><b>api/Scopes/{organisationNumber}</b></code> </summary>

####
Retrieves the scopes of recognition for an organisation

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | organisationNumber      |  required | string    | Recognition number for the Organisation record in the RN format eg. RN5353 |  Allowed without the RN prefix eg. 5353|


##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Scopes JSON                     | JSON object for the scopes                                    |
> | `404`         | `application/json`                | `{"message":"Not found"}`  | The organisation could not be found for the organisation number provided     
> | `400`         | `application/json`                | `{"error": "Please provide a valid organisation number"}`  | The organisation number provided is invalid                            |

##### Example Requests

> ```javascript
>  api/Scopes/RN5133
> ```

> ```javascript
>  api/Scopes/RN5133
> ```


##### Example response

```
{
    "inclusions": [
        {
            "type": "Advanced Extension Award",
            "levels": [
                {
                    "level": "Level 2",
                    "recognitions": [
                        "2.2 Mathematics and statistics",
                        "2.2 Mathematics and statistics"
                    ]
                }
            ]
        },
        {
            "type": "English For Speakers of Other Languages",
            "levels": [
                {
                    "level": "Entry Level - Entry 1,2,3",
                    "recognitions": [
                        "1.1 Medicine and Dentistry",
                        "1.2 Nursing and subjects and vocations allied to medicine",
                        "1.3 Health and social care",
                        "1.4 Public services",
                        "1.5 Child development and well-being",
                        "2.1 Science",
                        "2.2 Mathematics and statistics",
                        "3.1 Agriculture",
                        ...
                }
            ]
        },
        ...

```

 </details>
 

### Organisations Private

<details>
 <summary><code>GET</code> <code><b>gov/Organisations/{recognitionNumber}</b></code> </summary>

####
Retrieves an individual Organisation by the Recognition Number

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | recognitionNumber      |  required | string   | Recognition number for the Organisation record in the RN format eg. RN5353 |  Allowed without the RN prefix eg. 5353|


##### Headers
Requires the <code>Ocp-Apim-Subscription-Key</code> key for subscription access


##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Organisation JSON                     | JSON object for the Organisation                                    |
> | `404`         | `application/json`                | `{"message":"Not found"}`  | The organisation could not be found for the recognition number provided                            |


##### Example Requests

> ```javascript
>  gov/Organisations/RN5353
> ```

> ```javascript
>  gov/Organisations/5353
> ```


##### Example response

```
 {
    "name": "Example Organization", // String
    "recognitionNumber": "RN5353", // String
    "legalName": "Legal Organization Name", // String
    "acronym": "EO", // String
    "ofqualOrganisationStatus": null, // Nullable String
    "cceaOrganisationStatus": null, // Nullable String
    "ofqualRecognisedOn": null, // Nullable DateTime
    "ofqualRecognisedTo": null, // Nullable Date
    "ofqualSurrenderedOn": null, // Nullable DateTime
    "ofqualWithdrawnOn": null, // Nullable DateTime
    "cceaRecognisedOn": null, // Nullable DateTime
    "cceaRecognisedTo": null, // Nullable Date
    "cceaSurrenderedOn": null, // Nullable DateTime
    "cceaWithdrawnOn": null, // Nullable DateTime
    "contactEmail": "contact@example.org", // String
    "website": "https://example.org", // String
    "phoneNumber": "+1 123-456-7890", // String
    "feesUrl": "https://example.org/fees", // String
    "addressLine1": "123 Main Street", // String
    "addressLine2": "Suite 456", // String
    "addressCity": "Cityville", // String
    "addressCounty": "Countyshire", // String
    "addressCountry": "United Kingdom", // String
    "addressPostCode": "AB12 3CD", // String
    "lastUpdatedDate": "2024-02-20T09:07:22Z" // DateTime
}
```

 </details>

 
<details>
 <summary><code>GET</code> <code><b>gov/Organisations?search={search}&page={page}&limit={limit}</b></code> </summary>


####
Retrieves a list of organisations along with with the paging metadata ordered by Organisation name, Legal Name, then Acronym.

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | search      |  optional | string   | Search term that matches within the organisation name or ‘known as’/acronym field of one or more records| if not provided, all organisations are returned
> | page      |  optional | int   | Page number for the current set of search results|if not provided, defaults to page # 1
> | limit      |  optional | int   | Number of organisation records to return for the search | if not provided returns the whole list


##### Headers
Requires the <code>Ocp-Apim-Subscription-Key</code> key for subscription access

##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Organisation List response JSON                     | Paging metadata with list of Organisation records                                    |
> | `400`         | `application/json`                | `{"message": {error description}}`  | Parameters provided are not correct / data not supported                            |


 
##### Example Requests

> ```javascript
>  gov/Organisations?search=Chartered&page=1&limit=10
> ```

> ```javascript
>  gov/Organisations?page=5&limit=15
> ```

> ```javascript
>  gov/Organisations/
> ```

##### Example Response

```
 {
    "count": 27,
    "currentPage": 1,
    "limit": 15,
    "results": [
        {
            "name": "Example Organization", // String
            "recognitionNumber": "ABC123", // String
            "legalName": "Legal Organization Name", // String
            "acronym": "EO", // String
            "ofqualOrganisationStatus": null, // Nullable String
            "cceaOrganisationStatus": null, // Nullable String
            "ofqualRecognisedOn": null, // Nullable DateTime
            "ofqualRecognisedTo": null, // Nullable Date
            "ofqualSurrenderedOn": null, // Nullable DateTime
            "ofqualWithdrawnOn": null, // Nullable DateTime
            "cceaRecognisedOn": null, // Nullable DateTime
            "cceaRecognisedTo": null, // Nullable Date
            "cceaSurrenderedOn": null, // Nullable DateTime
            "cceaWithdrawnOn": null, // Nullable DateTime
            "contactEmail": "contact@example.org", // String
            "website": "https://example.org", // String
            "phoneNumber": "+1 123-456-7890", // String
            "feesUrl": "https://example.org/fees", // String
            "addressLine1": "123 Main Street", // String
            "addressLine2": "Suite 456", // String
            "addressCity": "Cityville", // String
            "addressCounty": "Countyshire", // String
            "addressCountry": "United Kingdom", // String
            "addressPostCode": "AB12 3CD", // String
            "lastUpdatedDate": "2024-02-20T09:07:22Z" // DateTime
        },
        ....
    ]
 }
 ```
 </details>


### Qualifications Private

<details>
 <summary><code>GET</code> <code><b>gov/Qualifications/{qualificationNumber}</b></code> </summary>

####
Retrieves an individual Qualification by the Qualification Number

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | qualificationNumber      |  required | string   | Qulification number for the qualification record eg. 100/0512/2 |  Allowed without obliques eg. 10005122|

##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Qualification JSON                     | JSON object for the Qualification                                    |
> | `404`         | `application/json`                | `{"message":"Not found"}`  | The qualification could not be found for the qualification number provided                            |

##### Example Requests

> ```javascript
>  gov/Qualifications/100/0512/2
> ```

> ```javascript
>  gov/Qualifications/10005122
> ```


##### Example response

```
{
    "QualificationNumber": "500/1522/9",
    "QualificationNumberNoObliques": "50015229",
    "Title": "Pearson EDI Level 3 Award in Preparing to Teach in the Lifelong Learning Sector (QCF)",
    "Status": "No longer awarded",
    "OrganisationName": "Pearson EDI",
    "OrganisationAcronym": "Pearson EDI",
    "OrganisationRecognitionNumber": "RN5134",
    "Type": "Vocational Certificate Of Education",
    "SSACode": "Teaching",
    "SSA": "Teaching and lecturing",
    "Level": "Level 2",
    "SubLevel": "None",
    "EQFLevel": "Level 3",
    "GradingType": "Pass/Fail",
    "GradingScale": null,
    "TotalCredits": 6,
    "TQT": null,
    "GLH": null,
    "MinimumGLH": 30,
    "MaximumGLH": 30,
    "RegulationStartDate": "2006-09-01T00:00:00",
    "OperationalStartDate": "2006-09-01T00:00:00",
    "OperationalEndDate": "2012-07-31T00:00:00",
    "CertificationEndDate": "2015-07-31T00:00:00",
    "ReviewDate": "2012-07-31T00:00:00",
    "EmbargoDate": null,
    "OfferedInEngland": true,
    "OfferedInNorthernIreland": true,
    "OfferedInternationally": null,
    "Specialism": null,
    "Pathways": null,
    "AssessmentMethods": [
        "Practical Demonstration/Assignment"
    ],
    "ApprovedForDELFundedProgramme": null,
    "LinkToSpecification": null,
    "ApprenticeshipStandardReferenceNumber": null,
    "ApprenticeshipStandardTitle": null,
    "RegulatedByNorthernIreland": false,
    "NIDiscountCode": null,
    "GCESizeEquivalence": null,
    "GCSESizeEquivalence": null,
    "EntitlementFrameworkDesignation": null,
    "LastUpdatedDate": "2021-09-20T15:17:09.427",
    "UILastUpdatedDate": "2012-07-27T15:59:13",
    "InsertedDate": "2016-06-21T07:12:17.42",
    "Version": 9,
    "AppearsOnPublicRegister": true,
    "OrganisationId": 1004,
    "LevelId": 10,
    "TypeId": 21,
    "SSAId": 42,
    "GradingTypeId": 1,
    "GradingScaleId": null,
    "PreSixteen": false,
    "SixteenToEighteen": true,
    "EighteenPlus": false,
    "NineteenPlus": true
}
```

 </details>
 
<details>
 <summary><code>GET</code> <code><b>gov/qualifications?title={title}&page={page}&limit={limit}&assessmentMethods={assessmentMethods}&gradingTypes={gradingTypes}&awardingOrganisations={awardingOrganisations}&availability={availability}&qualificationTypes={qualificationTypes}&qualificationLevels={qualificationLevels}&nationalAvailability={nationalAvailability}&sectorSubjectAreas={sectorSubjectAreas}&minTotalQualificationTime={minTotalQualificationTime}&maxTotalQualificationTime={maxTotalQualificationTime}&minGuidedLearninghours={minGuidedLearninghours}&maxGuidedLearninghours={maxGuidedLearninghours}</b></code> </summary>


####
Retrieves a list of qualifications along with with the paging metadata ordered by the qualification number.

##### Parameters
Same as the parameters on Qualifications Public

##### Headers
Requires the <code>Ocp-Apim-Subscription-Key</code> key for subscription access

##### Responses
Same as the responses on Qualifications Public

##### Example Requests

> ```javascript
>  gov/Qualifications?search=Chartered&assessmentMethods=Coursework,E-assessment&organisations=Trinity College London,ABE,AIM Qualifications&page=1&limit=10
> ```

> ```javascript
>  gov/Qualifications?page=5&limit=15
> ```

> ```javascript
>  gov/Qualifications/
> ```

##### Example Response
Same as the response on the Qualifications public with a few new fields: 

```
 {
    "Count": 44787,
    "CurrentPage": 6,
    "Limit": 20,
    "Results": [
        {
            "QualificationNumber": "500/1522/9",
            "QualificationNumberNoObliques": "50015229",
            "Title": "Pearson EDI Level 3 Award in Preparing to Teach in the Lifelong Learning Sector (QCF)",
            "Status": "No longer awarded",
            "OrganisationName": "Pearson EDI",
            "OrganisationAcronym": "Pearson EDI",
            "OrganisationRecognitionNumber": "RN5134",
            "Type": "Vocational Certificate Of Education",
            "SSACode": "Teaching",
            "SSA": "Teaching and lecturing",
            "Level": "Level 2",
            "SubLevel": "None",
            "EQFLevel": "Level 3",
            "GradingType": "Pass/Fail",
            "GradingScale": null,
            "TotalCredits": 6,
            "TQT": null,
            "GLH": null,
            "MinimumGLH": 30,
            "MaximumGLH": 30,
            "RegulationStartDate": "2006-09-01T00:00:00",
            "OperationalStartDate": "2006-09-01T00:00:00",
            "OperationalEndDate": "2012-07-31T00:00:00",
            "CertificationEndDate": "2015-07-31T00:00:00",
            "ReviewDate": "2012-07-31T00:00:00",
            "EmbargoDate": null,
            "OfferedInEngland": true,
            "OfferedInNorthernIreland": true,
            "OfferedInternationally": null,
            "Specialism": null,
            "Pathways": null,
            "AssessmentMethods": [
                "Practical Demonstration/Assignment"
            ],
            "ApprovedForDELFundedProgramme": null,
            "LinkToSpecification": null,
            "ApprenticeshipStandardReferenceNumber": null,
            "ApprenticeshipStandardTitle": null,
            "RegulatedByNorthernIreland": false,
            "NIDiscountCode": null,
            "GCESizeEquivalence": null,
            "GCSESizeEquivalence": null,
            "EntitlementFrameworkDesignation": null,
            "LastUpdatedDate": "2021-09-20T15:17:09.427",
            "UILastUpdatedDate": "2012-07-27T15:59:13",
            "InsertedDate": "2016-06-21T07:12:17.42",
            "Version": 9,
            "AppearsOnPublicRegister": true,
            "OrganisationId": 1004,
            "LevelId": 10,
            "TypeId": 21,
            "SSAId": 42,
            "GradingTypeId": 1,
            "GradingScaleId": null,
            "PreSixteen": false,
            "SixteenToEighteen": true,
            "EighteenPlus": false,
            "NineteenPlus": true
        },
        ....
    ]
 }
 ```

</details>



### Scopes of Recognition Private

<details>
 <summary><code>GET</code> <code><b>api/Scopes/{organisationNumber}</b></code> </summary>

####
Retrieves the scopes of recognition for an organisation

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | organisationNumber      |  required | string    | Recognition number for the Organisation record in the RN format eg. RN5353 |  Allowed without the RN prefix eg. 5353|



##### Headers
Requires the <code>Ocp-Apim-Subscription-Key</code> key for subscription access



##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Scopes JSON                     | JSON object for the scopes                                    |
> | `404`         | `application/json`                | `{"message":"Not found"}`  | The organisation could not be found for the organisation number provided     
> | `400`         | `application/json`                | `{"error": "Please provide a valid organisation number"}`  | The organisation number provided is invalid                            |

##### Example Requests

> ```javascript
>  gov/Scopes/RN5133
> ```

> ```javascript
>  gov/Scopes/RN5133
> ```


##### Example response

```
{
    "inclusions": [
        {
            "type": "Advanced Extension Award",
            "levels": [
                {
                    "level": "Level 2",
                    "recognitions": [
                        "2.2 Mathematics and statistics",
                        "2.2 Mathematics and statistics"
                    ]
                }
            ]
        },
        {
            "type": "English For Speakers of Other Languages",
            "levels": [
                {
                    "level": "Entry Level - Entry 1,2,3",
                    "recognitions": [
                        "1.1 Medicine and Dentistry",
                        "1.2 Nursing and subjects and vocations allied to medicine",
                        "1.3 Health and social care",
                        "1.4 Public services",
                        "1.5 Child development and well-being",
                        "2.1 Science",
                        "2.2 Mathematics and statistics",
                        "3.1 Agriculture",
                        ...
                }
            ]
        },
        ...

```

 </details>
 
## Deployment 
When a pull request is merged into `dev` or `main`, the function app is deployed automatically onto azure using an Azure pipeline set via the `azure-pipelines.yml` file for dev and `azure-pipelines-prod.yml` for the main branch. Updates are reflected instantly after a successful deploy. 

## Qualifications Search
We split the search query (`title` parameter) into individual tokens and then search the qualification title field for those tokens using a boolean AND operation on the title field in the database. A pre-defined set of tokenisation rules, delimiters and stop words are defined (`Constants\SearchConstants.cs`) which are used to tokenise the search term. 
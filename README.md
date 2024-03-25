# ofqual-register-api

An API for accessing the Register of qualifications, built using Azure Functions.

## API Documentation

### Organisations

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
> | search      |  optional | string   | Search term that matches within the organisation name or ‘known as’/acronym field of one or more records| if not povided, all organisations are returned
> | page      |  required | int   | Page number for the current set of search results|if not provided, defaults to page # 1
> | limit      |  required | int   | Number of organisation records to return for the search | if not provided defaults to 15


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
 <summary><code>GET</code> <code><b>api/Qualifications?search={search}&page={page}&limit={limit}</b></code> </summary>


####
Retrieves a list of qualifications along with with the paging metadata ordered by the qualification number.

##### Parameters

> | name      |  type     | data type               | description | example                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | search      |  optional | string   | Search term that matches within the qualification title| search=title
> | assessmentMethods       |  optional | string array (comma separated)  | assessment methods contain any of the param assessment methods| assessmentMethods=Coursework,E-assessment
> | gradingTypes       |  optional | string array (comma separated)    | Grading type is one of the param grading type | gradingTypes=Graded,Pass/Fail
> | organisations       |  optional | string array (comma separated)    | Organisation Name is one of the param Awarding Organisation | organisations=Trinity College London,ABE,AIM Qualifications
> | availability       |  optional | string array (comma separated)    | Availability matching the status column | availability=Available to learners,No longer awarded
> | types       |  optional | string array (comma separated)    | Types matching the type column | types=Project,Technical Qualification,QCF
> | levels       |  optional | string array (comma separated)    | Levels matching the level column | levels=Level 7,Level 4,Level 1
> | subLevels       |  optional | string array (comma separated)    | Sublevels matching the sub level column| sublevels=Entry 3,None
> | nationalAvailability       |  optional | string array (comma separated)    | Qualifications where boolean value for OfferedInCountry[CountryName] is set to true | nationalAvailability=England,Northern Ireland,Internationally
> | sectorSubjectAreas       |  optional | string array (comma separated)    | Sublevels matching the SSA column| sectorSubjectAreas=Politics,Science
> | minTotalQualificationTime       |  optional | int   | Qualifications where the TQT column is higher than minTotalQualificationTime | minTotalQualificationTime=1
> | maxTotalQualificationTime       |  optional | int   | Qualifications where the TQT column is lower than maxTotalQualificationTime | maxTotalQualificationTime=20
> | minGuidedLearninghours       |  optional | int   | Qualifications where the GLH column is higher than minGuidedLearninghours | minGuidedLearninghours=1
> | maxGuidedLearninghours       |  optional | int   | Qualifications where the GLH column is lower than maxGuidedLearninghours | maxGuidedLearninghours=20
> | page      |  required | int   | Page number for the current set of search results|if not provided, defaults to page # 1
> | limit      |  required | int   | Number of organisation records to return for the search | if not provided defaults to 15


##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Qualifications List response JSON                     | Paging metadata with list of Qualification records                                    |
> | `400`         | `application/json`                | `{"message": {error description}}`  | Parameters provided are not correct / data not supported                            |

##### Example Requests

> ```javascript
>  api/Qualifications?search=Chartered&assessmentMethods=Coursework,E-assessment&organisations=Trinity College London,ABE,AIM Qualifications&page=1&limit=10
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
> | search      |  optional | string   | Search term that matches within the organisation name or ‘known as’/acronym field of one or more records| if not povided, all organisations are returned
> | page      |  required | int   | Page number for the current set of search results|if not provided, defaults to page # 1
> | limit      |  required | int   | Number of organisation records to return for the search | if not provided defaults to 15


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
 <summary><code>GET</code> <code><b>gov/Qualifications?search={search}&page={page}&limit={limit}</b></code> </summary>


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

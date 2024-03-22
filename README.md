# ofqual-register-api

An API for accessing the Register of qualifications, built using Azure Functions.

## API Documentation

### Organisations

<details>
 <summary><code>GET</code> <code><b>/Organisations/{recognitionNumber}</b></code> </summary>

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
 <summary><code>GET</code> <code><b>/Organisations?search={search}&page={page}&limit={limit}</b></code> </summary>


####
Retrieves a list of organisations along with with the paging metadata ordered by Organisation name, Legal Name, then Acronym.

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | search      |  optional | string   | Search term that matches within the organisation name or ‘known as’/acronym field of one or more records| if parameter not povided, all organisations are returned
> | page      |  required | int   | Page number for the current set of search results|
> | limit      |  required | int   | Number of organisation records to return for the search |


##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Organisation List response JSON                     | JSON object for the Organisation                                    |
> | `400`         | `application/json`                | `{"message": {error description}}`  | Parameters provided are not correct / data not supported                            |


 
##### Example Requests

> ```javascript
>  api/Organisations?search=Chartered&page=1&limit=10
> ```

> ```javascript
>  api/Organisations?page=5&limit=15
> ```

##### Example Response

```
 {
    "Count": 27,
    "CurrentPage": 1,
    "Limit": 15,
    "Results": [
        {
            "id": 123, // Integer
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


### Qualifications

<details>
 <summary><code>GET</code> <code><b>/Qualifications/{qualificationNumber}</b></code> </summary>

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
 <summary><code>GET</code> <code><b>/Qualifications?search={search}&page={page}&limit={limit}</b></code> </summary>


####
Retrieves a list of qualifications along with with the paging metadata ordered by the qualification number.

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | search      |  optional | string   | Search term that matches within the qualification title| if parameter not povided, all qualifications are returned
> | page      |  required | int   | Page number for the current set of search results|
> | limit      |  required | int   | Number of organisation records to return for the search |


##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Organisation List response JSON                     | JSON object for the Organisation                                    |
> | `400`         | `application/json`                | `{"message": {error description}}`  | Parameters provided are not correct / data not supported                            |


 
##### Example Requests

> ```javascript
>  api/Organisations?search=Chartered&page=1&limit=10
> ```

> ```javascript
>  api/Organisations?page=5&limit=15
> ```

##### Example Response

```
 {
    "Count": 27,
    "CurrentPage": 1,
    "Limit": 15,
    "Results": [
        {
            "id": 123, // Integer
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


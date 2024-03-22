# ofqual-register-api

An API for accessing the Register of qualifications, built using Azure Functions.

## API Documentation

### Organisations

<details>
 <summary><code>GET</code> <code><b>/Organisations/{recognitionNumber}</b></code> <code>Retrieves an individual Organisation by the Recognition Number</code></summary>

####
Retrieves an individual Organisation by the Recognition Number

##### Parameters

> | name      |  type     | data type               | description | variations                                                           |
> |-----------|-----------|-------------------------|-----------|-----------------------------------------------------------------------|
> | recognitionNumber      |  required | string   | Recognition number for the Organisation record in the RN format eg. RN5353 |  Allowed without the RN prefix eg. 5353|

##### Example Requests

> ```javascript
>  api/Organisations/RN5353
> ```

> ```javascript
>  api/Organisations/5353
> ```


##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Organisation JSON                     | JSON object for the Organisation                                    |
> | `404`         | `application/json`                | `{"message":"Not found"}`  | The organisation could not be found for the recognition number provided                            |

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

##### Example Requests

> ```javascript
>  api/Organisations?search=Chartered&page=1&limit=10
> ```

> ```javascript
>  api/Organisations?page=5&limit=15
> ```


##### Responses

> | http code     | content-type                      | response                                  |Description                          |
> |---------------|-----------------------------------|-------------------------------------------|-------------------------------------|
> | `200`         | `application/json`              | Organisation List response JSON                     | JSON object for the Organisation                                    |
> | `400`         | `application/json`                | `{"message": {error description}}`  | Parameters provided are not correct / data not supported                            |

 </details>
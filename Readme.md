
Auto is a vehicle catalog microservice RESTful Web Api powered .NET Core 3.1 and MySql 8.0.19.

It's worth mentioning that it uses ASP.NET Core Identity for manager authentication and authorization with JWT tokens.

### What will you need to run the application?

[.NET Core 3.1 Runtime](https://www.microsoft.com/net/core)
[MySql Server](https://dev.mysql.com/downloads/mysql/)

### After downloading the source, what should you do?

 - Open the terminal or command prompt.

 - Go to the folder Auto.VehicleCatalog.API and the command 'dotnet restore'. Do the same in the folder Auto.VehicleCatalog.API.Test.

 ### How do I run the tests?

 - Go to the folder Auto.VehicleCatalog.API.Test and run the command 'dotnet test'.

### How do I run the Web API?

 - Go to Auto.VehicleCatalog.API and open the appsettings.json file.

 - Configure the connection string of the MySql instance with server, database, port, user and the password. The user must have database creator permission. 

 - Configure a new token verification key string.
 
 - Save the file.

- Execute the 'dotnet run' command.

- If all goes well you can access the API at http://localhost:5003.

## Is there a test mass?

- Yes, it is automatically inserted at system startup.

- It can be seen in the Vehicles.json, BrandsAndModels.json and Users.json files in the Setup subfolder, in the Auto.VehicleCatalog.API folder.

## What about the database, do I have to create it?

- No, it's created automatically by EF Core. Only the user in the connection string must have the correct rights to do so (root).

### How do I test the actions according to the roles? (Admin and Public)

- An action (/api/auth/login) was created in the AuthController controller, in which it's possible to authenticate with the user 'kelvis' with password '1234' and obtain a token as a response.

- This user 'kelvis' is like 'admin', so he will be able to execute all other methods (delete, put, etc.).

- There is the 'igor' user who is not admin, with the same password - 1234. To test how the API limit access to non-admin users.

- If you are going to use Postman to test, the token must go in header, in the Bearer field. However, using Swagger itself (which is already applied in the Web Api) is easier to test.

### What happens when I access the http://localhost:5003 address?

- Swagger (visual test tool) will open automatically, listing all the actions that we can perform in our api.

### What actions can we take?

Create: POST /api/brands [ADMIN]
Create a new brand

Read: GET /api/brands/{id} [ADMIN]
Read a brand by Id

Update: PUT /api/brands/{id} [ADMIN]
Update a brand

Delete: DELETE /api/brands/{id} [ADMIN]
Delete a brand

Read: GET /api/brands [PUBLIC]
List all brands ordered by name ascending

Create: POST /api/models [ADMIN]
Create a new model

Read: GET /api/models/{id} [ADMIN]
Read a model by Id

Update: PUT /api/models/{id} [ADMIN]
Update a model

Delete: DELETE /api/models/{id} [ADMIN]
Delete a model

Read: GET /api/models/brand/{brandId} [PUBLIC]
List all models from a brand ordered by name ascending

Create: POST /api/vehicles [ADMIN]
Create a new vehicle

Read: GET /api/vehicles/{id} [ADMIN]
Read a vehicle by Id

Update: PUT /api/vehicles/{id} [ADMIN]
Update a vehicle

Delete: DELETE /api/vehicles/{id} [ADMIN]
Delete a vehicle

Read: GET /api/vehicles/model/{modelId} [PUBLIC]
List all vehicles from a model ordered by value ascending

License
----

MIT

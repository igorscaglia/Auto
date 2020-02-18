
Auto is a vehicle catalog microservice RESTful Web Api powered .NET Core 3.1, MySql 8.0.19 and Docker.

It's worth mentioning that it uses ASP.NET Core Identity for manager authentication and authorization with JWT tokens.

### What will you need to run the application?

[.NET Core 3.1 Runtime](https://www.microsoft.com/net/core)

[MySql Server](https://dev.mysql.com/downloads/mysql/)

[Docker](https://www.docker.com/)

### After downloading the source, what should you do?

 - In this branch all application was containerized, so you should use VS Code tasks to create MySql 8.0.19 container and/or Development ou Production application.

 ### How do I run the tests?

 - Go to the folder Auto.VehicleCatalog.API.Test and run the command 'dotnet test'.

### How do I run the Web API?

 - Create the MySql container with the task 'docker run MySql 8.0.19'.

 - Run the task 'docker-run: release' if you wanna go in Production mode. Then navigate to http://localhost:3005/. You should see Swagger UI.

 - Run the task 'docker-run: debug' if you wanna go in Development mode. Then navigate to http://localhost:3001/. You should see Swagger UI.
 
- After that, all Readme.md instructions in master branch are valid.

License
----

MIT


Auto is a vehicle catalog microservice RESTful Web Api powered .NET Core 3.1, MySql 8.0.19 and Docker.

It's worth mentioning that it uses ASP.NET Core Identity for manager authentication and authorization with JWT tokens.

### What will you need to run the application?

[Docker](https://www.docker.com/)

[.NET Core 3.1 SDK](https://dotnet.microsoft.com/download)

[VS Code](https://code.visualstudio.com/download)

[VS Code Docker Extension](https://github.com/microsoft/vscode-docker)

### After downloading the source, what should you do?

 - In this branch, all components have been containerized, so you should use VS Code tasks to create MySql 8.0.19 and/or Development or Production container application.
 
 - All parameters, ports, versions, passwords, etc, could be changed in tasks.json file.

### How do I run the Web API?

 - Create the MySql container with the task 'docker run MySql 8.0.19'.

 - Run the task 'docker-run: release' if you wanna go in Production mode. Then navigate to http://localhost:3005/. You should see Swagger UI.

 - Run the task 'docker-run: debug' if you wanna go in Development mode. Then navigate to http://localhost:3001/. You should see Swagger UI.
 
- If you want to debug the application, just choose 'Docker .NET Core Launch' and click 'Start Debugging' in 'Run and Debug' window.
 
- After that, all Readme.md instructions in master branch are valid.

License
----

MIT

### README.md

**ASP.NET 6 Web API and Web Project**

This is a sample ASP.NET 6 Web API and Web Project with the following features:

- **Automapper Integration:** Seamlessly map between DTOs and entity models for efficient data transfer.
- **Entity Framework Core (EF Core):** Utilize EF Core for powerful database operations and object-relational mapping (ORM).
- **Repository Pattern:** Implement a structured data access layer following the repository pattern for maintainability.
- **JWT Token Authentication:** Secure your API with JSON Web Tokens (JWT) for user authentication.
- **API Versioning:** Maintain backward compatibility and evolve your API using versioning.
- **.NET Identity:** Manage authentication and authorization using .NET Identity for user management.
- **GitHub Actions Deployment:** Automate deployment to your FTP server using GitHub Actions.
- **Swagger Integration:** Document and explore your API endpoints with Swagger, supporting versions v1 and v2.

#### Prerequisites

* .NET 6 SDK
* Visual Studio 2022
* SQL Server Database

#### Getting Started

1. Clone this repository to your local machine.
2. Open the solution file in Visual Studio 2022.
3. Restore the NuGet packages.
4. Update the database connection string in the `appsettings.json` file.
5. Run the project.

#### Usage

The Web API is exposed at the following endpoint:

```
https://localhost:7001/api/v1/
```

You can use Swagger to explore the API and generate client code. To access Swagger, open the following URL in your web browser:

```
https://localhost:7001/swagger/v1
```

To authenticate with the API, you can use the following steps:

1. Send a POST request to the `api/v1/UsersAuthAPI/login` endpoint with your username and password.
2. The server will respond with a JWT token.
3. Include the JWT token in the `Authorization` header of all subsequent requests to the API.

#### GitHub Actions Deployment

Automate deployment to your FTP server using GitHub Actions with the following steps:

#### FTP Credentials:

* Generate an FTP username and password on your FTP server.
* In your GitHub repository, go to the "Settings" tab.
* In the left sidebar, click on "Secrets."
* Click the "New repository secret" button.
* Create two secrets:
* FTP_USERNAME: Set this secret with your FTP username.
* FTP_PASSWORD: Set this secret with your FTP password.
* Create a .github/workflows/deploy.yml file in your repository with the provided GitHub Actions workflow template. Customize it with your FTP server details.
* you can check **deploy-api.yml** for refrence.

Whenever changes are pushed to the main branch, GitHub Actions will automatically deploy your project to the specified FTP server.

#### Swagger

The Web API is integrated with Swagger. To access Swagger, open the following URL in your web browser:

```
https://localhost:7001/swagger/v1
```

Swagger allows you to explore the API and generate client code.

#### API Versioning

The Web API uses API versioning to allow different versions of the API to coexist. To access a specific version of the API, append the version number to the endpoint URL. For example, to access version 1 of the API, you would use the following endpoint:

```
https://localhost:7001/api/v1/
```

#### Authentication and Authorization

The Web API uses .NET Identity for authentication and authorization. To authenticate with the API, you can send a POST request to the `/api/v1/auth/login` endpoint with your username and password. The server will respond with a JWT token. You can then include the JWT token in the `Authorization` header of all subsequent requests to the API.

#### Conclusion

This is a sample ASP.NET 6 Web API and Web Project with the following features:

* AutoMapper Integration
* EF Core
* Repository Pattern
* JWT Token
* API Versioning
* .NET Identity for Authentication and Authorization
* Integrated GitHub Actions for Deployment
* Swagger Integration for versions v1 and v2

I hope this project is helpful. Please feel free to contribute or submit bug reports.
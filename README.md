LuxeLoft
========

LuxeLoft is an ASP.NET Core MVC project developed by Abdullah Basit. This application is designed to provide a robust and scalable web solution utilizing the Model-View-Controller (MVC) architectural pattern.

Project Structure
-----------------

The project is organized into several key directories:

*   **Areas/Identity/Pages**: Contains Razor Pages related to user identity and authentication.
    
*   **Controllers**: Houses the controller classes that handle HTTP requests and responses.
    
*   **Data**: Includes classes related to data access and database context.
    
*   **Migrations**: Contains Entity Framework Core migration files for database schema changes.
    
*   **Models**: Defines the data models used throughout the application.
    
*   **Properties**: Stores project metadata, including assembly information.
    
*   **Views**: Contains the Razor views for rendering the UI.
    
*   **wwwroot**: Serves as the web root, containing static files like CSS, JavaScript, and images.
    

Getting Started
---------------

To run this project locally:

1.  Clone the repository:
```sh
clone https://github.com/IAlphaK/LuxeLoft.gitcd LuxeLoft
```
2.  Restore dependencies:
```sh
dotnet restore
```    
3. Apply migrations:
```sh
dotnet ef database update
```    
4.  Run the application:
```sh
dotnet run
```

Ensure you have the .NET SDK installed on your machine before executing these commands.

License
-------

This project is licensed under the MIT License.

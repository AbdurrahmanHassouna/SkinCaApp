# SkinCa: AI-Powered Skin Cancer Detection API

SkinCa is a comprehensive RESTful API developed with ASP.NET Core 6 and Entity Framework.
It leverages AI to assist in the early detection of skin cancer, potentially saving lives by connecting doctors and patients.

## Features

- **AI-Driven Skin Cancer Detection**: Utilize state-of-the-art AI models to analyze skin images and provide insights into the likelihood of cancerous lesions.
- **RESTful API Design**: Seamlessly integrate SkinCa into your applications using a well-defined RESTful API architecture.
- **ASP.NET Core 6**: Benefit from the performance, scalability, and security enhancements of ASP.NET Core 6.
- **Entity Framework**: Leverage the powerful data access capabilities of Entity Framework for efficient data management.
- **JWT Authentication**: Secure your API with robust JWT (JSON Web Token) authentication for user authorization.
- **Identity Framework**: Streamline user registration, authentication, and authorization with Identity Framework.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- Code editor or IDE (e.g., Visual Studio, Visual Studio Code)

### Clone the Repository

```bash
git clone https://github.com/AbdurrahmanHassouna/SkinCaApp.git
```

### Build and Run the API

1. Open the project in your preferred IDE.
2. Ensure all dependencies are restored:
   ```
   dotnet restore
   ```

4. Apply Migrations
   ```
   dotnet ef database update
   ```
6. Build the project:
   ```
   dotnet build
   ```
   
7. Run the API:
   ```
   dotnet run
   ```
## Authentication

SkinCa utilizes JWT authentication for API access. You need to register an account and obtain a JWT token to access protected endpoints.

## Database Configuration

SkinCa requires a database connection to store user data and analysis results.
Refer to the project's configuration files (e.g., appsettings.json) for database connection string setup.

## License

MIT


## Disclaimer

SkinCa is intended to be a tool for assisting in skin cancer detection. It is not a substitute for professional medical diagnosis.
Always consult a qualified healthcare professional for any concerns about skin lesions.

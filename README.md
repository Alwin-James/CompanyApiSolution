# CompanyApiSolution

## Overview

**CompanyApiSolution** is a .NET 6+ Web API application for managing company records. It supports operations such as creating, retrieving, and updating companies, and it uses JWT-based authentication for security. Swagger is used for API documentation and testing. The API supports creating, retrieving, and updating Company records with the following fields:

-   **Name**
-   **Stock Ticker**
-   **Exchange**
-   **ISIN** (must start with two letters; duplicate ISINs are not allowed)
-   **Website URL** (optional)

Additionally, the API implements JWT-based authentication to secure endpoints. Swagger is used for API documentation and testing.

## Architecture

The solution is divided into several layers:
- **API Layer (CompanyApi.Api):** Provides RESTful endpoints.
- **Business Logic Layer (CompanyApi.Business):** Contains core business rules and validation.
- **Data Access Layer (CompanyApi.Data):** Uses EF Core for database operations.
- **Domain Models (CompanyApi.Models):** Shared entity definitions.
- **Unit Tests (CompanyApi.Tests):** Contains tests for the application.
- **Authentication (JWT):** Secures endpoints.
- **API Documentation (Swagger):** Interactive API testing interface.

## Prerequisites

- Visual Studio 2022 (or later)
- .NET 6 SDK (or later)
- SQL Server Management Studio (SSMS)
- Docker Desktop (optional)
- Git

## Setup Instructions

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/<YourGitHubUsername>/<YourRepositoryName>.git
2. **Database Setup:**

-   **Option A:** Use EF Core migrations:
    
    powershell
    
    Copy
    
    `Update-Database` 
    
-   **Option B:** Run the provided SQL script `CreateCompanyDb.sql`.
-    CREATE DATABASE CompanyDb;
	GO

	USE CompanyDb;
	GO

	CREATE TABLE Companies (
	    Id INT PRIMARY KEY IDENTITY(1,1),
	    Name NVARCHAR(100) NOT NULL,
	    Exchange NVARCHAR(50) NOT NULL,
	    Ticker NVARCHAR(20) NOT NULL,
	    Isin NVARCHAR(12) NOT NULL UNIQUE,
	    Website NVARCHAR(200) NULL
	);
	GO

	-- Optional: Insert sample records
	  ```bash 
	  INSERT INTO Companies (Name, Exchange, Ticker, Isin, Website)
	VALUES 
	('Apple Inc.', 'NASDAQ', 'AAPL', 'US0378331005', 'http://www.apple.com'),
	('British Airways Plc', 'Pink Sheets', 'BAIRY', 'US1104193065', NULL);
	GO
3. **Update the Connection String:**  
Update `appsettings.json` in **CompanyApi.Api**:

	```json
	{
	  "ConnectionStrings": {
	    "DefaultConnection": "Server=localhost;Database=CompanyDb;User Id=sa;Password=YourStrong@Passw0rd;"
	  }
	}

## Authentication using JWT

The API uses JWT-based authentication to secure endpoints.

### Configuration

-   **appsettings.json:**  
    Your JWT settings should be defined in your configuration file:   
    
    ```{
      "Jwt": {
        "Issuer": "https://localhost:<port>",
        "Audience": "https://localhost:<port>",
        "Key": "ThisIsASuperSecretKeyForDevelopment"
      }
    }
    
   **Note:** Replace `<port>` with your actual port number. In production, use a strong, secure key and store it securely (e.g., environment variables or a secrets manager).
    

### Authentication Endpoint

A dedicated authentication endpoint (e.g., `POST /api/auth/login`) is implemented. When valid credentials are provided 
username: `test` 
password: `alwin`
 the endpoint issues a JWT token.



### Using JWT with Swagger

Swagger is configured to support JWT authentication:

1.  **Click the "Authorize" Button:**  
    In the Swagger UI (accessible at `https://localhost:<port>/swagger`), click the **Authorize** button.
    
2.  **Enter the JWT Token:**  
    Provide the token in the following format:  
    
    `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...` 
    
3.  **Authorize and Test Endpoints:**  
    Once authorized, Swagger will include the JWT token in subsequent API calls.

## Building and Running the Application

### 1. Running Locally

1.  **Open the Solution in Visual Studio:**  
    Open `CompanyApiSolution.sln` in Visual Studio.
    
2.  **Build the Solution:**  
    Use **Build > Build Solution** or press `Ctrl+Shift+B`.
    
3.  **Run the API:**  
    Press `F5` (or use **Debug > Start Debugging**) to run the API.  
    Swagger UI is enabled (accessible at `https://localhost:<port>/swagger`).
    

### 2. Running Using Docker (Optional)

If you want to run the API inside a Docker container:

1.  **Ensure Docker Desktop is Running.**
2.  **Build the Docker Image:**  
    In the **CompanyApi.Api** project folder, run:
    
    `docker build -t companyapi .` 
    
3.  **Run the Docker Container:**    
    
    `docker run -d -p 5000:80 --name companyapi companyapi` 
    
    The API will be available at `http://localhost:5000`.
## Testing

Unit tests are provided in the **CompanyApi.Tests** project.

1.  **Run Tests in Visual Studio:**  
    Use **Test > Run All Tests**.
## API Endpoints

The API exposes the following endpoints:

-   **POST /api/auth/login:**  
    Authenticate a user and return a JWT token.    
    
    `{
      "username": "test",
      "password": "password"
    }` 
    
-   **POST /api/companies:**  
    Create a new Company record.  
    _Validation:_    
    -   ISIN must start with two letters and be unique.
-   **GET /api/companies/{id}:**  
    Retrieve a Company by its ID.
    
-   **GET /api/companies/isin/{isin}:**  
    Retrieve a Company by its ISIN.
    
-   **GET /api/companies:**  
    Retrieve all Company records.
    
-   **PUT /api/companies/{id}:**  
    Update an existing Company record.
    

Detailed API documentation is available via Swagger when the application is running.

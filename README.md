# SpendingTracker API Documentation

## Project Description
A financial tracking tool that allows users to record their expenses and incomes by categories, and get monthly balances.


## Why Clean Architecture?
This project implements Clean Architecture to ensure a clear and modular structure in the code, enhancing scalability and maintainability. The architecture divides the application into independent layers such as Domain, Application, and Infrastructure each with specific responsibilities:

- Separation of Concerns: By keeping responsibilities isolated within each layer, Clean Architecture makes the code easier to read, maintain and understand.
  
- Flexibility and Scalability: It allows components (such as databases, authentication providers, etc.) to be changed or enhanced without affecting the core business logic.

- Testability: The architecture enables the business logic layers to remain isolated from infrastructure, making unit and integration testing easier and contributing to a more robust, error resistant software.

- Independence from Infrastructure: Clean Architecture ensures that business logic is not dependent on specific implementation details. For instance, if the database system or authentication provider changes the impact on business logic is minimized.

## Why Result Pattern?
In this project, the Result Pattern is implemented to handle the flow of operations and manage errors effectively. This approach has several benefits:

- Clear Success/Failure Handling: It helps distinguish between successful and failed operations explicitly, reducing ambiguity in the flow of logic.

- Standardized Error Reporting: By using a unified pattern for result handling, the project ensures consistent error messages and status codes, making it easier to maintain and debug.
  
- Improved Testability: It simplifies unit testing by providing a clear path for checking success and failure scenarios, which contributes to more reliable software.

## Cache
Redis is implemented in this project to cache freecurrencyapi data, reducing external API calls, improving performance, and minimizing latency.

**Note:** While a local in-memory cache would be sufficient for this small, single-server application, Redis was chosen specifically for learning and exploration purposes. This implementation showcases how to integrate Redis in a .NET application, though it may be overengineered for the current requirements.

## Prerequisites
- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQL Server
- [Redis account](https://redis.io/es/)
- [freecurrencyapi account](https://freecurrencyapi.com/)

## How to Run
1. Clone the repository.
2. Set up the connection string in `appsettings.json`.
3. Run the project.

## Usage

### Auth
- **POST** `/auth/login`
  - **Description**: Authenticates a user  and returns a JWT token.
  - **Request Body**:
    ```json
    {
      "userName": "user@example.com",
      "password": "pas5word-*123"
    }
    ```
  - **Response**: JWT token on success.
    
- **POST** `/auth/register`
  - **Description**: Registers a new user in the system.
  - **Request Body**:
    ```json
    {
      "userName": "string",
      "email": "string",
      "fullName": "string",
      "password": "string"
    }
    ```
### Accounts
- **GET** `/accounts`
    - **Description** : Retrieve all user accounts (Ex: Wallet, Bank, etc..).
    -  **Authentication**: Required.
      
- **POST** `/accounts`
  - **Description**: Create a new account.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "accountName": "string",
        "amount": 0,
        "description": "string"
    }
    ```
    
- **PUT** `/accounts`
  - **Description**: Update an account.
  - **Authentication**: Required. 
  - **Request Body**:
    ```json
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "accountName": "string",
        "amount": 0,
        "description": "string"
    }
    ```
  
- **DELETE** `/accounts`
  - **Description**: Delete an account.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```

### CategoryExpense
- **GET** `/category-expense`
    - **Description** : Retrieve all expense categories.
    -  **Authentication**: Required.
      
- **POST** `/category-expense`
  - **Description**: Create a new expense category.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "categoryName": "string",
        "color": "string",
        "icon": "string"
    }
    ```
    
- **PUT** `/category-expense`
  - **Description**: Update expense category.
  - **Authentication**: Required. 
  - **Request Body**:
    ```json
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "categoryName": "string",
        "color": "string",
        "icon": "string"
    }
    ```
  
- **DELETE** `/category-expense`
  - **Description**: Delete expense category.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```
### CategoryIncome
- **GET** `/category-income`
    - **Description** : Retrieve all income categories.
    -  **Authentication**: Required.
      
- **POST** `/category-income`
  - **Description**: Create a new income category.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "categoryName": "string",
        "color": "string",
        "icon": "string"
    }
    ```
    
- **PUT** `/category-income`
  - **Description**: Update income category.
  - **Authentication**: Required. 
  - **Request Body**:
    ```json
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "categoryName": "string",
        "color": "string",
        "icon": "string"
    }
    ```
  
- **DELETE** `/category-income`
  - **Description**: Delete income category.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```
### Income
- **GET** `/income/{accountId},{year},{month}`
    - **Description** : Retrieve all income.
    -  **Authentication**: Required.
      
- **POST** `/income`
  - **Description**: Create a new income.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "description": "string",
        "amount": 0,
        "date": "2024-11-09T00:59:18.924Z",
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```
    
- **PUT** `/income`
  - **Description**: Update income.
  - **Authentication**: Required. 
  - **Request Body**:
    ```json
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "description": "string",
        "amount": 0,
        "date": "2024-11-09T00:59:18.924Z",
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```
  
- **DELETE** `/income`
  - **Description**: Delete income.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```
### Expense
- **GET** `/expense/{accountId},{year},{month}`
    - **Description** : Retrieve all expense.
    -  **Authentication**: Required.
      
- **POST** `/expense`
  - **Description**: Create a new expense.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "description": "string",
        "amount": 0,
        "date": "2024-11-09T00:59:18.924Z",
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```
    
- **PUT** `/expense`
  - **Description**: Update expense.
  - **Authentication**: Required. 
  - **Request Body**:
    ```json
    {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "description": "string",
        "amount": 0,
        "date": "2024-11-09T00:59:18.924Z",
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```
  
- **DELETE** `/expense`
  - **Description**: Delete expense.
  - **Authentication**: Required.
  - **Request Body**:
    ```json
    {
        "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
    ```
### Monthly Balances
- **GET** `/monthly-balances/{accountId},{year},{month}`
    - **Description** : Retrieve all monthly balances.
    -  **Authentication**: Required.
### Currency
- **GET** `/currency/currencies`
    - **Description** : Returns all our supported currencies.
    -  **Authentication**: Required.
- **GET** `/currency/latest?baseCurrency=USD&currencies=IDR`
    - **Description** : Returns the latest exchange rates.
    -  **Authentication**: Required.
    -  **More information**: https://www.nuget.org/packages/freecurrencyapi/  

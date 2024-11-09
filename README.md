# SpendingTracker API Documentation / Front-End(Working on it)

## Project Description
Tool that allows users to record their expenses and incomes by categories, and get monthly balances.

## Prerequisites
- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQL Server

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
      

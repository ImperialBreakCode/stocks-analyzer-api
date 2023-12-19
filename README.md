# Stocks Analyzer Api

## Contributors and microservices

## Routes

### 1. Gateway

* ### Account

    ***<code style="color:#c555ff;">POST /api/Account/Register</code>***

    - **Input**: `RegisterUserDTO` in the request body.
    - **Description:** Redirects to `api/Accounts/User/Register`.
 
    ***<code style="color:#c555ff;">POST /api/Account/Login</code>***

    - **Input**: `LoginUserDTO` in the request body.
    - **Description:** Redirects to `api/Accounts/User/Login`.
 
    ***<code style="color:#c555ff;">GET /api/Account/UserInformation/{username}</code>***

    - **Input**: **`username`** as a route parameter.
    - **Description:** Redirects to `api/Accounts/User/UserInformation/{username}`, if the information isn't cached.
 
    ***<code style="color:#c555ff;">PUT /api/Account/UpdateUser</code>***

    - **Input**: **`UpdateUserDTO`** in the request body.
    - **Description:** Redirects to `api/Accounts/User/UpdateUser/{username}`, username is passed in from the JWT token.
 
    ***<code style="color:#c555ff;">DELETE /api/Account/DeleteUser</code>***

    - **Input**: **`None`**.
    - **Description:** Redirects to `api/Accounts/User/DeleteUser/{username}`, username is passed in from the JWT token.
 
    ***<code style="color:#c555ff;">GET /api/Account/ConfirmUser/{userId}</code>***

    - **Input**: **`userId`** as a route parameter.
    - **Description:** Redirects to `api/Accounts/User/ConfirmUser/{userId}`.
 
    ***<code style="color:#c555ff;">GET /api/Account/GetTransactions</code>***

    - **Input**: **`None`**.
    - **Description:** Redirects to `api/Accounts/Transaction/GetTransactionsByUsername/{username}`, username is passed in from the JWT token.
 
* ### Analyzer

    ***<code style="color:#c555ff;">GET /api/Analyzer/PortfolioSummary/{walletId}</code>***

    - **Input**: `walletId` as a route parameter.
    - **Description:** Redirects to `api/Analyzer/User/PortfolioSummary/{walletId}`.
 
    ***<code style="color:#c555ff;">GET /api/Analyzer/CurrentBalanceInWallet/{walletId}</code>***

    - **Input**: `walletId` as a route parameter.
    - **Description:** Redirects to `api/Analyzer/User/CurrentBalanceInWallet/{walletId}`.
 
    ***<code style="color:#c555ff;">GET /api/Analyzer/GetUserStocksInWallet/{walletId}</code>***

    - **Input**: `walletId` as a route parameter.
    - **Description:** Redirects to `api/Analyzer/Stock/GetUserStocksInWallet/{walletId}`.
 
    ***<code style="color:#c555ff;">GET /api/Analyzer/CurrentProfitability/{username}/{symbol}/{type}</code>***

    - **Input**: `username`, `symbol` (stock symbol), and `type` (current, daily, weekly, monthly) as a route parameter.
    - **Description:** Redirects to `api/Analyzer/Stock/CurrentProfitability/{username}/{symbol}/{type}`.
 
    ***<code style="color:#c555ff;">GET /api/Analyzer/PercentageChange/{username}/{symbol}/{type}</code>***

    - **Input**: `username`, `symbol` (stock symbol), and `type` (current, daily, weekly, monthly) as a route parameter.
    - **Description:** Redirects to `api/Analyzer/Stock/PercentageChange/{username}/{symbol}/{type}`.
 
    ***<code style="color:#c555ff;">GET /api/Analyzer/CalculateAverageProfitability/{username}/{symbol}/{type}</code>***

    - **Input**: `username`, `symbol` (stock symbol), and `type` (current, daily, weekly, monthly) as a route parameter.
    - **Description:** Redirects to `api/Analyzer/Stock/CalculateAverageProfitability/{username}/{symbol}/{type}`.
 
* ### RequestInfo

    ***<code style="color:#c555ff;">GET /api/RequestInfo/ThisRouteInfoForLast24H/{route}</code>***

    - **Input**: `route` as a route parameter.
    - **Description:** Returns the number of requests made to this route in the last 24 hours and the user who made the most requests to it.
 
    ***<code style="color:#c555ff;">GET /api/RequestInfo/RouteInfoForLast24H</code>***

    - **Input**: `None`.
    - **Description:** Returns the number of requests made to the API in the past 24 hours, the user who made the most requests, the hour with the most requests and the most used route.
 
* ### Stock

    ***<code style="color:#c555ff;">GET /api/Stock/GetStock/{stockId}</code>***

    - **Input**: `stockId` as a route parameter.
    - **Description:** Redirects to `api/Stock/GetStock/{stockId}`.
 
    ***<code style="color:#c555ff;">GET /api/Stock/GetStocksInWallet/{walletId}</code>***

    - **Input**: `walletId` as a route parameter.
    - **Description:** Redirects to `api/Stock/GetStocksInWallet/{stockId}`.
 
    ***<code style="color:#c555ff;">PUT /api/Stock/AddStockForPurchace/{StockDTO}</code>***

    - **Input**: `StockDTO` in the request body.
    - **Description:** Redirects to `api/Stock/AddStockForPurchace/{username}`, username is passed in from the JWT token.
 
    ***<code style="color:#c555ff;">PUT /api/Stock/AddStockForSale/{StockDTO}</code>***

    - **Input**: `StockDTO` in the request body.
    - **Description:** Redirects to `api/Stock/AddStockForSale/{username}`, username is passed in from the JWT token.
 
    ***<code style="color:#c555ff;">POST /api/Stock/ConfirmPurchase</code>***

    - **Input**: `None`.
    - **Description:** Redirects to `api/Stock/ConfirmPurchase/{username}`, username is passed in from the JWT token.
 
    ***<code style="color:#c555ff;">POST /api/Stock/ConfirmSale</code>***

    - **Input**: `None`.
    - **Description:** Redirects to `api/Stock/ConfirmSale/{username}`, username is passed in from the JWT token.
 
* ### StockInfo

    ***<code style="color:#c555ff;">GET /api/StockInfo/Current/{companyName}</code>***

    - **Input**: `companyName` as a route parameter.
    - **Description:** Redirects to `api/Stock/current/{companyName}`.
 
    ***<code style="color:#c555ff;">GET /api/StockInfo/Daily/{companyName}</code>***

    - **Input**: `companyName` as a route parameter.
    - **Description:** Redirects to `api/Stock/daily/{companyName}`.
 
    ***<code style="color:#c555ff;">GET /api/StockInfo/Weekly/{companyName}</code>***

    - **Input**: `companyName` as a route parameter.
    - **Description:** Redirects to `api/Stock/weekly/{companyName}`.
 
    ***<code style="color:#c555ff;">GET /api/StockInfo/Monthly/{companyName}</code>***

    - **Input**: `companyName` as a route parameter.
    - **Description:** Redirects to `api/Stock/monthly/{companyName}`.
 
* ### Wallet

    ***<code style="color:#c555ff;">PUT /api/Wallet/Deposit</code>***

    - **Input**: `DepositWalletDTO` in the request body.
    - **Description:** Redirects to `api/Wallet/Deposit/{username}`, username is passed in from the JWT token.
 
    ***<code style="color:#c555ff;">POST /api/Wallet/CreateWallet</code>***

    - **Input**: `None`.
    - **Description:** Redirects to `api/Wallet/CreateWallet/{username}`, username is passed in from the JWT token.
 
    ***<code style="color:#c555ff;">DELETE /api/Wallet/DeleteWallet</code>***

    - **Input**: `None`.
    - **Description:** Redirects to `api/Wallet/DeleteWallet/{username}`, username is passed in from the JWT token.
 
    ***<code style="color:#c555ff;">GET /api/Wallet/GetWallet/{walletId}</code>***

    - **Input**: `walletId` as a route parameter.
    - **Description:** Redirects to `api/Wallet/GetWallet/{username}`, username is passed in from the JWT token.


---
### 2. Accounts

* ### User

    ***<code style="color:#c555ff;">POST /api/User/Register</code>***

    - **Input**: `RegisterUserDTO` in the request body.
    - **Description:** Registers a new user based on the provided user data. If successful, returns a 201 Created status with the user information; otherwise, returns an error message with status code Bad Request or Conflict (if the username or the email are already present in the database).

    ***<code style="color:#c555ff;">POST /api/User/Login</code>***

    - **Input:** **`LoginUserDTO`** in the request body.
    - **Description:** Attempts to authenticate a user. If successful, returns a 200 OK status with authentication details; otherwise, returns a 401 Unauthorized status with an error message.

    ***<code style="color:#5295ff;">GET /api/User/ConfirmUser/{userId}</code>***

    - **Input:** **`userId`** as a route parameter.
    - **Description:** Confirms the user account associated with the given **`userId`**. Returns a 200 OK status if successful; otherwise, returns a 400 Bad Request status.
    - **Note:** Changes the userId after the user confirmation.

    ***<code style="color:#5295ff;">GET /api/User/UserInformation/{username}</code>***

    - **Input:** **`username`** as a route parameter.
    - **Description:** Retrieves information about a user based on the provided **`username`**. Returns a 200 OK status with the user information if the user exists; otherwise, returns a 404 Not Found status.

    ***<code style="color:#f09500;">PUT /api/User/UpdateUser/{username}</code>***

    - **Input:** **`UpdateUserDTO`** in the request body, **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Updates the user information for the specified **`username`**. Returns a response indicating the result of the update operation.

    ***<code style="color:#ff4949;">DELETE /api/User/DeleteUser/{username}</code>***

    - **Input:** **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Deletes the user account associated with the provided **`username`**. Returns a 200 OK status.

* ### Wallet

    ***<code style="color:#c555ff;">POST /api/Wallet/CreateWallet/{username}</code>***

    - **Input:** **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Creates a new wallet for the specified **`username`**. Returns a 201 Created status if the wallet is created successfully, a 400 Bad Request status if the wallet already exists, and a 404 Not Found status if the user is not found.

    ***<code style="color:#5295ff;">GET /api/Wallet/GetWalletBalance/{walletId}</code>***

    - **Input:** **`walletId`** as a route parameter.
    - **Description:** Retrieves the balance of the wallet associated with the given **`walletId`**. Returns the wallet balance if the wallet is found; otherwise, returns a 404 Not Found status.

    ***<code style="color:#5295ff;">GET /api/Wallet/GetWallet/{walletId}</code>***

    - **Input:** **`walletId`** as a route parameter.
    - **Description:** Retrieves information about the wallet associated with the given **`walletId`**. Returns the wallet information if the wallet is found; otherwise, returns a 404 Not Found status.

    ***<code style="color:#f09500;">PUT /api/Wallet/Deposit/{username}</code>***

    - **Input:** **`DepositWalletDTO`** in the request body, **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Deposits funds into the wallet associated with the specified **`username`**. Returns a response indicating the result of the deposit operation.
    - **Note**: If the wallet is a demo wallet, then it deletes it and creates a new one (which is not a demo wallet) with the deposited funds.
    - **Note**: In the dto the user can specify the currency of the funds. At the moment the currency can be: USD, BGN EUR or GBP

    ***<code style="color:#ff4949;">DELETE /api/Wallet/DeleteWallet/{username}</code>***

    - **Input:** **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Deletes the wallet associated with the provided **`username`**. Returns an OK response.

* ### Stock

    ***<code style="color:#c555ff;">POST /api/Stock/ConfirmPurchase/{username}</code>***

    - **Input:** **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Confirms a stock purchase for the specified **`username`**. Handles potential exceptions related to settlement and stock API connections, returning appropriate status codes and messages.

    ***<code style="color:#c555ff;">POST /api/Stock/ConfirmSale/{username}</code>***

    - **Input:** **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Confirms a stock sale for the specified **`username`**. Handles potential exceptions related to settlement and stock API connections, returning appropriate status codes and messages.

    ***<code style="color:#5295ff;">GET /api/Stock/GetStock/{stockId}</code>***

    - **Input:** **`stockId`** as a route parameter.
    - **Description:** Retrieves information about a stock based on the provided **`stockId`**. Returns a 200 OK status with the stock information if the stock exists; otherwise, returns a 404 Not Found status.

    ***<code style="color:#5295ff;">GET /api/Stock/GetStocksInWallet/{walletId}</code>***

    - **Input:** **`walletId`** as a route parameter.
    - **Description:** Retrieves a collection of stocks associated with the specified **`walletId`**. Returns a 200 OK status with the list of stocks if the wallet is found; otherwise, returns a 404 Not Found status.

    ***<code style="color:#f09500;">PUT /api/Stock/AddStockForPurchase/{username}</code>***

    - **Input:** **`StockActionDTO`** in the request body, **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Adds a stock for purchase for the specified **`username`**. Handles potential exceptions related to the stock API connection, returning appropriate status codes and messages.

    ***<code style="color:#f09500;">PUT /api/Stock/AddStockForSale/{username}</code>***

    - **Input:** **`StockActionDTO`** in the request body, **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Adds a stock for sale for the specified **`username`**. Returns a response indicating the result of the stock addition operation.

* ### Transaction

    ***<code style="color:#c555ff;">POST /api/Transaction/CompleteTransaction</code>***

    - **Input:** **`FinalizeTransactionDTO`** in the request body.
    - **Description:** Completes a transaction based on the information provided in the **`FinalizeTransactionDTO`**. Returns a 200 OK status if the transaction is completed successfully; otherwise, returns a 404 Not Found status with an appropriate error message if the user wallet is not found.
    - **Note:** This route is only used by the settlement microservice.

    ***<code style="color:#5295ff;">GET /api/Transaction/GetTransactionsByUsername/{username}</code>***

    - **Input:** **`username`** as a route parameter. The **`username`** parameter could be the authenticated username passed from the gateway.
    - **Description:** Retrieves a list of transactions associated with the specified **`username`**. Returns a 200 OK status with the list of transactions if the user exists; otherwise, returns an empty list.

    ***<code style="color:#5295ff;">GET /api/Transaction/GetTransactionsByWallet/{walletId}</code>***

    - **Input:** **`walletId`** as a route parameter.
    - **Description:** Retrieves a list of transactions associated with the specified **`walletId`**. Returns a 200 OK status with the list of transactions if the wallet exists; otherwise, returns an empty list.

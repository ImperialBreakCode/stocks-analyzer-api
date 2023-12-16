# Stocks Analyzer Api

## Contributors and microservices

## Routes

### 1. Gateway
* .../Analyzer/CurrentProfitability
* .../Analyzer/PercentageChange
* .../Analyzer/PortfolioRisk
* .../Analyzer/DailyProfitabilityChanges

* .../Account/Register
* .../Account/Login
* .../Account/deposit
* .../Account/CreateWallet
* .../Account/UserInformation/{user}

* .../Stocks/BuyStock
* .../Stocks/Finalize
* .../Stocks/TrackStock

* .../StockInfo/GetCurrentStocks
* .../StockInfo/GetWeeklyStocks
* .../StockInfo/GetMonthlyStocks

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
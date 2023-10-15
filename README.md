# Stocks Analyzer Api

## Microservice Architecture

## Contributors

## Routes (work in progress)

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

### 2. Accounts
* .../User/register
* .../User/login
* .../User/UserInformation
* .../Wallet/Deposit
* .../Wallet/CreateWallet
* .../Stock/BuyStock
* .../Stock/Finalize
* .../Stock/TrackStock

### 3. Analyzer
* .../PortfolioSummary
* .../CurrentProfitability
* .../PercentageChange
* .../PortfolioRisk
* .../DailyProfitabilityChanges

### 4. StockAPI
* .../GetCurrentStocks
* .../GetWeeklyStocks
* .../GetMonthlyStocks

### 5. Settlement
* .../BuyStocks
* .../SellStocks
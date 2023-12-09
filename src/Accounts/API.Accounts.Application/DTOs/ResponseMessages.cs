namespace API.Accounts.Application.DTOs
{
    public static class ResponseMessages
    {
        // Auth
        public const string AuthSuccess = "Authentication is successful";
        public const string AuthPassIncorrect = "Incorrect password.";

        // User
        public const string UserNameAlreadyExists = "User with such username already exists.";
        public const string UserEmailAlreadyExists = "User with such email already exists.";
        public const string UserNotFound = "User not found";
        public const string UserUpdatedSuccessfully = "The user is updated successfully";

        // Wallet
        public const string CannotDepositWithCurrencyType = "Cannot deposit with the given currency type";
        public const string WalletNotFound = "Wallet does not exist.";
        public const string WalletRestricted = "Wallet restricted for actions such as purchasing or saling stocks";
        public const string WalletCreated = "Wallet created.";
        public const string WalletAlreadyExists = "The user already has a wallet.";
        public const string NotEnoughBalance = "Not enough balance.";
        public const string WalletDeletedSuccessfully = "The wallet is deleted successfully";

        // Stocks
        public const string NoStocksAddedForPurchaseSale = "No stocks added for transaction processing";
        public const string StockNotFoundInWallet = "This wallet does not contain the desired stock.";
        public const string StockActionSuccessfull = "Stock added for transaction processing successfully.";
        public const string TransactionSendForProccessing = "Transaction is send for proccessing.";
        public const string StockNotEnoughStocksToSale = "Not enough stocks to sale.";

        // Exceptions
        public const string ProblemWithSettlementService = "Could not send transactions to the settlement service.";
    }
}

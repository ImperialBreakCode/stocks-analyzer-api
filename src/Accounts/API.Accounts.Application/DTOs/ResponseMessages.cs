namespace API.Accounts.Application.DTOs
{
    public static class ResponseMessages
    {
        public const string AuthSuccess = "Authentication is successful";
        public const string AuthUserNotFound = "User is not found.";
        public const string AuthPassIncorrect = "Incorrect password.";

        public const string UserAlreadyExists = "User with such username already exists.";
        public const string UserNotFound = "User {0} not found";

        public const string CannotDepositWithCurrencyType = "Cannot deposit with the given currency type";
        public const string WalletNotFound = "Wallet does not exist.";
        public const string WalletRestricted = "Wallet restricted for actions such as purchasing or saling stocks";
        public const string WalletCreated = "Wallet created.";
        public const string WalletAlreadyExists = "The user already has a wallet.";
        public const string NotEnoughBalance = "Not enough balance.";

        public const string NoStocksAddedForPurchaseSale = "No stocks added for {0}";
        public const string StockNotFoundInWallet = "This wallet does not contain the desired stock.";
        public const string StockActionSuccessfull = "Stock added for {0} successfully.";
        public const string TransactionSendForProccessing = "Transaction is send for proccessing.";
        public const string StockNotEnoughStocksToSale = "Not enough stocks to sale.";
    }
}

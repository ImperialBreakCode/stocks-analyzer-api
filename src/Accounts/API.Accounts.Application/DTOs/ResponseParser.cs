namespace API.Accounts.Application.DTOs
{
    public enum ResponseType
    {
        NotFound,
        Unauthorized,
        Conflict,
        BadRequest,
        Success
    }

    public static class ResponseParser
    {
        public static ResponseType ParseResponseMessage(string message)
        {
            bool notFound = message == ResponseMessages.UserNotFound
                || message == ResponseMessages.WalletNotFound
                || message == ResponseMessages.StockNotFoundInWallet;

            if (notFound)
            {
                return ResponseType.NotFound;
            }

            if(message == ResponseMessages.AuthPassIncorrect)
            {
                return ResponseType.Unauthorized;
            }

            if(message == ResponseMessages.UserAlreadyExists || message == ResponseMessages.WalletAlreadyExists)
            {
                return ResponseType.Conflict;
            }

            bool badRequest = message == ResponseMessages.WalletRestricted
                || message == ResponseMessages.CannotDepositWithCurrencyType
                || message == ResponseMessages.NoStocksAddedForPurchaseSale
                || message == ResponseMessages.NotEnoughBalance
                || message == ResponseMessages.StockNotEnoughStocksToSale;

            if (badRequest)
            {
                return ResponseType.BadRequest;
            }

            return ResponseType.Success;
        }
    }
}

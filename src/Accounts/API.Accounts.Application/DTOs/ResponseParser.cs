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
            switch (message)
            {
                case ResponseMessages.UserNotFound:
                case ResponseMessages.WalletNotFound:
                case ResponseMessages.StockNotFoundInWallet:
                    return ResponseType.NotFound;

                case ResponseMessages.AuthPassIncorrect:
                    return ResponseType.Unauthorized;

                case ResponseMessages.UserNameAlreadyExists:
                case ResponseMessages.WalletAlreadyExists:
                case ResponseMessages.UserEmailAlreadyExists:
                    return ResponseType.Conflict;

                case ResponseMessages.WalletRestricted:
                case ResponseMessages.CannotDepositWithCurrencyType:
                case ResponseMessages.NoStocksAddedForPurchaseSale:
                case ResponseMessages.NotEnoughBalance:
                case ResponseMessages.StockNotEnoughStocksToSale:
                    return ResponseType.BadRequest;

                default:
                    return ResponseType.Success;
            }
        }
    }
}

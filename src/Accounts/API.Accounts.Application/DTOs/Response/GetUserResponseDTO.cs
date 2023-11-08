﻿namespace API.Accounts.Application.DTOs.Response
{
    public class GetUserResponseDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? WalletId { get; set; }
    }
}
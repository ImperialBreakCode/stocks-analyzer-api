﻿namespace API.Accounts.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public decimal Balance { get; set; }
        public string UserId { get; set; }
    }
}

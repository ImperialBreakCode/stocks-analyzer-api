    namespace API.Analyzer.Domain.DTOs
    {
        public class User
        {
            public string UserId { get; set; }
            public string Username { get; set; }
            public decimal Balance { get; set; }

        public User()
        {

        }
        public User(string userId, string name, decimal balance)
        {
                this.UserId = userId;
                this.Username = name;
                this.Balance = balance;
        }

        }
    }
﻿namespace API.Accounts.Application.DTOs.ExternalResponseDTOs
{
    public class StockApiResponseDTO
    {
        public string Symbol { get; set; }
        public string Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
    }
}

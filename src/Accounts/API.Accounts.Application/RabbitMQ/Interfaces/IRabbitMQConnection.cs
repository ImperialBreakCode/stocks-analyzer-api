﻿using RabbitMQ.Client;

namespace API.Accounts.Application.RabbitMQ.Interfaces
{
    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }
        void Connect(string hostName);
        IModel CreateChannel();
    }
}

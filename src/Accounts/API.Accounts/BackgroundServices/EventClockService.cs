﻿using API.Accounts.Application.EventClocks;
using API.Accounts.Application.RabbitMQ.Interfaces;
using API.Accounts.Application.Services.WalletService.Interfaces;
using API.Accounts.Application.Settings.UpdateHandlers;

namespace API.Accounts.BackgroundServices
{
    public class EventClockService : BackgroundService
    {
        private readonly IEventClock _eventClock;
        private readonly IDemoWalletDeleteHandler _deleteDemoWalletHandler;
        private readonly IWalletDeleteRabbitMQProducer _walletDeleteRabbitMQProducer;
        private readonly IAuthTokenGatewayNotifyer _secretKeyGatewayNotifyer;
        private readonly IRabbitMQSetupService _rabbitMQSetupService;

        public EventClockService(
            IEventClock eventClock,
            IDemoWalletDeleteHandler deleteDemoWalletHandler,
            IAuthTokenGatewayNotifyer secretKeyGatewayNotifyer,
            IRabbitMQSetupService rabbitMQSetupService,
            IWalletDeleteRabbitMQProducer walletDeleteRabbitMQProducer)
        {
            _eventClock = eventClock;
            _deleteDemoWalletHandler = deleteDemoWalletHandler;
            _secretKeyGatewayNotifyer = secretKeyGatewayNotifyer;
            _rabbitMQSetupService = rabbitMQSetupService;
            _walletDeleteRabbitMQProducer = walletDeleteRabbitMQProducer;

            RegisterHandlers();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventClock.RunClock(stoppingToken);
        }

        private void RegisterHandlers()
        {
            _eventClock.RegisterClockHandler(_deleteDemoWalletHandler.DeleteWallet);
            _eventClock.RegisterClockHandler(_secretKeyGatewayNotifyer.NotifyGateway);
            _eventClock.RegisterClockHandler(_rabbitMQSetupService.DelayedSetupHandler);
            _eventClock.RegisterClockHandler(_walletDeleteRabbitMQProducer.SendWaitingWalletIdsForDeletion);
        }
    }
}

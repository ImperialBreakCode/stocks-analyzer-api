﻿using API.Accounts.Application.Auth.PasswordManager;
using API.Accounts.Application.Auth.TokenManager;
using API.Accounts.Application.Data;
using API.Accounts.Application.Data.ExchangeRates;
using API.Accounts.Application.Data.StocksData;
using API.Accounts.Application.Data.AccountsDataSeeder;
using API.Accounts.Application.EventClocks;
using API.Accounts.Application.HttpClientService;
using API.Accounts.Application.RabbitMQ;
using API.Accounts.Application.RabbitMQ.Interfaces;
using API.Accounts.Application.Services.StockService;
using API.Accounts.Application.Services.StockService.SubServiceInterfaces;
using API.Accounts.Application.Services.StockService.SubServices;
using API.Accounts.Application.Services.TransactionService;
using API.Accounts.Application.Services.UserService;
using API.Accounts.Application.Services.UserService.UserRankService;
using API.Accounts.Application.Services.UserService.EmailService;
using API.Accounts.Application.Services.WalletService;
using API.Accounts.Application.Services.WalletService.Interfaces;
using API.Accounts.Application.Settings;
using API.Accounts.Application.Settings.GatewayAuthSettingsSender;
using API.Accounts.Application.Settings.GatewaySettingsSender;
using API.Accounts.Application.Settings.UpdateHandlers;
using API.Accounts.Infrastructure.DbManager;
using API.Accounts.Domain.Interfaces.DbManager;
using Microsoft.Extensions.DependencyInjection;


namespace API.Accounts.Application
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddRabbitMQServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQSetupService, RabbitMQSetupService>();
            services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();

            services.AddTransient<ITransactionSaleHandler, TransactionSaleHandler>();
            services.AddTransient<IWalletDeleteRabbitMQProducer, WalletDeleteRabbitMQProducer>();
            services.AddSingleton<IWaitingDeletedWalletIdsList, WaitingDeletedWalletIdsList>();

            return services;
        }

        public static IServiceCollection AddApplicationData(this IServiceCollection services)
        {
            services.AddTransient<IStocksData, StocksData>();
            services.AddTransient<IExchangeRatesData, ExchangeRateDataMockup>();

            services.AddTransient<IAccountsDataSeeder, AccountDataSeeder>();
            services.AddTransient<IAccountsData, AccountData>();
            services.AddTransient<IAccountsDbManager, AccountsDbManager>();

            return services;
        }

        public static IServiceCollection AddHttpClientServices<T>(this IServiceCollection services)
            where T : class, IHttpService
        {
            services.AddTransient<IHttpClientRoutes, HttpClientRoutes>();
            services.AddTransient<IHttpService, T>();

            return services;
        }

        public static IServiceCollection AddAccountAuthentication(this IServiceCollection services)
        {
            services.AddTransient<IPasswordManager, PasswordManager>();
            services.AddTransient<ITokenManager, TokenManager>();

            return services;
        }

        public static IServiceCollection AddApplicationEventClock(this IServiceCollection services)
        {
            services.AddSingleton<IEventClock, EventClock>();
            services.AddSingleton<IDemoWalletDeleteHandler, DemoWalletDeleteHandler>();
            services.AddSingleton<IAuthTokenGatewayNotifyer, AuthTokenGatewayNotifyer>();

            return services;
        }

        public static IServiceCollection AddAccountServices(this IServiceCollection services)
        {
            AddStockService(services);
            AddUserService(services);

            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<ITransactionService, TransactionService>();

            return services;
        }

        public static IServiceCollection AddSettings<TSettingsAdapter>(this IServiceCollection services)
            where TSettingsAdapter : class, IAccountsSettingsManager
        {
            services.AddSingleton<IAccountsSettingsManager, TSettingsAdapter>();
            services.AddTransient<ISocketGatewaySettingsSender, SocketGatewaySettingsSender>();

            return services;
        }

        private static void AddStockService(IServiceCollection services)
        {
            services.AddTransient<IStockService, StockService>();
            services.AddTransient<IStockActionExecuter, StockActionExecuter>();
            services.AddTransient<IStockActionFinalizer, StockActionFinalizer>();
            services.AddTransient<IStockActionManager, StockActionManager>();
        }

        private static void AddUserService(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRankManager, UserRankManager>();
            services.AddTransient<IEmailConfirmation, EmailConfirmation>();
        }
    }
}

﻿using Microsoft.Extensions.DependencyInjection;
using MyPasswordManager.Core.Domain.Login;
using MyPasswordManager.Core.Domain.Secrets;
using MyPasswordManager.Infrastructure.CosmosDb;

namespace MyPasswordManager.Infrastructure.Extensions
{
    public static class RegisterDI
    {
        public static IServiceCollection RegisterCosmosDbRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAuthenticate, SecretsRepository>();
            serviceCollection.AddSingleton<ISecret, SecretsRepository>();
            return serviceCollection;
        }
    }
}

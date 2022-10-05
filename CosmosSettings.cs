using System;
using Microsoft.Azure.Cosmos;

namespace oh5.serverless
{
    public static class CosmosSettings
    {
        // Pulls the connection string value from the environment variables
        // In Azure this pulls from the Key Vault, using the secret 'cosmosdbconnectionstring'
        public static string ConnectionString
        {
            get
            {
                string connStr = Environment.GetEnvironmentVariable("ConnectionStrings:COSMOSDBCONSTR");
                return connStr;
            }
        }

        // Auto-convert between JSON camelCase and C# PascalCase
        public static CosmosClientOptions Options
        {
            get
            {
                CosmosClientOptions options = new CosmosClientOptions
                {
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                };
                return options;
            }
        }

        public const string DatabaseId = "IceCreamDatabase";
        public const string ContainerId = "IceCreamRatings";
        public const string PartitionKeyPath = "/id";
        public const string TimestampFormat = "yyyy-MM-dd hh:mm:ssZ";
    }
}
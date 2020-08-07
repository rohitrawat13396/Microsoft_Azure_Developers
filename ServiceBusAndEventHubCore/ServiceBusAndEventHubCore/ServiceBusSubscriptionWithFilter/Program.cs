using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Threading.Tasks;

namespace ServiceBusSubscriptionWithFilter
{
    class Program
    {
        const string ServiceBusConnectionString = "<namespace connection string>";
        const string TopicName = "<topic>";
        const string SubscriptionName = "<subscription>";

        static void Main(string[] args)
        {
            CreateTopicSubscriptions().GetAwaiter().GetResult();

            CreateSubscriptionFilters().GetAwaiter().GetResult();
        }

        private static async Task CreateTopicSubscriptions()
        {
            var client = new ManagementClient(ServiceBusConnectionString);

            var subscriptions = client.GetSubscriptionsAsync(TopicName).Result;

            if (!await client.SubscriptionExistsAsync(TopicName, SubscriptionName))
            {
                await client.CreateSubscriptionAsync(new SubscriptionDescription(TopicName, SubscriptionName));
            }
        }

        private static async Task CreateSubscriptionFilters()
        {
            var client = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

            var rules = await client.GetRulesAsync();

            // Remove default filter
            foreach (var rule in rules)
            {
                await client.RemoveRuleAsync(rule.Name);
            }

            // Add  InformCourier rule
            var filter = new SqlFilter("ProcessWarehouse = 1");
            await client.AddRuleAsync("ProcessWarehouse", filter);
        }
    }
}

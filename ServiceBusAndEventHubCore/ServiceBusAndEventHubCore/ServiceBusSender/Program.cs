using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusSender
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://servicebusrawat.servicebus.windows.net/;SharedAccessKeyName=p1;SharedAccessKey=7nrp+/deGBB+TgFumz2KvGsMY0ox9z3LPKPvcxO+SoU=";
        const string TopicName = "firsttopic";
        static ITopicClient topicClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            const int numberOfMessages = 10;
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send messages.
            await SendMessagesAsync(numberOfMessages);

            await topicClient.CloseAsync();

            Console.ReadLine();           
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 21; i < numberOfMessagesToSend + 20; i++)
                {
                    // Create a new message to send to the topic
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Set the message id (helps in duplicate detection)
                    message.MessageId = "OP/00" + i;

                    // Set the user properties - this helps in filter evaluation
                    //message.UserProperties.Add("OrderPlaced", 1);
                    message.UserProperties.Add("subs3warehouse", 1);
                    //message.UserProperties.Add("InformCourier", 1);

                    // Write the body of the message to the console
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the topic
                    await topicClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}

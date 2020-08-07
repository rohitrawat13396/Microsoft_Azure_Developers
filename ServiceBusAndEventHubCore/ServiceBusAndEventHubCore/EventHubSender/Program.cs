using Microsoft.Azure.EventHubs;
using System;
using System.Text;
using System.Threading;

namespace EventHubSender
{
    class Program
    {
        static string eventHubName = "<event hub name>";
        static string connectionString = "<namespace connection string>";        

        static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to start now");
            Console.ReadLine();

            SendingRandomMessages();

            Console.ReadKey();
        }

        static void SendingRandomMessages()
        {
            string finalConnString = connectionString + ";EntityPath=" + eventHubName;

            var eventHubClient = EventHubClient.CreateFromConnectionString(finalConnString);
            while (true)
            {
                try
                {
                    var message = Guid.NewGuid().ToString();
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, message);

                    EventData eventData = new EventData(Encoding.UTF8.GetBytes(message))
                    {
                        // Specify partition key - optional
                        // PartitionKey = "IN-DELHI"
                    };

                    eventHubClient.SendAsync(eventData);
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                    Console.ResetColor();
                }

                Thread.Sleep(200);
            }
        }
    }
}

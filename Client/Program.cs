using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrleanGrains;
using OrleanInterface;
using Orleans;
using Orleans.Configuration;

namespace Client
{
    public class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            await Task.Delay(2000);//为了确保能连接Hosts先等待个2s
            
            try
            {
                await NoOrleans();
                using (var client = await ConnectClient())
                {
                    await Orleans(client);
                   

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }

            

            Console.ReadKey();
            return 1;
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                //.ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        /// <summary>
        ///     通过奥尔良来转账
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task Orleans(IClusterClient client)
        {
            var account = client.GetGrain<IAccount>(0);
            var taskList = new List<Task>();
            Parallel.For(0, 100, i =>
            {
                taskList.Add(account.TransferAsync(i));
            });
            await Task.WhenAll(taskList);
            Console.WriteLine("奥尔良版 当前资产 " + await account.AssertAsync());//恒定4950(正确结果)

        }

        /// <summary>
        ///     不通过奥尔良直接new个实例出来执行同样操作
        /// </summary>
        /// <returns></returns>
        private static async Task NoOrleans()
        {
            var account = new Account();
            var taskList = new List<Task>();
            Parallel.For(0, 100, i =>
            {
                taskList.Add(account.TransferAsync(i));
            });
            await Task.WhenAll(taskList);
            Console.WriteLine("原始版 当前资产 " + await account.AssertAsync());//实践下来此结果每次执行都不确定
        }
    }
}

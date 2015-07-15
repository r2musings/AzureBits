using System.Configuration;
using Microsoft.Azure.WebJobs;

namespace AzureBits.ImageMessageWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        public static void Main()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;

            var config = new JobHostConfiguration
            {
                DashboardConnectionString = connectionString,
                StorageConnectionString = connectionString
            };
            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}

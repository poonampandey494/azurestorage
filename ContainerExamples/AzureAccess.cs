using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExamples
{
   public class AzureAccess
    {
       static CloudStorageAccount cloudStorageAccount = null;

        static AzureAccess()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection")
                );
        }
        public static string GetSharedAccessSignature()
        {
            var sharedAccessAccountPolicy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.Write,
                Services = SharedAccessAccountServices.Blob,
                ResourceTypes = SharedAccessAccountResourceTypes.Object,
                SharedAccessStartTime = DateTime.Now,
                SharedAccessExpiryTime = DateTime.Now.AddHours(1),
                Protocols = SharedAccessProtocol.HttpsOnly                
            };

            return cloudStorageAccount.GetSharedAccessSignature(sharedAccessAccountPolicy);
        }
    }
}

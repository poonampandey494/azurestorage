using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using QdnAzureStorageModels;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace QdnAzureStorageLibrary
{
    public class BlobStorage
    {
       static CloudStorageAccount cloudStorageAccount = null;
        public static void Setup()
        {
           

            cloudStorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection")
                // "DefaultEndpointsProtocol=https;AccountName=poonamstorage;AccountKey=vGD4NMYIL2sA+eVaJjYQfZk7oACo4q/OwUcJsbPjlGwJFbI+EHoVAqKHF4+AGiDQgFRx2fgz9tO1xTF4ejvhKQ==;EndpointSuffix=core.windows.net"
                );
        }


        public static void CreateSharedAccessPolicy(string storagePath,string policyName)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(storagePath);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var permissions = container.GetPermissions();

            
            //Check if policyName already  exists
            var sharedAccessPolicies = permissions.SharedAccessPolicies.SingleOrDefault(x => x.Key == policyName);
            if (sharedAccessPolicies.Key == null)
            {
                var sharedPolicy = new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTimeOffset.UtcNow,
                    SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(72),
                    Permissions = SharedAccessBlobPermissions.Add | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read
                };
                permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
                container.SetPermissions(permissions);
            }

        }



        public static string GetSharedAccessPolicyToken(string storagePath, string policyName)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(storagePath);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var permissions = container.GetPermissions();
            return container.GetSharedAccessSignature(null, policyName);
        }

        public static void UploadFile(string filePath, string storagePath, string fileName)
        {
            



            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            
            
            var container = blobClient.GetContainerReference(storagePath);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            //var ImagesPath = container.ListBlobs();
            //var listImagesPath = new List<string>();
            //foreach (var imagePath in ImagesPath)
            //{
            //    listImagesPath.Add(imagePath.Uri.ToString());
            //}

            
             container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var blockBlob = container.GetBlockBlobReference(fileName);
            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                blockBlob.UploadFromStream(fileStream);
            }




        }


        public void AddCrossRules(string storagePath)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
         var serviceProperties = blobClient.GetServiceProperties();
            var container = blobClient.GetContainerReference(storagePath);
          //  var serviceProperties = container.;
            var cors = new CorsRule();
            cors.AllowedOrigins.Add("*");
            cors.AllowedMethods = CorsHttpMethods.Get;
            cors.MaxAgeInSeconds = 900;
           // serviceProperties.
            serviceProperties.Cors.CorsRules.Add(cors);
           // blobClient.SetServiceProperties(serviceProperties);
        }
        public static  List<BlobModel> GetList(string storagePath)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();


            var container = blobClient.GetContainerReference(storagePath);

            var lst = container.ListBlobs().ToList();
            List<BlobModel> b = new List<BlobModel>();


            foreach(var item in lst)
            { 
                b.Add(new BlobModel { Name = ((CloudBlob)item).Name, BlobUri = item.Uri });
            }
                
                //.Select(x=>new BlobModel{ Name=x.Container.b);

            return b;
        }
    }
}

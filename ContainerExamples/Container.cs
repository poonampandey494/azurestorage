using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExamples
{
    public class Container
    {
        CloudStorageAccount cloudStorageAccount = null;
        public void SetUp()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection")
                );
        }

        public void AddImages(String path, String fileName)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + fileName);
            using (var fileStream = System.IO.File.OpenRead(path))
            {
                blockBlob.UploadFromStream(fileStream);
            }

        }

        public List<string> ListImagesPath()
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var ImagesPath = container.ListBlobs();
            var listImagesPath = new List<string>();
            foreach (var imagePath in ImagesPath)
            {
                listImagesPath.Add(imagePath.Uri.ToString());
            }
            return listImagesPath;
        }

       
        public void DownLoadImage(string AzurePath, string localPath)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var blockBlob = container.GetBlockBlobReference(AzurePath);
            using (var fileStream = System.IO.File.OpenWrite(localPath))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }

        public void CopyImage(string AzurePath, string newName)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var blockBlob = container.GetBlockBlobReference(AzurePath);
            var blockBlobCopy = container.GetBlockBlobReference(newName);
            var copyAsncCallBack = new AsyncCallback(x => Console.WriteLine("Copy done"));
            blockBlobCopy.BeginStartCopy(blockBlob.Uri, copyAsncCallBack, null);
        }


        public void AddImagesInHierarchies(string path,string HierarchyName, string fileName)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var blockBlob = container.GetBlockBlobReference(HierarchyName+Guid.NewGuid().ToString() + fileName);
            using (var fileStream = System.IO.File.OpenRead(path))
            {
                blockBlob.UploadFromStream(fileStream);
            }

        }

        public void CreateSharedAccessPolicy(string policyName)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var permissions = container.GetPermissions();
            //Check if policyName already  exists
            var sharedAccessPolicies = permissions.SharedAccessPolicies.SingleOrDefault(x => x.Key == policyName);
            if (sharedAccessPolicies.Key == null)
            {
               var sharedPolicy = new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read
                };               
                permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
                container.SetPermissions(permissions);
            }
             
        }

        public void AddCrossRules()
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var serviceProperties = blobClient.GetServiceProperties();
            var cors = new CorsRule();
            cors.AllowedOrigins.Add("*");
            cors.AllowedMethods = CorsHttpMethods.Get;
            cors.MaxAgeInSeconds = 900;
            serviceProperties.Cors.CorsRules.Add(cors);
            blobClient.SetServiceProperties(serviceProperties);
        }
        public string GetSharedAccessPolicyToken(string policyName)
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var permissions = container.GetPermissions();
           return container.GetSharedAccessSignature(null, policyName);
        }
        public List<string> ListImagesInHierarchies()
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            var ImagesPath = container.ListBlobs(useFlatBlobListing: true);
            var listImagesPath = new List<string>();
            foreach (var imagePath in ImagesPath)
            {
                listImagesPath.Add(imagePath.Uri.ToString());
            }
            return listImagesPath;
        }

        public void SetMetaData()
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            container.Metadata.Clear();
            container.Metadata.Add("user", "abhay");
            container.Metadata.Add("role", "admin");
            container.Metadata["lastUpdatedBy"] = "abhay.v";
            container.SetMetadata();
        }


        public Dictionary<string, string> GetMetaData()
        {
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("containerbycsharpcode");
            container.FetchAttributes();
            var metaData = new Dictionary<string, string>();
            foreach(var item in container.Metadata)
            {
                metaData.Add(item.Key, item.Value);
            }

            return metaData;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using QdnAzureStorageModels;

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

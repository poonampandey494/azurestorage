using QdnAzureStorageLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storage
{
    class Program
    {
        static void Main(string[] args)
        {

            BlobStorage.Setup();
          //  BlobStorage.UploadFile(@"C:\projects\azuremvc2\azuremvc\imageicon.png", "imagesstoresd", "xyz2.png");
            BlobStorage.GetList("imagesstoresd");

        }
}
}

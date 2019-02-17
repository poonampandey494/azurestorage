using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QdnAzureStorageModels
{
  public  class BlobModel
    {

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Uri BlobUri { get; set; }



        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...


    }
}

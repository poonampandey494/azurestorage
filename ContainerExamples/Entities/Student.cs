using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExamples.Entities
{
    class Student : TableEntity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

        public Student()
        {

        }
        public Student(string email, string name, string subject)
        {
            this.Email = email;
            this.Name = name;
            this.Subject = subject;
            this.PartitionKey = subject;
            this.RowKey = email;
        }
    }
}

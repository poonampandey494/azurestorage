using ContainerExamples.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExamples
{
    class Program
    {


        static void Main_23(string[] args)
        {
            var container = new Container();
            container.SetUp();
            container.AddCrossRules();
            Console.ReadKey();
        }



        static void Main(string[] args)
        {

            var container = new Container();
            container.SetUp();
            var listImagesPath = container.ListImagesInHierarchies();
            Console.WriteLine("Image List");
            foreach (var path in listImagesPath)
            {
                Console.WriteLine(path);
            }
            Console.WriteLine("Sas Token");
            Console.WriteLine(container.GetSharedAccessPolicyToken("containerAccesspolicy"));
            Console.ReadKey();           
            Console.ReadKey();
        }
        static void Main_Read_Delete(string[] args)
        {
            var messageQueue = new MessageQueue();
            messageQueue.SetUp();
            // messageQueue.CreateQueue("message-tasks");
            var message = messageQueue.ReadAndDelete("message-tasks");
            Console.ReadKey();
        }


        static void Main_Add_message(string[] args)
        {
            var messageQueue = new MessageQueue();
            messageQueue.SetUp();
            messageQueue.CreateQueue("message-tasks");
        }
        static void Main_Table_Batch(string[] args)
        {




            var table = new Table();
            table.SetUp();
            var student = table.GetRow<Student>(nameof(Student), "Azure", "firstBatch1@gmail.com");

            table.Delete<Student>(nameof(Student), student);
            student = table.GetRow<Student>(nameof(Student), "Azure", "firstBatch2@gmail.com");

            table.Delete<Student>(nameof(Student), student);
            student = table.GetRow<Student>(nameof(Student), "Azure", "firstBatch3@gmail.com");

            table.Delete<Student>(nameof(Student), student);
            var studentList = table.GetAllRows<Student>(nameof(Student), "Azure");


            Console.WriteLine("Before Batch Operations");
            foreach (var stu in studentList)
            {
                Console.WriteLine(stu.Name);
            }

            List<Student> objectDataList = new List<Student>();
            student = new Student("firstBatch1@gmail.com", "firstBatch1", "Azure");
            objectDataList.Add(student);
            student = new Student("firstBatch2@gmail.com", "firstBatch2", "Azure");
            objectDataList.Add(student);
            student = new Student("firstBatch3@gmail.com", "firstBatch3", "Azure");
            objectDataList.Add(student);
            table.BatchInsert<Student>(nameof(Student), objectDataList);

            Console.WriteLine("After Batch Operations");
            studentList = table.GetAllRows<Student>(nameof(Student), "Azure");
            foreach (var stu in studentList)
            {
                Console.WriteLine(stu.Name);
            }


            //Console.WriteLine(student.Name);
            Console.ReadKey();
        }


        static void Main_Delete(string[] args)
        {
            var table = new Table();
            table.SetUp();
            var studentList = table.GetAllRows<Student>(nameof(Student), "Azure");
            Console.WriteLine("Before Delete");
            foreach (var stu in studentList)
            {
                Console.WriteLine(stu.Name);
            }
            //var student = new Student("QuickITDotnet@gmail.com", "QuickITDotnet", "Azure");
            //table.AddRow<Student>(nameof(student), student);
            var student = table.GetRow<Student>(nameof(Student), "Azure", "QuickITDotnet@gmail.com");

            table.Delete<Student>(nameof(Student), student);
            studentList = table.GetAllRows<Student>(nameof(Student), "Azure");
            Console.WriteLine("After Delete");
            foreach (var stu in studentList)
            {
                Console.WriteLine(stu.Name);
            }

            //Console.WriteLine(student.Name);
            Console.ReadKey();
        }
        static void Main_GET_ALL_ROWS(string[] args)
        {
            var table = new Table();
            table.SetUp();
            //var student = new Student("amruta@gmail.com", "amruta", "Azure");
            // table.AddRow<Student>(nameof(student), student);
            var studentList = table.GetAllRows<Student>(nameof(Student), "Azure");
            foreach (var stu in studentList)
            {
                Console.WriteLine(stu.Name);
            }
            Console.ReadKey();
        }


        static void Main_GET_ROW(string[] args)
        {
            var table = new Table();
            table.SetUp();
            //var student = new Student("QuickITDotnet@gmail.com", "QuickITDotnet", "Azure");
            //table.AddRow<Student>(nameof(student), student);
            var student = table.GetRow<Student>(nameof(Student), "Azure", "QuickITDotnet@gmail.com");
            Console.WriteLine(student.Name);
            Console.ReadKey();
        }

        static void Main_Add_ROW(string[] args)
        {
            var table = new Table();
            table.SetUp();
            var student = new Student("QuickITDotnet@gmail.com", "QuickITDotnet", "Azure");
            table.AddRow<Student>(nameof(student), student);
            Console.ReadKey();
        }



        static void Main_Add_Table(string[] args)
        {
            var table = new Table();
            table.SetUp();
            table.AddTable("Student");
            var student = new Student("QuickITDotnet@gmail.com", "QuickITDotnet", "Azure");
            Console.ReadKey();
        }


        static void Main_Container_For_Get_MetaData(string[] args)
        {
            Container container = new Container();
            container.SetUp();
            var metaData = container.GetMetaData();
            foreach (var item in metaData)
            {
                Console.WriteLine($"{item.Key}:{item.Value}");
            }
            Console.ReadKey();
        }


        static void Mainold(string[] args)
        {
            Container container = new Container();
            container.SetUp();
            //container.AddImages(@"C:\Users\abhayvel\OneDrive\Images\Azure.jpg", "Azure.jpg"); //Code to Add Image
            //List<string> listImagesPath = container.ListImagesPath();
            //foreach(var path in listImagesPath)
            //{
            //    Console.WriteLine(path);
            //}
            //var UlrsPart = listImagesPath[0].Split('/');
            //var imageName = UlrsPart[UlrsPart.Length - 1];
            //var imageNewName = "Azure" + Guid.NewGuid().ToString() + ".jpg";
            ////container.DownLoadImage(imageName, @"C:\Users\abhayvel\OneDrive\Images\download\Azure.jpg");
            //container.CopyImage(imageName, imageNewName);

            //container.AddImagesInHierarchies(@"C:\Users\abhayvel\OneDrive\Images\Azure.jpg", "hierarchy-folder/", "Azure.jpg");
            // container.SetMetaData();
            var metaData = container.GetMetaData();
            foreach (var item in metaData)
            {
                Console.WriteLine($"{item.Key}:{item.Value}");
            }
            Console.ReadKey();
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
namespace dependency_injection
{
    interface IClassB
    {
        public void ActionB();
    }
    interface IClassC
    {
        public void ActionC();
    }

    class ClassC : IClassC
    {
        public ClassC() => Console.WriteLine("ClassC is created");
        public void ActionC() => Console.WriteLine("Action in ClassC");
    }
    class ClassC1 : IClassC
    {
        public ClassC1() => Console.WriteLine("ClassC1 is created");
        public void ActionC() => Console.WriteLine("Action in ClassC1");
    }

    class ClassB : IClassB
    {
        IClassC c_dependency;
        public ClassB(IClassC classc)
        {
            c_dependency = classc;
            Console.WriteLine("ClassB is created");
        }
        public void ActionB()
        {
            Console.WriteLine("Action in ClassB");
            c_dependency.ActionC();
        }
    }
    class ClassB2 : IClassB
    {
        IClassC c_dependency;
        string message;
        public ClassB2(IClassC classc, string mgs)
        {
            c_dependency = classc;
            message = mgs;
            Console.WriteLine("ClassB2 is created");
        }
        public void ActionB()
        {
            Console.WriteLine(message);
            c_dependency.ActionC();
        }
    }


    class ClassA
    {
        IClassB b_dependency;
        public ClassA(IClassB classb)
        {
            b_dependency = classb;
            Console.WriteLine("ClassA is created");
        }
        public void ActionA()
        {
            Console.WriteLine("Action in ClassA");
            b_dependency.ActionB();
        }
    }
    class Program
    {
        public static IClassB CreateB2(IServiceProvider provider)
        {
            var b2 = new ClassB2(provider.GetService<IClassC>(), "Hello from ClassB2");
            return b2;
        }
        public class MyServiceOptions
        {
            public string data1 { get; set; }
            public int data2 { get; set; }
        }

        public class MyService
        {
            public string data1 { get; set; }
            public int data2 { get; set; }

            public MyService(IOptions<MyServiceOptions> options)
            {
                var _options = options.Value;   
                data1 = _options.data1;
                data2 = _options.data2;
            }
            public void PrintData()
            {
                Console.WriteLine($"Data1: {data1}, Data2: {data2}");
            }   
        }

        static void Main(string[] args)
        {

            IConfigurationRoot configurationRoot;

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            var path = @"D:\C#\DependencyInjection\DependencyInjection\cauhinh.json";
            builder.AddJsonFile(path, optional: false, reloadOnChange: true);


            configurationRoot = builder.Build();
            var sectionMyServiceOptions = configurationRoot.GetSection("MyServiceOptions");









            var service = new ServiceCollection();

            service.AddSingleton<MyService>();
            service.Configure<MyServiceOptions>(sectionMyServiceOptions);

            var provider = service.BuildServiceProvider();


            var myservice = provider.GetService<MyService>();
            myservice.PrintData();


            //service.AddSingleton<ClassA, ClassA>();
            //service.AddSingleton<IClassB>(CreateB2);
            //service.AddSingleton<IClassC,ClassC>();
            //a.ActionA();
        }
    }
}
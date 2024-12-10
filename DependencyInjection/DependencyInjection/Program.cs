using Microsoft.Extensions.DependencyInjection;
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
        static void Main(string[] args)
        {

            var service = new ServiceCollection();

            service.AddSingleton<IClassC,ClassC1>();

            var provider = service.BuildServiceProvider();


            var classC = provider.GetService<IClassC>();

            IClassB objectB = new ClassB(classC);
            ClassA objectA = new ClassA(objectB);

            objectA.ActionA();
        }
    }
}
using University.DotnetLabs.Lab5.DependencyInjectionLibrary.Interfaces;
using University.DotnetLabs.Lab5.DependencyInjectionLibrary.Configs;
using University.DotnetLabs.Lab5.DependencyInjectionLibrary.Tests.ClassesForTest;
using University.DotnetLabs.Lab5.DependencyInjectionLibrary.Services;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Tests;

[TestClass]
public class DependencyProviderTests
{

    [TestInitialize]
    public void TestInitialize()
    {

    }

    [TestMethod]
    public void Test_DependencyInjectionSimpleSingletons()
    {
        DependenciesConfiguration configuration = new DependenciesConfiguration();
        configuration.RegisterSingleton<ITestClass, TestClass1>();
        configuration.RegisterSingleton<TestClass1, TestClass2>();
        configuration.RegisterSingleton<TestClass2, TestClass2>();

        DependencyProvider provider = new DependencyProvider(configuration);

        object obj1 = provider.GetDependency<ITestClass>()!;
        object obj2 = provider.GetDependency<TestClass1>()!;
        object obj3 = provider.GetDependency<TestClass2>()!;

        Assert.IsNotNull(obj1);
        Assert.IsNotNull(obj2);
        Assert.IsNotNull(obj3);
        Assert.AreSame(obj2, obj3);
        Assert.AreNotSame(obj1, obj2);
        Assert.IsInstanceOfType(obj1, typeof(TestClass1));
        Assert.IsInstanceOfType(obj2, typeof(TestClass2));
    }

    [TestMethod]
    public void Test_DependencyInjectionSimpleTransient()
    {
        DependenciesConfiguration configuration = new DependenciesConfiguration();
        configuration.RegisterTransient<ITestClass, TestClass1>();
        configuration.RegisterTransient<TestClass1, TestClass2>();
        configuration.RegisterTransient<TestClass2, TestClass2>();

        DependencyProvider provider = new DependencyProvider(configuration);

        object obj1 = provider.GetDependency<ITestClass>()!;
        object obj2 = provider.GetDependency<TestClass1>()!;
        object obj3 = provider.GetDependency<TestClass2>()!;

        Assert.IsNotNull(obj1);
        Assert.IsNotNull(obj2);
        Assert.IsNotNull(obj3);
        Assert.AreNotSame(obj2, obj3);
        Assert.AreNotSame(obj1, obj2);
        Assert.IsInstanceOfType(obj1, typeof(TestClass1));
        Assert.IsInstanceOfType(obj2, typeof(TestClass2));
    }

    [TestMethod]
    public void Test_DependencyInjectionGenericSingleton()
    {
        DependenciesConfiguration configuration = new DependenciesConfiguration();
        configuration.RegisterSingleton<IGenericInterface<Type>, GenericClass1<Type>>();
        configuration.RegisterSingleton<GenericClass1<Type>, GenericClass2<Type>>();
        configuration.RegisterSingleton<GenericClass2<Type>, GenericClass2<Type>>();

        DependencyProvider provider = new DependencyProvider(configuration);

        object obj1 = provider.GetDependency<IGenericInterface<Type>>()!;
        object obj2 = provider.GetDependency<GenericClass1<Type>>()!;
        object obj3 = provider.GetDependency<GenericClass2<Type>>()!;

        Assert.IsNotNull(obj1);
        Assert.IsNotNull(obj2);
        Assert.IsNotNull(obj3);
        Assert.AreSame(obj2, obj3);
        Assert.AreNotSame(obj1, obj2);
        Assert.IsInstanceOfType(obj1, typeof(GenericClass1<Type>));
        Assert.IsInstanceOfType(obj2, typeof(GenericClass2<Type>));
    }

    public void Test_DependencyInjectionGenericTransient()
    {
        DependenciesConfiguration configuration = new DependenciesConfiguration();
        configuration.RegisterTransient<IGenericInterface<Type>, GenericClass1<Type>>();
        configuration.RegisterTransient<GenericClass1<Type>, GenericClass2<Type>>();
        configuration.RegisterTransient<GenericClass2<Type>, GenericClass2<Type>>();

        DependencyProvider provider = new DependencyProvider(configuration);

        object obj1 = provider.GetDependency<IGenericInterface<Type>>()!;
        object obj2 = provider.GetDependency<GenericClass1<Type>>()!;
        object obj3 = provider.GetDependency<GenericClass2<Type>>()!;

        Assert.IsNotNull(obj1);
        Assert.IsNotNull(obj2);
        Assert.IsNotNull(obj3);
        Assert.AreNotSame(obj2, obj3);
        Assert.AreNotSame(obj1, obj2);
        Assert.IsInstanceOfType(obj1, typeof(GenericClass1<Type>));
        Assert.IsInstanceOfType(obj2, typeof(GenericClass2<Type>));
    }

    [TestMethod]
    public void Test_IEnumerable()
    {
        DependenciesConfiguration configuration = new DependenciesConfiguration();
        configuration.RegisterSingleton<object, GenericClass1<Type>>();
        configuration.RegisterSingleton<object, GenericClass2<Type>>();
        configuration.RegisterSingleton<object, TestClass1>();
        configuration.RegisterSingleton<object, TestClass2>();
        configuration.RegisterSingleton<ITestClass, TestClass1>();

        DependencyProvider provider = new DependencyProvider(configuration);

        IEnumerable<object> enumerable = (IEnumerable<object>)provider.GetDependency<IEnumerable<object>>()!;
        int count = enumerable.Count();

        Assert.AreEqual(count, 4);
    }

    [TestMethod]
    public void Test_OpenGenerics()
    {
        DependenciesConfiguration configuration = new DependenciesConfiguration();
        configuration.RegisterSingleton(typeof(IService), typeof(Service1));
        configuration.RegisterSingleton(typeof(ServiceContainer<>), typeof(ServiceContainer<>));

        DependencyProvider provider = new DependencyProvider(configuration);

        ServiceContainer<IService> obj1 =  (ServiceContainer<IService>) provider.GetDependency<ServiceContainer<IService>>()!;
        IService service = obj1.PassedService;
        Assert.AreEqual(service.GetType(), typeof(Service1));
    }

}
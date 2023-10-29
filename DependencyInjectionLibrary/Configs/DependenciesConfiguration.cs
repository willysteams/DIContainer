using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Configs;

public class DependenciesConfiguration
{
    internal Dictionary<Type, ICollection<Type>> TransientDependences { get; private set; } = new();
    internal Dictionary<Type, ICollection<Type>> SingletonDependences { get; private set; } = new();
    
    public void RegisterTransient<TDependency, TImplementation>() 
        where TDependency : class
        where TImplementation:TDependency
    {
        RegisterTransient(typeof(TDependency), typeof(TImplementation));
    }
    public void RegisterTransient(Type dependency, Type implementation)
    {
        if (!TryRegisterTransient(dependency, implementation))
        {
            throw new ArgumentException("TDependency has already been registered");
        }
    }

    public void RegisterSingleton<TDependency, TImplementation>()
        where TDependency : class
        where TImplementation : TDependency
    {
        RegisterSingleton(typeof(TDependency), typeof(TImplementation));
    }

    public void RegisterSingleton(Type dependency, Type implementation)
    {
        if (!TryRegisterSingleton(dependency, implementation))
        {
            throw new ArgumentException("TDependency has already been registered");
        }
    }

    public bool TryRegisterTransient<TDependency, TImplementation>()
    where TDependency : class
    where TImplementation : TDependency
    {
        return TryRegisterTransient(typeof(TDependency), typeof(TImplementation));
    }

    public bool TryRegisterSingleton<TDependency, TImplementation>()
        where TDependency : class
        where TImplementation : TDependency
    {
        return TryRegisterSingleton(typeof(TDependency), typeof(TImplementation));
    }

    public bool TryRegisterTransient(Type depenpency, Type implementation)
    {
        ICollection<Type> implementations;
        if (!TransientDependences.TryGetValue(depenpency, out implementations!))
        {
            implementations = new List<Type>();
            TransientDependences.Add(depenpency, implementations);
        }
        implementations.Add(implementation);
        return true;
    }

    public bool TryRegisterSingleton(Type depenpency, Type implementation)
    {
        ICollection<Type> implementations;
        if (!SingletonDependences.TryGetValue(depenpency, out implementations!))
        {
            implementations = new List<Type>();
            SingletonDependences.Add(depenpency, implementations);
        }
        implementations.Add(implementation);
        return true;
    }
}

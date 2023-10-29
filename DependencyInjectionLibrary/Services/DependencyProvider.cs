using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using University.DotnetLabs.Lab5.DependencyInjectionLibrary.Configs;
using University.DotnetLabs.Lab5.DependencyInjectionLibrary.Interfaces;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Services;

public class DependencyProvider : IDependencyProvider
{ 
    internal DependenciesConfiguration Configs { get; private set; }
    private List<object> _singletons = new();
    private object _lock = new();
    
    public DependencyProvider(DependenciesConfiguration configuration)
    {
        DepenpenciesConfigurationValidator validator = new(configuration);
        if (!validator.Validate())
        {
            throw new ArgumentException(validator.Message);
        }
        Configs = configuration;
    }
    public object? GetDependency<T>() => GetDependency(typeof(T));

    public object? GetDependency(Type dependencyType)
    {
        if (dependencyType.IsGenericType)
        {
            Type[] genericArguments = dependencyType.GetGenericArguments();
            if (genericArguments.Length > 1 || genericArguments[0].IsGenericType)
            {
                throw new NotImplementedException("This shoud not be implemented according to the task");
            }
            //enumerable
            if (dependencyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                if (genericArguments.Length == 0)
                {
                    throw new ArgumentException("IEnumerable<> not supported");
                }
                Type enumerableArgument = genericArguments[0];

                Type enumerableType = dependencyType.GetGenericArguments().ElementAt(0);
                IList instances = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(enumerableType))!;
                if (Configs.SingletonDependences.TryGetValue(enumerableArgument, out ICollection<Type> singletonImplementations))
                {
                    foreach (Type implementation in singletonImplementations)
                    {
                        instances.Add(ProvideSingleton(enumerableArgument, implementation));
                    }
                }
                if (Configs.TransientDependences.TryGetValue(enumerableArgument, out ICollection<Type> transientImplementations))
                {
                    foreach (Type implementation in transientImplementations)
                    {
                        instances.Add(CreateInstance(implementation));
                    }
                }
                return (IEnumerable<object>)instances;
            }
            //other generics
            else
            {
                if (Configs.SingletonDependences.TryGetValue(dependencyType, out ICollection<Type> singletonImplementations))
                {
                    foreach (Type implementation in singletonImplementations)
                    {
                        return ProvideSingleton(dependencyType, implementation);
                    }
                }
                if (Configs.TransientDependences.TryGetValue(dependencyType, out ICollection<Type> transientImplementations))
                {
                    foreach (Type implementation in transientImplementations)
                    {
                        return CreateInstance(implementation);
                    }
                }
                //open generics
                Type genericDefinition = dependencyType.GetGenericTypeDefinition();
                Type genericArgument = dependencyType.GetGenericArguments().ElementAt(0);
                if (Configs.SingletonDependences.TryGetValue(genericDefinition, out ICollection<Type> openSingletonImplementations))
                {
                    foreach (Type implementation in openSingletonImplementations)
                    {
                        if (implementation.IsGenericTypeDefinition)
                        {
                            Type createdType = implementation.MakeGenericType(genericArgument);
                            return ProvideSingleton(dependencyType, createdType);
                        }
                        else 
                        {
                            ProvideSingleton(dependencyType, implementation);
                        }
                    }
                }
                if (Configs.TransientDependences.TryGetValue(genericDefinition, out ICollection<Type> openTransientImplementations))
                {
                    foreach (Type implementation in openSingletonImplementations)
                    {
                        if (implementation.IsGenericTypeDefinition)
                        {
                            Type createdType = implementation.MakeGenericType(genericArgument);
                            return CreateInstance(createdType);
                        }
                        else
                        {
                            return CreateInstance(implementation);
                        }
                    }
                }
            }
        }
        else
        {
            if (Configs.SingletonDependences.TryGetValue(dependencyType, out ICollection<Type> singletonImplementations))
            {
                foreach (Type implementation in singletonImplementations)
                {
                    return ProvideSingleton(dependencyType, implementation);
                }
            }
            if (Configs.TransientDependences.TryGetValue(dependencyType, out ICollection<Type> transientImplementations))
            {
                foreach (Type implementation in transientImplementations)
                {
                    return CreateInstance(implementation);
                }
            }
        }
        return null;
    }
    //------------------------------------------------------------
    private object? ProvideSingleton(Type dependencyType, Type implementationType)
    {
        object? instance;
        lock (_lock)
        {
            object[] foundSingletons = _singletons.Where(obj => obj.GetType() == implementationType).ToArray();
            if (foundSingletons.Length > 0)
            {
                instance = foundSingletons[0];
            }
            else 
            {
                instance = CreateInstance(implementationType);
                _singletons.Add(instance);
            }
        }
        return instance;
    }
    
    private object? CreateInstance(Type transientType)
    {
        IEnumerable<ConstructorInfo> constructors = transientType.GetConstructors().Where(ci=>ci.IsPublic).OrderBy(ci => ci.GetParameters().Length);
        if (constructors.Count() <= 0)
        {
            throw new ArgumentException($"{transientType.Name} declares no public constructors");
        }
        foreach (ConstructorInfo constructorInfo in constructors)
        {
            ParameterInfo[] parametersInfo = constructorInfo.GetParameters();
            object?[] parameters = new object?[parametersInfo.Length];
            for (int i = 0; i < parametersInfo.Length; i++)
            { 
                Type parameterType = parametersInfo[i].ParameterType;
                if (parameterType.IsAbstract || parameterType.IsInterface)
                {
                    parameters[i] = GetDependency(parameterType);
                }
                else 
                {
                    parameters[i] = CreateInstance(parameterType);
                }
            }
            return Activator.CreateInstance(transientType, parameters);
        }
        return null;
    }


}

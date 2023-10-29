using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Tests.ClassesForTest;

public class GenericClass1<T> : IGenericInterface<T>
{
    public virtual void SomeMethod(T param)
    {
    
    }
}

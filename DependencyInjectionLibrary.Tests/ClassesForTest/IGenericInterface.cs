using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Tests.ClassesForTest;

public interface IGenericInterface<T>
{
    public void SomeMethod(T param);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Tests.ClassesForTest;

public class GenericClass2<T> : GenericClass1<T>
{
    public override void SomeMethod(T param)
    {
        base.SomeMethod(param);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Tests.ClassesForTest;

public class TestClass2 : TestClass1
{
    public override string GetSomeString()
    {
        return "Some string v.2";
    }
}

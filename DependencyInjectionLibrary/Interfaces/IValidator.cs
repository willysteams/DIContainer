using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Interfaces;

internal interface IValidator
{
    public string Message { get; }
    public bool Validate();
}

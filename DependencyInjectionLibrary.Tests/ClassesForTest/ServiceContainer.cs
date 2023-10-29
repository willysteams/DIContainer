using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Tests.ClassesForTest;

public class ServiceContainer<TService>
    where TService : IService
{
    public TService PassedService { get; protected set; }
    public ServiceContainer(TService service)
    {
        PassedService = service;
    }
}

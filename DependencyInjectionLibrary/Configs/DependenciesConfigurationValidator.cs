using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using University.DotnetLabs.Lab5.DependencyInjectionLibrary.Interfaces;

namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Configs;

internal class DepenpenciesConfigurationValidator : IValidator
{
    private DependenciesConfiguration _configs;
    private StringBuilder _messageBuilder = new();

    public string Message
    { 
        get => _messageBuilder.ToString();
    }

    public DepenpenciesConfigurationValidator(DependenciesConfiguration configuration)
    {
        _configs = configuration;
    }

    public bool Validate()
    {
        _messageBuilder.Clear();
        bool result = true;
        foreach (ICollection<Type> implementationsSet in _configs.TransientDependences.Values)
        { 
            foreach (Type implementation in implementationsSet)
            {
                if (implementation.IsAbstract)
                {
                    result = false;
                    _messageBuilder.AppendLine($"{implementation.Name} is Abstract");
                }
                else if (implementation.IsInterface)
                { 
                    result |= false;
                    _messageBuilder.AppendLine($"{implementation.Name} is Interface");
                }
            }    
        }
        foreach (ICollection<Type> implementationsSet in _configs.SingletonDependences.Values)
        {
            foreach (Type implementation in implementationsSet)
            {
                if (implementation.IsAbstract)
                {
                    result = false;
                    _messageBuilder.AppendLine($"{implementation.Name} is Abstract");
                }
                else if (implementation.IsInterface)
                {
                    result |= false;
                    _messageBuilder.AppendLine($"{implementation.Name} is Interface");
                }
            }
        }
        return result;
    }
}

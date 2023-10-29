namespace University.DotnetLabs.Lab5.DependencyInjectionLibrary.Interfaces;
public interface IDependencyProvider
{
    object? GetDependency(Type dependencyType);
    object? GetDependency<T>() => GetDependency(typeof(T));
}
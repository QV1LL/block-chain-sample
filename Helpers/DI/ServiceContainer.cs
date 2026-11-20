namespace BlockChainSample.Helpers.DI;

public class ServiceContainer
{
  private readonly Dictionary<Type, ServiceDescriptor> _services = [];

  public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
  {
    _services[typeof(TService)] = new ServiceDescriptor(typeof(TService), typeof(TImplementation), Lifetime.Singleton);
  }

  public void RegisterSingleton<TService>(Func<TService> getInstance) where TService : notnull
  {
    _services[typeof(TService)] = new ServiceDescriptor(typeof(TService), getInstance());
  }

  public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
  {
    _services[typeof(TService)] = new ServiceDescriptor(typeof(TService), typeof(TImplementation), Lifetime.Transient);
  }

  public T Resolve<T>()
  {
    return (T)Resolve(typeof(T));
  }

  public object Resolve(Type serviceType)
  {
    if (!_services.TryGetValue(serviceType, out var descriptor))
    {
      if (serviceType.IsAbstract || serviceType.IsInterface)
        throw new InvalidOperationException($"Service of type {serviceType.Name} is not registered.");

      return CreateInstance(serviceType);
    }

    if (descriptor.Lifetime == Lifetime.Singleton)
    {
      descriptor.ImplementationInstance ??= CreateInstance(descriptor.ImplementationType);

      return descriptor.ImplementationInstance;
    }

    return CreateInstance(descriptor.ImplementationType);
  }

  private object CreateInstance(Type implementationType)
  {
    var constructors = implementationType.GetConstructors();
    var constructor = constructors.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault()
      ?? throw new InvalidOperationException($"No public constructor found for {implementationType.Name}");
    var parameters = constructor.GetParameters();

    if (parameters.Length == 0)
      return Activator.CreateInstance(implementationType)!;

    var parameterImplementations = new object[parameters.Length];
    for (int i = 0; i < parameters.Length; i++)
      parameterImplementations[i] = Resolve(parameters[i].ParameterType);

    return constructor.Invoke(parameterImplementations);
  }
}


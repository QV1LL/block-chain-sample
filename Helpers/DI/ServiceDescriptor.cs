namespace BlockChainSample.Helpers.DI;

public class ServiceDescriptor
{
  public Type ServiceType { get; }
  public Type ImplementationType { get; }
  public Lifetime Lifetime { get; }
  public object? ImplementationInstance { get; set; }

  public ServiceDescriptor(Type serviceType, Type implementationType, Lifetime lifetime)
  {
    ServiceType = serviceType;
    ImplementationType = implementationType;
    Lifetime = lifetime;
  }

  public ServiceDescriptor(Type serviceType, object instance)
  {
    ServiceType = serviceType;
    ImplementationType = instance.GetType();
    Lifetime = Lifetime.Singleton;
    ImplementationInstance = instance;
  }
}

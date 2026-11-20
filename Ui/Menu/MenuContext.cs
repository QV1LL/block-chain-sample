namespace BlockChainSample.Ui.Menu;

public class MenuContext(string commandKey)
{
  public string CommandKey { get; } = commandKey;
  private readonly Dictionary<string, object> _data = [];

  public void Set<T>(string key, T value) where T : notnull
  {
    _data[key] = value;
  }

  public T Get<T>(string key)
  {
    if (!_data.TryGetValue(key, out var value))
      throw new KeyNotFoundException($"Context key '{key}' was not found.");

    if (value is T typedValue)
      return typedValue;

    throw new InvalidCastException($"Value for '{key}' is {value.GetType().Name}, expected {typeof(T).Name}.");
  }
}

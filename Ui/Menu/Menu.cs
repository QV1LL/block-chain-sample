using BlockChainSample.Helpers.DI;

namespace BlockChainSample.Ui.Menu;

public class Menu(ServiceContainer container)
{
  private readonly ServiceContainer _container = container;
  private readonly Dictionary<string, MenuItem> _items = new(StringComparer.OrdinalIgnoreCase);
  private readonly List<MenuItem> _orderedItems = [];
  private readonly List<Type> _globalMiddlewareTypes = [];
  private Func<string>? _headerProvider;

  public Menu SetHeader(Func<string> headerProvider)
  {
    _headerProvider = headerProvider;

    return this;
  }

  public Menu UseGlobal<TMiddleware>() where TMiddleware : IMenuMiddleware
  {
    _globalMiddlewareTypes.Add(typeof(TMiddleware));

    return this;
  }

  public Menu AddItem(string key, string title, Action<MenuItem> configure)
  {
    var item = new MenuItem(key, title, _container);
    configure(item);
    _items[key] = item;
    _orderedItems.Add(item);

    return this;
  }

  public async Task RunAsync()
  {
    while (true)
    {
      Render();
      Console.Write("\nSelect an option: ");
      var choice = Console.ReadLine()?.Trim() ?? string.Empty;

      var result = await DispatchAsync(choice);

      if (!string.IsNullOrWhiteSpace(result.Message))
      {
        Console.WriteLine(result.Message);
      }

      if (result.ShouldExit) return;

      if (result.ShouldPause)
      {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
      }
    }
  }

  public Task<MenuResult> DispatchAsync(string commandKey)
  {
    var context = new MenuContext(commandKey);

    MenuDelegate terminalHandler = ctx =>
    {
      if (!_items.TryGetValue(ctx.CommandKey, out var menuItem))
        return Task.FromResult(MenuResult.Fail("\nInvalid option."));

      return menuItem.ExecuteAsync(ctx);
    };

    MenuDelegate pipeline = terminalHandler;
    for (int i = _globalMiddlewareTypes.Count - 1; i >= 0; i--)
    {
      var middlewareType = _globalMiddlewareTypes[i];
      var next = pipeline;

      pipeline = ctx =>
      {
        var middleware = (IMenuMiddleware)_container.Resolve(middlewareType);
        return middleware.InvokeAsync(ctx, next);
      };
    }

    return pipeline(context);
  }

  private void Render()
  {
    Console.Clear();
    Console.WriteLine("=== Blockchain Menu ===");
    if (_headerProvider != null)
      Console.WriteLine(_headerProvider());

    foreach (var item in _orderedItems)
      Console.WriteLine($"{item.Key}. {item.Title}");
  }
}

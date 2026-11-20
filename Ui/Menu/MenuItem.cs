using BlockChainSample.Helpers.DI;

namespace BlockChainSample.Ui.Menu;

public class MenuItem(string key, string title, ServiceContainer container)
{
  public string Key { get; } = key;
  public string Title { get; } = title;
  private readonly ServiceContainer _container = container;
  private readonly List<Type> _middlewareTypes = [];
  private MenuDelegate? _compiledPipeline;

  public MenuItem Use<TMiddleware>() where TMiddleware : IMenuMiddleware
  {
    _middlewareTypes.Add(typeof(TMiddleware));
    _compiledPipeline = null;
    return this;
  }

  public Task<MenuResult> ExecuteAsync(MenuContext context)
  {
    _compiledPipeline ??= BuildPipeline();
    return _compiledPipeline(context);
  }

  private MenuDelegate BuildPipeline()
  {
    MenuDelegate next = _ => Task.FromResult(MenuResult.Ok());

    for (int i = _middlewareTypes.Count - 1; i >= 0; i--)
    {
      var middlewareType = _middlewareTypes[i];
      var nextDelegate = next;

      next = ctx =>
      {
        var middleware = (IMenuMiddleware)_container.Resolve(middlewareType);
        return middleware.InvokeAsync(ctx, nextDelegate);
      };
    }

    return next;
  }
}

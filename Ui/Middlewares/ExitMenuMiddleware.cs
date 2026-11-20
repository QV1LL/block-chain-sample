using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares;

public class ExitMenuMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    return Task.FromResult(MenuResult.Exit());
  }
}


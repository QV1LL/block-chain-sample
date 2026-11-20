using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class ExceptionHandlingMiddleware : IMenuMiddleware
{
  public async Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    try
    {
      return await next(context);
    }
    catch (Exception ex)
    {
      return MenuResult.Fail($"Unexpected error: {ex.Message}");
    }
  }
}


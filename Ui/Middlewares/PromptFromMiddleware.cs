using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class PromptFromMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.Write("\nFrom: ");
    var fromName = Console.ReadLine()?.Trim() ?? string.Empty;

    if (string.IsNullOrEmpty(fromName))
      return Task.FromResult(MenuResult.Fail("Names cannot be empty"));

    context.Set(TransactionKeys.FromName, fromName);
    return next(context);
  }
}


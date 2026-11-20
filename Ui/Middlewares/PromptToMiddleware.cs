using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class PromptToMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.Write("To: ");
    var toName = Console.ReadLine()?.Trim() ?? string.Empty;

    if (string.IsNullOrEmpty(toName))
      return Task.FromResult(MenuResult.Fail("Names cannot be empty"));

    context.Set(TransactionKeys.ToName, toName);
    return next(context);
  }
}


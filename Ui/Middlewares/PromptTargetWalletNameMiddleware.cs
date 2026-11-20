using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class PromptTargetWalletNameMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.Write("\nTarget wallet: ");
    var name = Console.ReadLine()?.Trim() ?? string.Empty;

    if (string.IsNullOrEmpty(name))
      return Task.FromResult(MenuResult.Fail("Names cannot be empty"));

    context.Set(WalletKeys.TargetName, name);
    return next(context);
  }
}


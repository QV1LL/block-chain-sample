using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class PromptWalletAddressMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.Write("\nWallet address: ");
    var address = Console.ReadLine()?.Trim() ?? string.Empty;

    if (string.IsNullOrEmpty(address))
      return Task.FromResult(MenuResult.Fail("Address cannot be empty"));

    context.Set(WalletKeys.TargetAddress, address);
    return next(context);
  }
}


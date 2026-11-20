using BlockChainSample.Services;
using BlockChainSample.Services.Display;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class FindWalletByNameMiddleware(WalletService walletService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var name = context.Get<string>(WalletKeys.TargetName);
    var wallet = walletService.FindByName(name);

    if (wallet == null)
      return Task.FromResult(MenuResult.Fail($"No wallet found with name: {name}"));

    WalletsDisplayService.DisplayWallet(wallet);
    return Task.FromResult(MenuResult.Ok());
  }
}


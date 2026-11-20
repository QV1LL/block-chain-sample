using BlockChainSample.Services;
using BlockChainSample.Services.Display;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class FindWalletByAddressMiddleware(WalletService walletService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var address = context.Get<string>(WalletKeys.TargetAddress);
    var wallet = walletService.FindByAddress(address);

    if (wallet == null)
      return Task.FromResult(MenuResult.Fail($"No wallet found with address: {address}"));

    WalletsDisplayService.DisplayWallet(wallet);
    return Task.FromResult(MenuResult.Ok());
  }
}


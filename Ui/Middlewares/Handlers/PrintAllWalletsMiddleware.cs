using BlockChainSample.Services;
using BlockChainSample.Services.Display;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class PrintAllWalletsMiddleware(WalletService walletService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    WalletsDisplayService.DisplayWallets(walletService.GetWallets());
    return Task.FromResult(MenuResult.Ok());
  }
}


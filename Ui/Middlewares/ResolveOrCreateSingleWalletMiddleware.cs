using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class ResolveOrCreateSingleWalletMiddleware(WalletService walletService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var name = context.Get<string>(WalletKeys.TargetName);
    var wallet = walletService.FindByName(name) ?? walletService.CreateWallet(name)!;

    context.Set(WalletKeys.ResolvedWallet, wallet);
    return next(context);
  }
}


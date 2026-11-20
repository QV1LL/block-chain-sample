using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class ResolveOrCreateTransferWalletsMiddleware(WalletService walletService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var fromName = context.Get<string>(TransactionKeys.FromName);
    var toName = context.Get<string>(TransactionKeys.ToName);

    var fromWallet = walletService.FindByName(fromName) ?? walletService.CreateWallet(fromName)!;
    var toWallet = walletService.FindByName(toName) ?? walletService.CreateWallet(toName)!;

    context.Set(WalletKeys.FromWallet, fromWallet);
    context.Set(WalletKeys.ToWallet, toWallet);
    return next(context);
  }
}


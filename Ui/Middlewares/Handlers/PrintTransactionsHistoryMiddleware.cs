using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Services.Display;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class PrintTransactionsHistoryMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var wallet = context.Get<Wallet>(WalletKeys.ResolvedWallet);
    WalletsDisplayService.PrintTransactionHistory(blockChain, wallet);

    return Task.FromResult(MenuResult.Ok());
  }
}


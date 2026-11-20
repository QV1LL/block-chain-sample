using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class AddBlockMiddleware(BlockChain blockChain, TransactionService transactionService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var rewardWallet = context.Get<Wallet>(WalletKeys.ResolvedWallet);
    var pending = transactionService.GetPendingTransactions();

    var message = blockChain.AddBlock(pending, rewardWallet.Address);
    if (message != null)
      return Task.FromResult(MenuResult.Fail(message));

    transactionService.ClearPending();
    return Task.FromResult(MenuResult.Ok("\nBlock added and pending transactions cleared."));
  }
}


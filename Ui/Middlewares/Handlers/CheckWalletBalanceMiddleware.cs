using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class CheckWalletBalanceMiddleware(BalanceService balanceService, BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var wallet = context.Get<Wallet>(WalletKeys.ResolvedWallet);
    var balance = balanceService.GetBalance(wallet.Address, blockChain.Chain);

    return Task.FromResult(MenuResult.Ok($"{wallet.Name}`s balance: {balance}"));
  }
}


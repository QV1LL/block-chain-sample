using BlockChainSample.Services;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class FindCorruptedTransactionsMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    blockChain.FindCorruptedTransactions();

    return next(context);
  }
}


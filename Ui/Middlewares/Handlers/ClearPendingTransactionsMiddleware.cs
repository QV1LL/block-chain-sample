using BlockChainSample.Models;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class ClearPendingTransactionsMiddleware(MemPool memPool) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    memPool.RemoveTransaction(memPool.Pending);
    return Task.FromResult(MenuResult.Ok("Pending transaction cleared!"));
  }
}


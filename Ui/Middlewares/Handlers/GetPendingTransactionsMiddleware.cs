using BlockChainSample.Models;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class GetPendingTransactionsMiddleware(MemPool memPool) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    context.Set(TransactionKeys.Pending, memPool.Pending.ToList());
    Console.WriteLine(memPool);

    return next(context);
  }
}

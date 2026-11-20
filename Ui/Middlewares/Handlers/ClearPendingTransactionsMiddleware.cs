using BlockChainSample.Services;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class ClearPendingTransactionsMiddleware(TransactionService transactionService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    transactionService.ClearPending();
    return Task.FromResult(MenuResult.Ok("Pending transaction cleared!"));
  }
}


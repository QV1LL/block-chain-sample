using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class GetPendingTransactionsMiddleware(TransactionService transactionService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var transactions = transactionService.GetPendingTransactions();

    context.Set(TransactionKeys.Pending, transactions);
    Console.WriteLine("Pending transactions: ");
    Console.WriteLine(string.Join('\n', transactions));

    return next(context);
  }
}

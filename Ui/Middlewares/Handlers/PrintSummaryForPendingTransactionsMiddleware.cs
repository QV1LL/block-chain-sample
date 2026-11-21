using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class PrintSummaryForPendingTransactionsMiddleware(BlockChain blockchain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var transactions = context.Get<List<Transaction>>(TransactionKeys.Pending);
    var transactionsCount = transactions.Count;

    if (transactionsCount == 0)
    {
      Console.WriteLine("There is no pending transactions");
      return next(context);
    }

    var checkedTransactions = transactions.Select(t => blockchain.ValidateTransaction(t, transactions));
    var invalidCount = checkedTransactions.Count(ct => ct != null);
    var validCount = transactionsCount - invalidCount;

    Console.WriteLine($"""
        Pending transactions summary:
        - {transactionsCount} was checked
        - where {invalidCount} was invalid
        - and {validCount} was valid
        """);

    return next(context);
  }
}

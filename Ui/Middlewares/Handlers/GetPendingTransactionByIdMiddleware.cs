using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class GetPendingTransactionByIdMiddleware(TransactionService transactionService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var id = context.Get<Guid>(TransactionKeys.TransactionId);
    var transaction = transactionService.GetTransactionById(id);

    if (transaction == null)
      return Task.FromResult(MenuResult.Fail($"Didn`t find transaction with id: {id}"));

    context.Set(TransactionKeys.Transaction, transaction);
    Console.WriteLine("Found pending transaction: ");
    Console.WriteLine(transaction);

    return next(context);
  }
}

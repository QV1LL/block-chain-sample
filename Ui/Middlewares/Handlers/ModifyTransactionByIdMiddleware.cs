using BlockChainSample.Models;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class ModifyTransactionByIdMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var transaction = context.Get<Transaction>(TransactionKeys.Transaction);
    transaction.Ammount = 9999;

    Console.WriteLine($"Modified transaction: \n{transaction}");

    return next(context);
  }
}



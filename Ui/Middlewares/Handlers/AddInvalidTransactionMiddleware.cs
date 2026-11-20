using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Services.Display;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class AddInvalidTransactionMiddleware(TransactionService transactionService) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var transaction = new Transaction("me", 999999, "you");
    transactionService.AddPending(transaction);

    Console.WriteLine("Incorrect transaction added to pendings: ");
    BlockChainDisplayService.DisplayTransaction(transaction);
    return Task.FromResult(MenuResult.Ok());
  }
}


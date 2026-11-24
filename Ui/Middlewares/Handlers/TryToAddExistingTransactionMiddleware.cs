using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class TryToAddExistingTransactionMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var existingTransaction = context.Get<Transaction>(TransactionKeys.Transaction);
    var result = blockChain.AddTransaction(existingTransaction);
    Console.WriteLine(result);

    return Task.FromResult(MenuResult.Ok());
  }
}


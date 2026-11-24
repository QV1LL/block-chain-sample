using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares.Handlers;

public class CheckTransactionMiddleware(BlockChain blockChain, MemPool memPool) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var transaction = context.Get<Transaction>(TransactionKeys.Transaction);
    var result = blockChain.ValidateTransaction(transaction, memPool.Pending);

    Console.WriteLine(result);

    if (result != null)
      return Task.FromResult(MenuResult.Fail($"Transaction with id {transaction.Id} is not valid"));

    return Task.FromResult(MenuResult.Ok($"Transaction with id {transaction.Id} is valid!"));
  }
}

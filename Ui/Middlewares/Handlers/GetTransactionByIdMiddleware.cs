using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class GetTransactionByIdMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var id = context.Get<Guid>(TransactionKeys.TransactionId);
    var result = blockChain.FindTransactionById(id);

    if (result == null)
      return Task.FromResult(MenuResult.Fail($"Didn`t find transaction with id: {id}"));

    var (transaction, index) = result.Value;
    context.Set(TransactionKeys.Transaction, transaction);
    context.Set(BlockchainKeys.Index, index);

    Console.WriteLine($"Found transaction in block {result.Value.blockIndex}: ");
    Console.WriteLine(transaction);

    return next(context);
  }
}


using BlockChainSample.Services;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class BreakRandomBlockMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var transactions = blockChain.Chain.SelectMany(b => b.Transactions).ToList();
    if (transactions.Count == 0)
      return Task.FromResult(MenuResult.Fail("No transactions available to corrupt."));

    var index = Random.Shared.Next(transactions.Count);
    transactions[index].Id = Guid.NewGuid();

    return Task.FromResult(MenuResult.Ok(shouldPause: false));
  }
}


using BlockChainSample.Services;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class CheckBlockchainMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    return Task.FromResult(MenuResult.Ok($"Is chain valid: {blockChain.IsChainValid()}"));
  }
}


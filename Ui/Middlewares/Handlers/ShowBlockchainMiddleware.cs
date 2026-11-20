using BlockChainSample.Services;
using BlockChainSample.Services.Display;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class ShowBlockchainMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.WriteLine("\n=== Blockchain ===");
    BlockChainDisplayService.DisplayChain(blockChain);

    return Task.FromResult(MenuResult.Ok());
  }
}


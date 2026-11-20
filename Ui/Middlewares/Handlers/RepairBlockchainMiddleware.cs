using System.Diagnostics;
using BlockChainSample.Services;
using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares.Handlers;

public class RepairBlockchainMiddleware(BlockChain blockChain) : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    var startIndex = context.Get<int>(BlockchainKeys.StartIndex);

    var stopwatch = Stopwatch.StartNew();
    var (success, countOfTries, countOfRepaired) = blockChain.RepairChain(startIndex);
    stopwatch.Stop();

    if (!success)
      return Task.FromResult(MenuResult.Fail("Index was out of range"));

    return Task.FromResult(MenuResult.Ok(
      $"Chain repaired {countOfRepaired} blocks with {countOfTries} tries to mine blocks, time taken: {stopwatch.ElapsedMilliseconds}ms\nIs chain valid: {blockChain.IsChainValid()}"
    ));
  }
}


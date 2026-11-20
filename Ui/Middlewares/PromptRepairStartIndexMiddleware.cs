using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.Ui.Middlewares;

public class PromptRepairStartIndexMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.WriteLine("Enter an index from where to repair block chain: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out int startIndex))
      return Task.FromResult(MenuResult.Fail($"Entered an incorrect index: {input}"));

    context.Set(BlockchainKeys.StartIndex, startIndex);
    return next(context);
  }
}

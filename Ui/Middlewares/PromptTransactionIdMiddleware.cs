using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class PromptTransactionIdMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.WriteLine("Enter id of transaction: ");
    if (!Guid.TryParse(Console.ReadLine(), out Guid id))
      return Task.FromResult(MenuResult.Fail("Incorrect format of guid"));

    context.Set(TransactionKeys.TransactionId, id);
    return next(context);
  }
}


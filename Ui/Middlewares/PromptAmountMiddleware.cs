using BlockChainSample.Ui.Keys;
using BlockChainSample.Ui.Menu;

namespace BlockChainSample.UI.Middlewares;

public class PromptAmountMiddleware : IMenuMiddleware
{
  public Task<MenuResult> InvokeAsync(MenuContext context, MenuDelegate next)
  {
    Console.Write("Amount: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
      return Task.FromResult(MenuResult.Fail("Invalid amount."));

    context.Set(TransactionKeys.Amount, amount);
    return next(context);
  }
}


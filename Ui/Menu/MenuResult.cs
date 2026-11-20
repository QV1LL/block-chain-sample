namespace BlockChainSample.Ui.Menu;

public class MenuResult
{
  public bool IsSuccess { get; }
  public string Message { get; }
  public bool ShouldExit { get; }
  public bool ShouldPause { get; }

  private MenuResult(bool isSuccess, string message, bool shouldExit = false, bool shouldPause = true)
  {
    IsSuccess = isSuccess;
    Message = message;
    ShouldExit = shouldExit;
    ShouldPause = shouldPause;
  }

  public static MenuResult Ok(string message = "", bool shouldPause = true) => new(true, message, false, shouldPause);
  public static MenuResult Fail(string message) => new(false, message, false, true);
  public static MenuResult Exit(string message = "Exiting...") => new(true, message, true, false);
}

using System;

namespace DBCore
{
  public static class Helper
  {
    public static void Success(string msg)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.Write(msg);
      Console.ResetColor();
    }

    public static void Message(string msg)
    {
      Console.Write(msg);
    }

    public static void Warning(string msg)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write(msg);
      Console.ResetColor();
    }

    public static void Error(string msg)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write(msg);
      Console.ResetColor();
    }

  }
}

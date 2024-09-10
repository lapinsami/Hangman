namespace Hangman;

internal static class Program
{
    private static string secretWord = "muukalaislegioona";
    private static string revealedWord = new string('*', secretWord.Length);
    private static char[] secretWordArray = secretWord.ToCharArray();
    private static char[] revealedWordArray = revealedWord.ToCharArray();
    private static int numberOfGuesses = 0;
    private static List<char> guessHistory = [];
    private static string playerName = "player";

    private static void Main()
    {
        PrintGame();
        ConfirmExit();
    }

    private static void ConfirmExit()
    {
        Console.WriteLine("Press any key to close");
        Console.ReadKey(true);
    }

    static bool CheckForWin()
    {
        return revealedWordArray.SequenceEqual(secretWordArray);
    }

    private static void PrintGame()
    {
        Console.Clear();
        Console.WriteLine("HANGMAN");
        Console.WriteLine("\"quit\" to quit");
        Console.WriteLine($"Guesses: {numberOfGuesses} ({string.Join(", ", guessHistory)})");
        Console.WriteLine();

        Console.WriteLine($"{new string('#', revealedWordArray.Length + 6)}");
        Console.WriteLine($"# {new string(' ', revealedWordArray.Length + 2)} #");
        Console.Write("#  ");

        foreach (var c in revealedWordArray)
        {
            if (c == '*')
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            Console.Write(c);
            Console.ForegroundColor = ConsoleColor.White;
        }

        Console.Write("  #\n");
        Console.WriteLine($"# {new string(' ', revealedWordArray.Length + 2)} #");
        Console.WriteLine($"{new string('#', revealedWordArray.Length + 6)}");
    }
}
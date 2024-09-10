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
        StartUp();
        PrintGame();
        revealedWordArray[3] = 'k';
        ShutDown();
    }

    static void StartUp()
    {
        playerName = AskForUserName();
    }

    static void ShutDown()
    {
        PrintGame();
        Console.WriteLine($"Thanks for playing {playerName}");
        ConfirmExit();
    }

    private static void ConfirmExit()
    {
        Console.WriteLine("Press any key to close");
        Console.ReadKey(true);
    }

    static bool CheckForWin()
    {
        // Compares each element in the arrays and return true only if all are equal
        return revealedWordArray.SequenceEqual(secretWordArray);
    }

    static string AskForUserName()
    {
        string? name = null;
        Console.Write("Name: ");

        while (name == null)
        {
            name = Console.ReadLine();
        }

        return name;
    }

    private static void PrintGame()
    {
        Console.Clear();
        
        PrintHeader();
        
        Console.Write("#  ");
        PrintRevealedWord();
        Console.Write("  #\n");
        
        PrintFooter();
        
    }

    private static void PrintRevealedWord()
    {
        foreach (var c in revealedWordArray)
        {
            if (c == '*')
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.Write(c);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    private static void PrintHeader()
    {
        Console.WriteLine("HANGMAN");
        Console.WriteLine("\"quit\" to quit");
        Console.WriteLine($"Guesses: {numberOfGuesses} ({string.Join(", ", guessHistory)})");
        Console.WriteLine();

        Console.WriteLine($"{new string('#', revealedWordArray.Length + 6)}");
        Console.WriteLine($"# {new string(' ', revealedWordArray.Length + 2)} #");
    }

    private static void PrintFooter()
    {
        Console.WriteLine($"# {new string(' ', revealedWordArray.Length + 2)} #");
        Console.WriteLine($"{new string('#', revealedWordArray.Length + 6)}");
    }
}
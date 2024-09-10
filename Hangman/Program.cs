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
        MainGameLoop();
        ShutDown();
    }

    static void MainGameLoop()
    {
        while (true)
        {
            PrintGame();

            var guess = AskForLetter();

            if (guess == '0')
            {
                break;
            }
            
            guessHistory.Add(guess);

            for (var i = 0; i < secretWordArray.Length; i++)
            {
                if (guess == secretWordArray[i])
                {
                    revealedWordArray[i] = guess;
                }
            }

            if (CheckForWin())
            {
                break;
            }
        }
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

    static char AskForLetter()
    {
        Console.Write("\nChoose a letter: ");

        while (true)
        {
            var guess = Console.ReadLine();
            Console.WriteLine();

            if (guess is "quit" or "exit")
            {
                return '0';
            }
            if (guess == null)
            {
                Console.Write("Please select a single letter: ");
                continue;
            }
            if (guess.Length != 1)
            {
                Console.Write("Please select a single letter: ");
                continue;
            }
            if (!char.IsLetter(guess[0]))
            {
                Console.Write("Please select a single letter: ");
                continue;
            }
            if (guessHistory.Contains(guess[0]))
            {
                Console.Write("Letter has already been used. Try again: ");
                continue;
            }

            numberOfGuesses++;
            return guess[0];
        }
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

    private static void PrintGuessHistory()
    {
        Console.Write("Guesses: ");
        for (int i = 0; i < guessHistory.Count; i++)
        {
            if (secretWordArray.Contains(guessHistory[i]))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            
            Console.Write($"{guessHistory[i]} ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.Write($"({numberOfGuesses})");
    }

    private static void PrintHeader()
    {
        Console.WriteLine("HANGMAN");
        PrintGuessHistory();
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
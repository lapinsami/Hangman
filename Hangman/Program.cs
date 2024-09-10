namespace Hangman;

internal static class Program
{
    private static string secretWord = "sequence";
    private static string revealedWord = new('*', secretWord.Length);
    private static char[] secretWordArray = secretWord.ToCharArray();
    private static char[] revealedWordArray = revealedWord.ToCharArray();
    private static int numberOfGuesses = 0;
    private static int numberOfMistakes = 0;
    private static List<char> guessHistory = [];
    private static string playerName = "player";

    private static void Main()
    {
        StartUp();
        MainGameLoop();
        ShutDown();
    }

    private static void MainGameLoop()
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
            bool guessWasCorrect = false;

            for (var i = 0; i < secretWordArray.Length; i++)
            {
                if (guess == secretWordArray[i])
                {
                    revealedWordArray[i] = guess;
                    guessWasCorrect = true;
                }
            }

            if (!guessWasCorrect)
            {
                numberOfMistakes++;
            }

            if (CheckForWin())
            {
                break;
            }

            if (CheckForLoss())
            {
                break;
            }
        }
    }

    private static void StartUp()
    {
        playerName = AskForUserName();
    }

    private static void ShutDown()
    {
        PrintGame();
        Console.WriteLine(CheckForLoss() ? "\nYou Lose..." : "\nYou Win!");
        Console.WriteLine($"\nThanks for playing {playerName}");
        ConfirmExit();
    }

    private static void ConfirmExit()
    {
        Console.WriteLine("Press any key to close");
        Console.ReadKey(true);
    }

    private static bool CheckForWin()
    {
        // Compares each element in the arrays and return true only if all are equal
        return revealedWordArray.SequenceEqual(secretWordArray);
    }

    private static bool CheckForLoss()
    {
        return numberOfMistakes >= 6;
    }

    private static string AskForUserName()
    {
        string? name = null;
        Console.Write("Name: ");

        while (name == null)
        {
            name = Console.ReadLine();
        }

        return name;
    }

    private static char AskForLetter()
    {
        Console.Write("\nChoose a letter: ");

        while (true)
        {
            var guess = Console.ReadLine();
            Console.WriteLine();

            switch (guess)
            {
                case "quit" or "exit":
                    return '0';
                case null:
                    Console.Write("Please select a single letter: ");
                    continue;
            }

            guess = guess.ToLower();

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
        // 1st row of hangman
        Console.WriteLine(" H======    ");
        // 2nd row of hangman
        Console.WriteLine(" H/    |    ");
        // 3rd row of hangman
        Console.Write(" H     O    ");
        
        Console.Write(new string(' ', int.Max(2, 11 - revealedWordArray.Length / 2)));
        PrintRevealedWord();
        Console.Write("\n");
        
        PrintFooter();
        PrintKeyboard();
        
    }

    private static void PrintRevealedWord()
    {
        foreach (var c in revealedWordArray)
        {
            // Ternary
            // if c == '*' then gray else green
            Console.ForegroundColor = c == '*' ? ConsoleColor.DarkGray : ConsoleColor.Green;
            
            Console.Write(c);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    private static void PrintGuessHistory()
    {
        Console.Write("Guesses: ");
        foreach (var guess in guessHistory)
        {
            // Ternary
            // if guess in array then green else red
            Console.ForegroundColor = secretWordArray.Contains(guess) ? ConsoleColor.Green : ConsoleColor.DarkRed;
            Console.Write($"{guess} ");
            
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.Write($"({numberOfGuesses})");
        Console.Write($"\nMistakes: {numberOfMistakes}");
    }

    private static void PrintKeyboard()
    {
        // Keyboard is 24 characters wide total
        
        const string keyboardTop = "qwertyuiopå";
        const string keyboardMid = "asdfghjklöä";
        const string keyboardBot = "zxcvbnm";
        
        // 5th row of hangman
        Console.Write(" H    / \\   ");
        
        Console.Write(" ");

        foreach (var letter in keyboardTop)
        {
            if (guessHistory.Contains(letter))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            
            Console.Write($"{letter} ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        Console.Write(" ");
        Console.Write("\n");
        
        // 6th row of hangman
        Console.Write(" H          ");
        
        Console.Write("  ");
        
        foreach (var letter in keyboardMid)
        {
            if (guessHistory.Contains(letter))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            
            Console.Write($"{letter} ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        Console.Write("\n");
        
        // 7th row of hangman
        Console.Write("/_\\         ");
        
        Console.Write("   ");
        
        foreach (var letter in keyboardBot)
        {
            if (guessHistory.Contains(letter))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            
            Console.Write($"{letter} ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        Console.Write("       ");
        Console.Write("\n");
    }

    private static void PrintHeader()
    {
        Console.WriteLine("               HANGMAN");
        //PrintGuessHistory();
        Console.WriteLine();
    }

    private static void PrintFooter()
    {
        // 4th row of hangman
        Console.WriteLine(" H    -|-   ");
    }
}
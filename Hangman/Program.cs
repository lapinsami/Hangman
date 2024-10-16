namespace Hangman;

internal static class Program
{
    private static string secretWord = "??????";
    private static string revealedWord = new('*', secretWord.Length);
    private static char[] secretWordArray = secretWord.ToCharArray();
    private static char[] revealedWordArray = revealedWord.ToCharArray();
    private static int numberOfGuesses = 0;
    private static int numberOfMistakes = 0;
    private static List<char> guessHistory = [];
    private static string playerName = "player";
    private static string language = "en";

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
                revealedWordArray = secretWordArray;
                break;
            }
        }
    }

    private static void StartUp()
    {
        PrintGame();
        Console.WriteLine();
        playerName = AskForUserName();
        language = AskForLanguage();
        
        secretWord = GetRandomWord(language);
        revealedWord = new('*', secretWord.Length);
        secretWordArray = secretWord.ToCharArray();
        revealedWordArray = revealedWord.ToCharArray();
    }

    private static string AskForLanguage()
    {
        string[] validLanguages = ["en", "sv", "fi"];
        Console.Write("Language used for the dictionary (en, sv, fi): ");

        while (true)
        {
            string? input = Console.ReadLine();

            if (input == null)
            {
                Console.Write("Try again: ");
                continue;
            }

            if (!validLanguages.Contains(input))
            {
                Console.Write("Not a valid language. Try again: ");
                continue;
            }

            return input;
        }
    }

    private static string GetRandomWord(string lang)
    {
        string[] dictionary;
        
        try
        {
            dictionary = File.ReadAllLines($"../../../../{lang}.txt");
        }
        catch (FileNotFoundException e)
        {
            dictionary = ["competence", "sequence", "numerical", "sunglasses", "jewelry"];
        }
        catch (Exception e)
        {
            dictionary = ["competence", "sequence", "numerical", "sunglasses", "jewelry"];
            Console.WriteLine($"Something went wrong.\n{e}");
        }

        while (true)
        {
            int i = Random.Shared.Next(dictionary.Length);
            string word = dictionary[i];

            if (word.All(char.IsLetter) && word.Length > 3)
            {
                return word;
            }
        }
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
        return revealedWordArray.SequenceEqual(secretWordArray);
    }

    private static bool CheckForLoss()
    {
        return numberOfMistakes >= 6;
    }

    private static string AskForUserName()
    {
        Console.Write("Name: ");

        while (true)
        {
            string? input = Console.ReadLine();

            if (input == null)
            {
                Console.Write("Try again: ");
                continue;
            }

            if (input.Length < 2)
            {
                Console.Write("Name must be 2 characters or longer. Try again: ");
                continue;
            }

            return input;
        }
    }

    private static char AskForLetter()
    {
        Console.Write("\nChoose a letter: ");

        while (true)
        {
            var guess = Console.ReadLine();
            Console.WriteLine();

            if (guess == null)
            {
                Console.Clear();
                PrintGame();
                Console.WriteLine();
                Console.Write("Please select a single letter: ");
                continue;
            }

            if (guess.Length != 1)
            {
                Console.Clear();
                PrintGame();
                Console.WriteLine();
                Console.Write("Please select a single letter: ");
                continue;
            }
            
            if (!char.IsLetter(guess[0]))
            {
                Console.Clear();
                PrintGame();
                Console.WriteLine();
                Console.Write("Please select a single letter: ");
                continue;
            }
            
            guess = guess.ToLower();
            if (guessHistory.Contains(guess[0]))
            {
                Console.Clear();
                PrintGame();
                Console.WriteLine();
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
        
        Console.WriteLine($"               HANGMAN            {language}");
        Console.WriteLine("------------------------------------");
        PrintGuessHistory();
        Console.WriteLine();
        // 1st row of hangman
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(" H======    ");
        // 2nd row of hangman
        Console.WriteLine(" H/    |    ");
        // 3rd row of hangman
        Console.Write(" H     ");

        if (numberOfMistakes >= 1)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("O    ");
        
        Console.ForegroundColor = ConsoleColor.White;
        
        Console.Write(new string(' ', int.Max(2, 11 - revealedWordArray.Length / 2)));
        
        PrintRevealedWord();
        
        Console.Write("\n");
        
        // 4th row of hangman
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" H    ");
        
        if (numberOfMistakes >= 3)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("-");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        if (numberOfMistakes >= 2)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("|");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        if (numberOfMistakes >= 4)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("-   \n");
        
        Console.ForegroundColor = ConsoleColor.White;
        
        // Keyboard is 24 characters wide total
        
        const string keyboardTop = "qwertyuiopå";
        const string keyboardMid = "asdfghjklöä";
        const string keyboardBot = "zxcvbnm";
        
        // 5th row of hangman
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" H    ");
        
        if (numberOfMistakes >= 5)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("/ ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        if (numberOfMistakes >= 6)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("\\   ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        Console.ForegroundColor = ConsoleColor.White;
        
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
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" H          ");
        Console.ForegroundColor = ConsoleColor.White;
        
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
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("/_\\         ");
        Console.ForegroundColor = ConsoleColor.White;
        
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

    private static void PrintRevealedWord()
    {
        foreach (var c in revealedWordArray)
        {
            if (c == '*')
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else if (guessHistory.Contains(c))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            
            Console.Write(c);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    private static void PrintGuessHistory()
    {
        Console.Write("Guesses: ");
        foreach (var guess in guessHistory)
        {
            Console.ForegroundColor = secretWordArray.Contains(guess) ? ConsoleColor.Green : ConsoleColor.DarkRed;
            Console.Write($"{guess} ");
            
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
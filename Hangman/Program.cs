using System.Text.Json;

namespace Hangman;

internal static class Program
{
    // relative paths from Hangman/Hangman/bin/Debug/net8.0/
    private const string DictionaryLocation = "../../../../"; // file name missing because it depends on a variable
    private const string JsonLocation = "../../../../scores.json";
    private static List<GameInstance> _gameHistory = [];

    private static void Main()
    {
        GameInstance game = StartUp();
        GameLoop(game);
        ShutDown(game);
    }

    private static void GameLoop(GameInstance game)
    {
        while (true)
        {
            PrintGame(game);
            
            char guess = AskForLetter(game);
            game.GuessHistory.Add(guess);
            bool guessWasCorrect = false;

            for (int i = 0; i < game.SecretWordArray.Length; i++)
            {
                if (guess == game.SecretWordArray[i])
                {
                    game.RevealedWordArray[i] = guess;
                    guessWasCorrect = true;
                }
            }

            if (!guessWasCorrect)
            {
                game.NumberOfMistakes++;
            }

            if (CheckForWin(game))
            {
                break;
            }

            if (CheckForLoss(game))
            {
                game.RevealedWordArray = game.SecretWordArray;
                break;
            }
        }
    }

    private static GameInstance StartUp()
    {
        // Reading scores from json if it exists
        
        string? scoresAsJsonString;

        try
        {
            scoresAsJsonString = File.ReadAllText(JsonLocation);
        }
        catch (FileNotFoundException e)
        {
            scoresAsJsonString = null;
        }

        if (string.IsNullOrEmpty(scoresAsJsonString))
        {
            _gameHistory = [];
        }
        else
        {
            _gameHistory = JsonSerializer.Deserialize<List<GameInstance>>(scoresAsJsonString) ?? [];
        }
        
        Console.WriteLine();

        GameInstance game = new();
        
        game.PlayerName = AskForUserName();
        game.Language = AskForLanguage();
        
        //game.SecretWord = GetRandomWord(game.Language);
        //game.RevealedWord = new('*', game.SecretWord.Length);
        game.SecretWordArray = GetRandomWord(game.Language).ToCharArray();
        game.RevealedWordArray = game.SecretWordArray;
        Array.Fill(game.RevealedWordArray, '*');
        
        game.GuessHistory = [];
        game.NumberOfGuesses = 0;
        game.NumberOfMistakes = 0;

        return game;
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
        string[] dictionary = File.ReadAllLines(DictionaryLocation + lang + ".txt");

        while (true)
        {
            // pick a ramdom word from the dictionary
            int i = Random.Shared.Next(dictionary.Length);
            string word = dictionary[i];
            
            // check that it contains only letters (e.g. no "x-ray") and that it is longer than 3 letters
            if (word.All(char.IsLetter) && word.Length > 3)
            {
                return word;
            }
        }
    }

    private static void ShutDown(GameInstance game)
    {
        // writing scores to json if the player did not lose
        if (!CheckForLoss(game))
        {
            _gameHistory.Add(game);

            string scoresAsJsonString = JsonSerializer.Serialize(_gameHistory);
            File.WriteAllText(JsonLocation,scoresAsJsonString);
        }

        PrintGame(game);
        Console.WriteLine(CheckForLoss(game) ? "\nYou Lose..." : "\nYou Win!");
        Console.WriteLine($"\nThanks for playing {game.PlayerName}");
        
        // sorting and printing scores if they exist

        if (_gameHistory.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("High scores:");
            
            // sorting by number of mistakes and then by the length of the word (longer words are easier)
            List<GameInstance> sortedGames = _gameHistory.OrderBy(o=>o.NumberOfMistakes)
                .ThenBy(o=>o.SecretWordArray.Length)
                .ToList();

            foreach (GameInstance g in sortedGames)
            {
                Console.WriteLine(g);
            }

            Console.WriteLine();
        }
        
        ConfirmExit();
    }

    private static void ConfirmExit()
    {
        Console.WriteLine("Press any key to close");
        Console.ReadKey(true);
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

    private static char AskForLetter(GameInstance game)
    {
        Console.Write("\nChoose a letter: ");

        while (true)
        {
            var guess = Console.ReadLine();
            Console.WriteLine();

            if (guess == null)
            {
                Console.Clear();
                PrintGame(game);
                Console.WriteLine();
                Console.Write("Please select a single letter: ");
                continue;
            }

            if (guess.Length != 1)
            {
                Console.Clear();
                PrintGame(game);
                Console.WriteLine();
                Console.Write("Please select a single letter: ");
                continue;
            }
            
            if (!char.IsLetter(guess[0]))
            {
                Console.Clear();
                PrintGame(game);
                Console.WriteLine();
                Console.Write("Please select a single letter: ");
                continue;
            }
            
            guess = guess.ToLower();
            if (game.GuessHistory.Contains(guess[0]))
            {
                Console.Clear();
                PrintGame(game);
                Console.WriteLine();
                Console.Write("Letter has already been used. Try again: ");
                continue;
            }

            game.NumberOfGuesses++;
            return guess[0];
        }
    }
    
    private static bool CheckForWin(GameInstance game)
    {
        return game.RevealedWordArray.SequenceEqual(game.SecretWordArray);
    }
    
    private static bool CheckForLoss(GameInstance game)
    {
        return game.NumberOfMistakes >= 6;
    }

    // it's a mess
    private static void PrintGame(GameInstance game)
    {
        Console.Clear();
        
        Console.WriteLine($"               HANGMAN            {game.Language}");
        Console.WriteLine("------------------------------------");
        PrintGuessHistory(game);
        
        Console.WriteLine();
        // 1st row of hangman
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(" H======    ");
        // 2nd row of hangman
        Console.WriteLine(" H/    |    ");
        // 3rd row of hangman
        Console.Write(" H     ");

        if (game.NumberOfMistakes >= 1)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("O    ");
        
        Console.ForegroundColor = ConsoleColor.White;
        
        Console.Write(new string(' ', int.Max(2, 11 - game.RevealedWordArray.Length / 2)));
        
        PrintRevealedWord(game);
        
        Console.Write("\n");
        
        // 4th row of hangman
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" H    ");
        
        if (game.NumberOfMistakes >= 3)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("-");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        if (game.NumberOfMistakes >= 2)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("|");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        if (game.NumberOfMistakes >= 4)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("-   \n");
        
        Console.ForegroundColor = ConsoleColor.White;
        
        // Keyboard is 24 characters wide total
        // (11 letters wide + offset of 1 per row) x 2 to have gaps in between
        
        const string keyboardTop = "qwertyuiopå";
        const string keyboardMid = "asdfghjklöä";
        const string keyboardBot = "zxcvbnm";
        
        // 5th row of hangman
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" H    ");
        
        if (game.NumberOfMistakes >= 5)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("/ ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        if (game.NumberOfMistakes >= 6)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        
        Console.Write("\\   ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        Console.ForegroundColor = ConsoleColor.White;
        
        Console.Write(" ");

        foreach (char letter in keyboardTop)
        {
            if (game.GuessHistory.Contains(letter))
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
            if (game.GuessHistory.Contains(letter))
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
            if (game.GuessHistory.Contains(letter))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            
            Console.Write($"{letter} ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        Console.Write("       ");
        Console.Write("\n");
        
    }

    private static void PrintRevealedWord(GameInstance game)
    {
        foreach (var c in game.RevealedWordArray)
        {
            if (c == '*')
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else if (game.GuessHistory.Contains(c))
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

    private static void PrintGuessHistory(GameInstance game)
    {
        Console.Write("Guesses: ");
        
        foreach (char guess in game.GuessHistory)
        {
            Console.ForegroundColor = game.SecretWordArray.Contains(guess) ? ConsoleColor.Green : ConsoleColor.DarkRed;
            Console.Write($"{guess} ");
            
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
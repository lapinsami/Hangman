namespace Hangman;

public class GameInstance
{
    public string PlayerName { get; set; } = "player";
    public string Language { get; set; } = "en";

    public char[] SecretWordArray { get; set; } = [];
    public char[] RevealedWordArray { get; set; } = [];

    public int NumberOfGuesses { get; set; }
    public int NumberOfMistakes { get; set; }
    public List<char> GuessHistory { get; set; }  = [];

    public override string ToString()
    {
        return $"    {PlayerName}: {NumberOfMistakes} mistakes - {new string(SecretWordArray)}";
    }
}
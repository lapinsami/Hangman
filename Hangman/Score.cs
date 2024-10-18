namespace Hangman;

public class Score
{
    public string Name { get; set; }
    public string Word { get; set; }
    public int NumberOfGuesses { get; set; }
    public int NumberOfMistakes { get; set; }
    public int WordLength { get; set; }
}
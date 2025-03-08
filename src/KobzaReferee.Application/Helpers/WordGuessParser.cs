using System.Globalization;

namespace KobzaReferee.Application.Helpers;

internal static class WordGuessParser
{
    private static readonly CultureInfo UkrainianCulture = new CultureInfo("uk-UA");

    public static WordGuess? Execute(string text)
    {
        // Identify the start and end indices of the required content.
        var startIndex = text.IndexOf(WordGuessMessage.StartIdentifier);
        var endIndex = text.IndexOf(WordGuessMessage.EndIdentifier);

        // Check if both identifiers exist.
        if (startIndex == -1 || endIndex == -1)
        {
            return default;
        }

        // Extract the relevant content.
        var relevantPart = text.Substring(startIndex, endIndex + WordGuessMessage.EndIdentifier.Length - startIndex);
        var lines = relevantPart.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

        // Parse date.
        var dateLine = lines[0].Replace(WordGuessMessage.StartIdentifier, string.Empty).Trim();
        if (!DateTime.TryParse(dateLine, UkrainianCulture, DateTimeStyles.None, out var parsedDate))
        {
            return default;
        }

        // Remove date and end identifier lines for guess extraction.
        lines.RemoveAt(0);
        lines.RemoveAt(lines.Count - 1);

        var wordGuess = new WordGuess
        {
            Id = Guid.NewGuid().ToString(),
            Date = parsedDate,
            Guesses = new List<GuessDetail>()
        };

        // Iterate through each line and extract guess details.
        for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
        {
            var line = lines[lineIndex];
            var symbols = SplitIntoSymbols(line);

            var guessDetail = new GuessDetail
            {
                Id = Guid.NewGuid(),
                AttemptNumber = lineIndex,
                LetterDetails = new List<LetterDetail>()
            };

            for (int letterIndex = 0; letterIndex < symbols.Count; letterIndex++)
            {
                guessDetail.LetterDetails.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Position = letterIndex,
                    LetterStatus = GetLetterStatus(symbols[letterIndex])
                });
            }

            wordGuess.Guesses.Add(guessDetail);
        }

        return wordGuess;
    }

    // Convert a string into a list of symbols (including emojis).
    private static List<string> SplitIntoSymbols(string str)
    {
        var textElementEnumerator = StringInfo.GetTextElementEnumerator(str);
        var symbols = new List<string>();

        while (textElementEnumerator.MoveNext())
        {
            symbols.Add(textElementEnumerator.GetTextElement());
        }

        return symbols;
    }

    // Convert symbol to its corresponding status.
    private static string GetLetterStatus(string symbol)
    {
        switch (symbol)
        {
            case "🟩":
                return LetterStatus.Correct;
            case "🟨":
                return LetterStatus.Misplaced;
            case "⬛":
                return LetterStatus.Incorrect;
            default:
                return "unknown";
        }
    }
}

namespace Uspelite.Data.Common.Contracts
{
    public interface IRandomGenerator
    {
        string RandomString(int minLength = 5, int maxLength = 8);

        string RandomText(int wordsCount, int minWordLength = 4, int maxWordLength = 10);

        int RandomIntegerBetween(int min, int max);
    }
}

namespace Uspelite.Data.Common
{
    using System;
    using System.Text;
    using System.Threading;
    using Contracts;
    
    public class RandomGenerator : IRandomGenerator
    {
        private static readonly Random random = new Random();
        private string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public string RandomString(int minLength = 5, int maxLength = 8)
        {
            var result = new StringBuilder();
            var length = random.Next(minLength, maxLength + 1);
            
            for (int i = minLength; i <= length; i++)
            {
                result.Append(this.letters[random.Next(0, this.letters.Length)]);
            }

            return result.ToString();
        }

        public string RandomText(int wordsCount, int minWordLength = 4, int maxWordLength = 10)
        {
            var result = new StringBuilder();

            for (int i = 0; i < wordsCount; i++)
            {
                result.Append(this.RandomString(minWordLength, maxWordLength) + " ");
            }

            return result.ToString();
        }

        public int RandomIntegerBetween(int min, int max)
        {
            //TODO: Change it with CryptoRandom dryn dryn
            Thread.Sleep(5);
            return random.Next(min, max + 1);
        }
    }
}

﻿namespace Uspelite.Data.Common
{
    using System;
    using System.Text;
    using Contracts;

    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random random;
        private string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public RandomGenerator()
        {
            this.random = new Random();
        }

        public string RandomString(int minLength = 5, int maxLength = 8)
        {
            var result = new StringBuilder();
            var length = this.random.Next(minLength, maxLength + 1);
            
            for (int i = minLength; i <= length; i++)
            {
                result.Append(this.letters[this.random.Next(0, this.letters.Length)]);
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
            return this.random.Next(min, max + 1);
        }
    }
}
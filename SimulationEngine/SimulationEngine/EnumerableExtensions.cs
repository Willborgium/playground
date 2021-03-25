using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationEngine
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> collection, int count)
        {
            if (count > collection.Count())
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var output = new List<T>();

            var remaining = collection.Except(output);

            while (output.Count < count)
            {
                var element = remaining.ElementAt(_random.Next(0, remaining.Count()));
                output.Add(element);
            }

            return output;
        }

        private static readonly Random _random = new Random();
    }
}

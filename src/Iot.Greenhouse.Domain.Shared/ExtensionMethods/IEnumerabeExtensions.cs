using System;
using System.Collections.Generic;
using System.Linq;

namespace Iot.Greenhouse.ExtensionMethods
{
    public static class IEnumerabeExtensions
    {
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            double standardDeviation = 0;

            if (values.Any())
            {
                // Compute the average.     
                double avg = values.Average();

                // Perform the Sum of (value-avg)_2_2.      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.      
                standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
            }

            return standardDeviation;
        }

        public static double Median(this IEnumerable<double> values)
        {
            int n = values.Count();
            if (values.Count() % 2 == 0)
            {
                return values.OrderBy(x => x).Skip((n - 1) / 2).Take(2).Average(x => x);
            }
            else
            {
                return values.OrderBy(x => x).Skip(n / 2).First();
            }
        }
    }
}

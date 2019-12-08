using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnfoldTask.Models
{
    public static class Extensions
    {
        public static object[,] To2DArray<T>(this IEnumerable<T> lines, params Func<T, object>[] lambdas)
        {
            var array = new object[lines.Count(), lambdas.Count()];
            var lineCounter = 0;
            foreach (var line in lines)
            {
                for (var i = 0; i < lambdas.Length; i++)
                {
                    array[lineCounter, i] = lambdas[i](line);
                }
                lineCounter++;
            };
            return array;
        }
    }
}

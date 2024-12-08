using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.Day_07
{
    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }

    class Equation
    {
        public long Target { get; set; }
        public List<long> Numbers { get; set; }

        public List<string> Operators { get; set; } = new List<string>();

        public bool Valid { get; set; } = false;

        public Equation(long target, List<long> numbers)
        {
            Target = target;
            Numbers = numbers;
        }

        public void SetValid(bool valid)
        {
            Valid = valid;
        }

        public void SetOperators(List<string> operators)
        {
            Operators = operators;
        }
    }
    public class Day7
    {
        private string FilePath = "./Day_07/Input.txt";

        Dictionary<int, Equation> rows = new Dictionary<int, Equation>();
        public Day7()
        {
            var lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var splitted = lines[i].Split(":", StringSplitOptions.RemoveEmptyEntries);
                var target = Convert.ToInt64(splitted[0]);
                var numbers = splitted[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x)).ToList();
                rows.Add(i, new Equation(target, numbers));


            }

        }
        public long SolutionPart1()
        {
            long total = 0;
            foreach (var row in rows)
            {
                total = GetTotal(total, row.Key, row.Value.Target, row.Value.Numbers);
            }

            return total;
        }

        private long GetTotal(long total, int rowKey, long target, List<long> numbers, bool withAddition = false)
        {
            var operatorCounts = numbers.Count - 1;
            var possibleCombination = Math.Pow(2, operatorCounts);
            for (int i = 0; i < possibleCombination; i++)
            {
                var binary = Convert.ToString(i, 2).PadLeft(operatorCounts, '0');
                var operators = binary.ToCharArray().Select(x => x == '0' ? "+" : "*").ToList();
                if (withAddition)
                {
                    var isResultFound = false;
                    for (int j = 1; j < possibleCombination; j++)
                    {
                        var subBinary = Convert.ToString(j, 2).PadLeft(operatorCounts, '0').ToCharArray();
                        var subOperators = operators.Clone().ToList();
                        for (int k = 0; k < subBinary.Length; k++)
                        {
                            if (subBinary[k] == '1')
                            {
                                subOperators[k] = "||";
                            }
                        }
                        var subResult = ApplyOperators(numbers, subOperators);
                        if (subResult == target)
                        {
                            rows[rowKey].SetValid(true);
                            rows[rowKey].SetOperators(subOperators);
                            total += subResult;
                            isResultFound = true;
                            break;
                        }
                    }

                    if (isResultFound)
                    {
                        break;
                    }
                }
                else
                {
                    var result = ApplyOperators(numbers, operators);

                    if (result == target)
                    {
                        rows[rowKey].SetValid(true);
                        rows[rowKey].SetOperators(operators);
                        total += result;
                        break;
                    }
                }

            }

            return total;
        }

        private long ApplyOperators(List<long> numbers, List<string> operators)
        {
            var currentTotal = numbers[0];
            for (int j = 0; j < operators.Count; j++)
            {
                if (operators[j] == "+")
                {
                    currentTotal += numbers[j + 1];
                }
                else if (operators[j] == "*")
                {
                    currentTotal *= numbers[j + 1];
                }
                else
                {
                    currentTotal = Convert.ToInt64(currentTotal.ToString() + numbers[j + 1].ToString());
                }
            }
            return currentTotal;
        }

        // 21572148763543

        public long SolutionPart2()
        {
            long total = 0;
            var notValids = rows.Where(x => !x.Value.Valid).ToDictionary();

            foreach (var row in notValids)
            {
                total = GetTotal(total, row.Key, row.Value.Target, row.Value.Numbers, true);
            }

            var validCount = rows.Where(x => x.Value.Valid).Count();

            var totalTotal = rows.Where(x => x.Value.Valid).Sum(x => x.Value.Target);

            return totalTotal;
        }
    }

    // 21572148763543 + 560368945765620
    // 581941094529163
}

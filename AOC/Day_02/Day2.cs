using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOC.Day_02
{
    public class Day2
    {
        private string FilePath = "./Day_02/Input.txt";
        public int SolutionPart1()
        {
            var lines = File.ReadAllLines(FilePath);
            var safeCount = 0;

            foreach (var line in lines)
            {
                var splitted = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var (isSafe, index) = IsSafe(splitted);

                if (isSafe)
                {
                    safeCount++;
                }
            }


            return safeCount;
        }

        public int SolutionPart2()
        {
            var lines = File.ReadAllLines(FilePath);
            var safeCount = 0;
            var index = 0;

            foreach (var line in lines)
            {
                index++;
                var splitted = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var (isSafe, indexToRemove) = IsSafe(splitted);

                if (!isSafe)
                {
                    if (indexToRemove > 0)
                    {
                        var isPrevUnsafe = splitted.Where((source, index) => index != indexToRemove - 1).ToArray();
                        var (checkNew0, _) = IsSafe(isPrevUnsafe);
                        if (checkNew0)
                        {
                            safeCount++;
                            continue;
                        }
                    }


                    var isCurrentUnsafe = splitted.Where((source, index) => index != indexToRemove).ToArray();
                    var (checkNew, _) = IsSafe(isCurrentUnsafe);
                    if (checkNew)
                    {
                        safeCount++;
                        continue;
                    }

                    var isNextUnsafe = splitted.Where((source, index) => index != indexToRemove + 1).ToArray();
                    var (checkNew2, _) = IsSafe(isNextUnsafe);
                    if (checkNew2)
                    {
                        safeCount++;
                        continue;
                    }
                    //Console.WriteLine(line);
                }

                if (isSafe)
                {
                    safeCount++;
                }
            }


            return safeCount;
        }

        private (bool, int) IsSafe(string[] numbers)
        {
            var isIncrease = Convert.ToInt32(numbers[0]) < Convert.ToInt32(numbers[1]);
            var isSafe = true;
            var index = -1;

            for (int i = 0; i < numbers.Length - 1; i++)
            {
                var currentGap = 0;

                if (isIncrease)
                {
                    currentGap = Convert.ToInt32(numbers[i + 1]) - Convert.ToInt32(numbers[i]);

                }
                else
                {
                    currentGap = Convert.ToInt32(numbers[i]) - Convert.ToInt32(numbers[i + 1]);
                }

                if (currentGap <= 0 || currentGap > 3)
                {
                    index = i;
                    isSafe = false;
                    break;
                }
            }

            return (isSafe, index);
        }

        private bool IsSafeTwoNumbers(int number1, int number2)
        {
            var isIncrease = number1 < number2;
            var isSafe = true;

            var currentGap = 0;

            if (isIncrease)
            {
                currentGap = number2 - number1;

            }
            else
            {
                currentGap = number1 - number2;
            }

            if (currentGap <= 0 || currentGap > 3)
            {
                isSafe = false;
            }

            return isSafe;
        }
    }
}

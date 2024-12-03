using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.Day_03
{
    public class Day3
    {
        private string FilePath = "./Day_03/Input.txt";
        public int SolutionPart1()
        {
            var lines = File.ReadAllLines(FilePath);
            var total = 0;

            foreach (var line in lines)
            {
                var regex = new Regex(@"mul\([0-9]+,[0-9]+\)");
                var match = regex.Matches(line);

                foreach (var item in match) {
                    total += Calculate(item.ToString());
                }

            }

            return total;
        }

        public int SolutionPart2()
        {
            var lines = File.ReadAllLines(FilePath);
            var total = 0;

            var isLastDont = false;

            foreach (var line in lines)
            {
                var alteredLine = line;
                if(isLastDont)
                {
                    alteredLine = "don't()" + alteredLine;
                }

                var cleanedLines = alteredLine.Split("do()", StringSplitOptions.RemoveEmptyEntries);

                foreach (var newLine in cleanedLines)
                {
                    var process = "";

                    if(newLine.Contains("don't()"))
                    {
                        process = newLine.Substring(0, newLine.IndexOf("don't()"));
                    }
                    else
                    {
                        process = newLine;
                    }

                    var regex = new Regex(@"mul\([0-9]+,[0-9]+\)");
                    var match = regex.Matches(process);

                    foreach (var item in match)
                    {
                        total += Calculate(item.ToString());
                    }
                }

                if (cleanedLines[cleanedLines.Length - 1].Contains("don't()"))
                {
                    isLastDont = true;
                }
                else
                {
                    isLastDont = false;
                }
            }

            return total;
        }

        private int Calculate(string problem)
        {
            var cleanProblem = problem.Replace("mul(", "").Replace(")", "");

            var splitted = cleanProblem.Split(",", StringSplitOptions.RemoveEmptyEntries);

            return Convert.ToInt32(splitted[0]) * Convert.ToInt32(splitted[1]);
        }
    }
}

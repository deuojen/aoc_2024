using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.Day_05
{
    public class Day5
    {
        private string FilePath = "./Day_05/Input.txt";
        public int SolutionPart1()
        {
            var lines = File.ReadAllLines(FilePath);
            var total = 0;

            var orders = new Dictionary<int, List<int>>();
            var pageRows = new List<List<int>>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("|"))
                {
                    var splitted = lines[i].Split('|');
                    var key = Convert.ToInt32(splitted[0]);
                    var value = Convert.ToInt32(splitted[1]);

                    if (orders.ContainsKey(key))
                    {
                        orders[key].Add(value);
                    }
                    else
                    {

                        orders.Add(key, [value]);
                    }
                }
                else if (lines[i].Contains(","))
                {
                    pageRows.Add(lines[i].Split(',').Select(x => Convert.ToInt32(x)).ToList());
                }
            }

            foreach (var pages in pageRows)
            {
                var isValid = true;
                for (int i = pages.Count - 1; i > 0; i--)
                {
                    var current = pages[i];
                    var orderOfPage = orders[current];
                    for (int j = i - 1; j > -1; j--)
                    {
                        var next = pages[j];
                        if (orderOfPage.Contains(next))
                        {
                            isValid = false;
                        }
                    }
                }

                if (isValid)
                {
                    var middle = Math.Abs(pages.Count / 2);
                    total += pages[middle];
                }
            }

            return total;
        }

        public int SolutionPart2()
        {
            var lines = File.ReadAllLines(FilePath);
            var total = 0;

            var orders = new Dictionary<int, List<int>>();
            var pageRows = new List<List<int>>();
            var inValidPageRows = new List<List<int>>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("|"))
                {
                    var splitted = lines[i].Split('|');
                    var key = Convert.ToInt32(splitted[0]);
                    var value = Convert.ToInt32(splitted[1]);

                    if (orders.ContainsKey(key))
                    {
                        orders[key].Add(value);
                    }
                    else
                    {

                        orders.Add(key, [value]);
                    }
                }
                else if (lines[i].Contains(","))
                {
                    pageRows.Add(lines[i].Split(',').Select(x => Convert.ToInt32(x)).ToList());
                }
            }

            foreach (var pages in pageRows)
            {
                var isValid = true;
                for (int i = pages.Count - 1; i > 0; i--)
                {
                    var current = pages[i];
                    var orderOfPage = orders[current];
                    for (int j = i - 1; j > -1; j--)
                    {
                        var next = pages[j];
                        if (orderOfPage.Contains(next))
                        {
                            isValid = false;
                        }
                    }
                }

                if (!isValid)
                {
                    inValidPageRows.Add(pages);
                }
            }

            foreach (var pages in inValidPageRows)
            {
                while (true)
                {
                    var isValid = true;
                    for (int i = pages.Count - 1; i > 0; i--)
                    {
                        var current = pages[i];
                        var orderOfPage = orders[current];
                        for (int j = i - 1; j > -1; j--)
                        {
                            var next = pages[j];
                            if (orderOfPage.Contains(next))
                            {
                                pages[i] = next;
                                pages[j] = current;
                                current = pages[i];
                                orderOfPage = orders[current];
                                isValid = false;
                            }
                        }
                    }

                    if (isValid)
                    {
                        break;
                    }
                }


                var middle = Math.Abs(pages.Count / 2);
                total += pages[middle];
            }

            return total;
        }
    }
}

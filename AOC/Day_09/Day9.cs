using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AOC.Day_09
{
    struct DiskOption
    {
        public int Index { get; set; }
        public long Value { get; set; }
        public int DiskId { get; set; }
        public bool Fragment { get; set; }

        public DiskOption(int index, long value, int diskId, bool fragment)
        {
            Index = index;
            Value = value;
            DiskId = diskId;
            Fragment = fragment;
        }
    }

    public class Day9
    {
        private string FilePath = "./Day_09/Input.txt";

        List<long> mapping = new List<long>();
        List<long> mappingClone = new List<long>();
        Dictionary<int, DiskOption> disk = new Dictionary<int, DiskOption>();
        private long MaxID = 0;

        public Day9()
        {
            var lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var splitted = lines[i].ToCharArray().Select(x => Convert.ToInt64(x.ToString())).ToList();
                var space = false;
                var counter = 0;
                for (int j = 0; j < splitted.Count; j++)
                {
                    var times = splitted[j];

                    for (int k = 0; k < times; k++)
                    {
                        mapping.Add(space ? -1 : counter);
                        mappingClone.Add(space ? -1 : counter);
                        MaxID = counter;
                    }

                    disk.Add(j, new DiskOption(j, times, counter, space));

                    if (!space)
                    {
                        counter++;
                        //counter = counter % 10;
                    }

                    space = !space;
                }
            }
        }
        public long SolutionPart1()
        {
            long total = 0;

            for (int i = 0; i < mapping.Count; i++)
            {
                var current = mapping[i];
                if (current == -1)
                {
                    var latest = GetLastValidItem(i);
                    if (latest == -1)
                    {
                        break;
                    }
                    mapping[i] = latest;
                }
            }

            var filtered = mapping.Where(x => x != -1).ToList();

            for (int i = 0; i < filtered.Count; i++)
            {
                total += mapping[i] * (long)i;
            }

            //var index = 0;
            //total = filtered.Aggregate((sum, a) =>
            //{
            //    Console.WriteLine($"{index}:{a}");
            //    int current = index * a;
            //    index++;
            //    return sum + current;
            //});

            return total;
        }

        // 6279058075753

        public long SolutionPart2()
        {
            long total = 0;

            for (int i = (int)MaxID; i > 1; i--)
            {
                var currentDisk = disk.FirstOrDefault(x => x.Value.DiskId == i && !x.Value.Fragment);

                var latestIndex = mappingClone.IndexOf(i);
                var leftMostIndex = FindLeftMostIndex((int)currentDisk.Value.Value, latestIndex);

                if (latestIndex < leftMostIndex || leftMostIndex == -1)
                {
                    continue;
                }

                for (int j = 0; j < currentDisk.Value.Value; j++)
                {
                    mappingClone[latestIndex + j] = -1;
                }

                for (int k = 0; k < currentDisk.Value.Value; k++)
                {
                    mappingClone[leftMostIndex + 1 + k] = i;
                }
            }

            for (int i = 0; i < mappingClone.Count; i++)
            {
                var current = mappingClone[i];
                if (current != -1)
                {
                    total += mappingClone[i] * (long)i;
                }
            }

            return total;
        }

        // 6301361958738

        private long GetLastValidItem(int minLoop)
        {
            long result = -1;
            for (int i = mapping.Count - 1; i > minLoop; i--)
            {
                result = mapping[i];
                if (result > -1)
                {
                    mapping[i] = -1;
                    break;
                }
            }

            return result;
        }

        private int FindLeftMostIndex(int requiredItemCount, int maxLoop)
        {
            var result = -1;
            var counter = 0;

            for (int j = 0; j < maxLoop; j++)
            {
                var current = mappingClone[j];
                if (current == -1)
                {
                    counter++;
                    if (requiredItemCount == counter)
                    {
                        result = j - requiredItemCount;
                        break;
                    }
                }
                else
                {
                    counter = 0;
                }
            }

            return result;
        }
    }
}

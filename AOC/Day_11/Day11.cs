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

namespace AOC.Day_11
{

    //    Dictionary<long, long> cache = new();

    //    var input = File.ReadAllText("./Day_11/InputOriginal.txt").Trim()
    //        .Split(' ')
    //        .Select(long.Parse)
    //        .ToArray();

    //    Console.WriteLine(Solve(25));
    //Console.WriteLine(Solve(75));

    //long Solve(int n)
    //    {
    //        long sum = 0;
    //        for (int i = 0; i < input.Length; i++)
    //            sum += Calc(input[i], n);
    //        return sum;
    //    }

    //    long Calc(long value, long depth)
    //    {
    //        if (depth == 0)
    //            return 1;
    //        if (value == 0)
    //            return Calc(1, depth - 1);
    //        long key, div, mod;
    //        if (!cache.TryGetValue(key = value << 7 | --depth, out long r))
    //        {
    //            for ((div, mod) = (10, 10); r == 0; (div, mod) = (div * 100, mod * 10))
    //                r = value < div
    //                    ? Calc(value * 2024, depth)
    //                    : value < div * 10
    //                        ? Calc(value / mod, depth) + Calc(value % mod, depth)
    //                        : 0;
    //            cache.Add(key, r);
    //        }
    //        return r;
    //    }


    public class Day11_2
    {
        private string InputFilePath = "./Day_11/InputOriginal.txt";

        private readonly List<long> _input;

        public Day11_2()
        {
            _input = File.ReadAllText(InputFilePath).Split(' ').Select(long.Parse).ToList();
        }

        public ValueTask<string> Solve_1()
        {
            var stones = GetStonesAfterIteration(25);

            return new(stones.Values.Sum().ToString());
        }

        public ValueTask<string> Solve_2()
        {
            var stones = GetStonesAfterIteration(75);

            return new(stones.Values.Sum().ToString());
        }

        private Dictionary<long, long> GetStonesAfterIteration(int iteration)
        {
            var stones = _input.ToDictionary(x => x, x => _input.LongCount(y => y == x));
            stones.TryAdd(1, 0);

            for (var i = 0; i < iteration; i++)
            {
                var modifications = new Dictionary<long, long> { { 1, 0 } };
                foreach (var stone in stones)
                {
                    if (stone.Key == 0)
                        AddStoneToModifiedList(1, stone.Value, modifications);
                    else if (stone.Key.ToString().Length % 2 == 0)
                    {
                        var stoneString = stone.Key.ToString();
                        var leftStone = int.Parse(stoneString[..(stoneString.Length / 2)]);
                        var rightStone = int.Parse(stoneString[(stoneString.Length / 2)..]);

                        AddStoneToModifiedList(leftStone, stone.Value, modifications);
                        AddStoneToModifiedList(rightStone, stone.Value, modifications);
                    }
                    else
                        AddStoneToModifiedList(stone.Key * 2024, stone.Value, modifications);

                    stones.Remove(stone.Key);
                }

                foreach (var modification in modifications)
                    stones[modification.Key] = modification.Value;
            }

            return stones.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
        }

        private static void AddStoneToModifiedList(long key, long value, Dictionary<long, long> modifications)
        {
            if (!modifications.TryAdd(key, value))
                modifications[key] += value;
        }
    }

    class SetOfNumbers
    {
        public long StartingNumber { get; set; }

        public int BlinkStart { get; set; }

        public int BlinkIndex { get; set; }

        public List<long> BlinkResult { get; set; }

        public SetOfNumbers(long startingNumber, int blinkStart)
        {
            StartingNumber = startingNumber;
            BlinkStart = blinkStart;
            BlinkIndex = 0;
            BlinkResult = new List<long>() { StartingNumber };
        }

        public long GetCount()
        {
            return BlinkResult.Count;
        }

    }


    class CheckNumber
    {
        public long StartingNumber { get; set; }
        public int BlinkTimes { get; set; }
        public List<int> BlinkCountOrder { get; set; } = new List<int>();
        public List<long> BlinkResult { get; set; }
        public bool IsComplete { get; set; } = false;

        public CheckNumber(long startingNumber)
        {
            StartingNumber = startingNumber;
            BlinkResult = new List<long>() { StartingNumber };
        }

    }

    public class Day11
    {


        //private string FilePath = "./Day_11/SampleInput.txt";

        private string FilePath = "./Day_11/InputOriginal.txt";
        //private string FilePath = "./Day_11/Input.txt";

        List<long> numbers = new List<long>();

        Dictionary<long, SetOfNumbers> mapping = new Dictionary<long, SetOfNumbers>();

        HashSet<long> control = new HashSet<long>();
        HashSet<long> uniqueNumbers = new HashSet<long>();
        Dictionary<long, int> uniqueNumberCounts = new Dictionary<long, int>();

        Dictionary<long, SetOfNumbers> numberSet = new Dictionary<long, SetOfNumbers>();

        Dictionary<long, CheckNumber> mapCheck = new Dictionary<long, CheckNumber>();
        HashSet<long> mapCheckComplete = new HashSet<long>();

        Dictionary<long, long> cache = new();

        public Day11()
        {
            var lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var splitted = lines[i].Split(" ").Select(x => Convert.ToInt64(x)).ToList();
                for (int j = 0; j < splitted.Count; j++)
                {
                    numbers.Add(splitted[j]);

                    if (!mapping.ContainsKey(splitted[j]))
                    {
                        mapping.Add(splitted[j], new SetOfNumbers(splitted[j], 0));
                    }

                    if (!mapCheck.ContainsKey(splitted[j]))
                    {
                        //mapCheck.Add(splitted[j], new CheckNumber(splitted[j]));
                    }
                }

            }

            for (int i = 0; i < 11; i++)
            {
                mapCheck.Add(i, new CheckNumber(i));
            }

            var input = File.ReadAllText(FilePath).Trim()
                        .Split(' ')
                        .Select(long.Parse);

            long Solve(int n) => input.Sum(v => Calc(v, n));


            Console.WriteLine(Solve(25));
            Console.WriteLine(Solve(75));

        }



        long Calc(long v, long d)
        {
            if (d == 0)
                return 1;
            long k, p, m;
            if (!cache.TryGetValue(k = v << 7 | --d, out long r))
                cache.Add(k, r = v == 0
                    ? Calc(1, d)
                    : ((p = (long)Math.Log10(v) + 1) & 1) != 0
                        ? Calc(v * 2024, d)
                        : Calc(v / (m = (long)Math.Pow(10, p >> 1)), d) + Calc(v % m, d));
            return r;
        }

        public long SolutionPart1()
        {
            var blinkCounter = 0;
            while (true)
            {
                var keys = mapCheck.Where(x => !x.Value.IsComplete).Select(x => x.Key).ToList();
                if (keys.Any())
                {
                    blinkCounter++;
                    foreach (var key in keys)
                    {
                        var nextNumbers = ApplyRules(mapCheck[key].BlinkResult);

                        foreach (var item in nextNumbers)
                        {
                            if (!mapCheck.ContainsKey(item))
                            {
                                mapCheck.Add(item, new CheckNumber(item));
                            }
                        }

                        mapCheck[key].BlinkTimes++;
                        mapCheck[key].BlinkResult = nextNumbers;
                        mapCheck[key].BlinkCountOrder.Add(nextNumbers.Count);
                        if (nextNumbers.Contains(key))
                        {
                            mapCheckComplete.Add(key);
                            mapCheck[key].IsComplete = true;
                            break;
                        }
                        else
                        {
                            var isAllThere = true;
                            for (var i = 0; i < nextNumbers.Count; i++)
                            {
                                var current = nextNumbers[i];
                                if (!mapCheckComplete.Contains(current))
                                {
                                    if (!mapCheck.ContainsKey(current))
                                    {
                                        mapCheck.Add(current, new CheckNumber(current));
                                    }
                                    isAllThere = false;
                                    break;
                                }
                            }

                            if (isAllThere)
                            {
                                mapCheck[key].IsComplete = true;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < 25; i++)
            {
                //var keys = mapping.Keys.ToList();

                //foreach (var key in keys)
                //{
                //    var nextSet = ApplyBlink(mapping[key], 1);
                //}

                var nextNumbers = ApplyRules(numbers);
                numbers = nextNumbers;

                //Console.WriteLine(string.Join("", numbers));
            }

            var total = numbers.Count;

            return total;
        }

        // 193899
        //  19778

        public long SolutionPart2()
        {



            for (int i = 1; i <= 25; i++)
            {
                var keys = mapping.Keys.ToList();
                foreach (var key in keys)
                {
                    var nextSet = ApplyBlink(mapping[key], i);
                    mapping[key] = nextSet;
                }
            }

            if (mapping[0].BlinkResult.Equals(mapping[1].BlinkResult))
            {
                Console.WriteLine("equal");
            }


            for (int i = 0; i < numbers.Count; i++)
            {
                if (uniqueNumbers.Add(numbers[i]))
                {
                    numberSet.Add(numbers[i], new SetOfNumbers(numbers[i], 0));
                    uniqueNumberCounts.Add(numbers[i], 1);
                }
                else
                {
                    uniqueNumberCounts[numbers[i]] = uniqueNumberCounts[numbers[i]] + 1;
                }

            }

            for (int i = 0; i < 25; i++)
            {
                foreach (var item in numberSet)
                {
                    var currents = item.Value.BlinkResult;
                    var nextNumbers = ApplyRules(currents);
                    numberSet[item.Key].BlinkResult = nextNumbers;
                }
            }

            long counter = 0;
            List<long> notExists = new List<long>();

            HashSet<long> uniqueNumbers2 = new HashSet<long>();
            Dictionary<long, SetOfNumbers> numberSet2 = new Dictionary<long, SetOfNumbers>();
            Dictionary<long, int> uniqueNumberCounts2 = new Dictionary<long, int>();

            foreach (var item in numberSet)
            {
                var currentCount = item.Value.GetCount();
                counter += currentCount * uniqueNumberCounts[item.Key];
                for (int i = 0; i < item.Value.BlinkResult.Count; i++)
                {
                    var currentNumber = item.Value.BlinkResult[i];
                    //if (!uniqueNumberCounts.ContainsKey(currentNumber))
                    //{
                    notExists.Add(currentNumber);
                    if (uniqueNumbers2.Add(currentNumber))
                    {
                        numberSet2.Add(currentNumber, new SetOfNumbers(currentNumber, 0));
                        uniqueNumberCounts2.Add(currentNumber, 1);
                    }
                    else
                    {
                        uniqueNumberCounts2[currentNumber] = uniqueNumberCounts2[currentNumber] + 1;
                    }
                    //}
                }
            }

            // 6641374551
            // ---2152405446
            //  663251546

            for (int i = 0; i < 25; i++)
            {
                foreach (var item in numberSet2)
                {
                    var currents = item.Value.BlinkResult;
                    var nextNumbers = ApplyRules(currents);
                    numberSet2[item.Key].BlinkResult = nextNumbers;
                }
            }

            long counter2 = 0;
            foreach (var item in numberSet2)
            {
                var currentCount = item.Value.GetCount();
                counter2 += currentCount * uniqueNumberCounts2[item.Key];
            }

            // 461078364663

            var total = counter + counter2;

            return total;
        }

        // 467719739214
        //  70366991609

        private SetOfNumbers ApplyBlink(SetOfNumbers set, int currentIndex)
        {

            var newNumbers = new List<long>();

            for (int i = 0; i < set.GetCount(); i++)
            {
                var result = Rules(set.BlinkResult[i], currentIndex);
                newNumbers.AddRange(result);
            }

            set.BlinkResult = newNumbers;
            set.BlinkIndex++;

            return set;
        }

        private List<long> Rules(long input, int currentIndex)
        {
            var newNumbers = new List<long>();
            var strNumber = input.ToString();
            if (input == 0)
            {
                newNumbers.Add(1);
                AddIfNotExists(1, currentIndex);
            }
            else if (strNumber.Length % 2 == 0)
            {
                var leftNumber = strNumber.Substring(0, strNumber.Length / 2);
                var rightNumber = strNumber.Substring(strNumber.Length / 2);

                newNumbers.Add(Convert.ToInt64(leftNumber));
                newNumbers.Add(Convert.ToInt64(rightNumber));
                AddIfNotExists(Convert.ToInt64(leftNumber), currentIndex);
                AddIfNotExists(Convert.ToInt64(rightNumber), currentIndex);
            }
            else
            {
                newNumbers.Add(input * 2024);
                AddIfNotExists(input * 2024, currentIndex);
            }

            return newNumbers;
        }

        private void AddIfNotExists(long key, int currentIndex)
        {
            if (mapping.ContainsKey(key))
            {

            }
            else
            {
                mapping.Add(key, new SetOfNumbers(key, currentIndex));
            }
        }

        private List<long> ApplyRules(List<long> input)
        {
            var newNumbers = new List<long>();
            for (int j = 0; j < input.Count; j++)
            {
                control.Add(input[j]);

                var strNumber = input[j].ToString();
                if (input[j] == 0)
                {
                    newNumbers.Add(1);
                    control.Add(1);
                }
                else if (strNumber.Length % 2 == 0)
                {
                    var leftNumber = strNumber.Substring(0, strNumber.Length / 2);
                    var rightNumber = strNumber.Substring(strNumber.Length / 2);

                    newNumbers.Add(Convert.ToInt64(leftNumber));
                    newNumbers.Add(Convert.ToInt64(rightNumber));

                    control.Add(Convert.ToInt64(leftNumber));
                    control.Add(Convert.ToInt64(rightNumber));
                }
                else
                {
                    newNumbers.Add(input[j] * 2024);
                    control.Add(input[j] * 2024);
                }
            }

            return newNumbers;
        }

    }
}



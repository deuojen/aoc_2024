using AOC.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AOC.Day_12
{
    class FencePosition
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public string Value { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool Visited { get; set; }
        public bool IsCorner { get; set; } = false;
        public int FenceCount { get; set; } = 4;

        public FencePosition(string value, int positionX, int positionY, int maxX, int maxY)
        {
            Value = value;
            PositionX = positionX;
            PositionY = positionY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public bool IsValid()
        {
            return PositionX > -1 && PositionX < MaxX && PositionY > -1 && PositionY < MaxY;
        }

        public override string ToString()
        {
            return $"{PositionX}:{PositionY}";
        }

        public void UpdatePossibleFenceCount(int count)
        {
            if (!Visited)
            {
                FenceCount = count;
            }
        }

        public void SetToCorner()
        {
            IsCorner = true;
        }
    }


    public class Day12
    {
        //private static int _MaxX = 10;
        //private static int _MaxY = 10;
        //private string FilePath = "./Day_12/SampleInput.txt";

        private static int _MaxX = 140;
        private static int _MaxY = 140;
        private string FilePath = "./Day_12/Input.txt";

        Dictionary<string, List<FencePosition>> mapping = new Dictionary<string, List<FencePosition>>();
        Dictionary<string, List<FencePosition>> mappingFences = new Dictionary<string, List<FencePosition>>();
        HashSet<string> visited = new HashSet<string>();
        string[,] grid = new string[_MaxX, _MaxY];

        public Day12()
        {
            var lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var splitted = lines[i].ToCharArray().Select(x => x.ToString()).ToList();
                for (int j = 0; j < splitted.Count; j++)
                {
                    grid[i, j] = splitted[j];
                }
            }
        }

        public void SolveExternal()
        {
            var dirs = new Complex[] { new(1, 0), new(0, -1), new(-1, 0), new(0, 1) };
            var grid = File.ReadAllLines("./Day_12/Input.txt")
                .SelectMany((line, r) => line.Select((ch, c) => (r, c, ch)))
                .ToDictionary(tp => new Complex(tp.r, tp.c), tp => tp.ch);

            var regions = new List<(int perimeter, int sides, HashSet<Complex> points)>();
            foreach (var key in grid.Keys
                .Where(key => !regions.Any(region => region.points.Contains(key))))
            {
                (var visited, var walls) = (new HashSet<Complex>(), new List<(Complex p1, Complex p2)>());
                var queue = new Queue<Complex>([key]);
                while (queue.TryDequeue(out Complex curr))
                {
                    if (!visited.Add(curr))
                        continue;

                    foreach (var n in from dir in dirs select curr + dir)
                    {
                        if (grid.ContainsKey(n) && grid[n] == grid[curr])
                            queue.Enqueue(n);
                        if (!grid.ContainsKey(n) || grid[n] != grid[curr])
                            walls.Add(sort(curr, n));
                    }
                }
                int count = walls.Where(wall => wall.p1.Real == wall.p2.Real)
                    .GroupBy(wall => wall.p1.Imaginary)
                    .Select(grp => grp.OrderBy(wall => wall.p1.Real))
                    .Sum(set => set.Aggregate(
                        (set.First(), inner(set.First(), grid[key]).Imaginary, 1),
                        (acc, curr) => (curr, inner(curr, grid[key]).Imaginary,
                            (Math.Abs(curr.p1.Real - acc.Item1.p1.Real) > 1)
                            || acc.Item2 != inner(curr, grid[key]).Imaginary ? acc.Item3 + 1 : acc.Item3),
                        acc => acc.Item3));

                regions.Add((walls.Count, 2 * count, visited));
            }

            Complex inner((Complex c1, Complex c2) wall, char plant)
                => !grid.ContainsKey(wall.c1) || grid[wall.c1] != plant ? wall.c2 : wall.c1;

            (Complex, Complex) sort(Complex c1, Complex c2)
                => c1.Real <= c2.Real ? c1.Imaginary < c2.Imaginary ? (c1, c2) : (c2, c1) : (c1, c2);

            Console.WriteLine($"Part 1: {regions.Sum(tp => tp.perimeter * tp.points.Count)}");
            Console.WriteLine($"Part 2: {regions.Sum(tp => tp.sides * tp.points.Count)}");
        }

        public long SolutionPart1()
        {
            var gridLength = grid.GetLength(0);

            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridLength; j++)
                {
                    var current = grid[i, j];

                    var plots = GetNeighbours(current, i, j);

                    if (plots.Count > 0)
                    {
                        mapping.Add($"{i}:{j}", plots);
                    }
                }
            }

            var total = 0;

            foreach (var item in mapping)
            {
                total += GetRegionScore(item.Value);
            }

            return total;
        }

        // 1375574

        public long SolutionPart2()
        {
            var total = 0;

            foreach (var item in mapping)
            {
                var fences = GetFences(item.Value);
                var count = fences.Where(x => x.Value < 4).Sum(x => x.Value - 1);
                var triplets = fences.Count(x => x.Value == 3);
                var forths = fences.Count(x => x.Value == 4);

                if (forths > 0)
                {

                }

                total += item.Value.Count * (count + triplets + (forths * 4));
            }

            return total;
        }

        // 828280 + 143 - 833279
        // 830566

        private List<FencePosition> GetNeighbours(string value, int x, int y)
        {

            var current = new FencePosition(value, x, y, _MaxX, _MaxY);

            if (visited.Contains(current.ToString()))
            {
                return new List<FencePosition>();
            }

            visited.Add(current.ToString());

            var list = new List<FencePosition>() { current };

            //var FirstRun = true;
            while (true)
            {
                var toBeAdded = new List<FencePosition>();

                for (int i = 0; i < list.Count; i++)
                {
                    var currentItem = list[i];

                    // up
                    var up = CheckPosition(value, currentItem.PositionX - 1, currentItem.PositionY);
                    if (up != null)
                    {
                        toBeAdded.Add(up);
                    }

                    // right 
                    var right = CheckPosition(value, currentItem.PositionX, currentItem.PositionY + 1);
                    if (right != null)
                    {
                        toBeAdded.Add(right);
                    }

                    // down 
                    var down = CheckPosition(value, currentItem.PositionX + 1, currentItem.PositionY);
                    if (down != null)
                    {
                        toBeAdded.Add(down);
                    }

                    // left 
                    var left = CheckPosition(value, currentItem.PositionX, currentItem.PositionY - 1);
                    if (left != null)
                    {
                        toBeAdded.Add(left);
                    }

                    //list[i].UpdatePossibleFenceCount(4 - toBeAdded.Count);
                    //list[i].Visited = true;
                }

                if (toBeAdded.Count > 0)
                {
                    list.AddRange(toBeAdded);
                }
                else
                {
                    break;
                }

            }

            return list;
        }

        private FencePosition? CheckPosition(string value, int x, int y)
        {
            var position = new FencePosition(value, x, y, _MaxX, _MaxY);
            if (position.IsValid())
            {
                var current = grid[position.PositionX, position.PositionY];
                if (current == value && !visited.Contains(position.ToString()))
                {
                    visited.Add(position.ToString());
                    return position;
                }
                return null;
            }

            return null;
        }

        private int GetRegionScore(List<FencePosition> list)
        {
            var count = list.Count;
            var totalFence = 0;

            for (var i = 0; i < list.Count; i++)
            {
                var current = list[i];
                var currentFence = 0;
                var up = list.FirstOrDefault(x => x.PositionX == current.PositionX - 1 && x.PositionY == current.PositionY);
                if (up == null)
                {
                    currentFence++;
                }
                var right = list.FirstOrDefault(x => x.PositionX == current.PositionX && x.PositionY == current.PositionY + 1);
                if (right == null)
                {
                    currentFence++;
                }
                var down = list.FirstOrDefault(x => x.PositionX == current.PositionX + 1 && x.PositionY == current.PositionY);
                if (down == null)
                {
                    currentFence++;
                }
                var left = list.FirstOrDefault(x => x.PositionX == current.PositionX && x.PositionY == current.PositionY - 1);
                if (left == null)
                {
                    currentFence++;
                }
                list[i].UpdatePossibleFenceCount(4 - currentFence);
                totalFence += currentFence;
            }

            return count * totalFence;
        }

        private Dictionary<string, int> GetFences(List<FencePosition> list)
        {
            var fences = new Dictionary<string, int>();
            for (var i = 0; i < list.Count; i++)
            {
                var current = list[i];
                var up = list.FirstOrDefault(x => x.PositionX == current.PositionX - 1 && x.PositionY == current.PositionY);
                if (up == null)
                {
                    ;
                    var postionKey = $"{current.PositionX - 1}:{current.PositionY}";
                    if (fences.ContainsKey(postionKey))
                    {
                        fences[postionKey]++;
                    }
                    else
                    {
                        fences.Add(postionKey, 1);
                    }
                }
                var right = list.FirstOrDefault(x => x.PositionX == current.PositionX && x.PositionY == current.PositionY + 1);
                if (right == null)
                {
                    var postionKey = $"{current.PositionX}:{current.PositionY + 1}";
                    if (fences.ContainsKey(postionKey))
                    {
                        fences[postionKey]++;
                    }
                    else
                    {
                        fences.Add(postionKey, 1);
                    }
                }
                var down = list.FirstOrDefault(x => x.PositionX == current.PositionX + 1 && x.PositionY == current.PositionY);
                if (down == null)
                {
                    var postionKey = $"{current.PositionX + 1}:{current.PositionY}";
                    if (fences.ContainsKey(postionKey))
                    {
                        fences[postionKey]++;
                    }
                    else
                    {
                        fences.Add(postionKey, 1);
                    }
                }
                var left = list.FirstOrDefault(x => x.PositionX == current.PositionX && x.PositionY == current.PositionY - 1);
                if (left == null)
                {
                    var postionKey = $"{current.PositionX}:{current.PositionY - 1}";
                    if (fences.ContainsKey(postionKey))
                    {
                        fences[postionKey]++;
                    }
                    else
                    {
                        fences.Add(postionKey, 1);
                    }
                }
            }

            var keys = fences.Keys.ToList();
            var corners = new Dictionary<string, int>();

            foreach (var key in keys)
            {
                var current = fences[key];
                var pos = key.Split(':').Select(x => Convert.ToInt32(x)).ToArray();
                var posUp = $"{pos[0] - 1}:{pos[1]}";
                var posUpRight = $"{pos[0] - 1}:{pos[1] + 1}";
                var posRight = $"{pos[0]}:{pos[1] + 1}";
                var posDownRight = $"{pos[0] + 1}:{pos[1] + 1}";
                var posDown = $"{pos[0] + 1}:{pos[1]}";
                var posDownLeft = $"{pos[0] + 1}:{pos[1] - 1}";
                var posLeft = $"{pos[0]}:{pos[1] - 1}";
                var posUpLeft = $"{pos[0] - 1}:{pos[1] - 1}";

                //***
                //---
                //***
                if (current == 2)
                {
                    var up = list.FirstOrDefault(x => x.ToString() == posUp);
                    var right = list.FirstOrDefault(x => x.ToString() == posRight);
                    var down = list.FirstOrDefault(x => x.ToString() == posDown);
                    var left = list.FirstOrDefault(x => x.ToString() == posLeft);

                    //***
                    //---
                    //***
                    if (up != null && right == null && down != null && left == null)
                    {
                        fences[key] = 1;
                    }

                    if (up == null && right != null && down == null && left != null)
                    {
                        fences[key] = 1;
                    }
                }

                //... |..
                //.-- .--
                //|.. ...
                if (!fences.ContainsKey(posLeft) && fences.ContainsKey(posRight) && (fences.ContainsKey(posDownLeft) || fences.ContainsKey(posUpLeft)))
                {
                    var left = list.FirstOrDefault(x => x.ToString() == posLeft);
                    if (left == null)
                    {
                        corners.TryAdd(posLeft, 2);
                    }
                    else if (fences.ContainsKey(posDownLeft))
                    {
                        var down = list.FirstOrDefault(x => x.ToString() == posDown);
                        if (down == null)
                        {
                            corners.TryAdd(posDown, 2);
                        }
                    }
                    else if (fences.ContainsKey(posUpLeft))
                    {
                        var up = list.FirstOrDefault(x => x.ToString() == posUp);
                        if (up == null)
                        {
                            corners.TryAdd(posUp, 2);
                        }
                    }
                    continue;
                }

                //... ..|
                //--. --.
                //..| ...
                if (!fences.ContainsKey(posRight) && fences.ContainsKey(posLeft) && (fences.ContainsKey(posDownRight) || fences.ContainsKey(posUpRight)))
                {
                    var right = list.FirstOrDefault(x => x.ToString() == posRight);
                    if (right == null)
                    {
                        corners.TryAdd(posRight, 2);
                    }
                    else if (fences.ContainsKey(posDownRight))
                    {
                        var down = list.FirstOrDefault(x => x.ToString() == posDown);
                        if (down == null)
                        {
                            corners.TryAdd(posDown, 2);
                        }
                    }
                    else if (fences.ContainsKey(posUpRight))
                    {
                        var up = list.FirstOrDefault(x => x.ToString() == posUp);
                        if (up == null)
                        {
                            corners.TryAdd(posUp, 2);
                        }
                    }
                    continue;
                }

                //|..
                //.-.
                //..|
                if (!fences.ContainsKey(posRight) && !fences.ContainsKey(posLeft) && fences.ContainsKey(posUpLeft) && fences.ContainsKey(posDownRight))
                {
                    var up = list.FirstOrDefault(x => x.ToString() == posUp);
                    if (up == null)
                    {
                        corners.TryAdd(posUp, 2);
                    }

                    var down = list.FirstOrDefault(x => x.ToString() == posDown);
                    if (down == null)
                    {
                        corners.TryAdd(posDown, 2);
                    }

                    continue;
                }

                //..|
                //.-.
                //|..
                if (!fences.ContainsKey(posRight) && !fences.ContainsKey(posLeft) && fences.ContainsKey(posUpRight) && fences.ContainsKey(posDownLeft))
                {
                    var up = list.FirstOrDefault(x => x.ToString() == posUp);
                    if (up == null)
                    {
                        corners.TryAdd(posUp, 2);
                    }

                    var down = list.FirstOrDefault(x => x.ToString() == posDown);
                    if (down == null)
                    {
                        corners.TryAdd(posDown, 2);
                    }

                    continue;
                }

                //.|. .|.
                //.|. .|.
                //-.. ..-
                if (!fences.ContainsKey(posDown) && fences.ContainsKey(posUp) && (fences.ContainsKey(posDownLeft) || fences.ContainsKey(posDownRight)))
                {
                    var down = list.FirstOrDefault(x => x.ToString() == posDown);
                    if (down == null)
                    {
                        corners.TryAdd(posDown, 2);
                    }
                    else if (fences.ContainsKey(posDownLeft))
                    {
                        var left = list.FirstOrDefault(x => x.ToString() == posLeft);
                        if (left == null)
                        {
                            corners.TryAdd(posLeft, 2);
                        }
                    }
                    else if (fences.ContainsKey(posDownRight))
                    {
                        var right = list.FirstOrDefault(x => x.ToString() == posRight);
                        if (right == null)
                        {
                            corners.TryAdd(posRight, 2);
                        }
                    }
                    continue;
                }

                //-.. ..-
                //.|. .|.
                //.|. .|.
                if (!fences.ContainsKey(posUp) && fences.ContainsKey(posDown) && (fences.ContainsKey(posUpLeft) || fences.ContainsKey(posUpRight)))
                {
                    var up = list.FirstOrDefault(x => x.ToString() == posUp);
                    if (up == null)
                    {
                        corners.TryAdd(posUp, 2);
                    }
                    else if (fences.ContainsKey(posUpLeft))
                    {
                        var left = list.FirstOrDefault(x => x.ToString() == posLeft);
                        if (left == null)
                        {
                            corners.TryAdd(posLeft, 2);
                        }
                    }
                    else if (fences.ContainsKey(posUpRight))
                    {
                        var right = list.FirstOrDefault(x => x.ToString() == posRight);
                        if (right == null)
                        {
                            corners.TryAdd(posRight, 2);
                        }
                    }
                    continue;
                }

                //|.|
                //.-.
                //...
                if (!fences.ContainsKey(posRight) && !fences.ContainsKey(posLeft) && fences.ContainsKey(posUpRight) && fences.ContainsKey(posUpLeft))
                {
                    var left = list.FirstOrDefault(x => x.ToString() == posLeft);
                    if (left == null)
                    {
                        corners.TryAdd(posLeft, 2);
                    }
                    var right = list.FirstOrDefault(x => x.ToString() == posRight);
                    if (right == null)
                    {
                        corners.TryAdd(posRight, 2);
                    }
                    continue;
                }

                //...
                //.-.
                //|.|
                if (!fences.ContainsKey(posRight) && !fences.ContainsKey(posLeft) && fences.ContainsKey(posDownRight) && fences.ContainsKey(posDownLeft))
                {
                    var left = list.FirstOrDefault(x => x.ToString() == posLeft);
                    if (left == null)
                    {
                        corners.TryAdd(posLeft, 2);
                    }
                    var right = list.FirstOrDefault(x => x.ToString() == posRight);
                    if (right == null)
                    {
                        corners.TryAdd(posRight, 2);
                    }
                    continue;
                }

                //..-
                //.|.
                //..-
                if (!fences.ContainsKey(posUp) && !fences.ContainsKey(posDown) && fences.ContainsKey(posUpRight) && fences.ContainsKey(posDownRight))
                {
                    var up = list.FirstOrDefault(x => x.ToString() == posUp);
                    if (up == null)
                    {
                        corners.TryAdd(posUp, 2);
                    }
                    var down = list.FirstOrDefault(x => x.ToString() == posDown);
                    if (down == null)
                    {
                        corners.TryAdd(posDown, 2);
                    }
                    continue;
                }


                //-..
                //.|.
                //-..
                if (!fences.ContainsKey(posUp) && !fences.ContainsKey(posDown) && fences.ContainsKey(posDownLeft) && fences.ContainsKey(posUpLeft))
                {
                    var up = list.FirstOrDefault(x => x.ToString() == posUp);
                    if (up == null)
                    {
                        corners.TryAdd(posUp, 2);
                    }
                    var down = list.FirstOrDefault(x => x.ToString() == posDown);
                    if (down == null)
                    {
                        corners.TryAdd(posDown, 2);
                    }
                    continue;
                }
            }

            var currentValue = list[0].Value;
            //DisplayFences(currentValue, fences);
            //DisplayFences(currentValue, corners);

            var keys2 = corners.Keys.ToList();

            foreach (var key in keys2)
            {
                fences.TryAdd(key, corners[key]);
            }

            var keys3 = fences.Keys.ToList();
            //clean not touching
            foreach (var key in keys3)
            {
                var pos = key.Split(':').Select(x => Convert.ToInt32(x)).ToArray();
                var posUp = $"{pos[0] - 1}:{pos[1]}";
                var posUpRight = $"{pos[0] - 1}:{pos[1] + 1}";
                var posRight = $"{pos[0]}:{pos[1] + 1}";
                var posDownRight = $"{pos[0] + 1}:{pos[1] + 1}";
                var posDown = $"{pos[0] + 1}:{pos[1]}";
                var posDownLeft = $"{pos[0] + 1}:{pos[1] - 1}";
                var posLeft = $"{pos[0]}:{pos[1] - 1}";
                var posUpLeft = $"{pos[0] - 1}:{pos[1] - 1}";

                var up = list.FirstOrDefault(x => x.ToString() == posUp);
                var upRight = list.FirstOrDefault(x => x.ToString() == posUpRight);
                var right = list.FirstOrDefault(x => x.ToString() == posRight);
                var downRight = list.FirstOrDefault(x => x.ToString() == posDownRight);
                var down = list.FirstOrDefault(x => x.ToString() == posDown);
                var downLeft = list.FirstOrDefault(x => x.ToString() == posDownLeft);
                var left = list.FirstOrDefault(x => x.ToString() == posLeft);
                var upLeft = list.FirstOrDefault(x => x.ToString() == posUpLeft);

                if (up == null && upRight == null && right == null && downRight == null && down == null && downLeft == null && left == null && upLeft == null)
                {
                    fences.Remove(key);
                }
            }


            DisplayFences(currentValue, fences);
            return fences;
        }

        private void DisplayFences(string letter, Dictionary<string, int> fences)
        {
            var currentMinX = -2;
            var currentMaxX = _MaxX + 2;
            var currentMinY = -2;
            var currentMaxY = _MaxY + 2;

            for (int i = currentMinX; i < currentMaxX; i++)
            {
                var row = "";

                for (int j = currentMinY; j < currentMaxY; j++)
                {
                    var current = $"{i}:{j}";
                    if (fences.ContainsKey(current))
                    {
                        row += fences[current];
                    }
                    else
                    {
                        row += ".";
                    }
                }
                Console.WriteLine(row);
            }
        }
    }
}


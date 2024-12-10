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

namespace AOC.Day_10
{
    struct TrailPosition
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int Value { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public TrailPosition(int value, int positionX, int positionY, int maxX, int maxY)
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
    }

    class TrailPath
    {
        public TrailPosition Point0 { get; set; }
        public Dictionary<string, TrailPosition> Point1s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point1ds { get; set; } = new List<TrailPosition>();
        public Dictionary<string, TrailPosition> Point2s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point2ds { get; set; } = new List<TrailPosition>();
        public Dictionary<string, TrailPosition> Point3s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point3ds { get; set; } = new List<TrailPosition>();
        public Dictionary<string, TrailPosition> Point4s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point4ds { get; set; } = new List<TrailPosition>();
        public Dictionary<string, TrailPosition> Point5s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point5ds { get; set; } = new List<TrailPosition>();
        public Dictionary<string, TrailPosition> Point6s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point6ds { get; set; } = new List<TrailPosition>();
        public Dictionary<string, TrailPosition> Point7s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point7ds { get; set; } = new List<TrailPosition>();
        public Dictionary<string, TrailPosition> Point8s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point8ds { get; set; } = new List<TrailPosition>();
        public Dictionary<string, TrailPosition> Point9s { get; set; } = new Dictionary<string, TrailPosition>();
        public List<TrailPosition> Point9ds { get; set; } = new List<TrailPosition>();

        public bool IsValid { get; set; } = false;

        public TrailPath(TrailPosition point0)
        {
            Point0 = point0;
        }

        public int Score()
        {
            return Point9s.Count;
        }

        public int DistinctScore()
        {
            return Point9ds.Count;
        }
    }


    public class Day10
    {


        //private string FilePath = "./Day_10/SampleInput.txt";
        //private static int _MaxX = 8;
        //private static int _MaxY = 8;

        private static int _MaxX = 54;
        private static int _MaxY = 54;
        private string FilePath = "./Day_10/Input.txt";

        List<TrailPath> mapping = new List<TrailPath>();
        int[,] grid = new int[_MaxX, _MaxY];

        public Day10()
        {
            var lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var splitted = lines[i].ToCharArray().Select(x => Convert.ToInt32(x.ToString())).ToList();
                for (int j = 0; j < splitted.Count; j++)
                {
                    grid[i, j] = splitted[j];
                }
            }
        }
        public long SolutionPart1()
        {
            var gridLength = grid.GetLength(0);

            for (int i = 0; i < gridLength; i++)
            {
                for (int j = 0; j < gridLength; j++)
                {
                    var current = grid[i, j];
                    if (current == 0)
                    {
                        var path = FindPaths(new TrailPosition(current, i, j, _MaxX, _MaxY));
                        if (path.IsValid)
                        {
                            mapping.Add(path);
                        }
                    }
                }
            }

            var total = mapping.Sum(x => x.Score());

            return total;
        }

        // 624

        public long SolutionPart2()
        {
            var total = mapping.Sum(x => x.DistinctScore());

            return total;
        }

        // 1483

        private TrailPath FindPaths(TrailPosition startPosition)
        {
            var path = new TrailPath(startPosition);
            // 1
            path.Point1s = GetNexts(startPosition);
            path.Point1ds = path.Point1s.Select(x => x.Value).ToList();
            if (path.Point1s.Count == 0)
            {
                return path;
            }

            // 2
            foreach (var point in path.Point1ds)
            {
                var nextPoints = GetNexts(point);
                foreach (var item in nextPoints)
                {
                    path.Point2ds.Add(item.Value);

                    if (!path.Point2s.ContainsKey(item.Key))
                    {
                        path.Point2s.Add(item.Key, item.Value);
                    }
                }
            }

            if (path.Point2s.Count == 0)
            {
                return path;
            }

            // 3
            foreach (var point in path.Point2ds)
            {
                var nextPoints = GetNexts(point);
                foreach (var item in nextPoints)
                {
                    path.Point3ds.Add(item.Value);
                    if (!path.Point3s.ContainsKey(item.Key))
                    {
                        path.Point3s.Add(item.Key, item.Value);
                    }
                }
            }

            if (path.Point3s.Count == 0)
            {
                return path;
            }

            // 4
            foreach (var point in path.Point3ds)
            {
                var nextPoints = GetNexts(point);
                foreach (var item in nextPoints)
                {
                    path.Point4ds.Add(item.Value);
                    if (!path.Point4s.ContainsKey(item.Key))
                    {
                        path.Point4s.Add(item.Key, item.Value);
                    }
                }
            }

            if (path.Point4s.Count == 0)
            {
                return path;
            }

            // 5
            foreach (var point in path.Point4ds)
            {
                var nextPoints = GetNexts(point);
                foreach (var item in nextPoints)
                {
                    path.Point5ds.Add(item.Value);
                    if (!path.Point5s.ContainsKey(item.Key))
                    {
                        path.Point5s.Add(item.Key, item.Value);
                    }
                }
            }

            if (path.Point5s.Count == 0)
            {
                return path;
            }

            // 6
            foreach (var point in path.Point5ds)
            {
                var nextPoints = GetNexts(point);
                foreach (var item in nextPoints)
                {
                    path.Point6ds.Add(item.Value);
                    if (!path.Point6s.ContainsKey(item.Key))
                    {
                        path.Point6s.Add(item.Key, item.Value);
                    }
                }
            }

            if (path.Point6s.Count == 0)
            {
                return path;
            }

            // 7
            foreach (var point in path.Point6ds)
            {
                var nextPoints = GetNexts(point);
                foreach (var item in nextPoints)
                {
                    path.Point7ds.Add(item.Value);
                    if (!path.Point7s.ContainsKey(item.Key))
                    {
                        path.Point7s.Add(item.Key, item.Value);
                    }
                }
            }

            if (path.Point7s.Count == 0)
            {
                return path;
            }

            // 8
            foreach (var point in path.Point7ds)
            {
                var nextPoints = GetNexts(point);
                foreach (var item in nextPoints)
                {
                    path.Point8ds.Add(item.Value);
                    if (!path.Point8s.ContainsKey(item.Key))
                    {
                        path.Point8s.Add(item.Key, item.Value);
                    }
                }
            }

            if (path.Point8s.Count == 0)
            {
                return path;
            }

            // 9
            foreach (var point in path.Point8ds)
            {
                var nextPoints = GetNexts(point);
                foreach (var item in nextPoints)
                {
                    path.Point9ds.Add(item.Value);
                    if (!path.Point9s.ContainsKey(item.Key))
                    {
                        path.Point9s.Add(item.Key, item.Value);
                    }
                }
            }

            if (path.Point9s.Count == 0)
            {
                return path;
            }

            path.IsValid = true;

            return path;
        }

        private Dictionary<string, TrailPosition> GetNexts(TrailPosition prevPosition)
        {
            var result = new Dictionary<string, TrailPosition>();

            // top
            var topPosition = new TrailPosition(prevPosition.Value + 1, prevPosition.PositionX - 1, prevPosition.PositionY, _MaxX, _MaxY);
            if (topPosition.IsValid())
            {
                var top = grid[topPosition.PositionX, topPosition.PositionY];
                if (top == topPosition.Value)
                {
                    result.Add(topPosition.ToString(), topPosition);

                }
            }

            // right
            var rightPosition = new TrailPosition(prevPosition.Value + 1, prevPosition.PositionX, prevPosition.PositionY + 1, _MaxX, _MaxY);
            if (rightPosition.IsValid())
            {
                var right = grid[rightPosition.PositionX, rightPosition.PositionY];
                if (right == rightPosition.Value)
                {
                    result.Add(rightPosition.ToString(), rightPosition);
                }
            }

            // down
            var downPosition = new TrailPosition(prevPosition.Value + 1, prevPosition.PositionX + 1, prevPosition.PositionY, _MaxX, _MaxY);
            if (downPosition.IsValid())
            {
                var down = grid[downPosition.PositionX, downPosition.PositionY];
                if (down == downPosition.Value)
                {
                    result.Add(downPosition.ToString(), downPosition);
                }
            }

            // left
            var leftPosition = new TrailPosition(prevPosition.Value + 1, prevPosition.PositionX, prevPosition.PositionY - 1, _MaxX, _MaxY);
            if (leftPosition.IsValid())
            {
                var left = grid[leftPosition.PositionX, leftPosition.PositionY];
                if (left == leftPosition.Value)
                {
                    result.Add(leftPosition.ToString(), leftPosition);
                }
            }

            return result;
        }
    }
}


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

namespace AOC.Day_08
{
    struct Location
    {
        private readonly int _MaxX = 50;
        private readonly int _MaxY = 50;

        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public Location(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public bool IsValid()
        {
            return PositionX > -1 && PositionX < _MaxX && PositionY > -1 && PositionY < _MaxY;
        }

        public override string ToString()
        {
            return $"{PositionX}:{PositionY}";
        }
    }

    enum Direction
    {
        Same = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
    }

    public class Day8
    {
        private string FilePath = "./Day_08/Input.txt";

        Dictionary<string, List<Location>> map = new Dictionary<string, List<Location>>();
        private HashSet<string> Antinodes = new HashSet<string>();

        public Day8()
        {
            var lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var splitted = lines[i].ToCharArray().Select(x => x.ToString()).ToList();

                for (int j = 0; j < splitted.Count; j++)
                {
                    var currentKey = splitted[j];

                    if (currentKey != ".")
                    {
                        if (map.ContainsKey(currentKey))
                        {
                            map[currentKey].Add(new Location(i, j));
                        }
                        else
                        {
                            map.Add(splitted[j], new List<Location>() { new Location(i, j) });
                        }


                    }
                }
            }

        }
        public int SolutionPart1()
        {
            foreach (var item in map)
            {
                var nodes = item.Value.ToList();

                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    for (int j = i + 1; j < nodes.Count; j++)
                    {
                        var prev = nodes[i];
                        var next = nodes[j];
                        Direction directionX, directionY;
                        int distanceX, distanceY;
                        GetDirectionAndDistance(prev, next, out directionX, out distanceX, out directionY, out distanceY);

                        InsertTheAntinodeToDirection(prev, directionX == Direction.Up ? Direction.Down : Direction.Up, distanceX, directionY == Direction.Left ? Direction.Right : Direction.Left, distanceY);
                        InsertTheAntinodeToDirection(next, directionX, distanceX, directionY, distanceY);
                    }
                }

            }

            return Antinodes.Count;
        }

        // 398

        public int SolutionPart2()
        {
            foreach (var item in map)
            {
                var nodes = item.Value.ToList();

                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    for (int j = i + 1; j < nodes.Count; j++)
                    {
                        var prev = nodes[i];
                        var next = nodes[j];
                        Direction directionX, directionY;
                        int distanceX, distanceY;
                        GetDirectionAndDistance(prev, next, out directionX, out distanceX, out directionY, out distanceY);

                        var antinode1 = new Location(prev.PositionX, prev.PositionY);
                        if (antinode1.IsValid())
                        {
                            Antinodes.Add(antinode1.ToString());
                        }
                        var antinode2 = new Location(next.PositionX, next.PositionY);
                        if (antinode2.IsValid())
                        {
                            Antinodes.Add(antinode2.ToString());
                        }

                        InsertTheAntinodeToDirection(prev, directionX == Direction.Up ? Direction.Down : Direction.Up, distanceX, directionY == Direction.Left ? Direction.Right : Direction.Left, distanceY, true);
                        InsertTheAntinodeToDirection(next, directionX, distanceX, directionY, distanceY, true);
                    }
                }
            }

            return Antinodes.Count;
        }

        private static void GetDirectionAndDistance(Location prev, Location next, out Direction directionX, out int distanceX, out Direction directionY, out int distanceY)
        {
            directionX = Direction.Up;
            distanceX = prev.PositionX - next.PositionX;
            if (distanceX > 0)
            {
                directionX = Direction.Up;
            }
            else
            {
                directionX = Direction.Down;
                distanceX = Math.Abs(distanceX);
            }

            directionY = Direction.Left;
            distanceY = prev.PositionY - next.PositionY;
            if (distanceY > 0)
            {
                directionY = Direction.Left;
            }
            else
            {
                directionY = Direction.Right;
                distanceY = Math.Abs(distanceY);
            }
        }

        private bool InsertTheAntinodeToDirection(Location current, Direction directionX, int distanceX, Direction directionY, int distanceY, bool continues = false)
        {
            var isValid = false;
            var antinode = new Location(0, 0);

            if (directionX == Direction.Up || directionX == Direction.Same)
            {
                antinode.PositionX = current.PositionX - distanceX;
            }
            else
            {
                antinode.PositionX = current.PositionX + distanceX;
            }

            if (directionY == Direction.Left || directionX == Direction.Same)
            {
                antinode.PositionY = current.PositionY - distanceY;
            }
            else
            {
                antinode.PositionY = current.PositionY + distanceY;
            }

            if (antinode.IsValid())
            {
                Antinodes.Add(antinode.ToString());
                isValid = true;
                if (continues)
                {
                    while (true)
                    {
                        if (!InsertTheAntinodeToDirection(antinode, directionX, distanceX, directionY, distanceY, true))
                        {
                            break;
                        }
                    }
                    isValid = false;
                }
            }

            return isValid;
        }


        // 21572148763543 + 560368945765620
        // 581941094529163

    }
}

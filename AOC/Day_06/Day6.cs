using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.Day_06
{
    enum MovingDirection
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }

    struct MovingPoint
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public MovingPoint(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public bool IsValid()
        {
            return PositionX > -1 && PositionX < 130 && PositionY > -1 && PositionY < 130;
        }

        public override string ToString()
        {
            return $"{PositionX}:{PositionY}";
        }
    }
    public class Day6
    {
        private string FilePath = "./Day_06/Input.txt";

        private HashSet<string> steps = new HashSet<string>();
        private string[,] grid = new string[130, 130];
        private string[,] gridClean = new string[130, 130];
        MovingPoint currentPosition = new MovingPoint(-1, -1);
        MovingDirection direction = MovingDirection.Up;

        MovingPoint startingPosition = new MovingPoint(-1, -1);
        public Day6()
        {
            var lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var chars = lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    grid[i, j] = chars[j].ToString();
                    gridClean[i, j] = chars[j].ToString();
                    if (chars[j].ToString() == "^")
                    {
                        currentPosition = new MovingPoint(i, j);
                        startingPosition = new MovingPoint(i, j);
                        //grid[i, j] = "currentPosition.ToString()";
                        steps.Add(currentPosition.ToString());
                    }
                }
            }

        }
        public int SolutionPart1()
        {
            while (true)
            {
                var oldPosition = currentPosition;
                var nextDirection = MovingDirection.Down;
                switch (direction)
                {
                    case MovingDirection.Up:
                        currentPosition.PositionX += -1;
                        nextDirection = MovingDirection.Right;
                        break;
                    case MovingDirection.Down:
                        currentPosition.PositionX += 1;
                        nextDirection = MovingDirection.Left;
                        break;
                    case MovingDirection.Left:
                        currentPosition.PositionY += -1;
                        nextDirection = MovingDirection.Up;
                        break;
                    case MovingDirection.Right:
                        currentPosition.PositionY += 1;
                        nextDirection = MovingDirection.Down;
                        break;
                    default:
                        break;
                }

                if (!currentPosition.IsValid())
                {
                    break;
                }

                var nextPostion = grid[currentPosition.PositionX, currentPosition.PositionY];
                if (nextPostion != "#")
                {
                    var isAdded = steps.Add(currentPosition.ToString());
                    if (isAdded)
                    {
                        if (direction == MovingDirection.Up || direction == MovingDirection.Down)
                        {
                            grid[currentPosition.PositionX, currentPosition.PositionY] = "|";
                        }
                        else
                        {
                            grid[currentPosition.PositionX, currentPosition.PositionY] = "-";
                        }
                    }
                    else
                    {
                        grid[currentPosition.PositionX, currentPosition.PositionY] = "+";
                    }

                }
                else if (nextPostion == "#")
                {
                    currentPosition = oldPosition;
                    direction = nextDirection;
                    grid[currentPosition.PositionX, currentPosition.PositionY] = "+";
                }
            }

            //PrintLines();

            return steps.Count;
        }

        public int SolutionPart2()
        {
            currentPosition = startingPosition;
            direction = MovingDirection.Up;
            var copySteps = steps.ToList();
            var totalInfinity = 0;

            foreach (var item in copySteps)
            {
                var splitted = item.Split(":");
                var posX = Convert.ToInt32(splitted[0]);
                var posY = Convert.ToInt32(splitted[1]);
                if (posX == startingPosition.PositionX && posY == startingPosition.PositionY) { continue; }

                grid = gridClean;

                var current = grid[posX, posY];
                steps.Clear();
                grid[posX, posY] = "#";

                //var totalLoop = 130 * 130;
                var currentLoop = -1;

                while (true)
                {
                    
                    var oldPosition = currentPosition;
                    var nextDirection = MovingDirection.Down;
                    switch (direction)
                    {
                        case MovingDirection.Up:
                            currentPosition.PositionX += -1;
                            nextDirection = MovingDirection.Right;
                            break;
                        case MovingDirection.Down:
                            currentPosition.PositionX += 1;
                            nextDirection = MovingDirection.Left;
                            break;
                        case MovingDirection.Left:
                            currentPosition.PositionY += -1;
                            nextDirection = MovingDirection.Up;
                            break;
                        case MovingDirection.Right:
                            currentPosition.PositionY += 1;
                            nextDirection = MovingDirection.Down;
                            break;
                        default:
                            break;
                    }

                    if (!currentPosition.IsValid())
                    {
                        break;
                    }

                    var nextPostion = grid[currentPosition.PositionX, currentPosition.PositionY];
                    if (nextPostion != "#")
                    {
                        var isAdded = steps.Add(currentPosition.ToString());
                        if (isAdded)
                        {
                            if (direction == MovingDirection.Up || direction == MovingDirection.Down)
                            {
                                grid[currentPosition.PositionX, currentPosition.PositionY] = "|";
                            }
                            else
                            {
                                grid[currentPosition.PositionX, currentPosition.PositionY] = "-";
                            }
                        }
                        else
                        {
                            currentLoop++;
                            grid[currentPosition.PositionX, currentPosition.PositionY] = "+";
                        }

                    }
                    else if (nextPostion == "#")
                    {
                        currentPosition = oldPosition;
                        direction = nextDirection;
                        grid[currentPosition.PositionX, currentPosition.PositionY] = "+";
                    }

                    if (currentLoop == steps.Count)
                    {
                        totalInfinity++;
                        break;
                    }
                }

                //PrintLines();

                grid[posX, posY] = ".";
                currentPosition = startingPosition;
                direction = MovingDirection.Up;
            }

            return totalInfinity;
        }

        private void PrintLines()
        {
            for (int l = 0; l < 130; l++)
            {
                var newLine = "";
                for (int m = 0; m < 130; m++)
                {
                    newLine += grid[l, m];
                }
                Console.WriteLine(newLine);
            }
        }
    }
}

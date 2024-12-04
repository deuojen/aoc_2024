using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC.Day_04
{
    public class Day4
    {
        private string FilePath = "./Day_04/Input.txt";
        public int SolutionPart1()
        {
            var lines = File.ReadAllLines(FilePath);
            var total = 0;

            var grid = new string[140, 140];
            var gridNew = new string[140, 140];

            for (int i = 0; i < lines.Length; i++)
            {
                var chars = lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    grid[i, j] = chars[j].ToString();
                    gridNew[i, j] = ".";
                }
            }

            for (int i = 0; i < 140; i++)
            {
                for (int j = 0; j < 140; j++)
                {
                    total += hasXmas(i, j, grid);
                }
            }

            return total;
        }

        public int SolutionPart2()
        {
            var lines = File.ReadAllLines(FilePath);
            var total = 0;

            var grid = new string[140, 140];
            var gridNew = new string[140, 140];

            for (int i = 0; i < lines.Length; i++)
            {
                var chars = lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    grid[i, j] = chars[j].ToString();
                    gridNew[i, j] = ".";
                }
            }

            for (int i = 0; i < 140; i++)
            {
                for (int j = 0; j < 140; j++)
                {
                    if (hasX_Mas(i, j, grid))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        private int hasXmas(int x, int y, string[,] grid)
        {
            var count = 0;
            var current = grid[x, y];
            if (current == "X")
            {
                var checkWord = "";

                // right
                if (y < 137)
                {
                    checkWord = grid[x, y] + grid[x, y + 1] + grid[x, y + 2] + grid[x, y + 3];
                    if (checkWord == "XMAS")
                    {
                        count++;
                    }
                }

                // right - bottom
                if (y < 137 && x < 137)
                {
                    checkWord = grid[x, y] + grid[x + 1, y + 1] + grid[x + 2, y + 2] + grid[x + 3, y + 3];
                    if (checkWord == "XMAS")
                    {
                        count++;
                    }
                }

                // bottom
                if (x < 137)
                {
                    checkWord = grid[x, y] + grid[x + 1, y] + grid[x + 2, y] + grid[x + 3, y];
                    if (checkWord == "XMAS")
                    {
                        count++;
                    }
                }

                // left - bottom
                if (y > 2 && x < 137)
                {
                    checkWord = grid[x, y] + grid[x + 1, y - 1] + grid[x + 2, y - 2] + grid[x + 3, y - 3];
                    if (checkWord == "XMAS")
                    {
                        count++;
                    }
                }

                // left
                if (y > 2)
                {
                    checkWord = grid[x, y] + grid[x, y - 1] + grid[x, y - 2] + grid[x, y - 3];
                    if (checkWord == "XMAS")
                    {
                        count++;
                    }
                }

                // left - top
                if (y > 2 && x > 2)
                {
                    checkWord = grid[x, y] + grid[x - 1, y - 1] + grid[x - 2, y - 2] + grid[x - 3, y - 3];
                    if (checkWord == "XMAS")
                    {
                        count++;
                    }
                }

                // top
                if (x > 2)
                {
                    checkWord = grid[x, y] + grid[x - 1, y] + grid[x - 2, y] + grid[x - 3, y];
                    if (checkWord == "XMAS")
                    {
                        count++;
                    }
                }

                // right - top
                if (x > 2 && y < 137)
                {
                    checkWord = grid[x, y] + grid[x - 1, y + 1] + grid[x - 2, y + 2] + grid[x - 3, y + 3];
                    if (checkWord == "XMAS")
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool hasX_Mas(int x, int y, string[,] grid)
        {
            if (y > 137 || x > 137)
            {
                return false;
            }

            var current = grid[x, y];
            if (current == "M" || current == "S")
            {
                var diagonal1 = grid[x, y] + grid[x + 1, y + 1] + grid[x + 2, y + 2];
                var diagonal2 = grid[x, y + 2] + grid[x + 1, y + 1] + grid[x + 2, y];

                if ((diagonal1 == "MAS" || diagonal1 == "SAM") && (diagonal2 == "MAS" || diagonal2 == "SAM"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

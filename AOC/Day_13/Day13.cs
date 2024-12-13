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

namespace AOC.Day_13
{
    class Position
    {
        public long PosX { get; set; }
        public long PosY { get; set; }

        public Position(long posX, long posY)
        {
            PosX = posX;
            PosY = posY;
        }

        public void Step2()
        {
            PosX += 10_000_000_000_000;
            PosY += 10_000_000_000_000;
        }
    }

    class PrizePosition
    {
        public Position ButtonA { get; set; }
        public Position ButtonB { get; set; }
        public Position Target { get; set; }

        public long ButtonACount { get; set; }
        public long ButtonBCount { get; set; }

        public bool IsSolved { get; set; }

        public PrizePosition()
        {
        }
    }

    public class Day13
    {


        //private string FilePath = "./Day_13/SampleInput.txt";

        private string FilePath = "./Day_13/Input.txt";

        List<PrizePosition> mapping = new List<PrizePosition>();

        public Day13()
        {
            var lines = File.ReadAllLines(FilePath);
            var lineCount = 0;
            PrizePosition prizePosition = new PrizePosition();
            for (int i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];
                if (!string.IsNullOrEmpty(currentLine))
                {
                    var splitted = currentLine.Substring(currentLine.IndexOf(":") + 2).Split(", ", StringSplitOptions.RemoveEmptyEntries);

                    if (lineCount == 0)
                    {
                        var buttonAx = Convert.ToInt64(splitted[0].Substring(2));
                        var buttonAy = Convert.ToInt64(splitted[1].Substring(2));

                        prizePosition.ButtonA = new Position(buttonAx, buttonAy);
                    }

                    if (lineCount == 1)
                    {
                        var buttonBx = Convert.ToInt64(splitted[0].Substring(2));
                        var buttonBy = Convert.ToInt64(splitted[1].Substring(2));

                        prizePosition.ButtonB = new Position(buttonBx, buttonBy);
                    }

                    if (lineCount == 2)
                    {
                        var targetX = Convert.ToInt64(splitted[0].Substring(2));
                        var targetY = Convert.ToInt64(splitted[1].Substring(2));

                        prizePosition.Target = new Position(targetX, targetY);
                        mapping.Add(prizePosition);
                    }

                    lineCount++;
                }
                else
                {
                    prizePosition = new PrizePosition();
                    lineCount = 0;
                }
            }
        }
        public long SolutionPart1()
        {
            var total = 0;

            for (var i = 0; i < mapping.Count; i++)
            {
                var item = mapping[i];
                var lowestScore = int.MaxValue;
                var anyMatch = false;

                var buttonACount = 0;
                var buttonBCount = 0;

                for (int j = 0; j < 101; j++)
                {
                    var leftX = (j * item.ButtonA.PosX) + (100 * item.ButtonB.PosX);
                    if (leftX < item.Target.PosX)
                    {
                        continue;
                    }

                    for (int k = 0; k < 101; k++)
                    {
                        var totalX = (j * item.ButtonA.PosX) + (k * item.ButtonB.PosX);
                        var totalY = (j * item.ButtonA.PosY) + (k * item.ButtonB.PosY);

                        if (totalX == item.Target.PosX && totalY == item.Target.PosY)
                        {
                            var score = (j * 3) + (k * 1);
                            if (lowestScore > score)
                            {
                                buttonACount = j;
                                buttonBCount = k;
                                lowestScore = score;
                                anyMatch = true;
                            }
                        }
                    }
                }

                if (anyMatch && lowestScore > 0)
                {
                    mapping[i].ButtonACount = buttonACount;
                    mapping[i].ButtonBCount = buttonBCount;
                    mapping[i].IsSolved = true;
                    total += lowestScore;
                }
            }

            return total;
        }

        // 33209

        public long SolutionPart2()
        {
            long total = 0;

            for (var i = 0; i < mapping.Count; i++)
            {
                var item = mapping[i];

                var x_val = item.Target.PosX + 10_000_000_000_000;
                var y_val = item.Target.PosY + 10_000_000_000_000;

                //decimal buttonBCount = (y_val * item.ButtonA.PosX - x_val * item.ButtonA.PosY) / (item.ButtonA.PosX * item.ButtonB.PosY - item.ButtonA.PosY * item.ButtonB.PosX);
                //decimal buttonACount = (x_val - item.ButtonB.PosX * buttonBCount) / item.ButtonA.PosX;

                //if(buttonBCount % 1 > 0 && buttonACount % 1 > 0)
                //{
                //    total += 3 * buttonACount + buttonBCount;
                //}

                double denominator = item.ButtonA.PosX * item.ButtonB.PosY - item.ButtonA.PosY * item.ButtonB.PosX;
                double numerator = y_val * item.ButtonA.PosX - x_val * item.ButtonA.PosY;

                double buttonBPresses = numerator / denominator;
                if (buttonBPresses % 1 > 0) continue;

                double buttonAPresses = (x_val - item.ButtonB.PosX * buttonBPresses) / item.ButtonA.PosX;
                if (buttonAPresses % 1 > 0) continue;

                total += (long)(buttonAPresses * 3 + buttonBPresses);


            }

            return total;
        }

        // 83102355665474

        private bool Validate(Position a, long timesA, Position b, long timesB, Position target)
        {
            var aX = a.PosX * timesA;
            var aY = a.PosY * timesA;

            var bX = b.PosX * timesB;
            var bY = b.PosY * timesB;

            var totalX = aX + bX;
            var totalY = aY + bY;

            if (totalX == target.PosX && totalY == target.PosY)
            {
                return true;
            }

            return false;
        }

    }
}



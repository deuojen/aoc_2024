using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.Day_01
{
    public class Day1
    {
        private string FilePath = "./Day_01/Input.txt";
        public int SolutionPart1()
        {
            var lines = File.ReadAllLines(FilePath);
            var listLeft = new List<int>();
            var listRight = new List<int>();

            foreach (var line in lines)
            {
                var splitted = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                listLeft.Add(Convert.ToInt32(splitted[0]));
                listRight.Add(Convert.ToInt32(splitted[1]));
            }

            listLeft.Sort();
            listRight.Sort();

            var total = 0;

            for (int i = 0; i < listLeft.Count; i++)
            {
                total += Math.Abs(listLeft[i] - listRight[i]);
            }


            return total;
        }

        public int SolutionPart2()
        {
            var lines = File.ReadAllLines(FilePath);
            var listLeft = new List<int>();
            var listRight = new List<int>();

            foreach (var line in lines)
            {
                var splitted = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                listLeft.Add(Convert.ToInt32(splitted[0]));
                listRight.Add(Convert.ToInt32(splitted[1]));
            }

            listLeft.Sort();
            listRight.Sort();

            var total = 0;

            for (int i = 0; i < listLeft.Count; i++)
            {
                total += listLeft[i] * listRight.Count(x => x == listLeft[i]);
            }


            return total;
        }
    }
}

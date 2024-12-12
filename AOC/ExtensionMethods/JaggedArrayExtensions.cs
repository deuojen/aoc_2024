using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.ExtensionMethods
{
    public static class JaggedArrayExtensions
    {
        private const string DebugTextFilePath = "debug.txt";

        public static bool IsValidCoordinate<T>(this T[][] array, int x, int y)
        {
            return !(x < 0 || y < 0 || x > array.Length - 1 || y > array[0].Length - 1);
        }

        public static T[] GetVerticalSlice<T>(this T[][] array, int x, int y, int endY)
        {
            var slice = new List<T>();
            var absoluteLength = Math.Abs(y - endY);
            for (var i = 0; i <= absoluteLength; i++)
            {
                slice.Add(array[y + (endY > y ? 1 : -1) * i][x]);
            }

            return [.. slice];
        }

        public static T[] GetDiagonalSlice<T>(this T[][] array, int x, int y, int endX, int endY)
        {
            var slice = new List<T>();
            var absoluteLength = Math.Abs(y - endY);

            for (var i = 0; i <= absoluteLength; i++)
            {
                slice.Add(array[y + (endY > y ? 1 : -1) * i][x + (endX > x ? 1 : -1) * i]);
            }

            return [.. slice];
        }

        public static void ArrayPrinter<T>(this T[][] array)
        {
            foreach (var row in array)
            {
                foreach (var item in row)
                    Console.Write(item);

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public static bool ArraysAreTheSame<T>(this T[][] arr1, T[][] arr2) where T : IComparable<T>
        {
            var row = arr1.Length;
            var col = arr1[0].Length;

            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    if (arr1[i][j].CompareTo(arr2[i][j]) != 0)
                        return false;
                }
            }

            return true;
        }

        public static IEnumerable<(int, int)> GetSurroundingValues<T>(this T[][] array, int x, int y)
        {
            var surroundingCoordinates = new List<(int X, int Y)>
        {
            (x - 1, y - 1), (x - 1, y), (x - 1, y + 1), (x, y - 1), (x, y + 1), (x + 1, y - 1), (x + 1, y), (x + 1, y + 1)
        };

            return surroundingCoordinates.Where(coordinate => array.IsValidCoordinate(coordinate.X, coordinate.Y)).Select(coordinate => (coordinate.X, coordinate.Y));
        }

        public static IEnumerable<(int, int)> GetSurroundingCompassValues<T>(this T[][] array, int x, int y)
        {
            var surroundingCoordinates = new List<(int X, int Y)>
        {
            (x - 1, y), (x, y - 1), (x , y + 1), (x + 1, y)
        };

            return surroundingCoordinates.Where(coordinate => array.IsValidCoordinate(coordinate.X, coordinate.Y)).Select(coordinate => (coordinate.X, coordinate.Y));
        }

        public static IEnumerable<(int, int)> GetSurroundingCompassValues<T>(this T[][] array, (int x, int y) coordinates)
        {
            var surroundingCoordinates = new List<(int X, int Y)>
        {
            (coordinates.x - 1, coordinates.y), (coordinates.x, coordinates.y - 1), (coordinates.x , coordinates.y + 1), (coordinates.x + 1, coordinates.y)
        };

            return surroundingCoordinates.Where(coordinate => array.IsValidCoordinate(coordinate.X, coordinate.Y)).Select(coordinate => (coordinate.X, coordinate.Y));
        }

        public static IEnumerable<(int, int)> GetSurroundingDiagonalValues<T>(this T[][] array, int x, int y)
        {
            var surroundingCoordinates = new List<(int X, int Y)>
        {
            (x - 1, y - 1), (x + 1, y - 1), (x + 1 , y + 1), (x - 1, y + 1)
        };

            return surroundingCoordinates.Where(coordinate => array.IsValidCoordinate(coordinate.X, coordinate.Y)).Select(coordinate => (coordinate.X, coordinate.Y));
        }

        public static void CreateArrayTextFile<T>(this T[][] array)
        {
            File.WriteAllText(DebugTextFilePath, string.Empty);

            foreach (var t in array)
            {
                for (var i = 0; i < array[0].Length; i++)
                {
                    File.AppendAllText(DebugTextFilePath, $"{t[i]}");
                }

                File.AppendAllText(DebugTextFilePath, Environment.NewLine);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    class Program
    {
        static int[,] grid = new int[9, 9];
        static int x = 0;
        static int y = 0;
        static void Main(string[] args)
        {
            DrawGrid(true);
            while (true)
            {
                var key = Console.ReadKey();
                bool done = false;
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        y--;
                        break;
                    case ConsoleKey.DownArrow:
                        y++;
                        break;
                    case ConsoleKey.LeftArrow:
                        x--;
                        break;
                    case ConsoleKey.RightArrow:
                        x++;
                        break;
                    case ConsoleKey.Enter:
                        done = true;
                        break;
                    default:
                        int num = -1;
                        if (int.TryParse(key.KeyChar.ToString(),out num))
                            grid[y, x] = num;
                        break;
                }
                if (done) break;
                DrawGrid(true);
            }
            grid = Solve(grid);
            DrawGrid(false);
            var input = Console.ReadLine();
        }
        

        static void DrawGrid(bool Entering)
        {
            Console.Clear();
            if (Entering) Console.WriteLine("Please fill in the known spaces. Navigate with arrow keys and use numbers to modify values. 0 means empty.");
            for (int row = 0; row < 9; row++)
            {
                if (row != 0 && row % 3 == 0)
                {
                    for (int i = 0; i < 11; i++) Console.Write('■');
                    Console.Write('\n');
                }
                for (int col = 0; col < 9; col++)
                {
                    if (col != 0 && col % 3 == 0) Console.Write('█');
                        
                    if (Entering && (y == row && x == col)) Console.Write('X');
                    else Console.Write(grid[row, col]);
                }
                Console.Write('\n');
            }
            if (Entering) Console.Write("Press ENTER to solve.");
        }
        static int[,] Solve(int[,] Input)
        {
            var Grid = (int[,])Input.Clone();
            List<int>[,] possibleNums = new List<int>[9,9];

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    possibleNums[row, col] = new List<int>();
                }
            }


            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (Grid[row, col] == 0)
                    {
                        for (int i = 1; i <= 9; i++)
                        {
                            if (GetBox(Grid, row, col).Contains(i)) continue;
                            if (GetRow(Grid, row).Contains(i)) continue;
                            if (GetCol(Grid, col).Contains(i)) continue;
                            possibleNums[row, col].Add(i);
                        }
                    }
                }
            }

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (possibleNums[row, col].Count() == 1) 
                        Grid[row, col] = possibleNums[row, col].First();
                    else if (possibleNums[row, col].Count() > 1)
                    {
                        var box = GetBox(possibleNums, row, col);
                        var thisIndex = GetBoxIndex(row, col);
                        foreach (var n in possibleNums[row, col]) 
                        {
                            bool hasDupe = false;
                            for (int i = 0; i < 9; i++)
                            {
                                if (i != thisIndex)
                                {
                                    if (box[i].Contains(n))
                                    {
                                        hasDupe = true;
                                        break;
                                    }
                                }
                            }

                            if (!hasDupe)
                            {
                                Grid[row, col] = n;
                                break;
                            }
                        }
                    }
                }
            }

            if (Matches(Grid, Input))
                return Grid;
            else return Solve(Grid);
        }

        static int[] GetRow(int[,] Grid, int row)
        {
            var r = Enumerable.Range(0, Grid.GetLength(1))
                .Select(x => Grid[row, x])
                .ToArray();
            return r;
        }

        static int[] GetCol(int[,] Grid, int col)
        {
            var c = Enumerable.Range(0, Grid.GetLength(0))
                .Select(x => Grid[x, col])
                .ToArray();
            return c;
        }

        static T[] GetBox<T>(T[,] Grid, int x, int y)
        {
            int secX = x / 3;
            int secY = y / 3;
            List<T> nums = new List<T>();
            for (int X = 3 * secX; X < secX * 3 + 3; X++)
            {
                for (int Y = 3 * secY; Y < secY * 3 + 3; Y++)
                {
                    nums.Add(Grid[X, Y]);
                }
            }
            return nums.ToArray();
        }

        static int GetBoxIndex( int x, int y)
        {
            return y % 3 + (x % 3 * 3);
        }

        static bool Matches(int[,] a, int[,] b)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (a[row, col] != b[row, col]) return false;
                }
            }
            return true;
        }
    }
}

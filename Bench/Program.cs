using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SudokuLibrary;
using SudokuLibrary.Base;
using System.Diagnostics;

//Stopwatch stopwatch1 = Stopwatch.StartNew();
//for (int i = 0; i < 300; i++)
//{
//    Stopwatch stopwatch = Stopwatch.StartNew();
//    var s = new Sudoku9x9(Difficult.Hard, Algorithms.BruteForce);
//    stopwatch.Stop();

//    Console.WriteLine($"{stopwatch.ElapsedMilliseconds * 1e-3:f2}");
//}
//stopwatch1.Stop();
//Console.WriteLine($"Total: {stopwatch1.ElapsedMilliseconds * 1e-3:f2} s");

//Stopwatch stopwatch = Stopwatch.StartNew();
//var s = new Sudoku9x9(Difficult.Easy, Algorithms.BruteForce);
//stopwatch.Stop();

//Console.WriteLine($"{stopwatch.ElapsedMilliseconds*1e-3:f2} s");
//Print(s.Generated);
//Print(s.Solved);

//BenchmarkRunner.Run<Test>();

var a = new BruteForce(9, 3);
var b = new int[9, 9];
Console.WriteLine(a.TrySolve(
    new int[,]
{
            // ne norm
            //{ 0, 0, 8, 0, 0, 0, 0, 3, 0 },
            //{ 7, 4, 0, 0, 6, 0, 0, 0, 0 },
            //{ 0, 0, 9, 0, 5, 0, 0, 4, 1 },
            //{ 9, 0, 0, 0, 0, 0, 0, 7, 0 },
            //{ 8, 0, 0, 0, 7, 0, 0, 0, 3 },
            //{ 0, 0, 4, 0, 0, 0, 0, 0, 5 },
            //{ 2, 0, 5, 4, 0, 7, 0, 9, 6 },
            //{ 0, 0, 0, 0, 0, 0, 5, 0, 0 },
            //{ 0, 9, 0, 0, 0, 0, 0, 1, 0 },
            // norm
            { 0, 0, 0, 0, 0, 0, 4, 5, 2 },
            { 8, 9, 0, 0, 0, 0, 3, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 8, 0, 0 },
            { 0, 0, 8, 5, 0, 4, 0, 0, 0 },
            { 0, 5, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 4, 0, 0, 0, 2, 0, 0, 3 },
            { 0, 0, 0, 0, 3, 0, 0, 2, 0 },
            { 0, 0, 0, 8, 0, 0, 0, 0, 6 },
            { 7, 0, 6, 0, 9, 0, 0, 0, 0 },
            // norm2
            //{ 0, 0, 0, 0, 5, 7, 0, 0, 8 },
            //{ 0, 0, 6, 0, 0, 0, 0, 0, 0 },
            //{ 5, 0, 9, 0, 2, 0, 0, 7, 0 },
            //{ 0, 8, 0, 0, 1, 0, 0, 0, 0 },
            //{ 1, 0, 5, 9, 0, 0, 0, 0, 3 },
            //{ 0, 6, 0, 0, 0, 0, 0, 5, 0 },
            //{ 2, 0, 7, 0, 9, 0, 0, 8, 0 },
            //{ 0, 0, 0, 3, 0, 0, 4, 0, 0 },
            //{ 0, 1, 0, 0, 0, 0, 0, 0, 0 },
}, b));
Print(b);

void Print(int[,] s)
{
    for (int i = 0; i < 9; i++)
    {
        for (int j = 0; j < 9; j++)
        {
            Console.Write(s[i, j] + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}


[MemoryDiagnoser]
public class Test
{
    private Algorithm a = new BruteForce(9, 3);
    private int[,] b = new int[9, 9];

    [Benchmark]
    public bool SudokuBad()
    {
        return a.TrySolve(
            new int[,]
        {
            { 0, 0, 8, 0, 0, 0, 0, 3, 0 },
            { 7, 4, 0, 0, 6, 0, 0, 0, 0 },
            { 0, 0, 9, 0, 5, 0, 0, 4, 1 },
            { 9, 0, 0, 0, 0, 0, 0, 7, 0 },
            { 8, 0, 0, 0, 7, 0, 0, 0, 3 },
            { 0, 0, 4, 0, 0, 0, 0, 0, 5 },
            { 2, 0, 5, 4, 0, 7, 0, 9, 6 },
            { 0, 0, 0, 0, 0, 0, 5, 0, 0 },
            { 0, 9, 0, 0, 0, 0, 0, 1, 0 },
        }, b);
    }

    [Benchmark]
    public bool SudokuGood1()
    {
        return a.TrySolve(
            new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 4, 5, 2 },
            { 8, 9, 0, 0, 0, 0, 3, 0, 0 },
            { 1, 0, 0, 0, 0, 0, 8, 0, 0 },
            { 0, 0, 8, 5, 0, 4, 0, 0, 0 },
            { 0, 5, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 4, 0, 0, 0, 2, 0, 0, 3 },
            { 0, 0, 0, 0, 3, 0, 0, 2, 0 },
            { 0, 0, 0, 8, 0, 0, 0, 0, 6 },
            { 7, 0, 6, 0, 9, 0, 0, 0, 0 },
        }, b);
    }

    [Benchmark]
    public bool SudokuGood2()
    {
        return a.TrySolve(
            new int[,]
        {
            { 0, 0, 0, 0, 5, 7, 0, 0, 8 },
            { 0, 0, 6, 0, 0, 0, 0, 0, 0 },
            { 5, 0, 9, 0, 2, 0, 0, 7, 0 },
            { 0, 8, 0, 0, 1, 0, 0, 0, 0 },
            { 1, 0, 5, 9, 0, 0, 0, 0, 3 },
            { 0, 6, 0, 0, 0, 0, 0, 5, 0 },
            { 2, 0, 7, 0, 9, 0, 0, 8, 0 },
            { 0, 0, 0, 3, 0, 0, 4, 0, 0 },
            { 0, 1, 0, 0, 0, 0, 0, 0, 0 },
        }, b);
    }
}


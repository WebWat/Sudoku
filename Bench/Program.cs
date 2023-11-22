using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SudokuLibrary;
using SudokuLibrary.Base;
using System.Diagnostics;

//using var writer1 = new StreamWriter("test1.txt");
//using var writer2 = new StreamWriter("test2.txt");

//for (int i = 26; i <= 45; i += 9)
//{
//    Console.WriteLine(i);
//    Stopwatch stopwatch1 = Stopwatch.StartNew();

//    for (int j = 0; j < 10; j++)
//    {
//        var s = new Sudoku9x9(Difficult.Hard, Algorithms.BruteForce);
//    }
//    stopwatch1.Stop();

//    //writer1.WriteLine($"{i}");
//    //writer2.WriteLine($"{stopwatch1.ElapsedMilliseconds * 1e-3:f3}");
//    Console.WriteLine($"{stopwatch1.ElapsedMilliseconds * 1e-3:f2}");
//}
//Console.WriteLine("done");

//Stopwatch stopwatch = Stopwatch.StartNew();
//for (int j = 0; j < 200; j++)
//{
//    Stopwatch stopwatch1 = Stopwatch.StartNew();
//    var s1 = new Sudoku9x9(Difficult.Hard);
//    stopwatch1.Stop();
//    Print(s1.Generated);
//    Print(s1.Solved);
//    Thread.Sleep(100);
//    Console.WriteLine($"{stopwatch1.ElapsedMilliseconds * 1e-3:f2}");
//}
//stopwatch.Stop();
//Console.WriteLine($"Total: {stopwatch.ElapsedMilliseconds * 1e-3:f2} s");

BenchmarkRunner.Run<Sudoku>();


//var a = new BruteForce(9, 3);
//var b = new int[9, 9];
//Console.WriteLine(a.TrySolve(
//    new int[,]
//{
//            // ne norm
//            //{ 0, 0, 8, 0, 0, 0, 0, 3, 0 },
//            //{ 7, 4, 0, 0, 6, 0, 0, 0, 0 },
//            //{ 0, 0, 9, 0, 5, 0, 0, 4, 1 },
//            //{ 9, 0, 0, 0, 0, 0, 0, 7, 0 },
//            //{ 8, 0, 0, 0, 7, 0, 0, 0, 3 },
//            //{ 0, 0, 4, 0, 0, 0, 0, 0, 5 },
//            //{ 2, 0, 5, 4, 0, 7, 0, 9, 6 },
//            //{ 0, 0, 0, 0, 0, 0, 5, 0, 0 },
//            //{ 0, 9, 0, 0, 0, 0, 0, 1, 0 },
//            // norm
//            { 0, 0, 0, 0, 0, 0, 4, 5, 2 },
//            { 8, 9, 0, 0, 0, 0, 3, 0, 0 },
//            { 1, 0, 0, 0, 0, 0, 8, 0, 0 },
//            { 0, 0, 8, 5, 0, 4, 0, 0, 0 },
//            { 0, 5, 0, 0, 0, 0, 0, 0, 0 },
//            { 0, 4, 0, 0, 0, 2, 0, 0, 3 },
//            { 0, 0, 0, 0, 3, 0, 0, 2, 0 },
//            { 0, 0, 0, 8, 0, 0, 0, 0, 6 },
//            { 7, 0, 6, 0, 9, 0, 0, 0, 0 },
//            // norm2
//            //{ 0, 0, 0, 0, 5, 7, 0, 0, 8 },
//            //{ 0, 0, 6, 0, 0, 0, 0, 0, 0 },
//            //{ 5, 0, 9, 0, 2, 0, 0, 7, 0 },
//            //{ 0, 8, 0, 0, 1, 0, 0, 0, 0 },
//            //{ 1, 0, 5, 9, 0, 0, 0, 0, 3 },
//            //{ 0, 6, 0, 0, 0, 0, 0, 5, 0 },
//            //{ 2, 0, 7, 0, 9, 0, 0, 8, 0 },
//            //{ 0, 0, 0, 3, 0, 0, 4, 0, 0 },
//            //{ 0, 1, 0, 0, 0, 0, 0, 0, 0 },
//}, b));
//Print(b);

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
        }, out b);
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
        }, out b);
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
        }, out b);
    }
}

[MemoryDiagnoser]
public class Sudoku
{
    //int[,] initial = new int[9, 9]
    //{
    //    { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
    //    { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
    //    { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
    //    { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
    //    { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
    //    { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
    //    { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
    //    { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
    //    { 9, 1, 2, 3, 4, 5, 6, 7, 8 },
    //};
    [Benchmark]
    public void SudokuEasy()
    {
        var a = new Sudoku9x9(Difficult.Easy);
    }

    [Benchmark]
    public void SudokuMedium()
    {
        var a = new Sudoku9x9(Difficult.Medium);
    }

    [Benchmark]
    public void SudokuHard()
    {
        var a = new Sudoku9x9(Difficult.Hard);
    }
}


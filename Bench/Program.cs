using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SudokuLibrary;

//for (int i = 0; i < 500; i++)
//{
//    Stopwatch stopwatch = Stopwatch.StartNew();
//    _ = new Sudoku9x9(Difficult.Hard, Algorithms.BruteForce);
//    stopwatch.Stop();
//}

//BenchmarkRunner.Run<Test>();

var a = new BruteForce(9, 3);
var b = new int[9, 9];
a.TrySolve(
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

for (int i = 0; i < 9; i++)
{
    for (int j = 0; j < 9; j++)
    {
        Console.Write(b[i,j] + " ");
    }
    Console.WriteLine();
}
//a.TrySolve(
//    new int[,]
//{
//            { 0, 0, 0, 0, 0, 0, 4, 5, 2 },
//            { 8, 9, 0, 0, 0, 0, 3, 0, 0 },
//            { 1, 0, 0, 0, 0, 0, 8, 0, 0 },
//            { 0, 0, 8, 5, 0, 4, 0, 0, 0 },
//            { 0, 5, 0, 0, 0, 0, 0, 0, 0 },
//            { 0, 4, 0, 0, 0, 2, 0, 0, 3 },
//            { 0, 0, 0, 0, 3, 0, 0, 2, 0 },
//            { 0, 0, 0, 8, 0, 0, 0, 0, 6 },
//            { 7, 0, 6, 0, 9, 0, 0, 0, 0 },
//}, b);

//Console.ReadKey();


[MemoryDiagnoser]
public class Test
{
    [Benchmark]
    public void Sudoku()
    {
        var a = new BruteForce(9, 9);
        var b = new int[9, 9];
        a.TrySolve (
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
}


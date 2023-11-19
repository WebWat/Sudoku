using SudokuLibrary;

namespace Tests
{
    public class BruteForceTests
    {
        private BruteForce bruteForce = new(9, 3);
        private int[,] solved = new int[9, 9];

        [Fact]
        public void Bad_1()
        {
            var result = bruteForce.TrySolve(new int[,]
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
            }, solved);

            Assert.False(result);
        }

        [Fact]
        public void Good_1()
        {
            var result = bruteForce.TrySolve(new int[,]
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
            }, solved);

            Assert.True(result);
        }

        [Fact]
        public void Good_2()
        {
            var result = bruteForce.TrySolve(new int[,]
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
            }, solved);

            Assert.True(result);
        }
    }
}
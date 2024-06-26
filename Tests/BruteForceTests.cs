using SudokuLibrary.SolvingAlgorithms;

namespace Tests
{
    public class BruteForceTests
    {
        private BruteForce bruteForce = new(9, 3, 150_000);
        private int[,] solved = new int[9, 9];

        [Fact]
        public void Bad_1()
        {
            var actual = bruteForce.TrySolve(new int[,]
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
            }, out solved);

            Assert.False(actual);
        }

        [Fact]
        public void Good_1()
        {
            var actual = bruteForce.TrySolve(new int[,]
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
            }, out solved);

            var expected = new int[9, 9]
            {
                { 6, 7, 3, 9, 1, 8, 4, 5, 2 },
                { 8, 9, 5, 4, 2, 6, 3, 1, 7 },
                { 1, 2, 4, 7, 5, 3, 8, 6, 9 },
                { 3, 6, 8, 5, 7, 4, 2, 9, 1 },
                { 2, 5, 1, 3, 8, 9, 6, 7, 4 },
                { 9, 4, 7, 1, 6, 2, 5, 8, 3 },
                { 4, 8, 9, 6, 3, 1, 7, 2, 5 },
                { 5, 1, 2, 8, 4, 7, 9, 3, 6 },
                { 7, 3, 6, 2, 9, 5, 1, 4, 8 },
            };

            Assert.True(actual);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Assert.True(solved[i, j] == expected[i, j]);
                }
            }
        }

        [Fact]
        public void Good_2()
        {
            var actual = bruteForce.TrySolve(new int[,]
            {
                { 6, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 8, 0, 0, 0, 0, 4, 0, 0, 0 },
                { 0, 4, 0, 0, 6, 0, 0, 0, 8 },
                { 0, 0, 0, 0, 0, 8, 0, 0, 7 },
                { 0, 3, 0, 0, 0, 0, 2, 8, 4 },
                { 1, 0, 2, 0, 0, 0, 3, 0, 0 },
                { 4, 0, 6, 7, 5, 0, 0, 0, 0 },
                { 0, 1, 5, 0, 8, 0, 6, 0, 0 },
                { 0, 0, 8, 3, 1, 0, 5, 0, 0 },
            }, out solved);

            var expected = new int[9, 9]
            {
                { 6, 2, 9, 8, 7, 3, 4, 1, 5 },
                { 8, 5, 1, 9, 2, 4, 7, 6, 3 },
                { 7, 4, 3, 1, 6, 5, 9, 2, 8 },
                { 9, 6, 4, 2, 3, 8, 1, 5, 7 },
                { 5, 3, 7, 6, 9, 1, 2, 8, 4 },
                { 1, 8, 2, 5, 4, 7, 3, 9, 6 },
                { 4, 9, 6, 7, 5, 2, 8, 3, 1 },
                { 3, 1, 5, 4, 8, 9, 6, 7, 2 },
                { 2, 7, 8, 3, 1, 6, 5, 4, 9 },
            };

            Assert.True(actual);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Assert.True(solved[i, j] == expected[i, j]);
                }
            }
        }
    }
}
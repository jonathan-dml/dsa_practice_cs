namespace DsaPractice.Arrays.Tests;

public class Exercise08_SpiralOrderTests
{
    public static TheoryData<int[,], int[]> Matrices => new()
    {
        { new int[0, 0], [] },
        { new[,] { { 7 } }, [7] },
        { new[,] { { 1, 2, 3 } }, [1, 2, 3] },
        { new[,] { { 1 }, { 2 }, { 3 } }, [1, 2, 3] },
        { new[,] { { 1, 2 }, { 3, 4 } }, [1, 2, 4, 3] },
        { new[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }, [1, 2, 3, 6, 9, 8, 7, 4, 5] },
        { new[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } }, [1, 2, 3, 4, 8, 12, 11, 10, 9, 5, 6, 7] },
        { new[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } }, [1, 2, 3, 6, 9, 12, 11, 10, 7, 4, 5, 8] },
        { new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } }, [1, 2, 4, 6, 5, 3] },
    };

    [Theory]
    [MemberData(nameof(Matrices))]
    public void TraversesInSpiralOrder(int[,] matrix, int[] expected)
    {
        Assert.Equal(expected, SpiralMatrix.Traverse(matrix));
    }

    [Fact]
    public void VisitsEveryCellExactlyOnce()
    {
        var matrix = new int[37, 23];
        for (int r = 0; r < 37; r++)
            for (int c = 0; c < 23; c++)
                matrix[r, c] = r * 23 + c;

        var result = SpiralMatrix.Traverse(matrix);

        Assert.Equal(Enumerable.Range(0, 37 * 23), result.Order());
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => SpiralMatrix.Traverse(null!));
    }
}

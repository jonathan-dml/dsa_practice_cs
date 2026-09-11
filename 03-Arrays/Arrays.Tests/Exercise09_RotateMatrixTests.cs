namespace DsaPractice.Arrays.Tests;

public class Exercise09_RotateMatrixTests
{
    public static TheoryData<int[][], int[][]> Matrices => new()
    {
        { [], [] },
        { [[1]], [[1]] },
        { [[1, 2], [3, 4]], [[3, 1], [4, 2]] },
        { [[1, 2, 3], [4, 5, 6], [7, 8, 9]], [[7, 4, 1], [8, 5, 2], [9, 6, 3]] },
        {
            [[5, 1, 9, 11], [2, 4, 8, 10], [13, 3, 6, 7], [15, 14, 12, 16]],
            [[15, 13, 2, 5], [14, 3, 4, 1], [12, 6, 8, 9], [16, 7, 10, 11]]
        },
    };

    [Theory]
    [MemberData(nameof(Matrices))]
    public void RotatesClockwise(int[][] matrix, int[][] expected)
    {
        MatrixRotation.RotateClockwise(matrix);

        Assert.Equal(expected, matrix);
    }

    [Fact]
    public void FourRotationsRestoreTheOriginal()
    {
        int[][] matrix = Enumerable.Range(0, 7).Select(r => Enumerable.Range(0, 7).Select(c => r * 7 + c).ToArray()).ToArray();
        int[][] copy = matrix.Select(row => row.ToArray()).ToArray();

        for (int i = 0; i < 4; i++) MatrixRotation.RotateClockwise(matrix);

        Assert.Equal(copy, matrix);
    }

    [Fact]
    public void ThrowsForNonSquareMatrix()
    {
        Assert.Throws<ArgumentException>(() => MatrixRotation.RotateClockwise([[1, 2, 3], [4, 5, 6]]));
        Assert.Throws<ArgumentException>(() => MatrixRotation.RotateClockwise([[1, 2], [3]]));
    }

    [Fact]
    public void ThrowsForNull()
    {
        Assert.Throws<ArgumentNullException>(() => MatrixRotation.RotateClockwise(null!));
    }
}

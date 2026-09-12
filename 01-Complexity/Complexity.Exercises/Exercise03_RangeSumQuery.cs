namespace DsaPractice.Complexity;

/// <summary>
/// Exercise 03 — Answer inclusive range-sum queries in O(1) after O(n) preprocessing.
/// See README.md for details.
/// </summary>
public class RangeSumQuery
{
    // Suggested field: private readonly long[] _prefix;
    private readonly long[] _prefix;

    public RangeSumQuery(int[] numbers)
    {
        // TODO: validate the input and precompute the prefix sums.
        ArgumentNullException.ThrowIfNull(numbers);

        _prefix = new long[numbers.Length];

        _prefix[0] = numbers[0];
        for(int i = 1; i < numbers.Length; i++)
        {
            _prefix[i] = _prefix[i-1] + numbers[i];
        }

    }

    public long SumRange(int left, int right)
    {
        if(left < 0 || right >= _prefix.Length || left > right)
        {
            throw new ArgumentOutOfRangeException("Invalid parameters");
        }

        return left > 0 ? _prefix[right] - _prefix[left-1] : _prefix[right];
        
    }
}

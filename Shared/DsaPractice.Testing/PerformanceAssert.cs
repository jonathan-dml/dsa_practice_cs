using System.Runtime.ExceptionServices;
using Xunit;

namespace DsaPractice.Testing;

/// <summary>
/// Assertions used by tests that check an exercise meets its target time complexity.
/// </summary>
public static class PerformanceAssert
{
    // A large stack so deep (but correct) recursive solutions don't crash the test host.
    private const int StackSizeBytes = 256 * 1024 * 1024;

    /// <summary>
    /// Runs <paramref name="action"/> on a dedicated thread and fails the test if it doesn't finish
    /// within <paramref name="limit"/>. Exceptions thrown by the action are rethrown unchanged.
    /// </summary>
    public static void CompletesWithin(TimeSpan limit, Action action, string? hint = null)
    {
        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                error = ex;
            }
        }, StackSizeBytes)
        {
            IsBackground = true,
        };

        thread.Start();

        if (!thread.Join(limit))
        {
            var message = $"The code did not finish within {limit.TotalMilliseconds:N0} ms. " +
                          "Your solution is probably slower than the target complexity described in the lesson README.";
            if (hint is not null)
            {
                message += " Hint: " + hint;
            }

            Assert.Fail(message);
        }

        if (error is not null)
        {
            ExceptionDispatchInfo.Capture(error).Throw();
        }
    }

    /// <summary>Convenience overload that returns the value produced by <paramref name="func"/>.</summary>
    public static T CompletesWithin<T>(TimeSpan limit, Func<T> func, string? hint = null)
    {
        T result = default!;
        // Block body so the lambda binds to Action (an expression body would pick this overload again).
        CompletesWithin(limit, () => { result = func(); }, hint);
        return result;
    }
}

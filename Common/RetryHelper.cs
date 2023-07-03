using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace NordPassHomeWorkTAF.Common
{
    public static class RetryHelper
    {
        /// <summary>
        /// Retries a condition function until it returns true or a timeout is reached.
        /// </summary>
        /// <param name="condition">A function that returns a task resulting in a boolean. This function is retried until it returns true or the timeout is reached.</param>
        /// <param name="retryIntervalMilliseconds">The interval in milliseconds to wait between retries.</param>
        /// <param name="timeoutSeconds">The timeout in seconds. If this timeout is reached and the condition function still returns false, the method gives up and returns false.</param>
        /// <param name="logger">An ILogger instance used to log information about the retries and the condition status.</param>
        /// <returns>A task resulting in a boolean. The result is true if the condition function returned true within the timeout, and false otherwise.</returns>
        /// <remarks>
        /// This method uses a stopwatch to keep track of the elapsed time and ensure that the retries stop after the timeout is reached.
        /// </remarks>
        public static async Task<bool> RetryOnConditionAsync(Func<Task<bool>> condition, int retryIntervalMilliseconds, int timeoutSeconds, ILogger logger)
        {
            var stopwatch = Stopwatch.StartNew();
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);

            while (stopwatch.Elapsed < timeout)
            {
                bool conditionMet = await condition();

                if (conditionMet)
                {
                    logger.LogInformation($"Condition met after {stopwatch.Elapsed.TotalSeconds} seconds.");
                    return true;
                }

                logger.LogInformation($"Condition not met yet after {stopwatch.Elapsed.TotalSeconds} seconds. Waiting {retryIntervalMilliseconds / 1000.0} seconds before retrying.");
                await Task.Delay(retryIntervalMilliseconds);
            }

            logger.LogInformation($"Condition not met after {timeout.TotalSeconds} seconds. Giving up.");
            return false;
        }
    }
}

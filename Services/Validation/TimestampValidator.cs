namespace Server.Services.Validation
{
    public interface ITimestampValidator
    {
        bool IsValid(long timestamp);
    }
    public class TimestampValidator : ITimestampValidator
    {
        private readonly long _maxSkewSeconds;

        public TimestampValidator(long maxSkewSeconds = 30)
        {
            _maxSkewSeconds = maxSkewSeconds;
        }

        public bool IsValid(long timestamp)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return Math.Abs(now - timestamp) <= _maxSkewSeconds;
        }
    }
}

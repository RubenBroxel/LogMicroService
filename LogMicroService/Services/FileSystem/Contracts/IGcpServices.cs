public interface IGcpServices
{
    Task SendToGcpBucketAsync(GcpLogFile logFile);
}
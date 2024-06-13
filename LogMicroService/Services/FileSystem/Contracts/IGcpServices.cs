public interface IGcpServices
{
    Task SendToGcpBucketAsync(GcpLogFile logFile);
}

/// <summary>
/// Interfaz para servicios de GCP.
/// </summary>
public interface IGcpServices2
{
    /// <summary>
    /// Envía un archivo de registro a Google Cloud Logging.
    /// </summary>
    /// <param name="logFile">Información del archivo de registro.</param>
    Task SendToGcpLoggingAsync(GcpLogFile2 logFile);
}
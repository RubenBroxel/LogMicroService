using LogMicroService.Services.ServiceManager.Models;

public interface IGcpServices
{
    Task SendToGcpBucketAsync(GcpLogFile logFile);
}

/// <summary>
/// Interfaz para servicios de GCP.
/// </summary>
public interface IGcpServices2
{

    Task ReadLogFile(string filePath);
    /// <summary>
    /// Envía un archivo de registro a Google Cloud Logging.
    /// </summary>
    /// <param name="logFile">Información del archivo de registro.</param>
    ///Task WriteLogAsync(string message, LogSeverity severity = LogSeverity.Info);
}
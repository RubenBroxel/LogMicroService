using LogMicroService.Services.ServiceManager.Models;

public interface IGcpServices
{

    Task ReadLogFile(string filePath);
    /// <summary>
    /// Envía un archivo de registro a Google Cloud Logging.
    /// </summary>
    /// <param name="logFile">Información del archivo de registro.</param>
    ///Task WriteLogAsync(string message, LogSeverity severity = LogSeverity.Info);
}
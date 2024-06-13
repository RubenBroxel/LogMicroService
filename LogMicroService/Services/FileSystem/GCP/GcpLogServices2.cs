using Google.Cloud.Logging.V2;
using System;
using System.Threading.Tasks;

namespace YourNamespace // Reemplaza con el namespace de tu proyecto
{
    /// <summary>
    /// Clase para escribir logs en Google Cloud Logging.
    /// </summary>
    public class GoogleCloudLogger
    {
        private readonly LoggingServiceV2Client _loggingClient;
        private readonly string _logName;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="GoogleCloudLogger"/>.
        /// </summary>
        /// <param name="projectId">El ID del proyecto de Google Cloud.</param>
        /// <param name="logName">El nombre del log en Google Cloud Logging.</param>
        public GoogleCloudLogger(string projectId, string logName)
        {
            _loggingClient = LoggingServiceV2Client.Create();
            _logName = $"projects/{projectId}/logs/{logName}";
        }

        /// <summary>
        /// Escribe un mensaje de log en Google Cloud Logging.
        /// </summary>
        /// <param name="severity">La severidad del mensaje de log.</param>
        /// <param name="message">El mensaje de log.</param>
        /// <exception cref="ArgumentNullException">Se lanza si el mensaje es nulo o vacío.</exception>
        public async Task WriteLogAsync(LogSeverity severity, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message), "El mensaje de log no puede ser nulo o vacío.");
            }

            try
            {
                var logEntry = new LogEntry
                {
                    Severity = Google.Cloud.Logging.Type.LogSeverity.Info,
                    TextPayload = message
                };

                await _loggingClient.WriteLogEntriesAsync(_logName, null, null, new[] { logEntry });
            }
            catch (Exception ex)
            {
                // Manejo de errores. Puedes registrar la excepción en un archivo local, 
                // enviarla a un servicio de monitoreo, etc.
                Console.WriteLine($"Error al escribir en Google Cloud Logging: {ex}"); 
                // Considera lanzar la excepción nuevamente si necesitas detener la ejecución del programa.
                // throw; 
            }
        }
    }
}
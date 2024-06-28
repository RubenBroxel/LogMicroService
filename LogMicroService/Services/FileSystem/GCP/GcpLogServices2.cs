using LogMicroService.Services.ServiceManager.Models; 
using Google.Cloud.Logging.V2;
using Google.Apis.Auth.OAuth2;
using Google.Protobuf.WellKnownTypes;
using Google.Api;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace LogMicroService.Services.FileSystem.GCP; // Reemplaza con el namespace de tu proyecto

/// <summary>
/// Clase para escribir logs en Google Cloud Logging.
/// </summary>
public class GcpLogger: IGcpServices
{
    IConfiguration _configuration;
    private readonly LoggingServiceV2Client _loggingClient;
    private readonly string _logName;
    private readonly string _credentialPath;

    /// <summary>
    /// Constructor que utiliza la ruta al archivo JSON de credenciales.
    /// </summary>
    /// <param name="logName">Nombre del log en GCP Logging.</param>
    /// <param name="credentialPath">Ruta al archivo JSON de credenciales de la cuenta de servicio.</param>
    public GcpLogger( IConfiguration configuration )
    {
        _configuration = configuration;
        _credentialPath = Directory.GetCurrentDirectory() + "/" + _configuration["LogSevice:GCP-ENV-LOG:GCP-LOG:GcpCredential"];
        _logName = _configuration["LogSevice:GCP-ENV-LOG:GCP-LOG:LogName"] ?? String.Empty;
        // Configura las credenciales usando el archivo JSON
        var credential = GoogleCredential.FromFile(_credentialPath);
        _loggingClient = new LoggingServiceV2ClientBuilder
        {
            Credential = credential
        }.Build();
    }

    /// <summary>
    /// Lee el contenido de un archivo .log y lo extrae en una lista de strings, 
    /// donde cada string representa una línea del archivo.
    /// </summary>
    /// <param name="filePath">La ruta del archivo .log a leer.</param>
    /// <returns>Una lista de strings con el contenido del archivo.</returns>
    public async Task ReadLogFile(string filePath)
    {
        //List<string> logContent = new List<string>();

        // Verificar si el archivo existe
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("El archivo .log no se encontró en la ruta especificada.", filePath);
        }
        // Leer el contenido del archivo línea por línea
        using (StreamReader reader = new StreamReader(filePath))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if ( 
                    string.IsNullOrWhiteSpace(line) || 
                   ( line.StartsWith("--") && line.EndsWith("--") && Guid.TryParse(line.Trim('-'), out _ )) ||
                   ( line.StartsWith("--") && Guid.TryParse(line.Trim('-'), out _ )) ||
                   line.StartsWith("Content-Disposition")  

                ) 
                {
                    continue;
                }
                
                //logContent.Add(line);
                await WriteLogInfoAsync(line);
            }
        }
    }



    /// <summary>
    /// Envía un mensaje de log a GCP Logging.
    /// </summary>
    /// <param name="message">Mensaje de log.</param>
    /// <param name="severity">Severidad del log (opcional, por defecto: Info).</param>
    private async Task WriteLogInfoAsync(string message, LogSeverity severity = LogSeverity.Info)
    {
        //var s = context.WithValue(c.Context(), logging.TraceId{}, c.Get("X-Cloud-Trace-Context"));
        try
        {
            var logEntry = new LogEntry
            {
                Resource = new MonitoredResource
                {
                    Type = "global", // Tipo de recurso (por ejemplo, "global", "gce_instance", etc.)
                    Labels =
                    {
                        //{ "project_id", "tu-proyecto-gcp" }
                        { "project_id",_configuration["LogSevice:GCP-ENV-LOG:GCP-LOG:ProjectId"] }
                        
                    }  
                },
                Labels =
                {
                    { "correlationId", "" } // Agrega el CorrelationId como una etiqueta
                },
                Severity = Google.Cloud.Logging.Type.LogSeverity.Info,
                TextPayload = message,
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
            };
                                                                                            //"your-project-id"
            var logName = new LogName(_configuration["LogSevice:GCP-ENV-LOG:GCP-LOG:ProjectId"] ?? String.Empty, _logName);  
            await _loggingClient.WriteLogEntriesAsync(logName, null, null, new[] { logEntry });
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            throw;
        }

    }
}

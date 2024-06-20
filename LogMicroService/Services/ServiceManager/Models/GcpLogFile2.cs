/// <summary>
/// Representa la información de un archivo de registro para GCP.
/// </summary>
public class GcpLogFile2
{
     /// <summary>
    /// Ruta al archivo de credenciales de GCP.
    /// </summary>
    public string? GcpCredential { get; set; }

    /// <summary>
    /// Ruta local del archivo de log.
    /// </summary>
    public string? FileLocalPath { get; set; }

    /// <summary>
    /// Nombre del archivo de log.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Nombre del proyecto de GCP.
    /// </summary>
    public string? GcpProject { get; set; }     
    /// <summary>
    /// Identificador del proyecto de GCP.
    /// </summary>
    public string? Gcpidproject { get; set; }
}

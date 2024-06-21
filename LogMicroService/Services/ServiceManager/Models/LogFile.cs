namespace LogMicroService.Services.ServiceManager.Models;
/*
    Author: Broxel 
    Date: May-09-2014
    Description: Objeto que almacena información de archivo log almacenado en el micro-servicio 
*/
public class LogFile
{
    /// <summary>
    /// Nombre del archivo en texto
    /// </summary>
    public string? FileName  { get; set; } = $"{Guid.NewGuid()}.log"; 

    /// <summary>
    /// Ruta completa del archivo, almacenar en arreglo
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-8.0</remarks>
    public string[]? filePath  { get; set; } = [];
}
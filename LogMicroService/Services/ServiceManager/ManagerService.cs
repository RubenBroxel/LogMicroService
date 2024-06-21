
using LogMicroService.Services.ServiceManager.Models;
using System.Security.Claims;
using LogMicroService.Services.DataBase.Contracts;

/// <summary>
/// Clase ManagerService que implementa la interfaz IManager.
/// </summary>
public class ManagerService: IManager
{
    private readonly IFileServices _fileServices;
    private readonly IJwtSecurity _jwtSecurity;
    private readonly IPermissionServices _permissionService;
    private readonly ICommandService _commandService;
    private readonly IConfiguration _configuration;
    private readonly IGcpServices _gcpServices2;


    public ManagerService
    (
        IFileServices fileServices, IGcpServices gcpServices2, IJwtSecurity jwtSecurity, 
        IPermissionServices permissionServices, ICommandService commandService, IConfiguration configuration
    )
    {
        _fileServices  = fileServices;
        _gcpServices2  = gcpServices2;
        _jwtSecurity   = jwtSecurity;
        _permissionService = permissionServices;
        _commandService    = commandService;
        _configuration     = configuration;
    }

    /// <summary>
    /// Método asíncrono que envía un archivo a un bucket de Google Cloud Storage.
    /// </summary>
    /// <param name="fileStream">Flujo de datos del archivo.</param>
    /// <param name="gcpLogFile">Objeto GcpLogFile con la información del archivo en Google Cloud Storage.</param>
    /// <param name="logFile">Objeto LogFile con la información del archivo.</param>
    /// <returns>Una cadena de texto con el mensaje de éxito o error.</returns>
    public async Task<string> SendToBucketAsync(Stream fileStream, GcpLogFile gcpLogFile, LogFile logFile)
    {  
        try
        {
            var fileContainer   = 
                                Directory.GetCurrentDirectory()+ "/" + 
                                _configuration["LogSevice:GCP-ENV-LOG:LOCAL-STORAGE:PrincipalPath"] + "/" + 
                                _configuration["LogSevice:GCP-ENV-LOG:LOCAL-STORAGE:FolderUsers"] + "/" + 
                                gcpLogFile.FileName;
            string tempfile     = _fileServices.CreateTempfilePath(logFile.FileName ?? "" ,logFile.filePath );
            using var stream    = File.OpenWrite(tempfile);
            await fileStream.CopyToAsync(stream);
            stream.Close();
            await _gcpServices2.ReadLogFile( fileContainer );
            File.Delete(tempfile);
            
            return _configuration["LogSevice:MessageService:SUCCES"] ?? String.Empty;
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.Message.ToString(), ": ",  _configuration["LogSevice:MessageService:FAILD"] ?? String.Empty);
            return _configuration["LogSevice:MessageService:FAILD"] ?? String.Empty;
        } 
    }


    /// <summary>
    /// Método asíncrono que valida las credenciales de un usuario.
    /// </summary>
    /// <param name="credentials">Objeto AccountModelService con las credenciales del usuario.</param>
    /// <returns>Un valor booleano que indica si las credenciales son válidas o no.</returns>
    public async Task<bool> ValidateUser(AccountModelService credentials)
    {
        await _commandService.SaveLogSessionCommandAsync(credentials);
        return _jwtSecurity.IsValidUser(credentials);
    }


    /// <summary>
    /// Método que genera un token JWT para un usuario.
    /// </summary>
    /// <param name="userName">Nombre de usuario.</param>
    /// <returns>Una cadena de texto con el token JWT.</returns>
    public string GenerateToken(string userName)
    {
        var keys= _configuration["LogSevice:Key"] ?? String.Empty;
         return _jwtSecurity.GenerateToken( userName, keys);
    }

    /// <summary>
    /// Método que valida un token JWT.
    /// </summary>
    /// <param name="token">Token JWT a validar.</param>
    /// <returns>Un objeto ClaimsPrincipal si el token es válido, de lo contrario null.</returns>
    public ClaimsPrincipal ValidateAToken(string token)
    {
         return _permissionService.GetPrincipal( token, _configuration["LogSevice:Key"] ?? String.Empty);
    }
}
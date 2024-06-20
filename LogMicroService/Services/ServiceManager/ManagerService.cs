
using LogMicroService.Services.ServiceManager.Models;
using System.Security.Claims;
using LogMicroService.Services.DataBase.Contracts;
//using LogMicroServices.Services.GCP;

/*
    Author: Broxel 
    Date: May-09-2014
    Description: clase principal para funcionamiento del micro-servicio 
*/
public class ManagerService: IManager
{
    private readonly IFileServices _fileServices;
    private readonly IGcpServices  _gcpServices;
    private readonly IJwtSecurity _jwtSecurity;
    private readonly IPermissionServices _permissionService;
    private readonly ICommandService _commandService;
    private readonly IConfiguration _configuration;
    private readonly IGcpServices2 _gcpServices2;


    public ManagerService
    (
        IFileServices fileServices, 
        IGcpServices gcpServices, 
        IGcpServices2 gcpServices2,
        IJwtSecurity jwtSecurity, 
        IPermissionServices permissionServices,
        ICommandService commandService,
        IConfiguration configuration
    )
    {
        _fileServices  = fileServices;
        _gcpServices   = gcpServices;
        _gcpServices2  = gcpServices2;
        _jwtSecurity   = jwtSecurity;
        _permissionService = permissionServices;
        _commandService    = commandService;
        _configuration     = configuration;
    }

    /// <summary>
    /// Método para almacenado de archivo Log de aplicaciones moviles de Broxel y manda a Bucket en GCP
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="gcpLogFile"></param>
    /// <param name="logFile"></param>
    /// <returns>MESSAGE_SUCCESS_OK</returns>
    /// <returns>MESSAGE_ERROR_FATAL</returns>
    public async Task<string> SendToBucketAsync(Stream fileStream, GcpLogFile gcpLogFile, LogFile logFile)
    {  
        try
        {
            var mensaje = _configuration["KeySevice:GCP-ENV-LOG:GCP-DEV:LocalPath"]+ "LogMicroService/"  + gcpLogFile.FileLocalPath + "/" + gcpLogFile.FileName;
            string tempfile  = _fileServices.CreateTempfilePath(logFile.FileName,logFile.filePath);
            using var stream = File.OpenWrite(tempfile);
            await fileStream.CopyToAsync(stream);
            stream.Close();
            await _gcpServices2.ReadLogFile( mensaje );
            //await _gcpServices.SendToGcpBucketAsync(gcpLogFile);
            
            return _configuration["KeySevice:MessageService:SUCCES"] ?? String.Empty;
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.Message.ToString(), ": ",  _configuration["KeySevice:MessageService:FAILD"] ?? String.Empty);
            return _configuration["KeySevice:MessageService:FAILD"] ?? String.Empty;
        } 
    }


    /// <summary>
    /// Método para Validar credenciales de acceso del usuario
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns>Bool</returns>
    public async Task<bool> ValidateUser(AccountModelService credentials)
    {
        await _commandService.SaveLogSessionCommandAsync(credentials);
        return _jwtSecurity.IsValidUser(credentials);
    }


    /// <summary>
    /// Método para generar llave token para acceso seguro al microservicio con el fin de poder ser consumido
    /// </summary>
    /// <param name="userName"></param>
    /// <returns>string</returns>
    public string GenerateToken(string userName)
    {
        var keys= _configuration["KeySevice:Key"] ?? String.Empty;
         return _jwtSecurity.GenerateToken( userName, keys);
    }

    public ClaimsPrincipal ValidateAToken(string token)
    {
         return _permissionService.GetPrincipal( token, _configuration["KeySevice:Key"] ?? String.Empty);
    }
}
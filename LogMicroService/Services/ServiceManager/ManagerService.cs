/*
    Author: Broxel 
    Date: May-09-2014
    Description: clase principal para funcionamiento del micro-servicio 
*/
using System.Security.Claims;

public class ManagerService: IManager
{
    private readonly IFileServices _fileServices;
    private readonly IGcpServices  _gcpServices;
    private readonly IJwtSecurity _jwtSecurity2;
    private readonly IPermissionServices _permissionService;
    
    private readonly string MESSAGE_ERROR_FALTAL = "Upps!, Algo Orurrio...";
    private readonly string MESSAGE_SUCCESS_OK   = "En hora buena! el Archivo llego a su destino";
    private readonly string SECRET_BASE64_VALID  = "eyJhbGciOiJSUzI1NiIsImtpZCI6IlRlc3RDZXJ0IiwidHlwIjoiYXQrand0In0=";


    public ManagerService(IFileServices fileServices, IGcpServices gcpServices, IJwtSecurity jwtSecurity2, IPermissionServices permissionServices)
    {
        _fileServices = fileServices;
        _gcpServices  = gcpServices;
        _jwtSecurity2 = jwtSecurity2;
        _permissionService = permissionServices;
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
            string tempfile  = _fileServices.CreateTempfilePath(logFile.FileName,logFile.filePath);
            using var stream = File.OpenWrite(tempfile);
            await fileStream.CopyToAsync(stream);
            stream.Close();
            await _gcpServices.SendToGcpBucketAsync(gcpLogFile);
            
            return MESSAGE_SUCCESS_OK;
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.Message.ToString(), ": ", MESSAGE_ERROR_FALTAL);
            return MESSAGE_ERROR_FALTAL;
        } 
    }


    /// <summary>
    /// Método para Validar credenciales de acceso del usuario
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns>Bool</returns>
    public bool ValidateUser(UserCredential credentials)
    {
        return _jwtSecurity2.IsValidUser(credentials);
    }


    /// <summary>
    /// Método para generar llave token para acceso seguro al microservicio con el fin de poder ser consumido
    /// </summary>
    /// <param name="userName"></param>
    /// <returns>string</returns>
    public string GenerateToken(string userName)
    {
         return _jwtSecurity2.GenerateToken( userName, SECRET_BASE64_VALID);
    }

    public ClaimsPrincipal ValidateAToken(string token)
    {
         return _permissionService.GetPrincipal( token,  SECRET_BASE64_VALID);
    }
}
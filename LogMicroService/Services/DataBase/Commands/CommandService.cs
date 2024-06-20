using LogMicroService.Services.ServiceManager.Models;
using LogMicroService.Services.ServiceManager.ModelViews;
using LogMicroService.Services.DataBase.Contracts;

namespace LogMicroService.Services.DataBase.Commands;
public class CommandService: ICommandService
{
    private readonly LogMicroServiceSessionsContext _context;
    public CommandService(LogMicroServiceSessionsContext context)
    {
        _context = context;
    }

    public async Task SaveLogSessionCommandAsync(AccountModelService account)
    {
        try
        {
            var catalogItem = new LogServiceSession
            {
                TokenSession    = account.AppToken,
                AppName         = account.AppName,
                AppPackage      = account.AppPackage,
                AppBuild        = account.AppBuild,
                AppVersion      = account.AppVersion,
                IpAddress       = account.AppIpAddress,
                LocationSession = account.AppLocation,
                CountrySession  = account.AppCountry,
                DateSession     = DateTime.Today,
                UserSession     = 1,
            };

            _context.Set<LogServiceSession>().Add(catalogItem);
            await _context.SaveChangesAsync(); // Guarda los cambios de forma asíncrona
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString()); // Muestra información detallada de la excepción
            // Manejo de errores, como registrar la excepción en un archivo o base de datos
        }
    }


    public async Task SaveLogFileCommand( GcpLogFile2 file )
    {
        long filesize = 0;
        /*var catalogItem = new LogFileStorage() 
        { 
            //Idlogfile = "",
            //Idsession = "",
            Logfilename = file.FileName ?? "",
            Logfilesize = filesize,
            Gcpinstance = file.GcpProject,
            Gcpidproject = file.Gcpidproject,
            //Uploadstarttime = ,

        };*/
        //await _context.AddAsync(catalogItem);
        //await _context.SaveChangesAsync();
    }


}
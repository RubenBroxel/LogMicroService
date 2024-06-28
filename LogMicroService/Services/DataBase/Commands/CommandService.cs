using LogMicroService.Services.ServiceManager.Models;
using LogMicroService.Services.ServiceManager.ModelViews;
using LogMicroService.Services.DataBase.Contracts;

namespace LogMicroService.Services.DataBase.Commands;
public class CommandService: ICommandService
{

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
                UserSession     = Convert.ToInt32(account.AppUser),
            };

            using (var db = new LogMicroServiceSessionsContext())
            {
                var customers = db.Set<LogServiceSession>().Add(catalogItem);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
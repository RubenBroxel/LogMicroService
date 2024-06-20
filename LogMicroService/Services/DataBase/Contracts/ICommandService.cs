using LogMicroService.Services.ServiceManager.Models;

namespace LogMicroService.Services.DataBase.Contracts;
public interface ICommandService
{
    Task SaveLogSessionCommandAsync(AccountModelService account);
    Task SaveLogFileCommand(GcpLogFile2 file );
}
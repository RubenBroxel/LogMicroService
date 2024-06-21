using LogMicroService.Services.ServiceManager.Models;

namespace LogMicroService.Services.DataBase.Contracts;
public interface ICommandService
{
    Task SaveLogSessionCommandAsync(AccountModelService account);
}
using LogMicroService.Services.ServiceManager.Models;
using System.Security.Claims;

public interface IManager
{
    Task<string> SendToBucketAsync(Stream fileStream, GcpLogFile gcpLogFile, LogFile logFile );
    Task<bool> ValidateUser(AccountModelService credentials);
    string GenerateToken( string userName);

    ClaimsPrincipal ValidateAToken(string token);
}
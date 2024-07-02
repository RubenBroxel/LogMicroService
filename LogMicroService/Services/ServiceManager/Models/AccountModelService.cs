namespace LogMicroService.Services.ServiceManager.Models;    
    
public class AccountModelService
{
    public string? AppService  {get;set;}
    public string? AppName      {get; set;} 
    public string? AppPackage   {get; set;} 
    public string? AppVersion   {get; set;}
    public string? AppBuild     {get; set;}
    public string? AppIpAddress {get; set;} 
    public string? AppCountry   {get; set;}
    public string? AppLocation  {get; set;} 
    public string? AppToken { get; set; }
    public string? AppUser { get; set; }
    public string   CorrelationId {get; set;}
}
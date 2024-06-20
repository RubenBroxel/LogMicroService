namespace LogMicroService.Services.ServiceManager.Models;    
    
public class AccountModelService
{
    public string? AppService  {get;set;}
    public string? AppName      {get; set;} //= AppInfo.Current.Name;
    public string? AppPackage   {get; set;} //= AppInfo.Current.PackageName;
    public string? AppVersion   {get; set;} //= AppInfo.Current.VersionString;
    public string? AppBuild     {get; set;} //= AppInfo.Current.BuildString;
    public string? AppIpAddress {get; set;} 
    public string? AppCountry   {get; set;}
    public string? AppLocation  {get; set;} 
    public string? AppToken { get; set; }
    public string? AppUser { get; set; }
}
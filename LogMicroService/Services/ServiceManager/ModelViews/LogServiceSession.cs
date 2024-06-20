using System;
using System.Collections.Generic;

namespace LogMicroService.Services.ServiceManager.ModelViews;

public partial class LogServiceSession
{
    public int IdSession { get; set; }

    public string? TokenLog { get; set; }

    public string? TokenSession { get; set; }

    public int? UserSession { get; set; }

    public string? AppName { get; set; }

    public string? AppBuild { get; set; }

    public string? AppPackage { get; set; }

    public string? AppVersion { get; set; }

    public string? IpAddress { get; set; }

    public string? LocationSession { get; set; }

    public string? CountrySession { get; set; }

    public DateTime? DateSession { get; set; }
}

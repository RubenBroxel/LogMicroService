using LogMicroService.Services.DataBase.Contracts;
using LogMicroService.Services.DataBase.Commands;
using LogMicroService.Services.ServiceManager.Models;
using LogMicroService.Services.FileSystem.GCP;
using LogMicroService.Services.FileSystem.Local;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Bloque: Inyecciones de Dependencias
builder.Services.AddSingleton<IFileServices, LocalServices>();
builder.Services.AddSingleton<IJwtSecurity, JwtService>();
builder.Services.AddSingleton<IPermissionServices, PermissionServices>();
builder.Services.AddSingleton<IManager, ManagerService>();
builder.Services.AddSingleton<ICommandService, CommandService>();
builder.Services.AddSingleton<IGcpServices, GcpLogger>();

//Bloque: Variables de Entorno de configuración de GCP para los Buckets
var Local = builder.Configuration.GetSection("LogSevice:GCP-ENV-LOG:LOCAL-STORAGE");
var principal   = Local.GetValue<string>("PrincipalPath");
var user        = Local.GetValue<string>("FolderUsers");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok("OK")).ShortCircuit();

app.UseHttpsRedirection();

app.MapPost("api/logservice", async ( Stream logFile, HttpContext httpContext, IManager manager) =>
{
    var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    var validattion = manager.ValidateAToken(token);
    
    // Verifica si el token es válido
    if (validattion != null)
    {
        LogFile    tempFile   = new LogFile();
        GcpLogFile gcpLogFile = new GcpLogFile(); 

        tempFile.filePath = [principal ?? "", user ?? ""]; 
        gcpLogFile.FileLocalPath = principal+"/"+user;
        gcpLogFile.FileName      = tempFile.FileName;

        await manager.SendToBucketAsync(logFile,gcpLogFile,tempFile);
        // Accede a los datos seguros
        var username = validattion?.Identity?.Name;
        return Results.Ok($"Hola, {username}! El archivo subio con éxito.");
    }
    return Results.Unauthorized();
});


app.MapPost("/api/auth/token", async (AccountModelService credentials, IManager manager) =>
{
    // Verifica las credenciales del usuario
    if ( await manager.ValidateUser(credentials))
    {
        // Genera y retorna el token
        var token = manager.GenerateToken(credentials.AppService ?? "" );
        return Results.Ok(new { Token = token });
    }
    return Results.Unauthorized();
});



// Endpoint para datos seguros de pruebas
app.MapGet("/api/secure", (HttpContext httpContext, IManager manager) =>
{
    var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    var principal = manager.ValidateAToken(token);
    
    // Verifica si el token es válido
    if (principal != null)
    {
        // Accede a los datos seguros
        var username = principal?.Identity?.Name;
        return Results.Ok($"Hola, {username}! Este es un dato seguro.");
    }
    return Results.Unauthorized();
});

app.Run();


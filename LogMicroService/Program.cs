var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Bloque: Inyecciones de Dependencias
builder.Services.AddSingleton<IFileServices, LocalServices>();
builder.Services.AddSingleton<IGcpServices, GcpServices>();
builder.Services.AddSingleton<IJwtSecurity, JwtService>();
builder.Services.AddSingleton<IPermissionServices, PermissionServices>();
builder.Services.AddSingleton<IManager, ManagerService>();

//Bloque: Variables de Entorno de configuración de GCP para los Buckets
var GCP   = builder.Configuration.GetSection("GCP-ENV-LOG:GCP-DEV");
var Local = builder.Configuration.GetSection("GCP-ENV-LOG:LOCAL-STORAGE");


var bucket      = GCP.GetValue<string>("GcpBucketName");
var path        = GCP.GetValue<string>("LocalPath");
var credential  = GCP.GetValue<string>("GcpCredential");
var type        = GCP.GetValue<string>("GcpFileType");
var folder      = GCP.GetValue<string>("GcpBucketFolder");
var principal   = Local.GetValue<string>("PrincipalPath");
var user        = Local.GetValue<string>("FolderUsers");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", async (HttpContext context) => await context.Response.WriteAsync("Ok")).ShortCircuit();


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

        tempFile.FileName = $"{Guid.NewGuid()}.log";
        tempFile.filePath = [principal, user]; 
        
        gcpLogFile.GcpBucket     = bucket;
        gcpLogFile.GcpCredential = credential;
        gcpLogFile.FileLocalPath = principal+"/"+user;
        gcpLogFile.FileType      = type;
        gcpLogFile.FileName      =tempFile.FileName;
        gcpLogFile.GcpFolder     = folder;

        await manager.SendToBucketAsync(logFile,gcpLogFile,tempFile);
        // Accede a los datos seguros
        var username = validattion.Identity.Name;
        return Results.Ok($"Hola, {username}! El archivo subio con éxito.");
    }
    return Results.Unauthorized();
});


app.MapPost("/api/auth/token", (AccountModelService credentials, IManager manager) =>
{
    // Verifica las credenciales del usuario
    if ( manager.ValidateUser(credentials))
    {
        // Genera y retorna el token
        var token = manager.GenerateToken(credentials.AppService);
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
        var username = principal.Identity.Name;
        return Results.Ok($"Hola, {username}! Este es un dato seguro.");
    }
    return Results.Unauthorized();
});

app.Run();

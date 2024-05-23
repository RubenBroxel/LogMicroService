

public class LocalServices: IFileServices
{
    //Método para creación de ruta local temporal
    public string CreateTempfilePath(string fileName, string[] paths)
    {
        var directoryPath = Path.Combine(paths);
        
        if ( !Directory.Exists(directoryPath) ) Directory.CreateDirectory(directoryPath);
        
        return Path.Combine(directoryPath, fileName);
    }
}
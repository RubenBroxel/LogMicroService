
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

public class GcpServices : IGcpServices
{
    private readonly IConfiguration _configuration;
    public GcpServices(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendToGcpBucketAsync(GcpLogFile logFile)
    {
        // Explicitly use service account credentials by specifying the private key file.
        // The service account should have Object Manage permissions for the bucket.
        GoogleCredential credential;
        using (var jsonStream = new FileStream( logFile.GcpCredential ?? "", FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            credential = GoogleCredential.FromStream( jsonStream );
        }
        var storageClient = StorageClient.Create( credential );

        using (var fileStream = new FileStream( logFile.FileLocalPath + "/" + logFile.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            //subir recurso a bucket en GCP
            //1.-Nombre del Bucket
            //2.-Ruta destino dentro del Bucket con el nombre del archivo 
            //3.-Tipo de recurso 
            //4.-Recurso
            await storageClient.UploadObjectAsync( logFile.GcpBucket, logFile.GcpFolder + "/" + logFile.FileName, logFile.FileType, fileStream);
        }

        // Lista de objetos en ruta bucket 
        foreach (var obj in storageClient.ListObjects( logFile.GcpBucket, "") )
        {
            Console.WriteLine(obj.Name);
        }

        // no eliminar código es para descargar archivo
        /*using (var fileStream = File.Create("Program-copy.cs"))
        {
            storageClient.DownloadObject(bucketName, "Program.cs", fileStream);
        }
        foreach (var obj in Directory.GetFiles("."))
        {
            Console.WriteLine(obj);
        }*/
    }

}
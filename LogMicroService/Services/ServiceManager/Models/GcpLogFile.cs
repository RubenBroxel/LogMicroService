/*
    Author: Broxel 
    Date: May-09-2014
    Description: Objeto para contener información referente a GCP y sus caracteristicas
*/

public class GcpLogFile
{
    //Nombre del Archivo
    public string FileName      { get; set; } = null;
    //Tipo de archivo que se subira al Bucket
    public string FileType      { get; set; } = null;
    //Ubicación local de almacenamiento del microservicio
    public string FileLocalPath { get; set; } = null;
    //Nombre del Bucket en GCP
    public string GcpBucket     { get; set; } = null;
    //Ruta o Nombre del archivo que contenga las credenciales de GCP
    public string GcpCredential { get; set; } = null;
    //Nombre del Folder dentro del Bucket de GCP
    public string GcpFolder     { get; set; } = null;
}
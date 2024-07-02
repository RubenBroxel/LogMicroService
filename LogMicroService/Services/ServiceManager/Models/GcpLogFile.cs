/*
    Author: Broxel 
    Date: May-09-2014
    Description: Objeto para contener información referente a GCP y sus caracteristicas
*/


public class GcpLogFile
{
    //Nombre del Archivo
    public string? FileName      { get; set; }
    //Tipo de archivo que se subira al Bucket
    public string? FileType      { get; set; }
    //Ubicación local de almacenamiento del microservicio
    public string? FileLocalPath { get; set; } 
    //Nombre del Bucket en GCP
    public string? GcpCredential { get; set; }
    //Nombre del Folder dentro del Bucket de GCP
    public string? GcpFolder     { get; set; }
    
}
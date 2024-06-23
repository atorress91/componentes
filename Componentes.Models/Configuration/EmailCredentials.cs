namespace Componentes.Models.Configuration;

public class EmailCredentials
{
    public string? From { get; set; }
    public string? Password { get; set; }
    public string? Smtp { get; set; }
    public int Port { get; set; }
}
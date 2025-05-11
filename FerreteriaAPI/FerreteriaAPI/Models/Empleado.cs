namespace FerreteriaAPI.Models;
using System.Text.Json.Serialization;


public class Empleado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Contrasena { get; set; }
}


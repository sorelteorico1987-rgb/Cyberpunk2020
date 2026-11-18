using System.Text.Json;
using NEON2020.Modelo;
namespace NEON2020.Servicios;
public sealed class PartidaDto
{
    public int Version { get; set; } = 1;
    public string Nombre { get; set; } = "";
    public int Rol { get; set; }
    public int Vida { get; set; }
    public int Energia { get; set; }
    public int Creditos { get; set; }
    public int Humanidad { get; set; }
    public int Escudo { get; set; }
    public int Botiquines { get; set; }
    public int Estimulantes { get; set; }
    public int Capitulo { get; set; }
    public int Alerta { get; set; }
    public int Reputacion { get; set; }
    public bool Credencial { get; set; }
    public bool Pista { get; set; }
    public bool Datos { get; set; }
}
public sealed class GuardadoPartida
{
    private readonly string _ruta;
    private static readonly JsonSerializerOptions Opciones = new() { WriteIndented = true };
    public string Ruta => _ruta;
    public GuardadoPartida(string? ruta = null)
    {
        _ruta = ruta ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "NEON2020", "partida.json");
    }
    public bool Guardar(EstadoJuego estado, out string mensaje)
    {
        ArgumentNullException.ThrowIfNull(estado);
        if (!estado.Jugador.EstaVivo)
        {
            mensaje = "Una partida con el protagonista derrotado no puede guardarse.";
            return false;
        }
        var j = estado.Jugador;
        var datos = new PartidaDto
        {
            Nombre = j.Nombre, Rol = (int)j.Rol, Vida = j.VidaActual,
            Energia = j.Energia, Creditos = j.Creditos, Humanidad = j.Humanidad,
            Escudo = j.EscudoTemporal, Botiquines = j.Inventario.Botiquines,
            Estimulantes = j.Inventario.Estimulantes, Capitulo = estado.Capitulo,
            Alerta = estado.Alerta, Reputacion = estado.Reputacion,
            Credencial = estado.TieneCredencial, Pista = estado.TienePista,
            Datos = estado.TieneDatos
        };
        string temporal = _ruta + ".tmp";
        try
        {
            string? carpeta = Path.GetDirectoryName(_ruta);
            if (!string.IsNullOrEmpty(carpeta)) Directory.CreateDirectory(carpeta);
            File.WriteAllText(temporal, JsonSerializer.Serialize(datos, Opciones));
            File.Move(temporal, _ruta, true);
            mensaje = "Partida guardada: " + _ruta;
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            mensaje = "No fue posible guardar la partida: " + ex.Message;
            return false;
        }
        finally
        {
            try { if (File.Exists(temporal)) File.Delete(temporal); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
    public bool Cargar(out EstadoJuego? estado, out string mensaje)
    {
        estado = null;
        try
        {
            if (!File.Exists(_ruta))
            {
                mensaje = "Todavía no existe una partida guardada.";
                return false;
            }
            var info = new FileInfo(_ruta);
            if (info.Length > 20_000)
                throw new InvalidDataException("El archivo de guardado supera el tamaño permitido.");
            var dto = JsonSerializer.Deserialize<PartidaDto>(File.ReadAllText(_ruta));
            if (dto is null || dto.Version != 1 || !Enum.IsDefined(typeof(Rol), dto.Rol) ||
                string.IsNullOrWhiteSpace(dto.Nombre) || dto.Nombre.Length is < 3 or > 18 ||
                !dto.Nombre.All(c => char.IsLetterOrDigit(c) || c is ' ' or '-'))
                throw new InvalidDataException("La versión, rol o nombre de la partida no es válido.");
            Jugador jugador = FabricaJugadores.Crear((Rol)dto.Rol, dto.Nombre);
            jugador.RestaurarRecursos(dto.Vida, dto.Energia, dto.Creditos,
                dto.Humanidad, dto.Escudo, dto.Botiquines, dto.Estimulantes);
            var reconstruido = new EstadoJuego(jugador);
            reconstruido.Restaurar(dto.Capitulo, dto.Alerta, dto.Reputacion,
                dto.Credencial, dto.Pista, dto.Datos);
            estado = reconstruido;
            mensaje = "Partida cargada correctamente.";
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException or
            InvalidDataException or ArgumentException or NotSupportedException)
        {
            mensaje = "El guardado no se pudo cargar de forma segura: " + ex.Message;
            return false;
        }
    }
}
namespace NEON2020.Modelo;
public sealed class EstadoJuego
{
    public Jugador Jugador { get; }
    public int Capitulo { get; private set; } = 1;
    public int Alerta { get; private set; }
    public int Reputacion { get; private set; }
    public bool TieneCredencial { get; private set; }
    public bool TienePista { get; private set; }
    public bool TieneDatos { get; private set; }
    public EstadoJuego(Jugador jugador)
    {
        Jugador = jugador ?? throw new ArgumentNullException(nameof(jugador));
    }
    public void Avanzar()
    {
        if (Capitulo >= 4) throw new InvalidOperationException("No hay más capítulos.");
        Capitulo++;
    }
    public void CambiarAlerta(int puntos) => Alerta = Math.Clamp(Alerta + puntos, 0, 10);
    public void CambiarReputacion(int puntos) => Reputacion = Math.Clamp(Reputacion + puntos, -10, 10);
    public void ConseguirCredencial() => TieneCredencial = true;
    public void ConseguirPista() => TienePista = true;
    public void ConseguirDatos() => TieneDatos = true;
    internal void Restaurar(int capitulo, int alerta, int reputacion,
        bool credencial, bool pista, bool datos)
    {
        if (capitulo < 1 || capitulo > 4 || alerta < 0 || alerta > 10 ||
            reputacion < -10 || reputacion > 10 || (capitulo == 4 && !datos) ||
            (capitulo < 3 && datos))
            throw new InvalidDataException("Datos narrativos inconsistentes en el guardado.");
        Capitulo = capitulo;
        Alerta = alerta;
        Reputacion = reputacion;
        TieneCredencial = credencial;
        TienePista = pista;
        TieneDatos = datos;
    }
}
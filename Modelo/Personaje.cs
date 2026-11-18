namespace NEON2020.Modelo;

/// <summary>ABSTRACCIÓN: lo que comparten protagonistas y enemigos.</summary>
public abstract class Personaje
{
    public string Nombre { get; }
    public Estadisticas Estadisticas { get; }
    public int VidaMaxima { get; }
    public int VidaActual { get; private set; }
    public bool EstaVivo => VidaActual > 0;

    protected Personaje(string nombre, Estadisticas estadisticas, int vidaMaxima)
    {
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length > 32)
            throw new ArgumentException("El nombre es obligatorio (máximo 32 caracteres).", nameof(nombre));
        if (vidaMaxima < 1 || vidaMaxima > 100)
            throw new ArgumentOutOfRangeException(nameof(vidaMaxima));
        Nombre = nombre;
        Estadisticas = estadisticas ?? throw new ArgumentNullException(nameof(estadisticas));
        VidaMaxima = vidaMaxima;
        VidaActual = vidaMaxima;
    }

    public virtual int RecibirDanio(int puntos)
    {
        if (puntos < 0) throw new ArgumentOutOfRangeException(nameof(puntos));
        int real = Math.Min(VidaActual, puntos);
        VidaActual -= real;
        return real;
    }

    public int Curar(int puntos)
    {
        if (puntos < 0) throw new ArgumentOutOfRangeException(nameof(puntos));
        int real = Math.Min(VidaMaxima - VidaActual, puntos);
        VidaActual += real;
        return real;
    }

    internal void RestaurarVida(int puntos)
    {
        if (puntos < 1 || puntos > VidaMaxima)
            throw new InvalidDataException("Vida inválida en la partida guardada.");
        VidaActual = puntos;
    }
}
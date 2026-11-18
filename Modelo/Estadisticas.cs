namespace NEON2020.Modelo;

public enum Caracteristica { Inteligencia, Reflejos, Tecnica, Empatia }
public enum Habilidad { Pistolas, Interface, Calle, Ingenieria }
public enum Rol { Mercenario = 1, Netrunner = 2, Arreglador = 3 }

/// <summary>Características del personaje. No pueden modificarse directamente.</summary>
public sealed class Estadisticas
{
    public int Inteligencia { get; }
    public int Reflejos { get; }
    public int Tecnica { get; }
    public int Empatia { get; }

    public Estadisticas(int inteligencia, int reflejos, int tecnica, int empatia)
    {
        Validar(inteligencia); Validar(reflejos); Validar(tecnica); Validar(empatia);
        Inteligencia = inteligencia;
        Reflejos = reflejos;
        Tecnica = tecnica;
        Empatia = empatia;
    }

    private static void Validar(int numero)
    {
        if (numero < 1 || numero > 10)
            throw new ArgumentOutOfRangeException(nameof(numero), "Las características van de 1 a 10.");
    }

    public int Obtener(Caracteristica caracteristica) => caracteristica switch
    {
        Caracteristica.Inteligencia => Inteligencia,
        Caracteristica.Reflejos => Reflejos,
        Caracteristica.Tecnica => Tecnica,
        Caracteristica.Empatia => Empatia,
        _ => throw new ArgumentOutOfRangeException(nameof(caracteristica))
    };
}
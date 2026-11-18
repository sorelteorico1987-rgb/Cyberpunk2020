using NEON2020.Modelo;
using NEON2020.Servicios;
namespace NEON2020.Motores;
public sealed record ResultadoTirada(int Dado, int Caracteristica, int Habilidad, int Modificador, int Dificultad)
{
    public int Total => Dado + Caracteristica + Habilidad + Modificador;
    public bool Exito => Total >= Dificultad;
    public override string ToString() =>
        $"1D10 ({Dado}) + característica ({Caracteristica}) + habilidad ({Habilidad})" +
        $" + modificador ({Modificador}) = {Total} / dificultad {Dificultad}" +
        (Exito ? " [ÉXITO]" : " [FALLO]");
}
public sealed class MotorReglas
{
    private readonly IDado _dado;
    public MotorReglas(IDado dado) => _dado = dado ?? throw new ArgumentNullException(nameof(dado));
    public ResultadoTirada Resolver(Jugador jugador, Caracteristica caracteristica,
        Habilidad habilidad, int dificultad)
        => Resolver(jugador, caracteristica, habilidad, dificultad, 0);
    public ResultadoTirada Resolver(Jugador jugador, Caracteristica caracteristica,
        Habilidad habilidad, int dificultad, int modificador)
    {
        ArgumentNullException.ThrowIfNull(jugador);
        if (dificultad < 1 || dificultad > 40)
            throw new ArgumentOutOfRangeException(nameof(dificultad));
        if (modificador < -10 || modificador > 10)
            throw new ArgumentOutOfRangeException(nameof(modificador));
        return new ResultadoTirada(_dado.Lanzar(10),
            jugador.Estadisticas.Obtener(caracteristica), jugador.Habilidad(habilidad),
            modificador, dificultad);
    }
}
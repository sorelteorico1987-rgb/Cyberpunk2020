using NEON2020.Servicios;
namespace NEON2020.Modelo;
public abstract class Enemigo : Personaje
{
    private int _turnosAturdido;
    public int Defensa { get; }
    protected Enemigo(string nombre, Estadisticas estadisticas, int vida, int defensa)
        : base(nombre, estadisticas, vida)
    {
        if (defensa < 0 || defensa > 5) throw new ArgumentOutOfRangeException(nameof(defensa));
        Defensa = defensa;
    }
    public void Aturdir(int turnos)
    {
        if (turnos < 0 || turnos > 3) throw new ArgumentOutOfRangeException(nameof(turnos));
        _turnosAturdido = Math.Max(_turnosAturdido, turnos);
    }
    public bool PerderTurnoPorAturdimiento()
    {
        if (_turnosAturdido == 0) return false;
        _turnosAturdido--;
        return true;
    }
    public abstract string Atacar(Jugador jugador, IDado dado);
}
public sealed class GuardiaCorporativo : Enemigo
{
    public GuardiaCorporativo() : base("Guardia corporativo", new Estadisticas(4, 6, 4, 3), 13, 0) { }
    public override string Atacar(Jugador jugador, IDado dado)
    {
        int tirada = dado.Lanzar(10) + Estadisticas.Reflejos + 3;
        if (tirada < 9 + jugador.Estadisticas.Reflejos)
            return "El guardia dispara, pero falló.";
        int danio = jugador.RecibirDanio(dado.Lanzar(4) + 1);
        return $"El guardia te alcanza: {danio} de daño a tu vida.";
    }
}
public sealed class DronCentinela : Enemigo
{
    public DronCentinela() : base("Dron centinela", new Estadisticas(5, 7, 7, 1), 15, 1) { }
    public override string Atacar(Jugador jugador, IDado dado)
    {
        int tirada = dado.Lanzar(10) + Estadisticas.Reflejos + 3;
        if (tirada < 9 + jugador.Estadisticas.Reflejos)
            return "El dron escanea el área sin acertar.";
        int danio = jugador.RecibirDanio(dado.Lanzar(6) + 1);
        return $"El dron dispara un pulso de energía: {danio} de daño a tu vida.";
    }
}
using NEON2020.Servicios;
namespace NEON2020.Modelo;
public sealed class Mercenario : Jugador
{
    public Mercenario(string nombre) : base(nombre, new Estadisticas(5, 9, 5, 4)) { }
    public override Rol Rol => global::NEON2020.Modelo.Rol.Mercenario;
    public override string Especialidad => "Sentido de combate: ataque táctico";
    public override int Habilidad(Habilidad habilidad) => habilidad switch
    {
        global::NEON2020.Modelo.Habilidad.Pistolas => 6,
        global::NEON2020.Modelo.Habilidad.Interface => 1,
        global::NEON2020.Modelo.Habilidad.Calle => 3,
        global::NEON2020.Modelo.Habilidad.Ingenieria => 2,
        _ => 0
    };
    public override string UsarEspecial(Enemigo enemigo, IDado dado)
    {
        if (!GastarEnergia(2)) return "No tienes 2 puntos de energía.";
        int danio = enemigo.RecibirDanio(5 + dado.Lanzar(6));
        return $"Sentido de combate: ataque preciso, {danio} de daño.";
    }
}
public sealed class Netrunner : Jugador
{
    public Netrunner(string nombre) : base(nombre, new Estadisticas(9, 5, 8, 5)) { }
    public override Rol Rol => global::NEON2020.Modelo.Rol.Netrunner;
    public override string Especialidad => "Interface: interferencia electrónica";
    public override int Habilidad(Habilidad habilidad) => habilidad switch
    {
        global::NEON2020.Modelo.Habilidad.Pistolas => 2,
        global::NEON2020.Modelo.Habilidad.Interface => 7,
        global::NEON2020.Modelo.Habilidad.Calle => 2,
        global::NEON2020.Modelo.Habilidad.Ingenieria => 5,
        _ => 0
    };
    public override string UsarEspecial(Enemigo enemigo, IDado dado)
    {
        if (!GastarEnergia(2)) return "No tienes 2 puntos de energía.";
        int danio = enemigo.RecibirDanio(2 + dado.Lanzar(4));
        enemigo.Aturdir(1);
        return $"Interface: bloqueaste las comunicaciones enemigas; {danio} de daño y pierde 1 turno.";
    }
}
public sealed class Arreglador : Jugador
{
    public Arreglador(string nombre) : base(nombre, new Estadisticas(7, 6, 5, 9)) { }
    public override Rol Rol => global::NEON2020.Modelo.Rol.Arreglador;
    public override string Especialidad => "Contactos: refuerzo y protección";
    public override int Habilidad(Habilidad habilidad) => habilidad switch
    {
        global::NEON2020.Modelo.Habilidad.Pistolas => 3,
        global::NEON2020.Modelo.Habilidad.Interface => 2,
        global::NEON2020.Modelo.Habilidad.Calle => 7,
        global::NEON2020.Modelo.Habilidad.Ingenieria => 2,
        _ => 0
    };
    public override string UsarEspecial(Enemigo enemigo, IDado dado)
    {
        if (!GastarEnergia(2)) return "No tienes 2 puntos de energía.";
        ActivarEscudo(5);
        int danio = enemigo.RecibirDanio(dado.Lanzar(4));
        return $"Contactos: recibes 5 de escudo y un aliado causa {danio} de daño.";
    }
}
public static class FabricaJugadores
{
    public static Jugador Crear(Rol rol, string nombre) => rol switch
    {
        Rol.Mercenario => new Mercenario(nombre),
        Rol.Netrunner => new Netrunner(nombre),
        Rol.Arreglador => new Arreglador(nombre),
        _ => throw new ArgumentOutOfRangeException(nameof(rol), "Rol no reconocido.")
    };
}
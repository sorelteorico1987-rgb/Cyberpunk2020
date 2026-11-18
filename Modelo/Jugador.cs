using NEON2020.Servicios;
namespace NEON2020.Modelo;
public abstract class Jugador : Personaje
{
    public const int EnergiaMaxima = 6;
    public abstract Rol Rol { get; }
    public abstract string Especialidad { get; }
    public Inventario Inventario { get; } = new();
    public int Energia { get; private set; } = EnergiaMaxima;
    public int Creditos { get; private set; } = 120;
    public int Humanidad { get; private set; } = 90;
    public int EscudoTemporal { get; private set; }
    protected Jugador(string nombre, Estadisticas estadisticas) : base(nombre, estadisticas, 24)
    {
        Inventario.Agregar(new Botiquin());
        Inventario.Agregar(new Estimulante());
    }
    public abstract string UsarEspecial(Enemigo enemigo, IDado dado);
    public abstract int Habilidad(Habilidad habilidad);
    protected bool GastarEnergia(int cantidad)
    {
        if (cantidad < 0) throw new ArgumentOutOfRangeException(nameof(cantidad));
        if (Energia < cantidad) return false;
        Energia -= cantidad;
        return true;
    }
    public int RecuperarEnergia(int cantidad)
    {
        if (cantidad < 0) throw new ArgumentOutOfRangeException(nameof(cantidad));
        int real = Math.Min(EnergiaMaxima - Energia, cantidad);
        Energia += real;
        return real;
    }
    public bool GastarCreditos(int cantidad)
    {
        if (cantidad < 0) throw new ArgumentOutOfRangeException(nameof(cantidad));
        if (Creditos < cantidad) return false;
        Creditos -= cantidad;
        return true;
    }
    public void GanarCreditos(int cantidad)
    {
        if (cantidad < 0) throw new ArgumentOutOfRangeException(nameof(cantidad));
        Creditos = Math.Min(10000, (int)Math.Min(10000L, (long)Creditos + cantidad));
    }
    public bool UsarSobrecargaNeural()
    {
        if (Humanidad < 15) return false;
        Humanidad -= 8;
        return true;
    }
    public void ActivarEscudo(int puntos)
    {
        if (puntos < 0 || puntos > 20) throw new ArgumentOutOfRangeException(nameof(puntos));
        EscudoTemporal = Math.Min(20, EscudoTemporal + puntos);
    }
    public override int RecibirDanio(int puntos)
    {
        if (puntos < 0) throw new ArgumentOutOfRangeException(nameof(puntos));
        int absorbido = Math.Min(EscudoTemporal, puntos);
        EscudoTemporal -= absorbido;
        return base.RecibirDanio(puntos - absorbido);
    }
    internal void RestaurarRecursos(int vida, int energia, int creditos,
        int humanidad, int escudo, int botiquines, int estimulantes)
    {
        if (energia < 0 || energia > EnergiaMaxima || creditos < 0 || creditos > 10000 ||
            humanidad < 0 || humanidad > 100 || escudo < 0 || escudo > 20)
            throw new InvalidDataException("Recursos inválidos en la partida.");
        RestaurarVida(vida);
        Energia = energia;
        Creditos = creditos;
        Humanidad = humanidad;
        EscudoTemporal = escudo;
        Inventario.Restaurar(botiquines, estimulantes);
    }
}
namespace NEON2020.Modelo;

public abstract class Objeto
{
    public abstract string Nombre { get; }
    public abstract string Descripcion { get; }
    public abstract bool Usar(Jugador jugador);
}
public sealed class Botiquin : Objeto
{
    public override string Nombre => "Botiquín";
    public override string Descripcion => "Recupera hasta 8 de vida.";
    public override bool Usar(Jugador jugador) => jugador.Curar(8) > 0;
}
public sealed class Estimulante : Objeto
{
    public override string Nombre => "Estimulante";
    public override string Descripcion => "Recupera hasta 3 de energía.";
    public override bool Usar(Jugador jugador) => jugador.RecuperarEnergia(3) > 0;
}
public sealed class Inventario
{
    private readonly List<Objeto> _objetos = new();
    public IReadOnlyList<Objeto> Objetos => _objetos.AsReadOnly();
    public int Botiquines => _objetos.Count(x => x is Botiquin);
    public int Estimulantes => _objetos.Count(x => x is Estimulante);
    public void Agregar(Objeto objeto)
    {
        ArgumentNullException.ThrowIfNull(objeto);
        if (_objetos.Count >= 20) throw new InvalidOperationException("Inventario lleno.");
        _objetos.Add(objeto);
    }
    public bool Usar<T>(Jugador jugador) where T : Objeto
    {
        ArgumentNullException.ThrowIfNull(jugador);
        int indice = _objetos.FindIndex(x => x is T);
        if (indice < 0 || !_objetos[indice].Usar(jugador)) return false;
        _objetos.RemoveAt(indice);
        return true;
    }
    internal void Restaurar(int botiquines, int estimulantes)
    {
        if (botiquines < 0 || estimulantes < 0 || botiquines + estimulantes > 20)
            throw new InvalidDataException("Inventario inválido.");
        _objetos.Clear();
        for (int i = 0; i < botiquines; i++) _objetos.Add(new Botiquin());
        for (int i = 0; i < estimulantes; i++) _objetos.Add(new Estimulante());
    }
}
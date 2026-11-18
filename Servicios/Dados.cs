namespace NEON2020.Servicios;
public interface IDado { int Lanzar(int caras); }
public sealed class DadoAleatorio : IDado
{
    public int Lanzar(int caras)
    {
        if (caras < 2 || caras > 100) throw new ArgumentOutOfRangeException(nameof(caras));
        return Random.Shared.Next(1, caras + 1);
    }
}
public sealed class DadoFijo : IDado
{
    private readonly int _resultado;
    public DadoFijo(int resultado)
    {
        if (resultado < 1) throw new ArgumentOutOfRangeException(nameof(resultado));
        _resultado = resultado;
    }
    public int Lanzar(int caras)
    {
        if (caras < 2 || caras > 100) throw new ArgumentOutOfRangeException(nameof(caras));
        return Math.Min(caras, _resultado);
    }
}
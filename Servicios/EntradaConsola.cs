namespace NEON2020.Servicios;
public sealed class FinEntradaException : Exception { }
public sealed class EntradaConsola
{
    private readonly TextReader _lector;
    private readonly TextWriter _salida;
    public EntradaConsola(TextReader lector, TextWriter salida)
    {
        _lector = lector ?? throw new ArgumentNullException(nameof(lector));
        _salida = salida ?? throw new ArgumentNullException(nameof(salida));
    }
    public int LeerOpcion(string pregunta, int minimo, int maximo)
    {
        if (minimo > maximo) throw new ArgumentException("Rango de opciones inválido.");
        while (true)
        {
            _salida.Write($"{pregunta} [{minimo}-{maximo}]: ");
            string? texto = _lector.ReadLine();
            if (texto is null) throw new FinEntradaException();
            if (int.TryParse(texto.Trim(), out int numero) && numero >= minimo && numero <= maximo)
                return numero;
            _salida.WriteLine("Opción inválida. Escribe un número del rango indicado.");
        }
    }
    public string LeerNombre()
    {
        while (true)
        {
            _salida.Write("Alias (3 a 18 caracteres, letras, números, espacios o guion): ");
            string? texto = _lector.ReadLine();
            if (texto is null) throw new FinEntradaException();
            string nombre = texto.Trim();
            if (nombre.Length is >= 3 and <= 18 &&
                nombre.All(c => char.IsLetterOrDigit(c) || c is ' ' or '-'))
                return nombre;
            _salida.WriteLine("Alias no válido. Evita símbolos y deja entre 3 y 18 caracteres.");
        }
    }
}
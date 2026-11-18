using NEON2020.Motores;
using NEON2020.Servicios;

namespace NEON2020;

internal static class Program
{
    private static int Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (args.Length == 1 && args[0] == "--autoprueba")
        {
            return Autopruebas.Ejecutar();
        }

        var entrada = new EntradaConsola(Console.In, Console.Out);
        var dado = new DadoAleatorio();
        var reglas = new MotorReglas(dado);
        var combate = new MotorCombate(entrada, reglas, dado);
        var historia = new MotorHistoria(entrada, reglas, combate);
        var guardado = new GuardadoPartida();

        try
        {
            new MotorJuego(entrada, historia, guardado).Ejecutar();
        }
        catch (FinEntradaException)
        {
            Console.WriteLine("\nSe cerró la entrada. El programa terminó sin bloquearse.");
        }
        return 0;
    }
}
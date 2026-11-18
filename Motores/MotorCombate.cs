using NEON2020.Modelo;
using NEON2020.Servicios;
namespace NEON2020.Motores;
public sealed class MotorCombate
{
    private readonly EntradaConsola _entrada;
    private readonly MotorReglas _reglas;
    private readonly IDado _dado;
    public MotorCombate(EntradaConsola entrada, MotorReglas reglas, IDado dado)
    {
        _entrada = entrada;
        _reglas = reglas;
        _dado = dado;
    }
    public bool Combatir(Jugador jugador, Enemigo enemigo)
    {
        Console.WriteLine($"\n!!! COMBATE: {enemigo.Nombre.ToUpperInvariant()} !!!");
        while (jugador.EstaVivo && enemigo.EstaVivo)
        {
            Console.WriteLine($"\n{jugador.Nombre}: {jugador.VidaActual}/{jugador.VidaMaxima} PV |" +
                $" Energía {jugador.Energia}/6 | Escudo {jugador.EscudoTemporal}");
            Console.WriteLine($"{enemigo.Nombre}: {enemigo.VidaActual}/{enemigo.VidaMaxima} PV");
            Console.WriteLine("1. Disparar  2. Habilidad del rol  3. Botiquín  4. Estimulante  5. Inspeccionar");
            int opcion = _entrada.LeerOpcion("Acción", 1, 5);
            bool gastaTurno = true;
            switch (opcion)
            {
                case 1:
                    var tiro = _reglas.Resolver(jugador, Caracteristica.Reflejos,
                        Habilidad.Pistolas, 14 + enemigo.Defensa);
                    Console.WriteLine(tiro);
                    if (tiro.Exito)
                        Console.WriteLine($"Disparo acertado: {enemigo.RecibirDanio(_dado.Lanzar(6) + 2)} daño.");
                    else Console.WriteLine("Tu disparo no acertó.");
                    break;
                case 2:
                    if (jugador.Energia < 2)
                    {
                        Console.WriteLine("Sin energía. Elige otra acción.");
                        gastaTurno = false;
                    }
                    else Console.WriteLine(jugador.UsarEspecial(enemigo, _dado));
                    break;
                case 3:
                    if (!jugador.Inventario.Usar<Botiquin>(jugador))
                    {
                        Console.WriteLine("No tienes botiquines o tu vida está completa.");
                        gastaTurno = false;
                    }
                    else Console.WriteLine("Usaste un botiquín y recuperaste vida.");
                    break;
                case 4:
                    if (!jugador.Inventario.Usar<Estimulante>(jugador))
                    {
                        Console.WriteLine("No tienes estimulantes o tu energía está completa.");
                        gastaTurno = false;
                    }
                    else Console.WriteLine("Recuperaste energía.");
                    break;
                case 5:
                    Console.WriteLine($"Defensa enemiga: {enemigo.Defensa}. Tu habilidad: {jugador.Especialidad}.");
                    gastaTurno = false;
                    break;
            }
            if (!gastaTurno || !enemigo.EstaVivo) continue;
            if (enemigo.PerderTurnoPorAturdimiento())
            {
                Console.WriteLine("El enemigo está interferido y pierde su turno.");
                continue;
            }
            Console.WriteLine(enemigo.Atacar(jugador, _dado));
        }
        Console.WriteLine(jugador.EstaVivo ? "Combate ganado." : "Has caído en la misión.");
        return jugador.EstaVivo;
    }
}
using NEON2020.Modelo;
using NEON2020.Servicios;
namespace NEON2020.Motores;
public sealed class MotorJuego
{
    private readonly EntradaConsola _entrada;
    private readonly MotorHistoria _historia;
    private readonly GuardadoPartida _guardado;
    public MotorJuego(EntradaConsola entrada, MotorHistoria historia, GuardadoPartida guardado)
    {
        _entrada = entrada;
        _historia = historia;
        _guardado = guardado;
    }
    public void Ejecutar()
    {
        while (true)
        {
            Console.WriteLine("\n+--------------------------------------------+");
            Console.WriteLine("|        N E O N   2 0 2 0                 |");
            Console.WriteLine("|         ECOS DE NIGHT CITY                |");
            Console.WriteLine("+--------------------------------------------+");
            Console.WriteLine("1. Nueva partida");
            Console.WriteLine("2. Cargar partida");
            Console.WriteLine("3. Cómo jugar");
            Console.WriteLine("4. Salir");
            int opcion = _entrada.LeerOpcion("Menú", 1, 4);
            switch (opcion)
            {
                case 1: NuevaPartida(); break;
                case 2:
                    if (_guardado.Cargar(out EstadoJuego? partida, out string mensaje))
                    {
                        Console.WriteLine(mensaje);
                        if (partida is not null) Jugar(partida);
                    }
                    else Console.WriteLine(mensaje);
                    break;
                case 3: Instrucciones(); break;
                case 4:
                    Console.WriteLine("Desconectado. Nos vemos en Night City.");
                    return;
            }
        }
    }
    private void NuevaPartida()
    {
        Console.WriteLine("\nCREACIÓN DE PERSONAJE");
        string nombre = _entrada.LeerNombre();
        Console.WriteLine("1. Mercenario: combate y reflejos.");
        Console.WriteLine("2. Netrunner: hackeo e inteligencia.");
        Console.WriteLine("3. Arreglador: negociación y contactos.");
        Rol rol = (Rol)_entrada.LeerOpcion("Elige tu rol", 1, 3);
        var estado = new EstadoJuego(FabricaJugadores.Crear(rol, nombre));
        Console.WriteLine($"\nBienvenido, {nombre}. Rol: {rol}.");
        MostrarFicha(estado);
        Jugar(estado);
    }
    private void Jugar(EstadoJuego estado)
    {
        while (true)
        {
            var resultado = _historia.Ejecutar(estado);
            if (resultado != ResultadoCapitulo.Avanzar)
            {
                if (resultado == ResultadoCapitulo.Derrota)
                    Console.WriteLine("Fin de partida. Puedes comenzar una nueva aventura.");
                return;
            }
            while (true)
            {
                Console.WriteLine("\n--- PUNTO DE CONTROL ---");
                Console.WriteLine("1. Siguiente capítulo");
                Console.WriteLine("2. Ver ficha e inventario");
                Console.WriteLine("3. Guardar y continuar");
                Console.WriteLine("4. Guardar y volver al menú");
                Console.WriteLine("5. Volver al menú SIN guardar");
                int opcion = _entrada.LeerOpcion("Elige", 1, 5);
                if (opcion == 2) { MostrarFicha(estado); continue; }
                if (opcion is 3 or 4)
                {
                    bool guardado = _guardado.Guardar(estado, out string mensaje);
                    Console.WriteLine(mensaje);
                    if (opcion == 4 && guardado) return;
                    if (opcion == 4 && !guardado) continue;
                }
                if (opcion == 5) return;
                break;
            }
        }
    }
    private static void MostrarFicha(EstadoJuego estado)
    {
        Jugador j = estado.Jugador;
        Console.WriteLine($"\nFICHA: {j.Nombre} ({j.Rol}) | {j.Especialidad}");
        Console.WriteLine($"INT {j.Estadisticas.Inteligencia} | REF {j.Estadisticas.Reflejos}" +
            $" | TEC {j.Estadisticas.Tecnica} | EMP {j.Estadisticas.Empatia}");
        Console.WriteLine($"Vida {j.VidaActual}/{j.VidaMaxima} | Energía {j.Energia}/{Jugador.EnergiaMaxima}" +
            $" | Humanidad {j.Humanidad} | Créditos {j.Creditos}");
        Console.WriteLine($"Capítulo {estado.Capitulo} | Alerta {estado.Alerta}/10" +
            $" | Reputación {estado.Reputacion}");
        Console.WriteLine($"Inventario: {j.Inventario.Botiquines} botiquín(es)," +
            $" {j.Inventario.Estimulantes} estimulante(s)");
        Console.WriteLine($"Credencial: {(estado.TieneCredencial ? "sí" : "no")}" +
            $" | Pista: {(estado.TienePista ? "sí" : "no")}" +
            $" | Datos: {(estado.TieneDatos ? "sí" : "no")}");
    }
    private void Instrucciones()
    {
        Console.WriteLine("\nCÓMO JUGAR");
        Console.WriteLine("Escribe el número de una opción y pulsa Intro. Los textos y números inválidos" +
            " se vuelven a pedir.");
        Console.WriteLine("Tus decisiones modifican la alerta, reputación y recursos.");
        Console.WriteLine("Las pruebas usan 1D10 + característica + habilidad + modificadores.");
        Console.WriteLine("En combate puedes disparar, usar la especialidad de tu rol o consumir objetos.");
        Console.WriteLine("Las acciones especiales cuestan 2 de energía; los consumibles gastan un turno.");
        Console.WriteLine("Puedes guardar y salir al terminar cada capítulo; solo hay un espacio de guardado.");
        Console.WriteLine("Ambientación inspirada en Cyberpunk 2020; historia y reglas adaptadas para clase.");
        Console.WriteLine("Archivo: " + _guardado.Ruta);
    }
}
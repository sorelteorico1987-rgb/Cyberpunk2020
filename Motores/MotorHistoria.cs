using NEON2020.Modelo;
using NEON2020.Servicios;
namespace NEON2020.Motores;
public enum ResultadoCapitulo { Avanzar, Finalizar, Derrota }
public sealed class MotorHistoria
{
    private readonly EntradaConsola _entrada;
    private readonly MotorReglas _reglas;
    private readonly MotorCombate _combate;
    public MotorHistoria(EntradaConsola entrada, MotorReglas reglas, MotorCombate combate)
    {
        _entrada = entrada;
        _reglas = reglas;
        _combate = combate;
    }
    public ResultadoCapitulo Ejecutar(EstadoJuego estado) => estado.Capitulo switch
    {
        1 => CapituloUno(estado),
        2 => CapituloDos(estado),
        3 => CapituloTres(estado),
        4 => CapituloCuatro(estado),
        _ => throw new InvalidOperationException("Capítulo fuera del rango.")
    };
    private bool Probar(EstadoJuego estado, Caracteristica atributo, Habilidad habilidad,
        int dificultad, int bonus = 0)
    {
        var tirada = _reglas.Resolver(estado.Jugador, atributo, habilidad, dificultad, bonus);
        Console.WriteLine("  " + tirada);
        return tirada.Exito;
    }
    private ResultadoCapitulo ContinuarSiVivo(EstadoJuego estado)
    {
        if (!estado.Jugador.EstaVivo)
        {
            Console.WriteLine("FINAL: EL PRECIO DEL SILENCIO. La ciudad se traga otro nombre.");
            return ResultadoCapitulo.Derrota;
        }
        estado.Avanzar();
        return ResultadoCapitulo.Avanzar;
    }
    private ResultadoCapitulo CapituloUno(EstadoJuego e)
    {
        Console.WriteLine("\n=== CAPÍTULO 1: BAJO LA LLUVIA DE NEÓN ===");
        Console.WriteLine("Night City, año 2020. La corporación ficticia HELIX-NOVA oculta" +
            " un protocolo de vigilancia neuronal.");
        Console.WriteLine("Una informante desaparecida te ha dejado una dirección y un aviso:" +
            " 'No permitas que borren a la ciudad'.");
        Console.WriteLine("El contacto te espera en el mercado nocturno de Kuro Alley.");
        Console.WriteLine("1. Convencer a la contrabandista para obtener una credencial.");
        Console.WriteLine("2. Rastrear la terminal pública buscando una pista digital.");
        Console.WriteLine("3. Amenazar al mensajero corporativo para quitarle su pase.");
        int opcion = _entrada.LeerOpcion("Decisión", 1, 3);
        switch (opcion)
        {
            case 1:
                if (Probar(e, Caracteristica.Empatia, Habilidad.Calle, 15))
                {
                    e.ConseguirCredencial();
                    e.CambiarReputacion(1);
                    Console.WriteLine("El trato sale bien. Recibes una credencial falsa.");
                }
                else
                {
                    e.CambiarAlerta(1);
                    Console.WriteLine("La contrabandista no confía en ti y avisa a sus contactos.");
                }
                break;
            case 2:
                if (Probar(e, Caracteristica.Inteligencia, Habilidad.Interface, 16))
                {
                    e.ConseguirPista();
                    Console.WriteLine("Recuperas una ruta de mantenimiento hacia la torre.");
                }
                else
                {
                    e.CambiarAlerta(1);
                    Console.WriteLine("La terminal lanza una alerta por acceso anómalo.");
                }
                break;
            case 3:
                e.CambiarAlerta(1);
                if (Probar(e, Caracteristica.Reflejos, Habilidad.Pistolas, 16))
                {
                    e.ConseguirCredencial();
                    e.CambiarReputacion(-1);
                    Console.WriteLine("Consigues el pase, pero empiezan a circular rumores sobre ti.");
                }
                else
                {
                    e.CambiarAlerta(2);
                    int danio = e.Jugador.RecibirDanio(3);
                    Console.WriteLine($"El mensajero escapa. Sufres {danio} de daño en la persecución.");
                }
                break;
        }
        return ContinuarSiVivo(e);
    }
    private ResultadoCapitulo CapituloDos(EstadoJuego e)
    {
        Console.WriteLine("\n=== CAPÍTULO 2: LA PUERTA DE HELIX-NOVA ===");
        Console.WriteLine("La torre corporativa brilla bajo la lluvia ácida.");
        Console.WriteLine(e.TieneCredencial ? "Llevas una credencial falsa." : "No tienes credencial.");
        Console.WriteLine(e.TienePista ? "Conoces la ruta de mantenimiento." : "No conoces una ruta segura.");
        Console.WriteLine("1. Engañar al control de acceso (credencial ayuda).");
        Console.WriteLine("2. Manipular los sistemas de mantenimiento (pista ayuda).");
        Console.WriteLine("3. Abrirse paso enfrentándose al guardia.");
        int opcion = _entrada.LeerOpcion("Decisión", 1, 3);
        bool paso = false;
        switch (opcion)
        {
            case 1:
                paso = Probar(e, Caracteristica.Empatia, Habilidad.Calle,
                    15 + e.Alerta, e.TieneCredencial ? 4 : 0);
                break;
            case 2:
                paso = Probar(e, Caracteristica.Tecnica, Habilidad.Ingenieria,
                    15 + e.Alerta, e.TienePista ? 4 : 0);
                break;
            case 3:
                e.CambiarAlerta(2);
                break;
        }
        if (paso) Console.WriteLine("Pasas el control. Nadie nota tu presencia.");
        else
        {
            e.CambiarAlerta(2);
            Console.WriteLine("Se activa un protocolo de seguridad. No hay más que luchar.");
            if (!_combate.Combatir(e.Jugador, new GuardiaCorporativo()))
                return ResultadoCapitulo.Derrota;
            e.CambiarReputacion(1);
        }
        Console.WriteLine("Llegas al ascensor de la sala de servidores.");
        return ContinuarSiVivo(e);
    }
    private ResultadoCapitulo CapituloTres(EstadoJuego e)
    {
        Console.WriteLine("\n=== CAPÍTULO 3: PROTOCOLO FANTASMA ===");
        Console.WriteLine("Una inteligencia de seguridad protege los archivos del proyecto.");
        Console.WriteLine("1. Infiltrar la Red (la pista digital ayuda).");
        Console.WriteLine("2. Puenteo del servidor físico (la credencial ayuda).");
        Console.WriteLine("3. Sobrecarga del implante: +3 al hackeo, cuesta 8 de humanidad.");
        Console.WriteLine("4. Destruir las defensas del dron y tomar los datos.");
        int opcion = _entrada.LeerOpcion("Decisión", 1, 4);
        bool tieneAcceso = false;
        switch (opcion)
        {
            case 1:
                tieneAcceso = Probar(e, Caracteristica.Inteligencia, Habilidad.Interface,
                    17 + e.Alerta, e.TienePista ? 3 : 0);
                break;
            case 2:
                tieneAcceso = Probar(e, Caracteristica.Tecnica, Habilidad.Ingenieria,
                    17 + e.Alerta, e.TieneCredencial ? 3 : 0);
                break;
            case 3:
                if (e.Jugador.UsarSobrecargaNeural())
                {
                    Console.WriteLine("El implante se calienta. Pierdes 8 de humanidad.");
                    tieneAcceso = Probar(e, Caracteristica.Inteligencia, Habilidad.Interface,
                        17 + e.Alerta, 3);
                }
                else
                    Console.WriteLine("Tu humanidad es demasiado baja para sobrecargar el implante.");
                break;
            case 4:
                e.CambiarAlerta(2);
                break;
        }
        if (!tieneAcceso)
        {
            e.CambiarAlerta(2);
            Console.WriteLine("El dron centinela se activa. La única salida es vencerlo.");
            if (!_combate.Combatir(e.Jugador, new DronCentinela()))
                return ResultadoCapitulo.Derrota;
        }
        else Console.WriteLine("Las barreras digitales caen. Copias el archivo protegido.");
        e.ConseguirDatos();
        Console.WriteLine("¡Tienes el Protocolo Fantasma! Puedes decidir su destino.");
        return ContinuarSiVivo(e);
    }
    private ResultadoCapitulo CapituloCuatro(EstadoJuego e)
    {
        if (!e.TieneDatos) throw new InvalidOperationException("No se puede finalizar sin los datos.");
        Console.WriteLine("\n=== CAPÍTULO 4: EL PRECIO DE LA VERDAD ===");
        Console.WriteLine("Desde una azotea, observas las luces de Night City.");
        Console.WriteLine("Tu comunicador recibe tres ofertas incompatibles.");
        Console.WriteLine("1. Filtrar la evidencia al público.");
        Console.WriteLine("2. Vender el archivo a una corporación rival (500 créditos).");
        Console.WriteLine("3. Destruir la copia para impedir que alguien controle el sistema.");
        int opcion = _entrada.LeerOpcion("Decisión final", 1, 3);
        Console.WriteLine("\n==============================================");
        switch (opcion)
        {
            case 1:
                e.CambiarReputacion(3);
                Console.WriteLine("FINAL: UNA CIUDAD DESPIERTA");
                Console.WriteLine("La información aparece en miles de pantallas. No sabes si la ciudad" +
                    " cambiará, pero hoy sus habitantes pueden conocer la verdad.");
                break;
            case 2:
                e.Jugador.GanarCreditos(500);
                e.CambiarReputacion(-2);
                Console.WriteLine("FINAL: TODO TIENE UN PRECIO");
                Console.WriteLine("Cobras tu parte. HELIX-NOVA pierde el monopolio del proyecto," +
                    " pero quizá solo hayas cambiado quién sostiene las cadenas.");
                break;
            case 3:
                e.CambiarReputacion(1);
                Console.WriteLine("FINAL: CENIZAS DIGITALES");
                Console.WriteLine("Borras los archivos y destrozas la clave. Nadie podrá utilizar" +
                    " el proyecto hoy; su existencia quedará como un rumor.");
                break;
        }
        if (e.Alerta >= 6)
            Console.WriteLine("EPÍLOGO: las patrullas te buscan. Tendrás que desaparecer un tiempo.");
        if (e.Jugador.Humanidad < 90)
            Console.WriteLine("EPÍLOGO: el implante dejó una huella que no desaparecerá fácilmente.");
        Console.WriteLine($"Reputación final: {e.Reputacion} | Créditos: {e.Jugador.Creditos} |" +
            $" Humanidad: {e.Jugador.Humanidad}");
        Console.WriteLine("==============================================");
        return ResultadoCapitulo.Finalizar;
    }
}
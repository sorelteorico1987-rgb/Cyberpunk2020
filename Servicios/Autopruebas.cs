using NEON2020.Modelo;
using NEON2020.Motores;
namespace NEON2020.Servicios;
public static class Autopruebas
{
    private static int _correctas;
    private static int _fallidas;
    private static void Verificar(bool condicion, string caso)
    {
        if (condicion)
        {
            _correctas++;
            Console.WriteLine("[OK] " + caso);
        }
        else
        {
            _fallidas++;
            Console.WriteLine("[ERROR] " + caso);
        }
    }
    public static int Ejecutar()
    {
        Console.WriteLine("PRUEBAS AUTOMÁTICAS — NEON 2020\n");
        Jugador mercenario = new Mercenario("Raven");
        Jugador netrunner = new Netrunner("Cipher");
        Jugador arreglador = new Arreglador("Vega");
        Verificar(mercenario.Rol == Rol.Mercenario && netrunner.Rol == Rol.Netrunner &&
            arreglador.Rol == Rol.Arreglador, "La fábrica crea tres roles distintos");
        var dado = new DadoFijo(5);
        var reglas = new MotorReglas(dado);
        var simple = reglas.Resolver(netrunner, Caracteristica.Inteligencia, Habilidad.Interface, 15);
        var conBonus = reglas.Resolver(netrunner, Caracteristica.Inteligencia, Habilidad.Interface, 15, 3);
        Verificar(simple.Total + 3 == conBonus.Total && conBonus.Exito,
            "Sobrecarga Resolver() con/sin bonificación");
        int danio = mercenario.RecibirDanio(7);
        Verificar(danio == 7 && mercenario.VidaActual == 17, "Daño reduce correctamente la vida");
        Verificar(mercenario.Inventario.Usar<Botiquin>(mercenario) && mercenario.VidaActual == 24 &&
            mercenario.Inventario.Botiquines == 0, "Botiquín sana hasta el máximo y se consume");
        Verificar(!mercenario.Inventario.Usar<Botiquin>(mercenario),
            "No se pueden usar objetos que no existen");
        bool validacion = false;
        try { mercenario.RecibirDanio(-2); }
        catch (ArgumentOutOfRangeException) { validacion = true; }
        Verificar(validacion, "Los daños negativos se rechazan");
        validacion = false;
        try { _ = new Estadisticas(30, 3, 3, 3); }
        catch (ArgumentOutOfRangeException) { validacion = true; }
        Verificar(validacion, "Las estadísticas fuera del rango se rechazan");
        var guardia = new GuardiaCorporativo();
        string especial = netrunner.UsarEspecial(guardia, dado);
        Verificar(netrunner.Energia == 4 && guardia.PerderTurnoPorAturdimiento() &&
            especial.Contains("Interface", StringComparison.Ordinal),
            "Polimorfismo: el netrunner interfiere al enemigo");
        var dron = new DronCentinela();
        arreglador.UsarEspecial(dron, dado);
        Verificar(arreglador.EscudoTemporal == 5 && arreglador.Energia == 4,
            "Polimorfismo: el arreglador recibe protección");
        mercenario.UsarEspecial(dron, dado);
        Verificar(mercenario.Energia == 4 && dron.VidaActual < dron.VidaMaxima,
            "Polimorfismo: el mercenario ejecuta un ataque especial");
        var lector = new StringReader("hola\n99\n2\n");
        var escritor = new StringWriter();
        var entrada = new EntradaConsola(lector, escritor);
        Verificar(entrada.LeerOpcion("Prueba", 1, 3) == 2 &&
            escritor.ToString().Contains("inválida", StringComparison.Ordinal),
            "Teclado: se rechazan letras y opciones fuera de rango");
        var lectorNombre = new StringReader("@@@\nA\nNova-7\n");
        Verificar(new EntradaConsola(lectorNombre, new StringWriter()).LeerNombre() == "Nova-7",
            "Teclado: el alias se valida antes de aceptarse");
        var estado = new EstadoJuego(netrunner);
        estado.ConseguirPista();
        estado.CambiarAlerta(2);
        estado.Avanzar();
        string carpeta = Path.Combine(Path.GetTempPath(), "neon2020-tests-" + Guid.NewGuid().ToString("N"));
        string archivo = Path.Combine(carpeta, "partida.json");
        try
        {
            var guardado = new GuardadoPartida(archivo);
            bool guardo = guardado.Guardar(estado, out _);
            bool cargo = guardado.Cargar(out EstadoJuego? cargado, out _);
            Verificar(guardo && cargo && cargado is not null &&
                cargado.Capitulo == 2 && cargado.Jugador.Rol == Rol.Netrunner &&
                cargado.TienePista && cargado.Alerta == 2,
                "Guardado/carga JSON conserva el progreso");
            File.WriteAllText(archivo, "{ archivo roto");
            Verificar(!guardado.Cargar(out _, out _), "El archivo JSON corrupto no bloquea el juego");
            File.WriteAllText(archivo, "{\"Version\":1,\"Nombre\":\"Nova\",\"Rol\":99}");
            Verificar(!guardado.Cargar(out _, out _), "Los roles manipulados en JSON se rechazan");
        }
        catch (IOException ex)
        {
            _fallidas++;
            Console.WriteLine("[ERROR] Pruebas de archivos: " + ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _fallidas++;
            Console.WriteLine("[ERROR] Pruebas de archivos: " + ex.Message);
        }
        finally
        {
            try { if (Directory.Exists(carpeta)) Directory.Delete(carpeta, true); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
        Console.WriteLine($"\nRESULTADO: {_correctas} correctas, {_fallidas} fallidas.");
        return _fallidas == 0 ? 0 : 1;
    }
}
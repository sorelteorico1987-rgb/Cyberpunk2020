# Guía para explicar NEON 2020 ante el profesor

## ¿Qué hace el programa?
Es un juego de rol narrativo de consola en C#. El jugador elige entre Mercenario, Netrunner y Arreglador, toma decisiones, supera pruebas con dados, combate, utiliza consumibles y guarda su progreso.

## Los cuatro pilares de POO

**Abstracción:** `Personaje`, `Jugador`, `Enemigo` y `Objeto` son clases abstractas. Definen contratos y propiedades compartidas sin poder instanciarse directamente.

**Encapsulación:** `Jugador` protege vida, energía, humanidad y créditos mediante propiedades con setters restringidos y métodos que validan las modificaciones. `Inventario` mantiene privada su lista interna.

**Herencia:** `Mercenario`, `Netrunner` y `Arreglador` heredan de `Jugador`; `GuardiaCorporativo` y `DronCentinela` heredan de `Enemigo`.

**Polimorfismo:** `UsarEspecial()` se sobrescribe en cada rol; `Atacar()` se sobrescribe en cada enemigo. `MotorReglas.Resolver()` también está sobrecargado con cuatro y cinco argumentos.

## Reglas y validación
La prueba de habilidad usa un dado de diez caras más característica, habilidad y modificador. `EntradaConsola` valida todas las opciones de teclado y los alias. El guardado JSON verifica valores antes de reconstruir una partida.

## Preguntas de defensa
- ¿Por qué `Personaje` es abstracta? Porque representa rasgos compartidos, no un personaje concreto.
- ¿Qué diferencia hay entre herencia y polimorfismo? La herencia reutiliza estructura; el polimorfismo permite que una llamada se comporte de manera diferente según el objeto.
- ¿Dónde hay encapsulación? En propiedades con `private set` y listas privadas.
- ¿Cómo se prueba el juego? Con `dotnet run -- --autoprueba` y pruebas manuales de cada ruta.
- ¿Qué harías después? Ampliar misiones, añadir pruebas unitarias con un framework y una interfaz gráfica.

**Nota:** hay que ejecutar y documentar las pruebas antes de afirmar que el proyecto funciona sin errores.
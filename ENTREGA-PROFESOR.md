# Ficha de entrega para el profesor

**Proyecto:** NEON 2020 — Ecos de Night City  
**Asignatura:** Programación II  
**Alumno/a:** [Completar]  
**Docente:** [Completar]  
**Grupo:** [Completar]  
**Fecha límite:** 9 de diciembre de 2026

## Objetivo
Desarrollar en C# un motor de narrativa interactiva y rol para consola que permita crear personajes de distintos roles, tomar decisiones, resolver acciones con dados, combatir, administrar recursos y guardar/cargar partidas.

## Evidencias de POO
- **Abstracción:** `Modelo/Personaje.cs`, clase base abstracta, y `Modelo/Jugador.cs`.
- **Encapsulación:** propiedades con setters restringidos y control de modificaciones en `Modelo/Inventario.cs`, `Modelo/Jugador.cs` y `Modelo/EstadoJuego.cs`.
- **Herencia:** `Modelo/Roles.cs` y `Modelo/Enemigos.cs` especializan las clases base.
- **Polimorfismo:** sobrescritura de comportamientos de roles/enemigos y sobrecargas de resolución de acciones en `Motores/MotorReglas.cs`.
- **Diagramas UML:** `Diagramas/UML-Personajes.dot` y `Diagramas/UML-Motores.dot`.

## Criterios adicionales
- **Coherencia temática (20 puntos):** ambientación cyberpunk y mecánicas de RPG narrativo.
- **Abstracción (15):** clases abstractas y diagramas.
- **Encapsulación (15):** protección de estado y validaciones.
- **Herencia (15):** jerarquías de personajes y enemigos.
- **Polimorfismo (15):** métodos sobrescritos y sobrecargados.
- **Sin errores (10):** pruebas propuestas; confirmar compilación en .NET 10.
- **Validación de entradas (10):** `Servicios/EntradaConsola.cs`.

## Instrucciones rápidas
Con .NET SDK 10 instalado, abrir la carpeta que contiene `NEON2020.csproj` y ejecutar:

```bash
dotnet run
dotnet run -- --autoprueba
```

La segunda instrucción ejecuta las comprobaciones internas; sus resultados deben documentarse tras ejecutarlas.

## Referencias y alcance
Inspiración en *Cyberpunk 2020* de R. Talsorian Games. Las reglas se simplificaron para fines académicos. No se incluye el PDF comercial en este repositorio.

## Estado de validación
El proyecto se preparó con código y documentación, pero **no se ha confirmado una compilación real en este entorno**. La prueba en el equipo del estudiante está pendiente.
# Entrega 4 - BuddyCalendar
BuddyCalendar es una aplicación de calendario orientada a jóvenes y personas que buscan tener un acompañante en su escritorio. Basado e inspirado en Bonzi Buddy (https://es.wikipedia.org/wiki/Bonzi_Buddy).

## Stack
Se diseñó la aplicación en C# utilizando como base el framework multiplataforma y open-source .Net Core de Microsoft. La aplicación esta organizada bajo el modelo MVVM (Model-View-ViewController)

## Cómo usar
Para ejecutar, es posible abrir la solución en Visual Studio, compilar y ejecutar el proyecto "Calendar"

## Sobre warning CA2235
Se ha omitido solucionar los warnings de tipo CA2235 debido a que estos corresponden a un bug de FxCop al ser usado sobre un proyecto hecho con .NET Core 3.0. https://github.com/dotnet/roslyn-analyzers/issues/1510, https://github.com/dotnet/roslyn-analyzers/issues/3616

## Otros detalles
Debido a dificultades gráficas se ha implementado el marcador de eventos en la vista mensual de manera minimalista indicando con un "*" si hay eventos en ese día.

## Sobre Static Analyzer
Instalé visual studio 2019 enterprise junto a mi instalación de VS2019 community y esto introdujo unos errores en la lista pero además me mostró otros, aquí una captura donde se ve que no existe ningún error respecto al análisis estático (se arreglaron todos y desaparecio el del bug).
[screenshot](static_analyzer.png)

## Test Coverage
Aquí una captura del output de test coverage de VS2019 Enterprise. Al ser un proyecto .NET Core, este no es soportado por axoCover.
[screenshot](tests.png)
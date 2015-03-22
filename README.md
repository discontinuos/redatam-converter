﻿# redatam-converter
Aplicación para extraer información de base de REDATAM

RedatamConverter es un proyecto de código abierto que permite parcialmente leer bases de Redatam para permtir la exportación de sus microdatos. Fue creado para poder recuperar microdatos de modo de poder realizar análisis estadísticas trabajos de obtener por medio de exportaciones de Redatam, tales que medidas de dispersión, análisis de correlaciones, regresiones logísticas.

# COMPATIBILIDAD

El conversor fue probado con bases de Redatam en diferentes versiones, intentando ser compatible con cambios que pudieran haber ocurrido en el tiempo. La prueba más antigua se realizó con una base que se distribuyó junto con Redatam 2.3.0.5 (Censo 2001 Argentina), mientras que la más reciente fue una versión 2.5.7.0 (Censo 2010 Argentina).

# LIMITACIONES

La versión actual soporte variables de tipo texto (STRING) y de tipo enteros (INTEGER).

# REQUISITOS DE INSTALACION AUTOMATICA

Se puede realizar una instalación de la herramienta ejecutando el instalador contenido en /release/setup.exe.

# REQUISITOS DE INSTALACION MANUAL

La instalación manual se puede realizar utilizando al versión compilada en la carpeta /bin.

Framework .NET 4 
La aplicación utiliza el Microsoft Framework .NET 4 (client profile). Esto es un runtime de Microsoft para ejecutar aplicaciones, similar a la virtual machine de Java. Si no estuviera instalado en la PC, podrá descargarse de https://www.microsoft.com/en-us/download/details.aspx?id=17851.

SPSS.NET InteropLibrary 
La aplicación utiliza la librería de SPSS.NET InteropLibrary (https://spss.codeplex.com/) para generar archivos nativos de SPSS como salida. Esta librería no requiere una instalación independiente, pero posiblemente deba copiar los archivos de la carpeta SPSSio a su carpeta c:\windows\System32 (o equivalente... hacer Inicio > Ejecutar > %WINDIR%\SYSTEM32 <enter> puede ser una buena forma de identificar la carpeta System32 de la PC).

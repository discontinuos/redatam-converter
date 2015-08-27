# redatam-converter
Aplicación para extraer información de base de REDATAM

RedatamConverter es un proyecto de código abierto que permite leer bases de Redatam para exportar sus microdatos. Fue creado para poder recuperar información de base de REDATAM para realizar análisis estadísticas con herramientas estándar de análisis estadístico (SPSS, STATA, R, etc).

# COMPATIBILIDAD

El conversor fue probado con bases de REDATAM en diferentes versiones, intentando ser compatible con cambios que pudieran haber ocurrido en el tiempo. La prueba más antigua se realizó con una base que se distribuyó junto con REDATAM  2.3.0.5 (año 2003) mientras que la más reciente fue una versión 2.5.7.0 (año 2012). Las versiones 1.4 y 1.5 fueron ajustadas a partir de diccionarios de censos de Uruguay de 1963 y de Argentina de 1991.

La versión actual exporta a formato SPSS nativo y archivos separados por comas (CSV). 

# REQUISITOS DE INSTALACION AUTOMATICA

Se puede realizar una instalación de la herramienta ejecutando el instalador. El mismo se encuentra disponible en:

https://github.com/discontinuos/redatam-converter/blob/master/release/setup-win32.exe?raw=true.

# REQUISITOS DE INSTALACION MANUAL

La instalación manual se puede realizar utilizando al versión compilada en la carpeta /bin.

Framework .NET 4 
La aplicación utiliza el Microsoft Framework .NET 4 (client profile). Esto es un runtime de Microsoft para ejecutar aplicaciones, similar a la virtual machine de Java. Si no estuviera instalado en la PC, podrá descargarse de https://www.microsoft.com/en-us/download/details.aspx?id=17851.

SPSS.NET InteropLibrary 
La aplicación utiliza la librería de SPSS.NET InteropLibrary (https://spss.codeplex.com/) para generar archivos nativos de SPSS como salida. Esta librería no requiere una instalación independiente, pero posiblemente deba copiar los archivos de la carpeta SPSSio a su carpeta c:\windows\System32 (o equivalente... hacer Inicio > Ejecutar > %WINDIR%\SYSTEM32 <enter> puede ser una buena forma de identificar la carpeta System32 de la PC).
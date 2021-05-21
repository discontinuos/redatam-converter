# Conversor REDATAM
[English Readme Available](https://github.com/discontinuos/redatam-converter/blob/master/README-EN.md).
Aplicación para extraer información de bases de datos REDATAM.

RedatamConverter es un proyecto de código abierto que permite leer bases de Redatam para exportar sus microdatos. Fue creado para poder recuperar información de base de REDATAM para realizar análisis estadísticas con herramientas estándar de análisis estadístico (SPSS, STATA, R, etc).

# Descargas

Descargar: [Instalador Windows 32/64-bit](https://github.com/discontinuos/redatam-converter/blob/master/release/setup-win32.exe?raw=true) (8.74MB)

# Historial

Puede consultar aquí el [historial de versiones](https://github.com/discontinuos/redatam-converter/blob/master/HISTORY.md).

# Compatibilidad

El conversor fue probado con bases de REDATAM en diferentes versiones, intentando ser compatible con cambios que pudieran haber ocurrido en el tiempo. En la primera versión del Conversor, la prueba más antigua se realizó con una base que se distribuyó junto con REDATAM  2.3.0.5 (año 2003) mientras que la más reciente fue una versión 2.5.7.0 (año 2012). Las versiones 1.4 y 1.5 fueron ajustadas a partir de diccionarios de censos de Uruguay de 1963 y de Argentina de 1991. La versión 2.0 es compatibles con los diccionarios de Redatam 7, y se ajustó su lectura para reconocer etiquetas codificadas en CP850 (MSDOS) a partir de un diccionario del censo de Chile de 1992. 

La versión actual exporta a formato SPSS nativo y archivos separados por comas (CSV). 

# Consultas y sugerencias

Para consultas o sugerencias, escribir a Pablo De Grande (pablodg@gmail.com).

# Requisitos de instalación manual 

La instalación manual se puede realizar utilizando al versión compilada en la carpeta /bin.

Framework .NET 4 
La aplicación utiliza el Microsoft Framework .NET 4 (client profile). Esto es un runtime de Microsoft para ejecutar aplicaciones, similar a la virtual machine de Java. Si no estuviera instalado en la PC, podrá descargarse de https://www.microsoft.com/en-us/download/details.aspx?id=17851.

SPSS.NET InteropLibrary 
La aplicación utiliza la librería de SPSS.NET InteropLibrary (https://spss.codeplex.com/) para generar archivos nativos de SPSS como salida. Esta librería no requiere una instalación independiente, pero posiblemente deba copiar los archivos de la carpeta SPSSio a su carpeta c:\windows\System32 (o equivalente... hacer Inicio > Ejecutar > %WINDIR%\SYSTEM32 <enter> puede ser una buena forma de identificar la carpeta System32 de la PC).
  
# Créditos

El Conversor Redatam fue creado y es mantenido por Pablo De Grande (pablodg@gmail.com) en el marco de la iniciativa [Discontinuos](http://www.aacademica.org/discontinuos). El mismo ha tenido sustanciales mejoras a partir de consultas y sugerencias de los usuarios y de comentarios en el [blog](http://idiscontinuos.wordpress.com/2015/03/21/convirtiendo-bases-redatam-a-spss/). 

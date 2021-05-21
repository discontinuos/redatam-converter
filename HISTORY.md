[README.md](https://github.com/discontinuos/redatam-converter/blob/master/README.md).

# Versi�n 2.0 - 2021-05-21
- Soporte a diccionarios .dicx (Redatam 7)
- Soporte a diccionarios antiguos con codificaci�n de etiquetas 850 (MsDos).

# Versi�n 1.7 - 2016-01-30
- Soporte para archivos de datos .bin (previos a .PCK)
- Limpieza de nombres de variables en la exportaci�n a SPSS para evitar interrupciones por varibles largas o guion bajo al inicio.
- Verificaci�n del tama�o de los archivos de datos.
- Mejoras en la estimaci�n del tiempo total.
- Posibilidad de seleccionar variables a exportar.

# Versi�n 1.6 - 2015-09-30
- Soporte para bases de datos con entidades con m�s de una entidad hija.
- Test automatizados para validar integridad entre versiones.

# Versi�n 1.5 - 2015-08-04
- Busca los archivos de datos y punteros en la carpeta del diccionario si falla la ruta relativa.
- Valida el tipo de dato indicado para considerar o ignorar la marca de DATASET.

# Versi�n 1.4 - 2015-07-11
- Soporte para bases de datos m�s antiguas (sin puntero en la entidad de nivel 1).
- Separaci�n de fuentes en subproyectos RedatamConverter y RedatamLib (dll de lectura).
- Soporte para exportaci�n de bases de datos con archivos de datos faltantes.
- Mejoras en el instalador.

# Versi�n 1.3 - 2015-05-14
- Soporte para tipos de dato INT (int16) y LNG (int32).

# Versi�n 1.2 - 2015-03-24
- Fix para instalaciones en rutas con espacios (Windows 7).

# Versi�n 1.1 - 2015-03-20
- Exporta a formato CSV.
- Fix a c�lculo de tiempo restante.
- Mensaje de falta de espacio permite reintentar.
- Agregado de licencia GNU-GPL v3.

# Versi�n 1.0 - 2015-03-10
- Exporta bases REDATAM a formato SPSS (sav).
- Extrae datos y etiquetas de variables y valores.
- Parsea datos INTEGER, REAL y STRING.


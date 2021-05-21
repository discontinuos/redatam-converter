[README-EN.md](https://github.com/discontinuos/redatam-converter/blob/master/README-EN.md).

# Version 2.0 - 2021-05-21
- Support for .dicx diccionaries (Redatam 7)
- Support for old dicionaries with labels encoded in CP 850 (MsDos).

# Version 1.7 - 2016-01-30
- Support for .bin data files (older than .PCK)
- Sanitize variables names on SPSS export to avoid errores on long variables names or underscores.
- Checks for the size of data files.
- Improvements in total time calculation.
- Columns to export can be selected.

# Version 1.6 - 2015-09-30
- Support for databases having entities with more than one child-entity.
- Automated testing to check integrity among versions.

# Version 1.5 - 2015-08-04
- Search for data files and pointer files in the dictionary folder if it fails in the relative path.
- Validates the data type to consider or ignore DATASET tag.
 
# Version 1.4 - 2015-07-11
- Support for older databases (without pointer fule in the level 1 entity).
- Separation of sources in RedatamConverter and RedatamLib (reader dll) sub-projects.
- Support for exporting databases with missing data files.
- Improvements in the installer.
 
# Version 1.3 - 2015-05-14
- Support for INT (int16) and LNG (int32) data types.
 
# Version 1.2 - 2015-03-24
- Fix for installations in folder paths with spaces (Windows 7).
 
# Version 1.1 - 2015-03-20
- Export to CSV format.
- Fix for the estimation of time remaining.
- Allows retry when ran out of disk space.
- Added GNU- GPL license v3.
 
# Version 1.0 - 2015-03-10
- Export REDATAM database to SPSS (sav).
- Extract data and labels of the variables and values.
- Parses data types INTEGER, REAL, and STRING.
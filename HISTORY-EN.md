[README-EN.md](https://github.com/discontinuos/redatam-converter/blob/master/README-EN.md).

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
# REDATAM CONVERTER
[Spanish Readme Available](https://github.com/discontinuos/redatam-converter/blob/master/README.md).
Application for extracting raw information from REDATAM databases.
 
Redatam Converter is an open source project that lets you read Redatam datadatabasess to export their microdata. It was created to recover information of REDATAM databases for statistics analysis using standard tools such as SPSS, STATA, R, etc.
 
# DOWNLOADS

Download: [Windows 32/64-bit Setup](https://github.com/discontinuos/redatam-converter/blob/master/release/setup-win32.exe?raw=true) (8.74MB)

# HISTORY

The log for changes can be seen at [history record](https://github.com/discontinuos/redatam-converter/blob/master/HISTORY-EN.md).


# COMPATIBILITY
 
The converter was tested with REDATAM databases in different versions, trying to be compatible with changes that may have occurred over time. For the first version of the Converter the oldest test was performed with a databases that was distributed along with REDATAM 2.3.0.5 (year 2003) while the most recent was a version 2.5.7.0 (year 2012). Versions 1.4 and 1.5 were adjusted from dictionaries from censuses of 1963 from Uruguay and of 1991 from Argentina. The Version 2.0 supports .dicx diccionaries of Redatam 7 and adds supports dicionaries with labels encoded using CP850 (MSDOS) based on the dictionary of the 1992 census of Chile.
 
The current version exports to SPSS native format and to comma separated files (CSV). 
 
# MANUAL INSTALLATION REQUIREMENTS
 
Manual installation can be performed using the compiled version in the folder /bin.
 
.NET Framework 4 
The application uses the Microsoft Framework .NET 4 (client profile). This is a Microsoft runtime to run applications, similar to the virtual machine for Java. If your PC does not has  .NET Framework installed, you can download it from https://www.microsoft.com/en-us/download/details.aspx?id=17851.
 
SPSS.NET InteropLibrary 
The application uses the library of SPSS.NET InteropLibrary (https://spss.codeplex.com/) to generate native SPSS output files. This library does not require a separate installation, but possibly should copy the files from the SPSSio folder to your C:\Windows\System32 folder.


# CREDITS

The Redatam Converter was created and is supported by Pablo De Grande (pablodg@gmail.com). Many improvements have been made thanks to user's feedback and suggestions made at the [blog](http://idiscontinuos.wordpress.com/2015/03/21/convirtiendo-bases-redatam-a-spss/). 

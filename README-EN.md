# redatam - converter
Application for extracting information from REDATAM datadatabasess
 
RedatamConverter is an open source project that lets you read Redatam datadatabasess to export their microdata. It was created to recover information of REDATAM databases for statistics analysis using standard tools such as SPSS, STATA, R, etc.
 
# COMPATIBILITY
 
The converter was tested with REDATAM databases in different versions, trying to be compatible with changes that may have occurred over time. The oldest test was performed with a databases that was distributed along with REDATAM 2.3.0.5 (year 2003) while the most recent was a version 2.5.7.0 (year 2012). Versions 1.4 and 1.5 were adjusted from dictionaries from censuses of 1963 from Uruguay and of 1991 from Argentina.
 
The current version exports to SPSS native format and to comma separated files (CSV). 
 
# STANDALONE SETUP REQUIREMENTS
 
The can be installed using the standalone setup. It is available at:
 
https://github.com/discontinuos/redatam-converter/blob/master/release/setup-win32.exe?raw=true.
 
# MANUAL SETUP REQUIREMENTS
 
Manual installation can be performed using the compiled version in the folder /bin.
 
.NET Framework 4 
The application uses the Microsoft Framework .NET 4 (client profile). This is a Microsoft runtime to run applications, similar to the virtual machine for Java. If your PC does not has  .NET Framework installed, you can download it from https://www.microsoft.com/en-us/download/details.aspx?id=17851.
 
SPSS.NET InteropLibrary 
The application uses the library of SPSS.NET InteropLibrary (https://spss.codeplex.com/) to generate native SPSS output files. This library does not require a separate installation, but possibly should copy the files from the SPSSio folder to your C:\Windows\System32 folder.

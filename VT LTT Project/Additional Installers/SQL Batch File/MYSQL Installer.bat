@echo off
REM Path to the MySQL Installer MSI file
set MYSQL_INSTALLER=C:\ProgramData\NI LTT\Additional Installers\mysql-installer-community-8.0.35.0.msi
REM Silent installation using msiexec
msiexec /i "%MYSQL_INSTALLER%" /norestart /log install_log.txt
 
REM Wait for installation to complete
echo MySQL installation is in progress, Please press any button only after MYSQL Installation is completed... Check install_log.txt for details.
pause


REM Path to the MySQL Installer MSI file
set MYSQL_INSTALLER=C:\ProgramData\NI LTT\Additional Installers\mysql-connector-odbc-8.2.0-winx64.msi
REM Silent installation using msiexec
msiexec /i "%MYSQL_INSTALLER%" /norestart /log install_log.txt
 
REM Wait for installation to complete
echo MySQL ODBC Connector installation is in progress.Please press any button only after MYSQL ODBC Installation is completed.. Check install_log.txt for details.
pause

 
REM Batch file to create a User DSN for MySQL
 
REM Set variables
set DSN_NAME=LTTAnalysis
set USER=root
set PASSWORD=VTLTT
 
REM Create User DSN
odbcconf.exe /A {CONFIGDSN "MySQL ODBC 8.2 ANSI Driver" "DSN=%DSN_NAME%;USER=%USER%;PASSWORD=%PASSWORD%"}
 
REM Check for errors
if %ERRORLEVEL% equ 0 (
    echo User ODBC DSN "%DSN_NAME%" created successfully.
) else (
    echo Failed to create User ODBC DSN "%DSN_NAME%".
)
 
pause


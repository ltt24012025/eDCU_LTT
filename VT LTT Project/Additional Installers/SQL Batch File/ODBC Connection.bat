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
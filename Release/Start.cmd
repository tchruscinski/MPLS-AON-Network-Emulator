@ECHO OFF



COLOR 0A
ECHO "    __  ______  __   ____        "
ECHO "   /  |/  / _ \/ /  / __/___()__ _   "
ECHO "  / /|_/ / ___/ /___\ \(_-</ /  ' \  "
ECHO " /_/  /_/_/  /____/___/___/_/_/_/_/  "



ECHO .
ECHO .

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Starting ConnectionCloud ...
ECHO .
ECHO .

start cmd.exe /k ConnectionCloud\ConnectionCloud.exe
IF %ERRORLEVEL% EQU 9009 (
  ECHO error - ConnectionCloud.exe not found in your PATH
 )

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] ConnectionCloud started ...

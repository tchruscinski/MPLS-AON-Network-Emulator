@ECHO OFF



COLOR 0A
ECHO "    __  ______  __   ____        "
ECHO "   /  |/  / _ \/ /  / __/___()__ _   "
ECHO "  / /|_/ / ___/ /___\ \(_-</ /  ' \  "
ECHO " /_/  /_/_/  /____/___/___/_/_/_/_/  "



ECHO .
ECHO .

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Starting Connection Cloud...
ECHO .
ECHO .

start cmd.exe /k "Connection Cloud\ConnectionCloud\ConnectionCloud\bin\Debug\ConnectionCloud.exe"

IF %ERRORLEVEL% EQU 9009 (
  ECHO error - ConnectionCloud.exe not found in your PATH
 ) 

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Connection Cloud started ...

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Starting Management System...
ECHO .
ECHO .

start cmd.exe /k "Managment System\Management System\bin\Debug\Management System.exe"

IF %ERRORLEVEL% EQU 9009 (
  ECHO error - System Management.exe not found in your PATH
 ) 

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Management System started ...

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Starting Router...
ECHO .
ECHO .

start cmd.exe /k Router\Router\bin\Debug\Router.exe

IF %ERRORLEVEL% EQU 9009 (
  ECHO error - Router.exe not found in your PATH
 ) 

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Routerstarted ...

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Starting Host ...
ECHO .
ECHO .

start cmd.exe /k Host\bin\Debug\Host.exe

IF %ERRORLEVEL% EQU 9009 (
  ECHO error - Host.exe not found in your PATH
 ) 

set TIMESTAMP=%DATE:/=-%@%TIME::=-%
ECHO [%TIMESTAMP%] Host started ...
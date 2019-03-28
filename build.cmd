@echo off
cls

REM IF EXIST "paket.lock" (
REM   .paket\paket.exe restore
REM ) ELSE (
REM   .paket\paket.exe install
REM )

REM if errorlevel 1 (
REM   exit /b %errorlevel%
REM )

REM packages\build\FAKE\tools\FAKE.exe build.fsx %*

dotnet tool install fake-cli -g
fake run build.fsx %*

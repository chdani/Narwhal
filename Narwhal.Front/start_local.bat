@echo off

title Narwhal - Front

if not exist "%~dp0\temp" mkdir %~dp0\temp
if not exist "%~dp0\temp\logs" mkdir %~dp0\temp\logs
if not exist "%~dp0\temp\temp" mkdir %~dp0\temp\temp

cd %~dp0\temp

copy /Y %~dp0\nginx.conf.template %~dp0\temp\nginx.conf
copy /Y %~dp0\nginx-1.18.0\conf\mime.types %~dp0\temp\mime.types

%~dp0\fart199b_win64\fart.exe %~dp0\temp\nginx.conf "${SERVICE_HOST}" "127.0.0.1"
%~dp0\fart199b_win64\fart.exe %~dp0\temp\nginx.conf "${SERVICE_PORT}" "6161"
%~dp0\fart199b_win64\fart.exe %~dp0\temp\nginx.conf "${APP_HOST}" "127.0.0.1"
%~dp0\fart199b_win64\fart.exe %~dp0\temp\nginx.conf "${APP_PORT}" "6162"

echo Running nginx...

%~dp0\nginx-1.18.0\nginx.exe -g "daemon off; master_process off;" -c %~dp0\temp\nginx.conf
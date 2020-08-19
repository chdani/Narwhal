@echo off

title Narwhal - Database

set DATABASE_LOCATION=%~dp0\..\Playground\Database

if not exist "%DATABASE_LOCATION%" mkdir "%DATABASE_LOCATION%"
%~dp0\mongodb-4.4.0\bin\mongod.exe --dbpath "%DATABASE_LOCATION%"
@echo off

title Narwhal - Importer

PowerShell.exe -Command "& '%~dp0\import_navwarnings.ps1'"
PowerShell.exe -Command "& '%~dp0\import_tracking.ps1'"
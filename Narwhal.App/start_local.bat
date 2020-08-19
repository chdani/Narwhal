@echo off

title Narwhal - App

cd %~dp0

IF NOT EXIST "node_modules" (
    call npm install --prefer-offline --no-audit
)

call npm start
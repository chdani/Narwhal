@echo off

title Narwhal - App

docker build -t narwhal_app %~dp0
docker run --rm -it -p 6162:80 narwhal_app
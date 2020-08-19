@echo off

title Narwhal - Service

docker build -t narwhal_service %~dp0
docker run --rm -it -p 6161:6161 narwhal_service
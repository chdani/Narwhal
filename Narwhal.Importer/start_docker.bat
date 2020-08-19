@echo off

title Narwhal - Importer

docker build -t narwhal_importer %~dp0
docker run --rm -it -v %~dp0\..\Playground\Storage:/data narwhal_importer
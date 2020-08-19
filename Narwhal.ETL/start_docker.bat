@echo off

title Narwhal - ETL

docker build -t narwhal_etl %~dp0
docker run --rm -it -v %~dp0\..\Playground\Storage:/data narwhal_etl
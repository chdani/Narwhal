@echo off

title Narwhal - Database

docker build -t narwhal_database %~dp0
docker run --rm -it -p 27017:27017 -v %~dp0\..\Playground\Database:/data/db narwhal_database
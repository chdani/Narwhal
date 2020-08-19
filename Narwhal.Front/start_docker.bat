@echo off

title Narwhal - Front

docker build -t narwhal_front %~dp0
docker run --rm -it -p 80:80 narwhal_front
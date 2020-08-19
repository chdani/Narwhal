@echo off

title Narwhal - Messaging

docker build -t narwhal_messaging %~dp0
docker run --rm -it -p 1883:1883 narwhal_messaging
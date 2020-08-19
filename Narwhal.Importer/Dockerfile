# docker build -t narwhal_importer .

FROM mcr.microsoft.com/powershell:7.0.3-debian-11

ENV STORAGE_DIRECTORY=/data
VOLUME /data

COPY import.ps1 .
CMD pwsh import.ps1
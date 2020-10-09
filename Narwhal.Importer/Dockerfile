# docker build -t narwhal_importer .

FROM mcr.microsoft.com/powershell:7.0.3-debian-11

ENV STORAGE_DIRECTORY=/data
VOLUME /data

COPY import_navwarnings.ps1 .
COPY import_tracking.ps1 .

CMD pwsh import_navwarnings.ps1 && pwsh import_tracking.ps1
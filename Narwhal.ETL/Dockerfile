# docker build -t narwhal_etl .

FROM python:3.7

RUN pip install pymongo
RUN pip install paho-mqtt

ENV STORAGE_DIRECTORY=/data
VOLUME /data

ENV DATABASE_URL=mongodb://1.2.3.4

COPY process.py .
CMD python process.py
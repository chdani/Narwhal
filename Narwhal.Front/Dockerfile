# docker build -t narwhal_front .

FROM nginx:1.18.0

#COPY nginx.conf /etc/nginx/conf.d/default.conf

ENV SERVICE_HOST=127.0.0.1
ENV SERVICE_PORT=6161
ENV APP_HOST=127.0.0.1
ENV APP_PORT=6162

COPY nginx.conf.template .
CMD [ "/bin/sh" , "-c" , "envsubst '${SERVICE_HOST},${SERVICE_PORT},${APP_HOST},${APP_PORT}' < nginx.conf.template > nginx.conf && exec nginx -g 'daemon off; master_process off;' -c /nginx.conf" ]

EXPOSE 80
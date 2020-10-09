# docker build -t narwhal_app .

FROM node:14.10.1 AS build
COPY . /src
WORKDIR /src
RUN npm install --no-audit
RUN npm run-script publish

FROM nginx:1.18.0 AS final
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /src/dist /usr/share/nginx/html
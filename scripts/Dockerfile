FROM apache/kafka:latest AS build
USER root
WORKDIR /app

RUN apk add --no-cache curl

COPY .env .env
COPY ./init-databases.sh ./init-databases.sh
RUN chmod +x ./init-databases.sh

ENTRYPOINT ["bash", "./init-databases.sh"]
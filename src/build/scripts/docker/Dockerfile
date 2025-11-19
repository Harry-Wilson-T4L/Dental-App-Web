FROM mcr.microsoft.com/dotnet/aspnet:6.0.16-focal
  
LABEL maintainer="DevGuild <contact@devguild.ru>"

RUN apt-get update \
 && apt-get install -y libfontconfig1 \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY ./app .
RUN chmod +x /app/docker-entrypoint.sh

EXPOSE 80/tcp

VOLUME /app/wwwroot/images
VOLUME /app/wwwroot/files
VOLUME /app/data-protection

ENTRYPOINT ["./docker-entrypoint.sh"]
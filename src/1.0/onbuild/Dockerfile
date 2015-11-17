FROM dotnet:1.0

RUN mkdir -p /dotnetapp
WORKDIR /dotnetapp

ENTRYPOINT dotnet run

ONBUILD COPY . /dotnetapp
ONBUILD RUN dotnet restore --runtime ubuntu.14.04-x64

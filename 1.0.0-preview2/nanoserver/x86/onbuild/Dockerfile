FROM microsoft/dotnet:1.0.0-preview2-nanoserver-x86

RUN mkdir dotnetapp
WORKDIR dotnetapp

ENTRYPOINT ["dotnet", "run"]

ONBUILD COPY . .
ONBUILD RUN dotnet restore

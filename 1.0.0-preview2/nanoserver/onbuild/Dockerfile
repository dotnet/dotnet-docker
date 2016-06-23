FROM microsoft/dotnet:1.0.0-preview2-nanoserver-sdk

RUN mkdir dotnetapp
WORKDIR dotnetapp

ENTRYPOINT ["dotnet", "run"]

ONBUILD COPY . .
ONBUILD RUN dotnet restore

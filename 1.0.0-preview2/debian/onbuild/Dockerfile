FROM microsoft/dotnet:1.0.0-preview2-sdk

RUN mkdir -p /dotnetapp
WORKDIR /dotnetapp

ENTRYPOINT ["dotnet", "run"]

ONBUILD COPY . /dotnetapp
ONBUILD RUN dotnet restore

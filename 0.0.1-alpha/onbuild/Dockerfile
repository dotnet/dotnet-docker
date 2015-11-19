FROM microsoft/dotnet:0.0.1-alpha

RUN mkdir -p /dotnetapp
WORKDIR /dotnetapp

ENTRYPOINT ["dotnet", "run"]

ONBUILD COPY . /dotnetapp
ONBUILD RUN dotnet restore

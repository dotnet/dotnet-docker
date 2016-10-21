FROM microsoft/dotnet:1.0.0-preview2-nanoserver-sdk

RUN mkdir dotnetapp
WORKDIR dotnetapp

ENTRYPOINT ["dotnet", "out/dotnetapp"]

ONBUILD COPY project.json dotnetapp
ONBUILD RUN dotnet restore
ONBUILD COPY . /dotnetapp
ONBUILD RUN dotnet publish -c Release -o out

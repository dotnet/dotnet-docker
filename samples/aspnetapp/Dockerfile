FROM microsoft/dotnet:sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY aspnetapp/*.csproj ./aspnetapp/
RUN dotnet restore

# copy and build everything else
COPY aspnetapp/. ./aspnetapp/

RUN dotnet build

FROM build AS testrunner
WORKDIR /app/tests
ENTRYPOINT ["dotnet", "test","--logger:trx"]

FROM build AS test
WORKDIR /app/tests
RUN dotnet test

FROM test AS publish
WORKDIR /app/aspnetapp
RUN dotnet publish -o out

FROM microsoft/dotnet:2.0-runtime AS runtime
WORKDIR /app
COPY --from=publish /app/aspnetapp/out ./
ENTRYPOINT ["dotnet", "aspnetapp.dll"]

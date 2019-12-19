# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY complexapp/*.csproj complexapp/
COPY libfoo/*.csproj libfoo/
COPY libbar/*.csproj libbar/
RUN dotnet restore complexapp/complexapp.csproj

# copy and build app and libraries
COPY complexapp/ complexapp/
COPY libfoo/ libfoo/
COPY libbar/ libbar/
WORKDIR /source/complexapp
RUN dotnet build -c release --no-restore

# test app
FROM build AS test
COPY tests/ /source/tests/
WORKDIR /source/tests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

# Uncomment out next `FROM` and 'RUN' lines if you want to run tests as
# part of docker build and for test failure to cascade to build failure
# otherwise, you can build and run 'test' stage using the test stage ENTRYPOINT
# FROM test
# RUN dotnet test

FROM build AS publish
WORKDIR /source/complexapp
RUN dotnet publish -c release --no-build -o /app

# final stage, which will result in generated image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet","complexapp.dll"]

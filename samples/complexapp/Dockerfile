# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY complexapp/*.csproj complexapp/
COPY libfoo/*.csproj libfoo/
COPY libbar/*.csproj libbar/
COPY tests/*.csproj tests/
RUN dotnet restore

# copy and build app and libraries
COPY complexapp/ complexapp/
COPY libfoo/ libfoo/
COPY libbar/ libbar/
COPY tests/ tests/
RUN dotnet build

# test app
FROM build AS test
WORKDIR /source/tests

# Uncomment out next 'RUN' line if you want to run tests as part of docker build
# and for test failure to cascade to build failure
# otherwise, you can build and run 'test' stage using the test ENTRYPOINT
#RUN dotnet test --logger:trx
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM build AS publish
RUN dotnet publish -c release -o /app

# final stage, which will result in generated image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["./complexapp"]

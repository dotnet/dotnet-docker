ARG base_image
FROM $base_image

ARG netcoreapp_version
ARG optional_new_args

WORKDIR testApp
RUN dotnet new console --framework netcoreapp$netcoreapp_version $optional_new_args
RUN dotnet restore -s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json -s https://api.nuget.org/v3/index.json
RUN dotnet build

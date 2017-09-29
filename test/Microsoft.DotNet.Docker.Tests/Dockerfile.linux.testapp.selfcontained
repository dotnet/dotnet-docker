ARG base_image
FROM $base_image

ARG rid
RUN dotnet restore -r $rid -s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json -s https://api.nuget.org/v3/index.json

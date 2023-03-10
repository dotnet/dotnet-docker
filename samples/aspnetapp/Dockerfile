# https://hub.docker.com/_/microsoft-dotnet
ARG SDK_IMAGE
ARG RUNTIME_IMAGE

FROM $SDK_IMAGE AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY aspnetapp/*.csproj .
RUN dotnet restore --use-current-runtime

# copy everything else and build app
COPY aspnetapp/. .
RUN dotnet publish --use-current-runtime --self-contained false --no-restore -o /app


# final stage/image
FROM $RUNTIME_IMAGE
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "aspnetapp.dll"]

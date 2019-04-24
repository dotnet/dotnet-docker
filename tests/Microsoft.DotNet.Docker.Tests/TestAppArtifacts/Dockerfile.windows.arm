ARG sdk_image
ARG runtime_image

FROM $sdk_image as build

EXPOSE 80

WORKDIR app
COPY . .

# Trigger registry hive creation to workaround intermittent Windows arm Docker issue
# https://github.com/dotnet/dotnet-docker/issues/1054
USER ContainerAdministrator

RUN dotnet publish -c Release -o out


FROM $runtime_image AS fx_dependent_app
EXPOSE 80
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "app.dll"]

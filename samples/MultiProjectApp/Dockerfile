# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

# build copies all project files and restores NuGet packages
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:11.0 AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore as distinct layers
COPY --link Directory.Build.props .
COPY --link NuGet.config .
COPY --link MultiProjectApp/*.csproj MultiProjectApp/
COPY --link LibFoo/*.csproj LibFoo/
COPY --link LibBar/*.csproj LibBar/
RUN dotnet restore -a $TARGETARCH MultiProjectApp/MultiProjectApp.csproj

# Copy source code and publish app
COPY --link MultiProjectApp/ MultiProjectApp/
COPY --link LibFoo/ LibFoo/
COPY --link LibBar/ LibBar/


# test-build builds the xUnit test project
FROM build AS test-build

COPY --link Tests/*.csproj Tests/
WORKDIR /source/Tests
RUN dotnet restore

COPY --link Tests/ .
RUN dotnet build --no-restore


# test-entrypoint exposes tests as the default executable for the stage
FROM test-build AS test
ENTRYPOINT ["dotnet", "test", "--no-build", "--logger:trx"]


# publish builds and publishes MultiProjectApp
FROM build AS publish
WORKDIR /source/MultiProjectApp
RUN dotnet publish -a $TARGETARCH --no-restore -o /app


# final is the final runtime stage for running the app
FROM mcr.microsoft.com/dotnet/runtime:11.0 AS final
WORKDIR /app
COPY --link --from=publish /app .
ENTRYPOINT ["dotnet", "MultiProjectApp.dll"]

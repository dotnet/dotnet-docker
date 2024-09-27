# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

# build copies all project files and restores NuGet packages
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG TARGETARCH
WORKDIR /source

# Copy project file and restore as distinct layers
COPY --link complexapp/*.csproj complexapp/
COPY --link libfoo/*.csproj libfoo/
COPY --link libbar/*.csproj libbar/
RUN dotnet restore -a $TARGETARCH complexapp/complexapp.csproj

# Copy source code and publish app
COPY --link complexapp/ complexapp/
COPY --link libfoo/ libfoo/
COPY --link libbar/ libbar/


# test-build builds the xUnit test project
FROM build AS test-build

COPY --link tests/*.csproj tests/
WORKDIR /source/tests
RUN dotnet restore

COPY --link tests/ .
RUN dotnet build --no-restore


# Run tests in a separate stage, to optimize caching when running locally.
# If you need to re-run tests without re-building, you can pass the Docker build
# argument `--no-cache-filter test-run` to re-run only this stage.
# test-run runs the test project
FROM test-build AS test-run

# Learn more about the `dotnet test` command:
#     https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test
RUN dotnet test --no-build \
    --logger "console;verbosity=detailed" \
    --logger "trx;logfilename=TestResults.trx" \
    # Without this condition, a failing test would cause the Dockerfile build to fail.
    # This would make it impossible to get the test results out of the test-output stage.
    || true

# test-output is a from-scratch stage containing only test results
FROM scratch AS test-output
COPY --from=test-run /source/tests/TestResults/TestResults.trx .


# test-entrypoint exposes tests as the default executable for the stage
FROM test-build AS test-entrypoint
ENTRYPOINT ["dotnet", "test", "--no-build", "--logger:trx"]


# publish builds and publishes complexapp
FROM build AS publish
WORKDIR /source/complexapp
RUN dotnet publish -a $TARGETARCH --no-restore -o /app


# final is the final runtime stage for running the app
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --link --from=publish /app .
ENTRYPOINT ["dotnet", "complexapp.dll"]

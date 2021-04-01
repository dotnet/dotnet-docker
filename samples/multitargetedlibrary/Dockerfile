FROM scratch AS source
WORKDIR /source

# copy source
COPY . .

# test stage -- target with: docker build --target test60
FROM mcr.microsoft.com/dotnet/nightly/sdk:6.0 AS test60
COPY --from=source /source /source
WORKDIR /source/tests
RUN dotnet build -f net6.0
ENTRYPOINT ["dotnet", "test", "-f", "net6.0", "--no-build", "--logger:trx"]

# test stage -- target with: docker build --target test31
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS test31
COPY --from=source /source /source
WORKDIR /source/tests
RUN dotnet build -f netcoreapp3.1
ENTRYPOINT ["dotnet", "test", "-f", "netcoreapp3.1", "--no-build", "--logger:trx"]

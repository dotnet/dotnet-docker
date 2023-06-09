FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

ARG SAS_QUERY_STRING

RUN dotnet_version=6.0.18 \
    && curl -fSL --output dotnet-runtime-deps.rpm https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-deps-$dotnet_version-cm.2-aarch64.rpm$SAS_QUERY_STRING \
    && dotnet_sha512='6e80871efe0575afff29f3f2e79230def7c5aa796ffa699fd2c3012ec8988bda3bcc49b16bb627087dc5740f7b2a6ed68910114a858cdd54e0438372739cfe54' \
    && echo "$dotnet_sha512  dotnet-runtime-deps.rpm" | sha512sum -c -


FROM mcr.microsoft.com/cbl-mariner/base/core:2.0

ENV \
    # Configure web servers to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true

RUN tdnf install -y \
        ca-certificates \
        \
        # .NET dependencies
        glibc \
        icu \
        krb5 \
        libgcc \
        libstdc++ \
        openssl-libs \
        zlib \
    && tdnf clean all

COPY --from=installer /dotnet-runtime-deps.rpm /dotnet-runtime-deps.rpm

# Install dotnet-runtime-deps package
RUN tdnf install -y --disablerepo=* dotnet-runtime-deps.rpm \
    && rm dotnet-runtime-deps.rpm

ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM cblmariner.azurecr.io/base/core:1.0 AS installer

RUN tdnf install -y ca-certificates-microsoft \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version='7.0.0-alpha.1.22066.4' \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='54035da58611ad409fa12026aabc3b1eb1ace645e2f4409762968d2197d6d288856425d0433eabc564a69906a08fe9dbab8ce730d5f7d649c9b09ddeb94a2477' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-cbl-mariner1.0-distroless-amd64

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-alpha.1.22066.4

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

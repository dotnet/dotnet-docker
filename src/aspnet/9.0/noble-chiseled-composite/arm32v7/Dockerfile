ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.0-rtm.24516.19  \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-9.0.0-linux-arm.tar.gz \
    && dotnet_sha512='53e64541b6adb3f4b9c32e2998d3e9175a057d9d763bff42636be3c2e16b4e18e43cd83c00f8ef65a5af24c032ac0346e7fe5a0b1a964e59a5340186aa56a9a3' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:9.0.0-noble-chiseled-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-rtm.24516.10 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-rtm.24516.19

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]

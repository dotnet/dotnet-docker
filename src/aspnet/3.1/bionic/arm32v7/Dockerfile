ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.23-bionic-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.23 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='f4e79df54702ffd11c0e86300e9649c54ac30df6d5d42c7c7d8b78e1fb7987fb17789be853546fe6938d573dbda552bec07404f39f8b35efd39da33dff28c13f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz

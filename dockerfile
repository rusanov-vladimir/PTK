FROM mono:5.18.1.0-slim AS build-env

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu57 \
        liblttng-ust0 \
        libssl1.0.2 \
        libstdc++6 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install Curl
RUN apt-get update \
    && apt-get -qq -y install curl

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 3.0.100-preview5-011568

RUN curl -SL --output dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='207af1f3161cbd0864902370475fcad5b500bf2b7199b06445e7c338053bc7cb0157ee1b4d00a37a351eae6e209417ee4a15eeacee086aac0ac1be7a62f3ad0b' \
    && echo "$dotnet_sha512 dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
&& ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help

WORKDIR /app
#ENV HTTP_PROXY "http://proxy-md.ktn.group:3128"
#ENV HTTPS_PROXY "http://proxy-md.ktn.group:3128"

# Copy fsproj and restore as distinct layers
COPY . ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -f netcoreapp3.0 -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.0.0-preview5
WORKDIR /app
COPY --from=build-env /app/out/ .
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]
CMD [""]
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env

ENV MONO_VERSION 5.18.1.0

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF

RUN echo "deb http://download.mono-project.com/repo/debian stable-stretch/snapshots/$MONO_VERSION main" > /etc/apt/sources.list.d/mono-official-stable.list \
  && apt-get update \
  && apt-get install -y mono-runtime binutils curl mono-devel ca-certificates-mono fsharp mono-vbnc nuget referenceassemblies-pcl \
  && rm -rf /var/lib/apt/lists/* /tmp/*

ENV FrameworkPathOverride=/usr/lib/mono/4.5/

WORKDIR /app
#ENV HTTP_PROXY "http://proxy-md.ktn.group:3128"
#ENV HTTPS_PROXY "http://proxy-md.ktn.group:3128"

# Copy fsproj and restore as distinct layers
COPY . ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /app
COPY --from=build-env /app/src/PTK/out/ .
ENTRYPOINT ["dotnet", "PTK.dll"]
CMD [""]
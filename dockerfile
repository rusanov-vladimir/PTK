FROM microsoft/dotnet:2.1.403-sdk AS build-env
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
FROM microsoft/dotnet:2.1.5-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/src/PTK/out/ .
ENTRYPOINT ["dotnet", "PTK.dll"]
CMD [""]
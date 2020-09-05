FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build-env

WORKDIR /app


# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

COPY . ./
# copy everything else and build
RUN dotnet publish -c Release  --output out

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim   
WORKDIR /app
COPY --from=build-env  /app/out .
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "ShopWebMvc.dll"]

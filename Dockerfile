FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY src/AnagramSolver/*.csproj .
RUN dotnet restore

# copy everything else and build app
COPY src/AnagramSolver/. ./

RUN dotnet publish -c Release -o /app/out

FROM node:18 as client-build

WORKDIR /app/client
COPY src/AnagramSolver/ClientApp ./

RUN npm install
RUN npm run build

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime

ENV ASPNETCORE_URLS=https://+:443;

WORKDIR /app
COPY --from=build /app/out ./
COPY --from=client-build /app/client/build ./wwwroot

EXPOSE 443

ENTRYPOINT ["dotnet", "AnagramSolver.dll"]


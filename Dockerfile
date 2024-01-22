FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY Cyh/*.csproj ./Cyh/
COPY CyhEFCore/*.csproj ./CyhEFCore/
COPY CyhModules/*.csproj ./CyhModules/
COPY CyhWebServices/*.csproj ./CyhWebServices/
COPY MemeRepository/*.csproj ./MemeRepository/
COPY MemeRepositoryDb/*.csproj ./MemeRepositoryDb/
COPY MemeRepository.sln ./
RUN dotnet restore

COPY . ./
RUN dotnet publish MemeRepository -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS="http://*:80"
ENTRYPOINT ["dotnet", "MemeRepository.dll"]
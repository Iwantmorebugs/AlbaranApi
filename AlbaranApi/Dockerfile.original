FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AlbaranApi/AlbaranApi.csproj", "AlbaranApi/"]
RUN dotnet restore "AlbaranApi/AlbaranApi.csproj"
COPY . .
WORKDIR "/src/AlbaranApi"
RUN dotnet build "AlbaranApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlbaranApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlbaranApi.dll"]
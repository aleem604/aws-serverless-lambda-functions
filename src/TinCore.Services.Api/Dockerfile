FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["TinCore.Services.Api/TinCore.Services.Api.csproj", "TinCore.Services.Api/"]
RUN dotnet restore "TinCore.Services.Api/TinCore.Services.Api.csproj"
COPY . .
WORKDIR "/src/TinCore.Services.Api"
RUN dotnet build "TinCore.Services.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TinCore.Services.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TinCore.Services.Api.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Blogifier/Blogifier.csproj", "src/Blogifier/"]
COPY ["src/Blogifier.Widgets/Blogifier.Widgets.csproj", "src/Blogifier.Widgets/"]
COPY ["src/Blogifier.Core/Blogifier.Core.csproj", "src/Blogifier.Core/"]
RUN dotnet restore "src/Blogifier/Blogifier.csproj"
COPY . .
WORKDIR "/src/src/Blogifier"
RUN dotnet build "Blogifier.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blogifier.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blogifier.dll"]
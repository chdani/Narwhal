# docker build -t narwhal_service .

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
COPY . /src
WORKDIR /src
RUN dotnet restore "Narwhal.Service.csproj"
RUN dotnet build "Narwhal.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Narwhal.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Narwhal.Service.dll"]
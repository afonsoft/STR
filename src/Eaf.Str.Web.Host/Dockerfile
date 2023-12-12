FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8001
ENV ASPNETCORE_ENVIRONMENT=Local
ENV ASPNETCORE_URLS=http://+:8001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./nuget.config", "./"]
COPY ["src/Eaf.Str.Web.Host/Eaf.Str.Web.Host.csproj", "src/Eaf.Str.Web.Host/"]
COPY ["src/Eaf.Str.Application/Eaf.Str.Application.csproj", "src/Eaf.Str.Application/"]
COPY ["src/Eaf.Str.EntityFrameworkCore/Eaf.Str.EntityFrameworkCore.csproj", "src/Eaf.Str.EntityFrameworkCore/"]
COPY ["src/Eaf.Str.Core/Eaf.Str.Core.csproj", "src/Eaf.Str.Core/"]
RUN dotnet restore "src/Eaf.Str.Web.Host/Eaf.Str.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/Eaf.Str.Web.Host"
RUN dotnet build "Eaf.Str.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Eaf.Str.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ="America/Sao_Paulo"

RUN apt-get update && \
    apt-get install -yq tzdata libc6-dev libgdiplus libmsquic zlib1g-dev && \
    ln -fs /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata

ENTRYPOINT ["dotnet", "Eaf.Str.Web.Host.dll"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
ADD . .

RUN dotnet restore "WebApp/WebApp.csproj"
RUN dotnet build WebApp -c Release -o /app/build

FROM build AS publish
RUN dotnet publish WebApp -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=publish /app/publish .

RUN apt-get update && apt-get install -y ffmpeg libgdiplus

ENTRYPOINT ["dotnet", "WebApp.dll"]

EXPOSE 80
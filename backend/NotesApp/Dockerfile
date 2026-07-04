FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["NotesApp.Api/NotesApp.Api.csproj", "NotesApp.Api/"]
COPY ["NotesApp.Application/NotesApp.Application.csproj", "NotesApp.Application/"]
COPY ["NotesApp.Core/NotesApp.Core.csproj", "NotesApp.Core/"]
COPY ["NotesApp.Infrastructure/NotesApp.Infrastructure.csproj", "NotesApp.Infrastructure/"]
RUN dotnet restore "NotesApp.Api/NotesApp.Api.csproj"

# Copy the remaining files and build
COPY . .
WORKDIR "/src/NotesApp.Api"
RUN dotnet build "NotesApp.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "NotesApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotesApp.Api.dll"]

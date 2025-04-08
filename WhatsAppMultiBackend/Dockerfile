# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY WhatsAppMultiBackend.sln .
COPY WhatsAppMultiBackend/

# Restore the project
WORKDIR /src/WhatsAppMultiBackend
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o /app/out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "WhatsAppMultiBackend.dll"]

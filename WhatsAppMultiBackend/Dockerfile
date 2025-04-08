# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY WhatsAppMultiBackend.sln .
COPY WhatsAppMultiBackend/ /src/WhatsAppMultiBackend/

# Restore the project
WORKDIR /src/WhatsAppMultiBackend
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o /app/out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Create a volume for the SQLite database
VOLUME /app/data

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose ports
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "WhatsAppMultiBackend.dll"]

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . .
WORKDIR /app/WhatsAppMultiBackend
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/WhatsAppMultiBackend/out .

ENTRYPOINT ["dotnet", "WhatsAppMultiBackend.dll"]

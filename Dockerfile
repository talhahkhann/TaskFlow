# Step 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything and build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Step 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080   
ENV ASPNETCORE_ENVIRONMENT=Development

# Expose port
EXPOSE 8080

ENTRYPOINT ["dotnet", "TaskFlow.dll"]

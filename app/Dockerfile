FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /build

ARG version=0.0.0

COPY ./Microservice/Microservice.csproj ./Microservice/Microservice.csproj
COPY ./Microservice.Tests/Microservice.Tests.csproj ./Microservice.Tests/Microservice.Tests.csproj

RUN dotnet restore ./Microservice/Microservice.csproj
RUN dotnet restore ./Microservice.Tests/Microservice.Tests.csproj

COPY . .

RUN dotnet publish --configuration Release /p:Version=$version ./Microservice/Microservice.csproj
RUN dotnet test --configuration Release ./Microservice.Tests/Microservice.Tests.csproj

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app

EXPOSE 5000
EXPOSE 5001

COPY --from=build /build/Microservice/bin/Release/netcoreapp3.1/publish .

# Using the exec form of CMD allows SIGINT to be passed from container to app
CMD ["dotnet", "Microservice.dll"]

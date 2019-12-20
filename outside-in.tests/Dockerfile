FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /build

COPY ./Microservice.OutsideIn.Tests/Microservice.OutsideIn.Tests.csproj ./Microservice.OutsideIn.Tests/Microservice.OutsideIn.Tests.csproj

RUN dotnet restore ./Microservice.OutsideIn.Tests/Microservice.OutsideIn.Tests.csproj

COPY . .

RUN dotnet build --no-incremental -c Release ./Microservice.OutsideIn.Tests/Microservice.OutsideIn.Tests.csproj

CMD ["dotnet", "test", "-c", "Release", "./Microservice.OutsideIn.Tests/Microservice.OutsideIn.Tests.csproj"]

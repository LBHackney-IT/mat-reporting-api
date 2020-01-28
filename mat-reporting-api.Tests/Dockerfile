FROM microsoft/dotnet:2.2-sdk

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./mat-reporting-api.sln ./
COPY ./mat-reporting-api/mat-reporting-api.csproj ./mat-reporting-api/
COPY ./mat-reporting-api.Tests/mat-reporting-api.Tests.csproj ./mat-reporting-api.Tests/

RUN dotnet restore ./mat-reporting-api/mat-reporting-api.csproj
RUN dotnet restore ./mat-reporting-api.Tests/mat-reporting-api.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out mat-reporting-api.Tests/mat-reporting-api.Tests.csproj

CMD dotnet test

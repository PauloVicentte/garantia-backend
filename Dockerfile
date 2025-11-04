FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "SolicitacaoGarantia_Functions/SolicitacaoGarantia_Functions.csproj"
RUN dotnet publish "SolicitacaoGarantia_Functions/SolicitacaoGarantia_Functions.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "SolicitacaoGarantia_Functions.dll"]

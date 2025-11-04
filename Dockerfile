FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "SolicitacaoGarantia.Functions/SolicitacaoGarantia.Functions.csproj"
RUN dotnet publish "SolicitacaoGarantia.Functions/SolicitacaoGarantia.Functions.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

# Rodar a função com o host local do Azure Functions
CMD ["func", "start", "--cors", "*", "--port", "8080", "--verbose"]

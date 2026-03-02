## ESTÁGIO 1: Base de Execução (Runtime)
## Usa uma imagem leve apenas com o necessário para rodar o app
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
#WORKDIR /app
## Porta padrão que o Kestrel (servidor do .NET) usa no container
#EXPOSE 8080
#EXPOSE 8081
#
## ESTÁGIO 2: Build (SDK)
## Usa o SDK completo para compilar o código
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
## Copia o arquivo de projeto e restaura as dependências (NuGet)
## Isso é feito separado para aproveitar o cache do Docker
#COPY ["GeekShopping.ProductAPI.csproj", "GeekShopping.ProductAPI/"]
#RUN dotnet restore "GeekShopping.ProductAPI/GeekShopping.ProductAPI.csproj"
#
## Copia o restante dos arquivos e compila
#COPY . .
#WORKDIR "/src/GeekShopping.ProductAPI"
#RUN dotnet build "GeekShopping.ProductAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## ESTÁGIO 3: Publish
## Gera os binários finais otimizados
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "GeekShopping.ProductAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## ESTÁGIO 4: Final
## Pega apenas os arquivos publicados e coloca na imagem base de execução
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "GeekShopping.ProductAPI.dll"]
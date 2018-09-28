FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 18707
EXPOSE 44395

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ObITwahn.Trinity.Services.Web/ObITwahn.Trinity.Services.Web.csproj", "ObITwahn.Trinity.Services.Web/"]
RUN dotnet restore "ObITwahn.Trinity.Services.Web/ObITwahn.Trinity.Services.Web.csproj"
COPY . .
WORKDIR "/src/ObITwahn.Trinity.Services.Web"
RUN dotnet build "ObITwahn.Trinity.Services.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ObITwahn.Trinity.Services.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ObITwahn.Trinity.Services.Web.dll"]
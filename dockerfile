

FROM mcr.microsoft.com/dotnet/aspnet:5.0



WORKDIR /app
COPY ./bin/Debug/net5.0 .
#COPY  ./published .
EXPOSE 80

ENTRYPOINT ["dotnet", "ShopApi.dll"]
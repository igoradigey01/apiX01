# apiX01

Content Management System from shopping asp net core 5. <br/>
Angular 13.0.3 <br/>
asp net core 5.0 <br/>
# Asp net core
//------ build prodject VS Code ------------- <br/>
ng build <br/>
//-------------- <br/>
dotnet restore <br/>
dotnet build <br/>
dotnet publish -c release -o published <br/>
-- remove shopapi_web old image in image---- <br/>
docker-compose up  //create shopapi_web image   in images <br/>
//-- push image shopapi_web on dockerHub --- <br/>
 docker pull igoradigey01/shopapi_web <br/>
 docker-compose up -d <br/>
docker-compose down <br/>
# VPS 
//------ work  on host provider VPS -------- <br/>
ssh root@46.---.--.--- <br/>
cd mydir <br/>
ls // show file in dir <br/>
//---- docker-отчет об использовании дискового пространства <br/>
docker system df <br/>
docker volume ls <br/>
docker volume prune <br/>

# Angular 
ng build <br/>
scp -r C:\Users\Ks34\Documents\AngularProject\xf01\dist\xf01 root@46.---.---.---:~/myapp/nginx/data <br/>


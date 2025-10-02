# Deployment Guide

## 🚀 Guia de Deploy

### **🔧 Pré-requisitos**

#### **Sistema Operacional**
- **Linux**: Ubuntu 20.04+ / CentOS 8+ / RHEL 8+
- **Windows**: Windows Server 2019+ / Windows 10+
- **macOS**: macOS 10.15+ (apenas desenvolvimento)

#### **Runtime Requirements**
```bash
# .NET 8.0 Runtime
dotnet --version  # Deve ser >= 8.0.0

# Docker (para containers)
docker --version  # Recomendado 24.0+
docker-compose --version  # v2.0+

# PostgreSQL (se não usar container)
psql --version  # 13.0+
```

### **🐳 Deploy com Docker**

#### **1. Preparação do Ambiente**
```bash
# Clone do repositório
git clone <repository-url>
cd backend/

# Configurar variáveis de ambiente
cp .env.example .env
```

#### **2. Configuração do .env**
```env
# Database
DATABASE_CONNECTION_STRING=Host=postgres;Database=ambev_dev;Username=postgres;Password=postgres123

# JWT
JWT_SECRET=your-super-secret-jwt-key-min-32-chars
JWT_EXPIRATION_HOURS=24

# CORS
ALLOWED_ORIGINS=http://localhost:3000,https://yourdomain.com

# Logging
LOG_LEVEL=Information
SERILOG_MIN_LEVEL=Information

# Services
REDIS_CONNECTION_STRING=redis:6379
MONGODB_CONNECTION_STRING=mongodb://mongodb:27017/ambev
```

#### **3. Build e Deploy**
```bash
# Build da aplicação
docker-compose build

# Deploy completo (API + Banco + Redis + MongoDB)
docker-compose up -d

# Verificar status
docker-compose ps

# Logs da aplicação
docker-compose logs -f webapi
```

#### **4. Health Check**
```bash
# Verificar se a API está respondendo
curl http://localhost:5000/health

# Swagger UI (desenvolvimento)
open http://localhost:5000/swagger
```

### **🔄 Deploy Tradicional (sem Docker)**

#### **1. Preparar Banco de Dados**
```sql
-- Conectar no PostgreSQL
CREATE DATABASE ambev_production;
CREATE USER ambev_user WITH ENCRYPTED PASSWORD 'secure_password';
GRANT ALL PRIVILEGES ON DATABASE ambev_production TO ambev_user;
```

#### **2. Build da Aplicação**
```bash
# Restore packages
dotnet restore

# Build em modo Release
dotnet build -c Release

# Executar migrações
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update --connection "Host=localhost;Database=ambev_production;Username=ambev_user;Password=secure_password"
```

#### **3. Publicação**
```bash
# Publish da aplicação
dotnet publish -c Release -o ./publish

# Copiar para servidor
scp -r ./publish/ user@server:/opt/ambev-api/

# No servidor - configurar como serviço systemd
sudo nano /etc/systemd/system/ambev-api.service
```

#### **4. Configuração do Systemd**
```ini
[Unit]
Description=Ambev Developer Evaluation API
After=network.target

[Service]
Type=simple
User=www-data
WorkingDirectory=/opt/ambev-api
ExecStart=/usr/bin/dotnet /opt/ambev-api/Ambev.DeveloperEvaluation.WebApi.dll
Restart=always
RestartSec=10
SyslogIdentifier=ambev-api
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
```

```bash
# Habilitar e iniciar serviço
sudo systemctl enable ambev-api
sudo systemctl start ambev-api
sudo systemctl status ambev-api
```

### **🌐 Configuração do Nginx**

#### **Reverse Proxy Setup**
```nginx
server {
    listen 80;
    server_name api.yourdomain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
        proxy_buffering off;
    }
}
```

#### **HTTPS com Let's Encrypt**
```bash
# Instalar Certbot
sudo apt install certbot python3-certbot-nginx

# Obter certificado SSL
sudo certbot --nginx -d api.yourdomain.com

# Renovação automática
sudo crontab -e
# Adicionar linha:
0 12 * * * /usr/bin/certbot renew --quiet
```

### **☁️ Deploy na Nuvem**

#### **Azure App Service**
```yaml
# azure-pipelines.yml
trigger:
  branches:
    include:
    - main

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'

steps:
- task: DotNetCoreCLI@2
  inputs:
    command: 'restore'
    projects: '**/*.csproj'

- task: DotNetCoreCLI@2
  inputs:
    command: 'build'
    projects: '**/*.csproj'
    arguments: '--configuration $(buildConfiguration)'

- task: DotNetCoreCLI@2
  inputs:
    command: 'publish'
    projects: 'src/Ambev.DeveloperEvaluation.WebApi/*.csproj'
    arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)'

- task: AzureWebApp@1
  inputs:
    azureSubscription: 'Azure-Subscription'
    appType: 'webAppLinux'
    appName: 'ambev-api'
    package: '$(Build.ArtifactStagingDirectory)/**/*.zip'
```

#### **AWS ECS (Fargate)**
```dockerfile
# Dockerfile.production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj", "WebApi/"]
COPY ["src/Ambev.DeveloperEvaluation.Application/Ambev.DeveloperEvaluation.Application.csproj", "Application/"]
COPY ["src/Ambev.DeveloperEvaluation.Domain/Ambev.DeveloperEvaluation.Domain.csproj", "Domain/"]
COPY ["src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj", "ORM/"]
COPY ["src/Ambev.DeveloperEvaluation.IoC/Ambev.DeveloperEvaluation.IoC.csproj", "IoC/"]
COPY ["src/Ambev.DeveloperEvaluation.Common/Ambev.DeveloperEvaluation.Common.csproj", "Common/"]

RUN dotnet restore "WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"
COPY src/ .
WORKDIR "/src/WebApi"
RUN dotnet build "Ambev.DeveloperEvaluation.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ambev.DeveloperEvaluation.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ambev.DeveloperEvaluation.WebApi.dll"]
```

### **📊 Monitoramento e Logs**

#### **Application Insights (Azure)**
```csharp
// Program.cs
services.AddApplicationInsightsTelemetry();

// appsettings.Production.json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-instrumentation-key"
  }
}
```

#### **ELK Stack (Elasticsearch + Logstash + Kibana)**
```yaml
# docker-compose.logging.yml
version: '3.8'
services:
  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:8.11.0
    environment:
      - discovery.type=single-node
      - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
    ports:
      - "9200:9200"

  kibana:
    image: docker.elastic.co/kibana/kibana:8.11.0
    environment:
      - ELASTICSEARCH_HOSTS=http://elasticsearch:9200
    ports:
      - "5601:5601"
    depends_on:
      - elasticsearch

  logstash:
    image: docker.elastic.co/logstash/logstash:8.11.0
    volumes:
      - ./logstash.conf:/usr/share/logstash/pipeline/logstash.conf
    depends_on:
      - elasticsearch
```

### **🔐 Segurança em Produção**

#### **Configurações de Segurança**
```json
// appsettings.Production.json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:5000"
      },
      "Https": {
        "Url": "https://0.0.0.0:5001",
        "Certificate": {
          "Path": "/etc/ssl/certs/cert.pfx",
          "Password": "certificate-password"
        }
      }
    }
  },
  "Security": {
    "RequireHttps": true,
    "UseHsts": true,
    "HstsMaxAge": 31536000
  }
}
```

#### **Firewall Rules**
```bash
# UFW Configuration
sudo ufw enable
sudo ufw allow 22/tcp      # SSH
sudo ufw allow 80/tcp      # HTTP
sudo ufw allow 443/tcp     # HTTPS
sudo ufw deny 5000/tcp     # Block direct API access
```

### **📋 Checklist de Deploy**

#### **Antes do Deploy**
- [ ] Testes unitários passando (100%)
- [ ] Testes de integração executados
- [ ] Code review completo
- [ ] Documentação atualizada
- [ ] Variáveis de ambiente configuradas
- [ ] Certificados SSL válidos
- [ ] Backup do banco de dados atual

#### **Durante o Deploy**
- [ ] Modo de manutenção ativado
- [ ] Aplicação antiga parada
- [ ] Migrações de banco executadas
- [ ] Nova versão iniciada
- [ ] Health checks verificados
- [ ] Logs monitorados por 15min

#### **Após o Deploy**
- [ ] Smoke tests executados
- [ ] Endpoints críticos testados
- [ ] Modo de manutenção desativado
- [ ] Equipe notificada
- [ ] Monitoramento ativo
- [ ] Rollback plan preparado

### **🔄 Estratégias de Rollback**

#### **Rollback Rápido**
```bash
# Docker
docker-compose down
docker-compose -f docker-compose.yml -f docker-compose.rollback.yml up -d

# Systemd
sudo systemctl stop ambev-api
sudo cp -r /opt/ambev-api-backup/* /opt/ambev-api/
sudo systemctl start ambev-api
```

#### **Blue-Green Deployment**
```yaml
# docker-compose.blue-green.yml
services:
  webapi-blue:
    build: .
    ports:
      - "5001:5000"
    
  webapi-green:
    build: .
    ports:
      - "5002:5000"
    
  nginx:
    image: nginx
    volumes:
      - ./nginx-blue-green.conf:/etc/nginx/nginx.conf
    ports:
      - "80:80"
```
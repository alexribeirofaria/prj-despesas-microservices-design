# 💸 Microservices Solution Design - Despesas Pessoais

> Solução de controle financeiro pessoal baseada em microserviços, com backend em .NET, frontend Angular/Ionic, API Gateway Ocelot, descoberta de serviços com Consul, autenticação via IdentityServer e persistência relacional com Entity Framework Core.

![.NET](https://img.shields.io/badge/.NET-10.0-512bd4?logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-20-dd0031?logo=angular&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ed?logo=docker&logoColor=white)
![Ocelot](https://img.shields.io/badge/API%20Gateway-Ocelot-111827)
![Consul](https://img.shields.io/badge/Discovery-Consul-f24c53)
![MySQL](https://img.shields.io/badge/Database-MySQL%20%7C%20SQL%20Server-4479a1)

## 📌 Sumário

- [🧭 Visão Geral](#-visão-geral)
- [🎯 Objetivos da Solução](#-objetivos-da-solução)
- [🧩 Capacidades Funcionais](#-capacidades-funcionais)
- [🏗️ Arquitetura](#️-arquitetura)
- [🔁 Fluxo de Requisição](#-fluxo-de-requisição)
- [🛰️ Serviços da Solução](#️-serviços-da-solução)
- [⚙️ Backend](#️-backend)
- [🖥️ Frontend](#️-frontend)
- [🔐 Autenticação e Autorização](#-autenticação-e-autorização)
- [🗄️ Persistência e Banco de Dados](#️-persistência-e-banco-de-dados)
- [🚪 API Gateway e Service Discovery](#-api-gateway-e-service-discovery)
- [🐳 Docker e Ambientes](#-docker-e-ambientes)
- [🚀 Execução Local](#-execução-local)
- [🧪 Testes e Qualidade](#-testes-e-qualidade)
- [📁 Estrutura de Pastas](#-estrutura-de-pastas)
- [⚠️ Pontos de Atenção Técnicos](#️-pontos-de-atenção-técnicos)
- [🛣️ Roadmap Técnico Sugerido](#️-roadmap-técnico-sugerido)
- [📄 Licença](#-licença)

## 🧭 Visão Geral

O **Microservices Solution Design - Despesas Pessoais** é uma plataforma para gerenciamento de finanças pessoais. A aplicação permite organizar receitas, despesas, categorias, lançamentos, saldo, perfil de usuário e indicadores financeiros por meio de uma interface web/mobile e APIs segregadas por domínio.

A solução foi desenhada para demonstrar uma arquitetura distribuída com responsabilidades bem definidas:

- 🖥️ **Frontend Angular/Ionic** para experiência do usuário, rotas protegidas, componentes reutilizáveis e clientes HTTP tipados.
- 🚪 **API Gateway Ocelot** como ponto único de entrada para APIs internas.
- 🧭 **Consul** para registro e descoberta de microserviços.
- 🔐 **IdentityServer** como Security Token Service para emissão e validação de tokens.
- ⚙️ **Microserviços .NET** para domínios de categoria, despesa e receita.
- 🧱 **Camadas compartilhadas** de domínio, aplicação, infraestrutura, exceções, CQRS, repositórios e Unit of Work.
- 🗄️ **Entity Framework Core** com suporte a MySQL/MariaDB e SQL Server.
- 🐳 **Docker Compose** para orquestração de gateway, serviços, STS e Consul.

## 🎯 Objetivos da Solução

- 🚪 Centralizar o acesso aos microserviços por meio de um API Gateway.
- 🧩 Separar domínios financeiros em serviços independentes e escaláveis.
- 🧠 Manter regras de negócio fora dos controllers, concentradas na camada de aplicação.
- 📦 Utilizar DTOs e AutoMapper para separar contratos de API das entidades de domínio.
- 🔐 Proteger recursos por autenticação JWT e políticas de autorização por perfil.
- 🗄️ Permitir evolução de persistência com estratégias por provedor de banco.
- 📱 Disponibilizar frontend responsivo com suporte a web, Android e iOS via Capacitor.
- 🧪 Apoiar a qualidade por meio de testes unitários, mocks, coverage e organização modular.

## 🧩 Capacidades Funcionais

| Área | Funcionalidade | Implementação principal |
| --- | --- | --- |
| 🔐 Autenticação | Login, claims, tokens, scopes e refresh token | `src/IdentityServer`, `src/BackEnd/Services/AuthService` |
| 🏷️ Categorias | CRUD de categorias e filtro por tipo | `CategoriaService`, `ICategoriaBusiness` |
| 📉 Despesas | CRUD de despesas do usuário | `DespesaService`, `DespesaBusinessImpl` |
| 📈 Receitas | CRUD de receitas do usuário | `ReceitaService`, `ReceitaBusinessImpl` |
| 🔁 Lançamentos | Agregação conceitual de movimentações financeiras | `Application`, `Domain`, `FrontEnd/pages/lancamentos` |
| 💰 Saldo | Cálculo e apresentação de saldo financeiro | `SaldoBusinessImpl`, `saldo.service.ts` |
| 📊 Dashboard | Indicadores e gráficos financeiros | `GraficoBusinessImpl`, `bar-chart`, `dashboard.service.ts` |
| 👤 Perfil | Dados do usuário e imagem de perfil | `UsuarioBusinessImpl`, `ImagemPerfilUsuarioBusinessImpl` |
| 📱 Mobile | Empacotamento para Android/iOS | `src/FrontEnd/app-android`, `src/FrontEnd/app-ios` |

## 🏗️ Arquitetura

A solução combina **microserviços**, **Clean Architecture em camadas**, **Repository Pattern**, **Unit of Work**, **DTOs**, **AutoMapper**, **CQRS** e **API Gateway Pattern**.

```text
Usuário
  -> Angular/Ionic SPA
  -> API Gateway Ocelot
  -> Consul Service Discovery
  -> Microserviços .NET
  -> Application Business Layer
  -> Domain Entities
  -> Infrastructure / Repository / Unit of Work
  -> Banco relacional
```

### 🧭 Visão Lógica

```mermaid
flowchart LR
    User[Usuário] --> Front[Angular / Ionic]
    Front --> Gateway[API Gateway Ocelot]
    Gateway --> Consul[Consul]
    Gateway --> Cat[Categoria Service]
    Gateway --> Desp[Despesa Service]
    Gateway --> Rec[Receita Service]
    Front --> STS[IdentityServer / STS]
    Cat --> App[Application Layer]
    Desp --> App
    Rec --> App
    App --> Domain[Domain Layer]
    App --> Infra[Infrastructure]
    Infra --> Db[(MySQL / SQL Server)]
```

### 🧱 Camadas Principais

| Camada | Caminho | Responsabilidade |
| --- | --- | --- |
| 🚪 Gateway | `src/api-gateway` | Entrada única, roteamento Ocelot, autenticação JWT e descoberta via Consul |
| 🔐 STS | `src/IdentityServer` | Emissão de tokens, recursos, scopes, clients, claims e Swagger do serviço de identidade |
| 🛰️ Services | `src/BackEnd/Services` | Microserviços HTTP por domínio |
| ⚙️ Application | `src/BackEnd/Application` | Casos de uso, regras de aplicação, DTOs, profiles AutoMapper e contratos de negócio |
| 🧬 Domain | `src/BackEnd/Domain` | Entidades, value objects e base de domínio |
| 🗄️ Infrastructure | `src/BackEnd/Infrastructure` | EF Core, DbContext, repositórios, Unit of Work, S3, estratégias de banco e mapeamentos |
| 🧰 CrossCutting | `src/BackEnd/CrossCutting` | CQRS, handlers e injeções transversais |
| 🛡️ GlobalException | `src/BackEnd/GlobalException` | Exceções customizadas e middleware de tratamento global |
| 🖥️ Frontend | `src/FrontEnd` | SPA Angular/Ionic, componentes, páginas, services, models e builds mobile |

## 🔁 Fluxo de Requisição

1. O usuário acessa a aplicação Angular.
2. O frontend autentica o usuário e armazena tokens conforme os services de autenticação.
3. As chamadas HTTP são enviadas para a URL base configurada em `src/FrontEnd/src/environments`.
4. O API Gateway recebe a requisição e aplica o middleware de token.
5. O Ocelot roteia a requisição para o serviço adequado usando configuração local e descoberta via Consul.
6. O microserviço resolve dependências, valida o usuário, chama a camada de aplicação e aplica regras de negócio.
7. A camada de infraestrutura persiste ou consulta dados por EF Core, repositórios e Unit of Work.
8. O DTO de resposta retorna ao frontend pelo gateway.

## 🛰️ Serviços da Solução

| Serviço | Caminho | Porta interna | Rota base | Responsabilidade |
| --- | --- | ---: | --- | --- |
| 🚪 API Gateway | `src/api-gateway` | `9000` | `/api/*` | Roteamento, gateway e autenticação |
| 🏷️ Categoria Service | `src/BackEnd/Services/CategoriaService` | configurada por ambiente | `/api/categoria` | CRUD e filtros de categorias |
| 📉 Despesa Service | `src/BackEnd/Services/DespesaService` | `9002` | `/api/despesa` | CRUD de despesas |
| 📈 Receita Service | `src/BackEnd/Services/ReceitaService` | `9003` | `/api/receita` | CRUD de receitas |
| 🔐 IdentityServer | `src/IdentityServer` | `8080/8081` no Compose | `/connect/*`, Swagger em dev | STS, tokens, clients e scopes |
| 🧭 Consul | imagem `consul:1.15.4` | `8500` | UI Consul | Service discovery |
| 🖥️ Frontend | `src/FrontEnd` | `4200/4201` em dev | `/` | Interface web e mobile |

## ⚙️ Backend

### 🧰 Tecnologias

| Tecnologia | Uso |
| --- | --- |
| 🟣 .NET `net10.0` | Target framework dos projetos backend |
| 🌐 ASP.NET Core | APIs HTTP dos microserviços e STS |
| 🗄️ Entity Framework Core | ORM, DbContext, migrations e persistência |
| 🐬 Pomelo.EntityFrameworkCore.MySql | Provider MySQL/MariaDB |
| 🧱 Microsoft.EntityFrameworkCore.SqlServer | Provider SQL Server |
| 🔄 AutoMapper | Conversão entre entidades e DTOs |
| 📬 MediatR | Base para CQRS e handlers |
| 🧭 Consul | Registro e descoberta de serviços |
| 🚪 Ocelot | API Gateway |
| 🔐 IdentityServer4 | Serviço de token e autorização |
| 🧪 xUnit, Bogus, Coverlet | Testes unitários, massa fake e cobertura |
| ☁️ AWSSDK.S3 | Infraestrutura para armazenamento de arquivos/imagens |

### 🌐 Microserviços HTTP

#### 🏷️ Categoria Service

Controller: `src/BackEnd/Services/CategoriaService/Controllers/CategoriaController.cs`

| Método | Rota | Descrição | Autorização |
| --- | --- | --- | --- |
| `GET` | `/api/categoria` | Lista categorias do usuário | `User, Admin` |
| `GET` | `/api/categoria/GetById/{idCategoria}` | Busca categoria por ID | `User, Admin` |
| `GET` | `/api/categoria/GetByTipoCategoria/{tipoCategoria}` | Lista categorias por tipo | `User, Admin` |
| `POST` | `/api/categoria` | Cria categoria | `User, Admin` |
| `PUT` | `/api/categoria` | Atualiza categoria | `User, Admin` |
| `DELETE` | `/api/categoria/{idCategoria}` | Remove categoria | `User, Admin` |

#### 📉 Despesa Service

Controller: `src/BackEnd/Services/DespesaService/Controllers/DespesaController.cs`

| Método | Rota | Descrição |
| --- | --- | --- |
| `GET` | `/api/despesa` | Lista despesas do usuário |
| `GET` | `/api/despesa/GetById/{id}` | Busca despesa por ID |
| `POST` | `/api/despesa` | Cria despesa |
| `PUT` | `/api/despesa` | Atualiza despesa |
| `DELETE` | `/api/despesa/{idDespesa}` | Remove despesa |

#### 📈 Receita Service

Controller: `src/BackEnd/Services/ReceitaService/Controllers/ReceitaController.cs`

| Método | Rota | Descrição |
| --- | --- | --- |
| `GET` | `/api/receita` | Lista receitas do usuário |
| `GET` | `/api/receita/GetById/{id}` | Busca receita por ID |
| `POST` | `/api/receita` | Cria receita |
| `PUT` | `/api/receita` | Atualiza receita |
| `DELETE` | `/api/receita/{idReceita}` | Remove receita |

### ⚙️ Application Layer

A camada `Application` concentra contratos e implementações de negócio:

- 🔌 `Abstractions`: interfaces como `IBusinessBase`, `ICategoriaBusiness`, `IUsuarioBusiness`, `IAcessoBusiness`, `ISaldoBusiness` e `IGraficosBusiness`.
- 🧠 `Implementations`: regras de negócio para acesso, usuário, categoria, despesa, receita, lançamento, saldo, gráficos e imagem de perfil.
- 📦 `Dtos`: contratos trafegados entre API, aplicação e frontend.
- 🔄 `Dtos/Profile`: profiles AutoMapper para conversão entre entidades e DTOs.
- 🔐 `Authentication`: configurações e abstrações relacionadas a token e assinatura.
- 🧩 `CommonDependenceInject`: extensões de injeção de dependência.

### 🧬 Domain Layer

A camada `Domain` representa o núcleo do modelo financeiro:

| Entidade / Value Object | Responsabilidade |
| --- | --- |
| 👤 `Usuario` | Usuário proprietário dos dados financeiros |
| 🔑 `Acesso` | Credenciais e informações de acesso |
| 🏷️ `Categoria` | Classificação de receitas/despesas |
| 📉 `Despesa` | Saída financeira |
| 📈 `Receita` | Entrada financeira |
| 🔁 `Lancamento` | Movimentação financeira consolidada |
| 🖼️ `ImagemPerfilUsuario` | Imagem associada ao usuário |
| 📊 `Grafico` | Modelo para indicadores financeiros |
| 🧾 `TipoCategoria` | Tipo de categoria |
| 🛡️ `PerfilUsuario` | Perfil e papéis do usuário |

### 🗄️ Infrastructure Layer

A infraestrutura encapsula detalhes de persistência e integrações:

- 🧱 `DatabaseContexts`: `RegisterContext`, `BaseContext` e estratégias por provedor.
- 📚 `Repository`: repositórios genéricos e específicos.
- 🗺️ `Repository.Mapping`: mapeamentos EF Core das entidades.
- 🔁 `UnitOfWork`: coordenação transacional.
- ☁️ `Amazon`: abstração e implementação para bucket S3.
- ✉️ `Email`: estrutura para integrações de e-mail.
- 🧩 `CommonInjectDependence`: extensões para registrar contexto, S3 e repositórios.

### 🧱 Padrões Aplicados

| Padrão | Onde aparece | Benefício |
| --- | --- | --- |
| 🚪 API Gateway | `src/api-gateway` | Ponto único de entrada e roteamento centralizado |
| 🧭 Service Discovery | `Consul`, `AddConsulSettings`, `UseConsul` | Registro dinâmico dos serviços |
| 📚 Repository | `Infrastructure/Repository/Persistency` | Abstrai acesso a dados |
| 🔁 Unit of Work | `Infrastructure/Repository/UnitOfWork` | Coordena persistência e transações |
| 📦 DTO | `Application/Dtos` | Evita expor entidades diretamente |
| 🔄 AutoMapper | `Application/Dtos/Profile` | Centraliza mapeamento entidade/DTO |
| ♟️ Strategy | `DatabaseContexts/Strategy` | Alterna comportamento por provedor de banco |
| 📨 CQRS | `CrossCutting/CQRS` | Separa comandos e consultas genéricas |
| 🧱 Middleware | `GlobalException`, `TokenMiddleware` | Trata exceções e tokens de forma transversal |

## 🖥️ Frontend

O frontend fica em `src/FrontEnd` e utiliza Angular 20 com suporte a Ionic/Capacitor.

### 🧰 Tecnologias

| Tecnologia | Uso |
| --- | --- |
| 🅰️ Angular 20 | SPA, rotas, componentes e services |
| 🧩 Angular Material / CDK | Componentes e infraestrutura visual |
| 🎨 Bootstrap / MDB UI Kit | Layout e elementos de interface |
| 📱 Ionic / Capacitor | Empacotamento mobile Android/iOS |
| 🔄 RxJS | Fluxos assíncronos |
| 📊 Chart.js / ng2-charts | Gráficos financeiros |
| 📋 DataTables | Tabelas e listagens |
| 🧪 Karma / Jasmine | Testes unitários |
| 🌐 Nginx | Publicação da SPA em container |

### 🧭 Rotas Principais

Arquivo: `src/FrontEnd/src/app/app.routing.module.ts`

| Rota | Protegida | Descrição |
| --- | --- | --- |
| 🔑 `/` | Não | Login |
| 📝 `/register` | Não | Cadastro/acesso |
| 📊 `/dashboard` | Sim | Painel financeiro |
| 🏷️ `/categoria` | Sim | Gestão de categorias |
| 📉 `/despesa` | Sim | Gestão de despesas |
| 📈 `/receita` | Sim | Gestão de receitas |
| 🔁 `/lancamento` | Sim | Lançamentos |
| 👤 `/perfil` | Sim | Perfil do usuário |
| ⚙️ `/configuracoes` | Sim | Configurações |
| 🛡️ `/privacy` | Não | Privacidade |

### 📁 Organização

| Pasta | Responsabilidade |
| --- | --- |
| 📄 `src/app/pages` | Páginas de negócio e módulos lazy-loaded |
| 🧩 `src/app/components` | Componentes reutilizáveis como tabela, modal, alerta, gráficos e toolbar |
| 🌐 `src/app/services/api` | Services HTTP por domínio |
| 🔐 `src/app/services/auth` | Autenticação local e Google |
| 🎟️ `src/app/services/token` | Armazenamento de token |
| 📦 `src/app/models` | Interfaces TypeScript dos contratos |
| ⚙️ `src/environments` | URLs base e configurações por ambiente |
| 📱 `app-android`, `app-ios` | Projetos mobile via Capacitor |

### 🌎 URLs por Ambiente

| Arquivo | `BASE_URL` |
| --- | --- |
| `environment.ts` | `https://alexfariakof.com/api` |
| `environment.local.ts` | `https://localhost:42535/api` |
| `environment.dev.ts` | `https://alexfariakof.com:42535/api` |

## 🔐 Autenticação e Autorização

O projeto possui um STS em `src/IdentityServer`, baseado em IdentityServer4, com:

- 🪪 recursos de identidade `openid`, `profile` e `email`;
- 🚪 API resource `api-gateway`;
- 🎯 scope `sts-scope`;
- 🧾 client `client-microservices`;
- 🔑 suporte a `ResourceOwnerPassword` e `ClientCredentials`;
- 🔄 refresh token com expiração deslizante;
- 🧬 claims `openid`, `profile`, `email`, `userid` e `role`;
- 📚 Swagger habilitado em ambiente de desenvolvimento.

O API Gateway configura autenticação JWT com:

```text
Authority: https://internal:7199
Audience: api-gateway
```

Os controllers podem aplicar autorização por roles. Em `CategoriaController`, por exemplo, as rotas usam:

```csharp
[Authorize("Bearer", Roles = "User, Admin")]
```

## 🗄️ Persistência e Banco de Dados

A persistência usa Entity Framework Core e suporta provedores diferentes por configuração.

### ⚙️ Configuração de Provedor

A extensão `ConfigureRegisterSqlContext` lê a chave:

```json
{
  "DatabaseProvider": "MySql"
}
```

Provedores suportados na implementação atual:

- 🐬 `MySql`
- 🧱 `SqlServer`

As connection strings esperadas são:

- 🔌 `ConnectionStrings:MySqlConnectionString`
- 🔌 `ConnectionStrings:SqlConnectionString`

### 🐬 MariaDB para Desenvolvimento

O arquivo `src/BackEnd/Migrations.MySqlServer/docker-compose.yml` disponibiliza MariaDB:

| Configuração | Valor |
| --- | --- |
| 🖼️ Imagem | `mariadb:10.3.39` |
| 🚪 Porta | `3306:3306` |
| 🗄️ Database | `DespesasPessoaisDB` |
| 👤 Usuário | `docker` |
| 🔑 Senha | `docker` |
| 🛡️ Root password | `!12345` |

Comando:

```bash
docker compose -f src/BackEnd/Migrations.MySqlServer/docker-compose.yml up -d
```

### 🧬 Migrações

Documentação base: `src/BackEnd/Migrations.MySqlServer/migrations.md`

```bash
dotnet ef migrations add Initial -o Migrations.Application
dotnet ef database update
```

## 🚪 API Gateway e Service Discovery

O API Gateway fica em `src/api-gateway` e usa Ocelot com Consul.

### 🛣️ Rotas Ocelot

Arquivo: `src/api-gateway/ocelot.json`

| Upstream | Downstream | ServiceName | Métodos |
| --- | --- | --- | --- |
| `/api/categoria` | `/api/categoria` | `categoria-service` | `GET`, `POST`, `PUT`, `DELETE` |
| `/api/despesa` | `/api/despesa` | `despesa-service` | `GET`, `POST`, `PUT`, `DELETE` |
| `/api/receita` | `/api/receita` | `receita-service` | `GET`, `POST`, `PUT`, `DELETE` |

### 🧭 Consul

Configuração global do Ocelot:

```json
{
  "ServiceDiscoveryProvider": {
    "Scheme": "http",
    "Host": "consul",
    "Port": 8500,
    "Type": "Consul",
    "PollingInterval": 100000
  }
}
```

UI local do Consul:

```text
http://localhost:8500
```

## 🐳 Docker e Ambientes

### 📦 Arquivos Compose da Raiz

| Arquivo | Uso |
| --- | --- |
| 🧱 `docker-compose.yml` | Compose base com gateway e microserviços principais |
| 🛠️ `docker-compose.override.yml` | Override de desenvolvimento com STS e Consul |
| 🧪 `docker-compose.dev.yml` | Variante de desenvolvimento |
| 🚪 `docker-compose.api-gateway.yml` | Execução isolada do gateway |
| 🔐 `docker-compose.IdentityServer.yml` | Execução isolada do IdentityServer |
| 🧭 `docker-compose.consul.yml` | Execução isolada do Consul |

### 🧱 Containers Principais

| Container | Origem | Observação |
| --- | --- | --- |
| 🚪 `api-gateway` | `src/api-gateway/Dockerfile` | Publicado em `4000:9000` no compose base |
| 🏷️ `categoria-service` | `src/BackEnd/Services/CategoriaService/Dockerfile` | Registrado como `categoria-service` |
| 📉 `despesa-service` | `src/BackEnd/Services/DespesaService/Dockerfile` | Registrado como `despesa-service` |
| 📈 `receita-service` | `src/BackEnd/Services/ReceitaService/Dockerfile` | Registrado como `receita-service` |
| 🔐 `app-sts` | `src/IdentityServer/Dockerfile` | Security Token Service |
| 🧭 `consul` | `consul:1.15.4` | Service discovery |

### 🚀 Subida Completa

```bash
docker compose up --build
```

Com override de desenvolvimento:

```bash
docker compose -f docker-compose.yml -f docker-compose.override.yml up --build
```

Parar e remover containers:

```bash
docker compose down
```

## 🚀 Execução Local

### ✅ Pré-requisitos

- 🟣 .NET SDK compatível com `net10.0`
- 🟢 Node.js compatível com Angular 20
- 🐳 Docker e Docker Compose
- 🗄️ MariaDB/MySQL ou SQL Server
- 🔐 Certificados HTTPS locais quando executar frontend com SSL

### ⚙️ Backend

Restaurar e compilar a solution principal:

```bash
dotnet restore sln-api-gateway.sln
dotnet build sln-api-gateway.sln
```

Executar gateway:

```bash
dotnet run --project src/api-gateway/api-gateway.csproj
```

Executar serviços individualmente:

```bash
dotnet run --project src/BackEnd/Services/CategoriaService/CategoriaService.csproj
dotnet run --project src/BackEnd/Services/DespesaService/DespesaService.csproj
dotnet run --project src/BackEnd/Services/ReceitaService/ReceitaService.csproj
```

Executar STS:

```bash
dotnet run --project src/IdentityServer/sts-server.csproj
```

### 🖥️ Frontend

Instalar dependências:

```bash
cd src/FrontEnd
npm install
```

Executar em desenvolvimento local:

```bash
npm start
```

Build:

```bash
npm run build
```

Build com configuração local:

```bash
npm run build:local
```

Executar em container local:

```bash
cd src/FrontEnd
docker compose up --build
```

## 🧪 Testes e Qualidade

### ⚙️ Backend

Projeto de testes:

```text
src/BackEnd/XunitTests/XUnit.Tests.csproj
```

Executar testes:

```bash
dotnet test src/BackEnd/XunitTests/XUnit.Tests.csproj
```

Executar com cobertura via Coverlet:

```bash
dotnet test src/BackEnd/XunitTests/XUnit.Tests.csproj \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=cobertura
```

O projeto usa:

- 🧪 xUnit;
- 🧬 Bogus para geração de massa fake;
- 🧱 Moq.EntityFrameworkCore;
- 📊 Coverlet;
- 🛠️ Microsoft.NET.Test.Sdk.

### 🖥️ Frontend

Executar testes unitários:

```bash
cd src/FrontEnd
npm test
```

Gerar cobertura:

```bash
npm run test:coverage
```

Scripts auxiliares:

```bash
./generate_coverage_report.sh
./generate_coverage_report.ps1
```

### 📈 Qualidade Contínua

O README do frontend faz referência ao SonarCloud e a testes end-to-end em um projeto separado com Python/Playwright. Esses itens indicam uma preocupação com qualidade, cobertura, confiabilidade e análise estática, embora a execução E2E não esteja neste repositório.

## 📁 Estrutura de Pastas

```text
.
├── docker-compose.yml
├── docker-compose.override.yml
├── docker-compose.dev.yml
├── docker-compose.api-gateway.yml
├── docker-compose.IdentityServer.yml
├── docker-compose.consul.yml
├── sln-api-gateway.sln
├── src
│   ├── api-gateway
│   │   ├── Program.cs
│   │   ├── ocelot.json
│   │   ├── ocelot.development.json
│   │   └── Configuration
│   ├── IdentityServer
│   │   ├── Program.cs
│   │   ├── IdentityServerConfigurations.cs
│   │   ├── Quickstart
│   │   ├── Views
│   │   └── Data
│   ├── BackEnd
│   │   ├── Application
│   │   ├── Domain
│   │   ├── Infrastructure
│   │   ├── CrossCutting
│   │   ├── GlobalException
│   │   ├── Services
│   │   │   ├── AuthService
│   │   │   ├── CategoriaService
│   │   │   ├── DespesaService
│   │   │   └── ReceitaService
│   │   ├── Migrations.MySqlServer
│   │   ├── Migrations.DataSeeders
│   │   └── XunitTests
│   └── FrontEnd
│       ├── src/app
│       ├── app-android
│       ├── app-ios
│       ├── Dockerfile
│       ├── Dockerfile.local
│       └── nginx.conf
└── readme.md
```

## ⚙️ Variáveis e Configurações Importantes

| Chave | Uso |
| --- | --- |
| 🌎 `ASPNETCORE_ENVIRONMENT` | Ambiente de execução dos serviços |
| 🌐 `ASPNETCORE_URLS` | URL/porta exposta pelo ASP.NET Core |
| 🚪 `ASPNETCORE_HTTP_PORTS` | Porta HTTP no container |
| 🔐 `ASPNETCORE_HTTPS_PORTS` | Porta HTTPS no container |
| 🧭 `CONSUL_HOST` | Host do Consul |
| 🛰️ `SERVICE_DISCOVERY_URL` | URL do service discovery |
| 🗄️ `DatabaseProvider` | Provider de banco: `MySql` ou `SqlServer` |
| 🔌 `ConnectionStrings:MySqlConnectionString` | Connection string MySQL/MariaDB |
| 🔌 `ConnectionStrings:SqlConnectionString` | Connection string SQL Server |
| 🔑 `CryptoConfigurations` | Opções de criptografia usadas pelo STS |

## ⚠️ Pontos de Atenção Técnicos

- 📄 O arquivo `readme.md` da raiz estava vazio antes desta documentação.
- 🧭 Alguns scripts e referências de teste ainda mencionam nomes antigos como `Despesas.Backend`, `Despesas.Repository` e `AngularApp`. Isso indica legado de estrutura anterior e pode exigir ajuste antes de automatizar CI/CD.
- 🧪 O projeto de testes faz referência a `..\Repository\Repository.csproj`, mas a estrutura atual concentra repositórios em `src/BackEnd/Infrastructure/Repository`. É recomendável validar o build dos testes após a reorganização.
- 🔐 `CategoriaController` aplica `[Authorize]`, enquanto `DespesaController` e `ReceitaController` possuem atributos de autorização comentados. Padronizar isso é importante para segurança.
- 🔑 O `IdentityServerConfigurations.cs` contém client secret e outros identificadores em código. Para produção, mover segredos para variáveis de ambiente, cofre de segredos ou user secrets.
- 🌐 A configuração do gateway aponta `Authority` para `https://internal:7199`. Em Docker/local, validar DNS, certificados e porta real do STS.
- 🧹 Há arquivos com espaço antes de `.cs`, como `OracleProviderStrategy .cs` e `SqlServerProviderStrategy .cs`; isso pode prejudicar manutenção e automações.
- 📄 O arquivo `licence` contém termos de licença, mas mistura trechos de domínio público com licença proprietária. Recomenda-se revisar o conteúdo jurídico antes de distribuição pública.

## 🛣️ Roadmap Técnico Sugerido

1. 🔐 Padronizar autenticação em todos os microserviços e controllers.
2. 🧪 Corrigir referências legadas do projeto de testes e scripts de cobertura.
3. 🔑 Externalizar segredos do IdentityServer e connection strings.
4. ⚙️ Criar pipelines de CI com `dotnet restore`, `dotnet build`, `dotnet test`, `npm ci`, `npm test` e build Docker.
5. ❤️ Adicionar health checks por serviço e expor readiness/liveness no Compose.
6. 📚 Versionar contratos OpenAPI dos microserviços e do gateway.
7. 📝 Criar documentação específica por serviço em cada pasta de `Services`.
8. 🔄 Avaliar migração do IdentityServer4, conforme estratégia de suporte e compatibilidade do ambiente.
9. 🧱 Consolidar nomes de namespaces e pastas após a modernização para `.NET 10`.
10. 📈 Definir política de logs, correlation ID e observabilidade distribuída.

## 📄 Licença

O arquivo `licence` existe na raiz do repositório e deve ser revisado antes de uso, distribuição ou publicação, pois contém termos jurídicos com possíveis interpretações conflitantes.

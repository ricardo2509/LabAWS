# AWS Lab - Gerenciador de Arquivos com Clean Architecture & EF Core

Este é um projeto de laboratório desenvolvido para aplicar e consolidar conceitos modernos do ecossistema **.NET 8+** (Minimal APIs e Blazor WebAssembly) integrados a serviços de nuvem da **AWS (Amazon S3 e Amazon SQS)** e persistência em banco de dados **PostgreSQL**, seguindo os princípios rigorosos da **Clean Architecture** e **DDD (Domain-Driven Design)**.

O projeto conta com um **Modo de Simulação (Mock em memória)** configurável para os serviços da AWS, permitindo que a infraestrutura de armazenamento e filas seja executada e testada de forma 100% anônima e gratuita. Os logs de auditoria são persistidos em um banco de dados real em nuvem.

## 🚀 Tecnologias Utilizadas

- **Frontend:** Blazor WebAssembly (.NET)
- **Backend:** .NET Minimal APIs
- **Banco de Dados:** PostgreSQL (Hospedado na **Neon**)
- **ORM / Persistência:** Entity Framework Core (EF Core) com Fluent API e Migrations
- **Nuvem / Infraestrutura:** Amazon S3 (Armazenamento) e Amazon SQS (Mensageria / Filas)
- **Segurança:** Configurações mascaradas via Variáveis de Ambiente (.NET Environment Variables)

---

## 🏗️ Arquitetura do Projeto

A solução está dividida em camadas bem definidas para garantir o desacoplamento, testabilidade e a inversão de dependências:

### 1. LabAws.Domain
Camada puramente lógica, isolada de qualquer tecnologia externa ou pacotes HTTP.
- **Invariantes e Regras Críticas:** A entidade `FileUpload` auto-valida suas regras de negócio no construtor (como limite máximo de 5MB e restrição de extensões como `.pdf`, `.png`, `.jpg`).
- **Entidades de Persistência:** Classe pura `UploadLog` que mapeia a estrutura de dados de auditoria que será persistida.

### 2. LabAws.Application
Orquestra o fluxo da aplicação e dita as regras de caso de uso do software.
- Contém os **Use Cases** (ex: `UploadFileUseCase` e `CriarBucketUseCase`).
- Define os **DTOs / Commands** que trafegam dados de forma tipada entre a API e a aplicação.
- Centraliza as **Interfaces (Contratos)** de infraestrutura e repositórios (ex: `IS3StorageRepository`, `IUploadLogRepository`).
- Centraliza as **Exceções de Aplicação** para controle de fluxo de erros técnicos (ex: `BucketAlreadyExistsException`).

### 3. LabAws.Infrastructure
Implementa os contratos definidos na camada de Application.
- **Data & Repositories:** Contém o `ApplicationDbContext` do EF Core, configurado via Fluent API utilizando a convenção *snake_case* para tabelas e chaves no PostgreSQL.
- **Modo Mock:** Contém as implementações em memória (`MockS3StorageRepository` e `MockMensageriaRepository`) para simular o comportamento da nuvem AWS localmente ou em ambientes de demonstração.

### 4. LabAws.Api
Minimal API leve que atua como o mecanismo de transporte HTTP e inicializador do contêiner de DI.
- **Segurança de Credenciais:** Configuração de banco de dados fortemente tipada e extraída de forma segura do sistema através de variáveis de ambiente (`ConnectionStrings__postgresql`), mitigando o risco de exposição de credenciais em texto puro (*plain text*).
- **Middleware Global de Exceções:** Captura de forma centralizada as exceções de domínio e aplicação, convertendo-as em respostas HTTP padrão (Bad Request 400) com mensagens amigáveis para o cliente.

---

## 💾 Banco de Dados & Migrations

O projeto utiliza a estratégia de **Migrations Assembly** direcionada para a camada de apresentação (`LabAws.Api`), centralizando a execução de deploys na nuvem.

### Comandos de Infraestrutura Utilizados:
```bash
# Gerar uma nova migração baseada nas alterações de domínio
dotnet ef migrations add NomeDaMigration --project src/LabAws.Api --startup-project src/LabAws.Api

# Atualizar a estrutura do banco de dados na nuvem (Neon)
dotnet ef database update --project src/LabAws.Api --startup-project src/LabAws.Api
```

---

## 🛠️ Como Executar o Projeto Localmente

### Pré-requisitos
- SDK do .NET instalado (Versão 8 ou superior)

### Configuração de Segurança Local
Crie uma variável de ambiente no seu sistema operacional chamada `ConnectionStrings__postgresql` contendo a sua string de conexão do Postgres. O .NET irá ler esta variável automaticamente substituindo o arquivo local.

```bash
dotnet run --project src/LabAws.Api
dotnet run --project src/AwsLab.Web
```

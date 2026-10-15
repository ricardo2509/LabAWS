# AWS Lab - Gerenciador de Arquivos com Clean Architecture

Este é um projeto de laboratório desenvolvido para aplicar e consolidar conceitos modernos do ecossistema **.NET** (Minimal APIs e Blazor WebAssembly) integrados a serviços de nuvem da **AWS (Amazon S3 e Amazon SQS)**, seguindo os princípios rigorosos da **Clean Architecture** e **DDD (Domain-Driven Design)**.

O projeto conta com um **Modo de Simulação (Mock em memória)** configurável, permitindo que a aplicação seja executada e testada de forma 100% anônima e gratuita na nuvem, sem a necessidade de chaves reais da AWS.

## 🚀 Tecnologias Utilizadas

- **Frontend:** Blazor WebAssembly (.NET)
- **Backend:** .NET Minimal APIs
- **Nuvem / Infraestrutura:** Amazon S3 (Armazenamento) e Amazon SQS (Mensageria / Filas)
- **Testes & Validações:** Princípios de Invariantes de Domínio e Fail-Fast

---

## 🏗️ Arquitetura do Projeto

A solução está dividida em camadas bem definidas para garantir o desacoplamento, testabilidade e a inversão de dependências:

### 1. LabAws.Domain (O Coração)
Camada puramente lógica, isolada de qualquer tecnologia externa ou pacotes HTTP.
- **Invariantes e Regras Críticas:** A entidade `FileUpload` auto-valida suas regras de negócio no construtor (como limite máximo de 5MB e restrição de extensões como `.pdf`, `.png`, `.jpg`), impedindo a existência de dados inválidos no sistema.

### 2. LabAws.Application (Os Casos de Uso)
Orquestra o fluxo da aplicação e dita as regras de caso de uso do software.
- Contém os **Use Cases** (ex: `UploadFileUseCase` e `CriarBucketUseCase`).
- Define os **DTOs / Commands** que trafegam dados de forma tipada entre a API e a aplicação.
- Centraliza as **Interfaces (Contratos)** de infraestrutura e as **Exceções de Aplicação** (ex: `BucketAlreadyExistsException`).

### 3. LabAws.Infrastructure (Os Detalhes Técnicos)
Implementa os contratos definidos na camada de Application.
- Contém a integração real com os SDKs da AWS (`AmazonS3Client` e `AmazonSQSClient`).
- **Modo Mock:** Contém as implementações em memória (`MockS3StorageRepository` e `MockMensageriaRepository`) para simular o comportamento da nuvem localmente ou em ambientes de demonstração gratuita.

### 4. LabAws.Api (A Porta de Entrada)
Minimal API leve que atua como o mecanismo de transporte HTTP.
- Gerencia o ciclo de vida de streams de arquivos (`IFormFile`) evitando erros de concorrência.
- **Middleware Global de Exceções:** Captura de forma centralizada as exceções de domínio e aplicação, convertendo-as em respostas HTTP padrão (Bad Request 400) com mensagens amigáveis para o cliente.

---

## 🛠️ Como Executar o Projeto Localmente

### Pré-requisitos
- SDK do .NET instalado (Versão 8 ou superior)

### Passo a Passo

1. **Clonar o Repositório:**
   ```bash
   git clone https://github.com
   cd seu-repositorio
   ```

2. **Configurar o Modo de Execução:**
   No arquivo `src/LabAws.Api/appsettings.json`, certifique-se de que a flag de simulação está ativada se não quiser configurar credenciais da AWS:
   ```json
   "ConfiguracoesAws": {
     "UsarSimulacao": true
   }
   ```

3. **Executar a Solução:**
   Abra o arquivo `LabAws.Api.sln` no Visual Studio e execute os projetos de API e Web simultaneamente, ou utilize o terminal dotnet na raiz:
   ```bash
   dotnet run --project src/LabAws.Api
   dotnet run --project src/AwsLab.Web
   ```

---

## 🛡️ Tratamento de Erros e Resiliência
A aplicação adota o padrão de tratamento global onde erros disparados no coração do **Domain** ou no fluxo da **Application** cortam a execução imediatamente (*Fail-Fast*) e são traduzidos em mensagens limpas e tipadas pelo Middleware da API, garantindo que o front-end Blazor sempre saiba exatamente o que falhou de forma amigável.

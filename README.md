# Projeto de Integração com ChatGPT 3.5 para Consultoria de Viagens

## Descrição do Projeto
Este projeto é uma API desenvolvida em **C# .NET** com integração ao ChatGPT 3.5. Seu objetivo é atuar como um especialista em viagens, fornecendo informações detalhadas sobre pacotes, hotéis e preços, além de gerenciar atendimentos e cadastros de clientes e oferecer relatorio dos ultimos atendimentos.

## Funcionalidades

### **1. Endpoints Disponíveis**

#### **Atendimento**
- **POST** `/api/atendimentos`
  - Criar um novo atendimento.

- **GET** `/api/atendimentos?dataMin=2025-01-01&dataMax=2025-01-22`
  - Obter relatório de atendimentos realizados dentro de um período específico.
  - Parâmetros:
    - `dataMin`: Data inicial (obrigatório).
    - `dataMax`: Data final (obrigatório).

#### **Clientes**
- **POST** `/api/clientes`
  - Cadastrar novos clientes.

### **2. Banco de Dados**
- Utiliza **MongoDB** para persistência de dados.
- Configurado para ser executado em contêiner Docker.

### **3. Docker e Docker Compose**
- O projeto inclui um arquivo `docker-compose.yml` para configurar e subir os contêineres.
- Comando para inicializar os contêineres:
  ```bash
  docker-compose up -d
  ```

## Pré-requisitos
- **.NET**: Versão 8.0 ou superior.
- **Docker**: Versão 24.0 ou superior.
- **MongoDB**: Configurado no Docker via `docker-compose.yml` (versão 6.0 ou superior).

## Configuração e Execução

1. **Clone o Repositório**
   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd <NOME_DO_PROJETO>
   ```

2. **Configure as Dependências**
   - Certifique-se de que o arquivo `docker-compose.yml` está configurado corretamente.

3. **Suba os Contêineres**
   ```bash
   docker-compose up -d
   ```

4. **Execute a API**
   - Utilize o comando:
     ```bash
     dotnet run
     ```

5. **Acesse a API**
   - Endereço padrão: `http://localhost:5000`

## Estrutura do Projeto
- **Components**: Contém o consumidor e o produtor do RabbitMQ para troca de mensagens entre serviços.
- **Configurations**: Inclui arquivos ou classes para configuração do projeto, como conexão com o banco de dados, serviços ou outras integrações.
- **Controllers**: Contém os controladores responsáveis por gerenciar as rotas e lógica de endpoints da API.
- **Dtos**: Define os Data Transfer Objects usados para transportar dados entre camadas.
- **Entities**: Contém as entidades do domínio que mapeiam os objetos no banco de dados (MongoDB).
- **Repositories**: Implementa a lógica de acesso e manipulação de dados no banco.
- **Services**: Contém as regras de negócio e serviços utilizados pelos controladores.

## Tecnologias Utilizadas
- **C# .NET 8.0**
- **MongoDB 6.0**
- **Docker 24.0**
- **RabbitMQ**
- **ChatGPT 3.5**

---

**Autor:**  Daniel Nascimento
**Email:**  danielnmaciel02@gmail.com

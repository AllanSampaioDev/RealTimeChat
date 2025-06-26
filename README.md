# **📌 Desafio Técnico – Desenvolvedor Full Stack**
## **Objetivo**
Criar um **chat em tempo real** com autenticação de usuários, listagem de usuários disponíveis e trocas de mensagens.

## 📌 Como Participar
1. **Fork** este repositório para a sua conta do GitHub.
2. Desenvolva a solução no seu fork.
3. Após finalizar, **abra um Pull Request (PR)** para este repositório.
4. Aguarde o feedback da equipe.

## **🎯 Requisitos do Desafio**

### **1️⃣ Backend**
Criar uma **API REST + WebSockets** utilizando **C# (.NET)** ou **Java (Spring Boot)** com as seguintes funcionalidades:
- **Autenticação e Registro de Usuários**  
  - Criar um endpoint para **login** e outro para **cadastro de usuários**.  
  - Utilizar **JWT** para autenticação.  
- **Listagem de Usuários Online**  
  - Criar um endpoint que retorna os usuários conectados.  
- **Mensagens em Tempo Real**  
  - Implementar **WebSockets** para o envio e recebimento de mensagens.  
  - Criar um **histórico de mensagens** (armazenar em MongoDB ou outro banco de sua escolha).  

---

### **2️⃣ Frontend**
Criar uma **aplicação web** utilizando **Vue.js** com três telas:
- **Tela de Login**
  - Input de **usuário e senha**.
  - Botão para **cadastrar-se**.
- **Tela de Usuários Disponíveis**
  - Listagem dos usuários conectados.
  - Clique no usuário para iniciar um chat.
- **Tela de Conversa**
  - Exibir **histórico de mensagens**.
  - Permitir envio de mensagens em tempo real via **WebSockets**.

---

### **3️⃣ Docker**
Criar um **Dockerfile e um docker-compose.yml** para subir a aplicação de forma rápida.

- O **backend** deve rodar no **.NET Core** ou **Spring Boot**.
- O **frontend** deve rodar no Vue.js
- Banco de dados pode ser **MongoDB, PostgreSQL ou outro**.
- Criar um **arquivo README.md** com instruções para rodar o projeto.

---

## **🛠 Tecnologias Sugeridas**
### **Backend**
✅ **C# com .NET Core** (ou) **Java 17+ com Spring Boot**  
✅ **Autenticação com JWT**  
✅ **WebSockets para mensagens em tempo real**  
✅ **Banco de dados** (MongoDB, PostgreSQL, ou outro de sua escolha)  
✅ **Docker para containerização**

### **Frontend**
✅ **Vue.js**  
✅ **Consumo de APIs via Axios ou Fetch**  
✅ **Uso de WebSockets para chat em tempo real**  

---

## **📌 O que será avaliado?**
✔ **Código bem estruturado e organizado**  
✔ **Boas práticas de desenvolvimento** (Clean Code, SOLID, etc.)  
✔ **Segurança na autenticação e API**  
✔ **Uso correto de WebSockets**  
✔ **Uso eficiente do banco de dados**  
✔ **Documentação clara para rodar a aplicação**  


---

## ** ⏳ Prazo **
- **5** dias.

---

## 🔧 Documentação do desenvolvedor

# 📡 Real-Time Chat App

Este projeto é a solução desenvolvida para o desafio técnico de Desenvolvedor Full Stack, que consiste em criar um **chat em tempo real** com autenticação de usuários, listagem de usuários online e troca de mensagens via WebSockets.

---

## ✅ Funcionalidades

### 🔐 Autenticação de Usuários
- Registro e login via endpoints REST.
- Tokens JWT gerados no login e utilizados para autenticação nas requisições.

### 👥 Listagem de Usuários Online
- Usuários são exibidos em tempo real com seus status de conexão.
- A conexão é gerenciada via SignalR.

### 💬 Chat em Tempo Real
- Envio e recebimento de mensagens entre usuários conectados via SignalR.
- Histórico de mensagens é persistido em banco de dados (PostgreSQL - via docker).

### 🌐 Frontend em Vue.js
- Tela "Login" com cadastro embutido.
  - Ultilizado os mesmos campos destinados à login, um usuário pode informar nome e senha para cadastro imediato.
- Tela "Home" possui seções de:
  - Contatos de usuários com indicadores de status. 
    - Clicando em um contato será carregado o historio de mensagens na seção "Chat"
  - "Chat" com rolagem automática focada nas mensagens mais recentes.

---

## ⚙️ Tecnologias Utilizadas

### Backend
- [.NET 8 - Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [SignalR](https://learn.microsoft.com/aspnet/core/signalr)
- [JWT Bearer Authentication](https://learn.microsoft.com/aspnet/core/security/authentication/jwt)
- [PostgreSQL + EF Core](https://learn.microsoft.com/ef/core/)
- [Docker](https://docs.docker.com/get-started/)

### Frontend
- [Vue 3](https://vuejs.org/)
- [Vite](https://vitejs.dev/)
- [Axios](https://axios-http.com/)
- [@microsoft/signalr](https://www.npmjs.com/package/@microsoft/signalr)

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- Docker
- Docker Compose

### Passos

```bash
# Clone o repositório
git clone https://github.com/AllanSampaioDev/RealTimeChat.git
cd seu-repositorio

# Suba os containers
docker-compose up -d
```

O [backend](http://localhost:2020/swagger/index.html) estará disponível na porta 2020
O [frontend](http://localhost:8080/login) estará disponível na porta 8080
O banco de dados estará disponível em um container na porta 5432

---

## Estrutura do Projeto

```
.
├── Api/                 # Projeto backend (.NET)
│   ├── Controllers/
│   ├── Hubs/
│   ├── Infra/
│   ├── Domain/
│   └── Dockerfile
├── App/                 # Projeto frontend (Vue)
│   ├── components/
│   ├── views/
│   ├── main.js
│   └── Dockerfile
├── nginx.conf
├── docker-compose.yml
├── README.md

```

## Considerações Técnicas
- A API foi construída usando Minimal APIs para reduzir complexidade e focar em performance.
- O SignalR gerencia as conexões e o envio de mensagens entre clientes conectados.
- As mensagens enviadas são persistidas no banco de dados para utilização de histórico.
- O Vue.js se conecta ao SignalR via WebSocket para atualização em tempo real das mensagens.
- O login e o cadastro armazenam o JWT no localStorage, que é utilizado para autenticação nas requisições subsequentes.
- O projeto inclui tratamento para:
  - Usuários que se desconectam abruptamente, redirecionando-os para página de login.
  - Diferenciação entre mensagens enviadas e recebidas com layout visual distinto.
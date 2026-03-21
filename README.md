# 🛒 Geek Shopping - Microservices Architecture

![.NET 8](https://img.shields.io/badge/.NET-8.0-512bd4?style=for-the-badge&logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-2496ed?style=for-the-badge&logo=docker&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)

O **Geek Shopping** é uma solução de e-commerce completa, desenvolvida com uma arquitetura de microserviços moderna e escalável. O projeto foca no desacoplamento total, utilizando padrões como **Database per Service** e comunicação assíncrona baseada em eventos.

---

## 🏗️ Arquitetura e Tecnologias

O projeto foi construído utilizando as ferramentas mais atuais do mercado para garantir alta performance e manutenibilidade:

* **Runtime:** `.NET 8` em todos os serviços.
* **Frontend:** `Blazor` (WebAssembly/Server) para uma experiência de usuário rica.
* **Persistência:** `MySQL` com isolamento total (um banco por serviço).
* **Mensageria:** `RabbitMQ` para comunicação assíncrona entre APIs.
* **Segurança:** `IdentityServer` para autenticação e autorização via JWT/OAuth2.
* **Orquestração:** `Docker` e `Docker Compose` para padronização de ambientes.

---

## 📂 Estrutura do Projeto

Abaixo, a descrição de cada componente da solução:

| Projeto | Descrição |
| :--- | :--- |
| **GeekShopping.Web** | Interface web construída em Blazor. |
| **GeekShopping.ProductAPI** | Gerenciamento de catálogo e produtos. |
| **GeekShopping.CartAPI** | Lógica de carrinho de compras e persistência temporária. |
| **GeekShopping.CouponAPI** | Gerenciamento e validação de cupons de desconto. |
| **GeekShopping.IdentityServer** | Centralizador de identidade, logins e tokens de acesso. |
| **GeekShopping.MessageBus** | Implementação genérica para integração com RabbitMQ. |

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado.
* SDK do .NET 8 (opcional, para desenvolvimento local).

### Execução via Docker Compose

Para subir toda a infraestrutura (Bancos MySQL, RabbitMQ e as APIs), execute o comando na raiz do projeto:

```bash
docker-compose up -d

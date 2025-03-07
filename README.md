# Wallet Buddy

### Sobre o projeto

Esta API, desenvolvida com **.NET 8**, adota os princípios do **Domain-Driven Design (DDD)** para oferecer uma solução estruturada e eficaz no gerenciamento de despesas pessoais. O principal objetivo é permitir que os usuários registrem suas despesas, detalhando informações como título, data e hora, descrição, valor e tipo de pagamento. Os dados são armazenados em um banco de dados **PostgreSQL**. A API disponibiliza rotas para relatórios mensais das despesas em formatos Excel e PDF.

A arquitetura da API é baseada em **REST** e segue os princípios **SOLID**, utilizando métodos HTTP padrão para uma comunicação eficiente e simplificada. Além disso, a API conta com documentação **Swagger**, que proporciona uma interface gráfica interativa para que os desenvolvedores possam explorar e testar os endpoints de forma rápida e fácil.

Entre os pacotes NuGet utilizados, o **AutoMapper** é responsável pelo mapeamento entre objetos de domínio e requisição/resposta, reduzindo a necessidade de código repetitivo e manual. Para realizar as validações, o **FluentValidation** é empregado para implementar regras de validação de forma simples e intuitiva nas classes de requisição, mantendo o código limpo e de fácil manutenção. Por fim, o **Entity Framework** atua como um ORM (Object-Relational Mapper), simplificando as interações com o banco de dados e permitindo o uso de objetos .NET para manipular dados diretamente, sem a necessidade de consultas SQL.

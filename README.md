# Wallet Buddy

#### English

## About the Project

This API, developed with **.NET 8**, adopts the principles of **Domain-Driven Design (DDD)** to offer a structured and efficient solution for managing personal expenses. The main objective is to allow users to record their expenses, detailing information such as title, date and time, description, value, and payment type. The data is stored in a **PostgreSQL** database. The API also provides routes for monthly expense reports in Excel and PDF formats.

The API architecture is based on **REST** and follows **SOLID** principles, using standard HTTP methods for efficient and streamlined communication. Additionally, the API includes **Swagger** documentation, which provides an interactive graphical interface for developers to explore and test endpoints quickly and easily.

Among the NuGet packages used, **AutoMapper** is responsible for mapping between domain objects and request/response objects, reducing the need for repetitive and manual code. For validations, **FluentValidation** is employed to implement validation rules in a simple and intuitive way in the request classes, keeping the code clean and easy to maintain. Finally, **Entity Framework** acts as an ORM (Object-Relational Mapper), simplifying interactions with the database and allowing the use of .NET objects to manipulate data directly, without the need for SQL queries.

### Features

- **Domain-Driven Design (DDD)**: Modular structure that facilitates understanding and maintenance of the application's domain;
- **RESTful API with Swagger Documentation**: Documented interface that facilitates integration and testing by developers;
- **Report Generation**: Ability to export detailed reports to **PDF and Excel**, offering a visual and effective analysis of expenses;
- **Unit Tests**: Comprehensive tests with FluentAssertions to ensure functionality and quality.




#### Português (Brasil)

## Sobre o projeto

Esta API, desenvolvida com **.NET 8**, adota os princípios do **Domain-Driven Design (DDD)** para oferecer uma solução estruturada e eficaz no gerenciamento de despesas pessoais. O principal objetivo é permitir que os usuários registrem suas despesas, detalhando informações como título, data e hora, descrição, valor e tipo de pagamento. Os dados são armazenados em um banco de dados **PostgreSQL**. A API também disponibiliza rotas para relatórios mensais das despesas em formatos Excel e PDF.

A arquitetura da API é baseada em **REST** e segue os princípios **SOLID**, utilizando métodos HTTP padrão para uma comunicação eficiente e simplificada. Além disso, a API conta com documentação **Swagger**, que proporciona uma interface gráfica interativa para que os desenvolvedores possam explorar e testar os endpoints de forma rápida e fácil.

Entre os pacotes NuGet utilizados, o **AutoMapper** é responsável pelo mapeamento entre objetos de domínio e requisição/resposta, reduzindo a necessidade de código repetitivo e manual. Para realizar as validações, o **FluentValidation** é empregado para implementar regras de validação de forma simples e intuitiva nas classes de requisição, mantendo o código limpo e de fácil manutenção. Por fim, o **Entity Framework** atua como um ORM (Object-Relational Mapper), simplificando as interações com o banco de dados e permitindo o uso de objetos .NET para manipular dados diretamente, sem a necessidade de consultas SQL.

### Features

- **Domain-Driven Design (DDD)**: Estrutura modular que facilita o entendimento e a manutenção do domínio da aplicação;
- **RESTful API com Documentação Swagger**: Interface documentada que facilita a integração e o teste por parte dos desenvolvedores;
- **Geração de Relatórios**: Capacidade de exportar relatórios detalhados para **PDF e Excel**, oferecendo uma análise visual e eficaz das despesas;
- **Testes de Unidade**: Testes abrangentes com FluentAssertions para garantir a funcionalidade e a qualidade.
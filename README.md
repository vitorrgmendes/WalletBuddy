<p align="center">
  <img src="https://raw.githubusercontent.com/stevenrskelton/flag-icon/master/png/75/country-4x3/us.png" alt="United States" title="United States">
</p>

## About the Project

This API, developed with **.NET 8**, adopts the principles of **Domain-Driven Design (DDD)** and **Clean Architecture** to offer a structured and efficient solution for managing personal expenses. The main objective is to allow users to record their expenses, detailing information such as title, date and time, description, value, and payment type. The data is stored in a **PostgreSQL** database. The API also provides routes for monthly expense reports in Excel and PDF formats.

The API architecture is based on **REST** and follows **SOLID** principles, using standard HTTP methods for efficient and streamlined communication. Additionally, the API includes **Swagger** documentation, which provides an interactive graphical interface for developers to explore and test endpoints quickly and easily.

Among the NuGet packages used, **AutoMapper** is responsible for mapping between domain objects and request/response objects, reducing the need for repetitive and manual code. For validations, **FluentValidation** is employed to implement validation rules in a simple and intuitive way in the request classes, keeping the code clean and easy to maintain. For robust application logging, **Serilog** has been integrated. This library provides structured logging capabilities, allowing for more detailed and easily searchable logs, which are crucial for monitoring, debugging, and understanding the application's behavior in various environments. Finally, **Entity Framework Core (EF Core)** acts as our ORM (Object-Relational Mapper), simplifying interactions with the database without the need for manual SQL queries. Beyond its core data access functionalities, **EF Core is also leveraged for auditing purposes**. By tracking changes made to our entities, we can effectively monitor and verify modifications to data within the database, providing an audit trail for important operations.

![][hero-image]

### Features

- **Domain-Driven Design (DDD)**: Modular structure that facilitates understanding and maintenance of the application's domain;
- **Clean Architecture**: Implementation of a layered architecture that separates concerns, making the application more testable, maintainable, and independent of frameworks and infrastructure;
- **Logging & Audit**: Comprehensive logging using Serilog for structured and easily searchable logs, crucial for monitoring and debugging. Additionally, Entity Framework Core is leveraged to audit data changes, providing a history of modifications for enhanced accountability and security;
- **Security, Authentication and Authorization (JWT)**: Implementation of a secure authentication and authorization mechanism using JSON Web Tokens (JWT) to protect API endpoints and ensure only authenticated and authorized users can access specific resources and functionalities;
- **RESTful API with Swagger Documentation**: Documented interface that facilitates integration and testing by developers;
- **Report Generation**: Ability to export detailed reports to **PDF and Excel**, offering a visual and effective analysis of expenses;
- **Unit Tests**: Comprehensive tests with FluentAssertions to ensure functionality and quality.

### Tools

![Windows Badge][windows-badge]
![Visual Studio Badge][visual-studio-badge]
![.NET Badge][dot-net-badge]
![PostgreSQL Badge][postgresql-badge]
![Swagger Badge][swagger-badge]
![Git-Badge][git-badge]

## Getting Started

To run the application locally, follow these simple steps.

### Prerequisites

* [Visual Studio][vs-studio] version 2022+ or [Visual Studio Code][vs-code];
* Windows 10+ or Linux/MacOS with [.NET SDK][dot-net-sdk-en] installed;
* [PostgreSQL][postgre] database.

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/vitorrgmendes/WalletBuddy.git
    ```

2. Fill in the information in the `appsettings.Development.json` file according to the `appsettings.Example.json` file, and remove `.Example` to have the default `appsettings.json`;
3. Run the API and enjoy.

<br><br><br>

<p align="center">
  <img src="https://raw.githubusercontent.com/stevenrskelton/flag-icon/master/png/75/country-4x3/br.png" alt="Brazil" title="Brazil">
</p>

## Sobre o projeto

Esta API, desenvolvida com **.NET 8**, adota os princípios do **Domain-Driven Design (DDD)** e da **Arquitetura Limpa** para oferecer uma solução estruturada e eficaz no gerenciamento de despesas pessoais. O principal objetivo é permitir que os usuários registrem suas despesas, detalhando informações como título, data e hora, descrição, valor e tipo de pagamento. Os dados são armazenados em um banco de dados **PostgreSQL**. A API também disponibiliza rotas para relatórios mensais das despesas em formatos Excel e PDF.

A arquitetura da API é baseada em **REST** e segue os princípios **SOLID**, utilizando métodos HTTP padrão para uma comunicação eficiente e simplificada. Além disso, a API conta com documentação **Swagger**, que proporciona uma interface gráfica interativa para que os desenvolvedores possam explorar e testar os endpoints de forma rápida e fácil.

Entre os pacotes NuGet utilizados, o **AutoMapper** é responsável por mapear entre objetos de domínio e objetos de requisição/resposta, reduzindo a necessidade de código repetitivo e manual. Para validações, o **FluentValidation** é empregado para implementar regras de validação de forma simples e intuitiva nas classes de requisição, mantendo o código limpo e fácil de manter. Para um registro de logs robusto da aplicação, o **Serilog** foi integrado. Esta biblioteca fornece capacidades de logging estruturado, permitindo logs mais detalhados e facilmente pesquisáveis, que são cruciais para monitoramento, depuração e compreensão do comportamento da aplicação em diversos ambientes. Finalmente, o **Entity Framework Core (EF Core)** atua como nosso ORM (Object-Relational Mapper), simplificando as interações com o banco de dados e sem a necessidade de consultas SQL manuais. Além de suas funcionalidades principais de acesso a dados, **o EF Core também é utilizado para fins de auditoria**. Ao rastrear as alterações feitas em nossas entidades, podemos monitorar e verificar efetivamente as modificações nos dados dentro do banco de dados, fornecendo uma trilha de auditoria para operações importantes.

![][hero-image-br]

### Funcionalidades

- **Domain-Driven Design (DDD)**: Estrutura modular que facilita o entendimento e a manutenção do domínio da aplicação;
- **Clean Architecture**: Implementação de uma arquitetura em camadas que separa as responsabilidades, tornando a aplicação mais testável, de fácil manutenção e independente de frameworks e infraestrutura;
- **Logging & Audit**: Registro abrangente utilizando Serilog para logs estruturados e facilmente pesquisáveis, cruciais para monitoramento e depuração. Adicionalmente, o Entity Framework Core é utilizado para auditar as mudanças nos dados, fornecendo um histórico de modificações para maior responsabilidade e segurança;
- **Security, Authentication and Authorization (JWT)**: Implementação de um mecanismo de autenticação e autorização seguro utilizando JSON Web Tokens (JWT) para proteger os endpoints da API e garantir que apenas usuários autenticados e autorizados possam acessar recursos e funcionalidades específicas;
- **RESTful API com Documentação Swagger**: Interface documentada que facilita a integração e o teste por parte dos desenvolvedores;
- **Geração de Relatórios**: Capacidade de exportar relatórios detalhados para **PDF e Excel**, oferecendo uma análise visual e eficaz das despesas;
- **Testes de Unidade**: Testes abrangentes com FluentAssertions para garantir a funcionalidade e a qualidade.

### Ferramentas

![Windows Badge][windows-badge]
![Visual Studio Badge][visual-studio-badge]
![.NET Badge][dot-net-badge]
![PostgreSQL Badge][postgresql-badge]
![Swagger Badge][swagger-badge]
![Git-Badge][git-badge]

## Primeiros passos

Para obter uma cópia local funcionando, siga esses passos simples.

### Requisitos

* [Visual Studio][vs-studio] versão 2022+ ou [Visual Studio Code][vs-code]
* Windows 10+ ou Linux/MacOS com [.NET SDK][dot-net-sdk-br] instalado
* Banco de dados [PostgreSQL][postgre]

### Instalação

1. Clone o repositório:
    ```sh
    git clone https://github.com/vitorrgmendes/WalletBuddy.git
    ```

2. Preencha as informações no arquivo `appsettings.Development.json` conforme o arquivo `appsettings.Example.json`, e apague `.Example` para ter o `appsettings.json` padrão;
3. Execute a API e aproveite.




<!-- Links -->
[vs-studio]: https://visualstudio.microsoft.com/vs/community/
[vs-code]: https://code.visualstudio.com/download
[dot-net-sdk-br]: https://dotnet.microsoft.com/pt-br/download/dotnet/8.0
[dot-net-sdk-en]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[postgre]: https://www.postgresql.org/download/

<!-- Images -->
[hero-image]: images/heroimage.png
[hero-image-br]: images/heroimage-br.png

<!-- Badges -->
[dot-net-badge]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[postgresql-badge]: https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=fff&style=for-the-badge
[windows-badge]: https://img.shields.io/badge/Windows-0078D4?logo=windows&logoColor=fff&style=for-the-badge
[swagger-badge]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
[visual-studio-badge]: https://img.shields.io/badge/Visual%20Studio-5C2D91?logo=visualstudio&logoColor=fff&style=for-the-badge
[git-badge]: https://img.shields.io/badge/Git-F05032?logo=git&logoColor=fff&style=for-the-badge
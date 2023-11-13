# Sistema Simples Fluxo Caixa
Sistema desenvolvido por Edson de Araujo
Cadastro Simples com ASP.NET CORE 2.2 C# EntityFramework para entrevistas

# Modo de Instalção e Utilização

Requisitos Basicos:<br>
Visual Studio 2019<br>
SQL Server Manager<br>
EntityFramework Core<br>
Biblioteca Rotativa PDF<br>
Servidor IIS<br>

1 - Fazer o clone do projeto<br>
2 - Aprir Usando Visual Studio 2019<br>
3 - Atualizar os pacotes Nugets<br>
4 - Sistema com Acesso ao Banco em Nuvem SQLSERVER Windows<br>
5 - Configura o banco de dados no arquivo appsettings.json, conforme seu banco:<br>

"ConnectionStrings": { "DefaultConnection": "Data Source = localhost; Initial Catalog = Seguro; Uid = sa; Password = 1234; MultipleActiveResultSets=true; Pooling=true; Min Pool Size=0; Max Pool Size=250; Connect Timeout=30;" },<br>

6 - Rodar a Aplicação em modo de Varios projetos Cmd (webApi) Cmd (webApp)<br>
7 - No Console do Gerenciador de Pacotes entre com PM> Add-Migration 00000000000000_CreateIdentitySchema.cs<br>
8 - Crie um novo usuário para que as tabelas possam ser geradas<br><br>

Ultima Atualização Abiitado Origem Cruzada Cors.

<br>
link do Sistema para Teste: https://www.utyum.com.br/Seguro/NetApi

<br>
link da API via Swagger para Teste: https://www.utyum.com.br/Seguro/Api/api/Seguro
<br>

Usuario: admin@net.com  Senha: Bt123Senha#

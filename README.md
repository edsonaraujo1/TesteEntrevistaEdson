# Sistema Simples Gestão de Seguros
Sistema desenvolvido por Edson de Araujo
Cadastro Simples com ASP.NET CORE 2.2 C# EntityFramework com consumo de API seguro com autenticação JWT para entrevista tecnica.

# Modo de Instalação e Utilização

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
7 - No Console do Gerenciador de Pacotes entre com PM> Add-Migration 00000000000000_CreateIdentitySchema.cs ou pule direto para o passo 8<br>
8 - Crie um novo usuário para que as tabelas possam ser geradas<br><br>
<img src='https://i.postimg.cc/Js5M93qV/migration-01.png' border='0' alt='migration-01'/><br>
Após o termino será exibido como esta escrito a baixo<br>
<img src='https://i.postimg.cc/GBjHr8kV/migration-02.png' border='0' alt='migration-02'/><br>


Ultima Atualização Habilitado Origem Cruzada Cors.
       Protegendo as APIs de Acesso não autorizado. 
       Ajustando criação de Banco e tabelas para o Entrevistador.

link do Sistema para Teste: https://www.utyum.com.br/NetApi
<br><br>
link da API via Swagger para Teste: https://www.utyum.com.br/Seguro/Api/api/Seguro
<br><br>
Usuario: admin@net.com  Senha: Bt123Senha#

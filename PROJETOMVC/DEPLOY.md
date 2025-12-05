Configuração de Ambientes - Development / Production

Resumo
- O ASP.NET Core carrega `appsettings.json` e, automaticamente, `appsettings.{Environment}.json` quando `ASPNETCORE_ENVIRONMENT` estiver definido.
- Você pode sobrescrever valores via variáveis de ambiente. Para connection strings use o nome com `__` (duplo underscore): `ConnectionStrings__DataBase`.

Arquivos criados/esperados
- `appsettings.json` (base) — já existe
- `appsettings.Development.json` (desenvolvimento) — já existe
- `appsettings.Production.json` (produção) — criado com placeholders

Como executar em Development (local / VS Code)
- Por padrão o `launchSettings.json` define `ASPNETCORE_ENVIRONMENT=Development`.
- Use `dotnet watch run` normalmente.

Como executar em Production local (teste)

```powershell
$env:ASPNETCORE_ENVIRONMENT = 'Production'
$env:ConnectionStrings__DataBase = 'Host=localhost;Port=5432;Database=geral_db_prod;Username=postgres;Password=SUASENHA'
dotnet run --project .\PROJETOMVC\PROJETOMVC.csproj

 No PowerShell (só para teste local), antes de rodar:

 ```powershell
 $env:ASPNETCORE_ENVIRONMENT = 'Production'
 # Opção A: fornecer a string de conexão completa em uma variável
 $env:ConnectionStrings__DataBase = 'Host=localhost;Port=5432;Database=geral_db_prod;Username=postgres;Password=SUASENHA'
 dotnet run --project .\PROJETOMVC\PROJETOMVC.csproj
 ```

 Ou, Opção B: defina variáveis individuais DB_* (mais seguro para gerenciar separadamente)

 ```powershell
 $env:ASPNETCORE_ENVIRONMENT = 'Production'
 $env:DB_HOST = 'localhost'
 $env:DB_PORT = '5432'
 $env:DB_NAME = 'geral_db_prod'
 $env:DB_USER = 'postgres'
 $env:DB_PASSWORD = 'SUASENHA'
 dotnet run --project .\PROJETOMVC\PROJETOMVC.csproj
 ```

 O aplicativo preferirá `ConnectionStrings__DataBase` se estiver presente; caso contrário, se `DB_HOST/DB_NAME/DB_USER/DB_PASSWORD` estiverem presentes, ele construirá a string de conexão automaticamente. Se nenhum estiver presente, ele recuará para `appsettings.{Environment}.json`.
```

- Para desfazer no terminal atual:
```powershell
Remove-Item Env:\ASPNETCORE_ENVIRONMENT
Remove-Item Env:\ConnectionStrings__DataBase
```

Como configurar em um servidor Windows (IIS) ou Linux
- Em servidores use as configurações do sistema (variáveis de ambiente) ou o provedor de segredos da sua infraestrutura (Azure Key Vault, AWS Secrets Manager).
- Exemplo no IIS: adicione `ASPNETCORE_ENVIRONMENT=Production` nas configurações de ambiente do App Pool / no Web.config de publicação.

Boas práticas
- Nunca comite senhas reais no `appsettings.Production.json` — use variáveis de ambiente ou secret managers.
- Em desenvolvimento você pode usar `dotnet user-secrets` para armazenar localmente:

```powershell
cd PROJETOMVC
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DataBase" "Host=localhost;Port=5432;Database=geral_db;Username=postgres;Password=admincaixa"
```

Verificação rápida no código
- O `Program.cs` foi atualizado para preferir `ConnectionStrings__DataBase` se definida.

Precisa que eu:
- Adicione exemplos de `systemd` service file para Linux? (sim / não)
- Configure um `Dockerfile` e `docker-compose` com variáveis de ambiente? (sim / não)


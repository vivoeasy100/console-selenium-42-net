# Console Selenium 42 - Automação de Testes com DuckDuckGo

Este projeto implementa uma solução completa de automação de testes utilizando **Selenium WebDriver** com **C#** e **.NET**, seguindo as boas práticas de Clean Code e arquitetura modularizada.

## 📋 Sobre o Projeto

Esta solução foi desenvolvida como parte dos exercícios da disciplina **Garantia da Qualidade de Software** / **Gestão e Qualidade de Software**, ministrada pelo Professor **Daniel Henrique Matos de Paiva**.

O objetivo é automatizar cenários reais de busca no **DuckDuckGo** (https://duckduckgo.com), demonstrando:
- Configuração de ambiente Selenium profissional
- Uso de esperas explícitas (WebDriverWait)
- Interação com elementos dinâmicos (autocompletar)
- Navegação por histórico do navegador
- Validações com asserções MSTest

## 🏗️ Estrutura da Solução

```
ConsoleSelenium42.sln
├── ConsoleSelenium/                 # Projeto Console Application (.NET 10)
│   └── ConsoleSelenium.csproj
└── ConsoleSelenium.Tests/           # Projeto de Testes MSTest
    ├── BuscaDesafioTest.cs          # Testes dos 3 exercícios
    ├── ConsoleSelenium.Tests.csproj
    └── MSTestSettings.cs
```

## 🧪 Exercícios Implementados

### Exercício 1: Experiência de Pesquisa Básica e Validação de Título
- Navega até https://duckduckgo.com
- Localiza campo de pesquisa (name="q")
- Digita "Selenium WebDriver C#" e pressiona Enter
- Valida se o título ou URL da página de resultados contém o termo pesquisado

### Exercício 2: Interagindo com o Autocompletar (Suggestions)
- Acessa a página inicial do DuckDuckGo
- Digita "C# unit testing" sem pressionar Enter
- Aguarda carregamento das sugestões via WebDriverWait
- Clica na terceira opção sugerida
- Valida redirecionamento correto para resultados

### Exercício 3: Navegação por Histórico e Botões de Ação
- Realiza busca por "Automated Testing"
- Clica no primeiro link orgânico dos resultados
- Valida carregamento da página externa (verificando URL)
- Usa `_driver.Navigate().Back()` para retornar aos resultados
- Valida restauração correta da página de resultados

## 🚀 Como Executar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Google Chrome](https://www.google.com/chrome/) instalado
- ChromeDriver compatível (gerenciado automaticamente pelo Selenium Manager)

### Executar os Testes

```bash
# Clonar o repositório
git clone https://github.com/vivoeasy100/console-selenium-42-net.git
cd console-selenium-42-net

# Restaurar dependências
dotnet restore

# Compilar a solução
dotnet build

# Executar todos os testes
dotnet test

# Executar testes com detalhes
dotnet test --logger "console;verbosity=detailed"

# Executar um exercício específico
dotnet test --filter "TestCategory=Exercicio1"
dotnet test --filter "TestCategory=Exercicio2"
dotnet test --filter "TestCategory=Exercicio3"
```

## 📦 Dependências Principais

| Pacote | Versão | Descrição |
|--------|--------|-----------|
| Selenium.WebDriver | 4.52.0 | Driver principal do Selenium |
| Selenium.Support | 4.52.0 | Suporte adicional (WebDriverWait, etc.) |
| MSTest | 4.0.2 | Framework de testes unitários |
| Microsoft.NET.Test.Sdk | 18.0.1 | SDK de testes do .NET |

## 🛠️ Tecnologias Utilizadas

- **C# 12** / **.NET 10**
- **Selenium WebDriver 4.x**
- **MSTest** (Framework de testes)
- **ChromeDriver** (via Selenium Manager)
- **Git** (Controle de versão com commits semânticos)

## 📝 Padrão de Commits

Este projeto segue o padrão **Conventional Commits** (commits semânticos):

```
feat(setup): cria estrutura inicial da solution console-selenium-42
test(busca): implementa exercicio 1 - pesquisa basica e validacao de titulo
test(autocompletar): implementa exercicio 2 - interacao com sugestoes
test(navegacao): implementa exercicio 3 - navegacao por historico
```

## 👨‍🏫 Professor

**Daniel Henrique Matos de Paiva**  
Disciplina: Garantia da Qualidade de Software / Gestão e Qualidade de Software

## 📋 Informações do Aluno

**Nome:** Fernando Almeida  
**RA:** 326132695

## 📄 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.
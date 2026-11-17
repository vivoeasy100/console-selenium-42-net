using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests;

/// <summary>
/// Testes automatizados para cenários de busca no DuckDuckGo.
/// Exercícios propostos na disciplina de Garantia da Qualidade de Software.
/// </summary>
[TestClass]
public sealed class BuscaDesafioTest
{
    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;
    private const string BaseUrl = "https://duckduckgo.com";

    /// <summary>
    /// Configuração executada antes de cada teste.
    /// Inicializa o WebDriver do Chrome com opções adequadas para automação.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        // Usar pageLoadStrategy "eager" para não esperar carregamento completo
        options.PageLoadStrategy = PageLoadStrategy.Eager;

        _driver = new ChromeDriver(options);
        // Definir timeout de carregamento de página para 60 segundos
        _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        _driver.Navigate().GoToUrl(BaseUrl);
    }

    /// <summary>
    /// Limpeza executada após cada teste.
    /// Fecha o navegador e libera recursos.
    /// </summary>
    [TestCleanup]
    public void Teardown()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    #region Exercício 1: A Experiência de Pesquisa Básica e Validação de Título

    /// <summary>
    /// Exercício 1: Automatizar a digitação na barra de pesquisa principal do DuckDuckGo,
    /// acionar a submissão e validar o carregamento da página de resultados.
    /// </summary>
    [TestMethod]
    [TestCategory("Exercicio1")]
    public void Exercicio1_PesquisaBasica_ValidarTitulo()
    {
        // Arrange
        const string termoPesquisa = "Selenium WebDriver C#";

        // Act
        // 1. Navegar até a URL base (já feito no Setup)
        // 2. Localizar o campo de pesquisa (identificado pelo atributo name="q")
        var campoPesquisa = _wait.Until(d => d.FindElement(By.Name("q")));

        // 3. Digitar o termo "Selenium WebDriver C#" utilizando o método SendKeys
        campoPesquisa.Clear();
        campoPesquisa.SendKeys(termoPesquisa);

        // 4. Enviar o formulário pressionando a tecla Enter (ou Keys.Enter)
        campoPesquisa.SendKeys(Keys.Enter);

        // 5. Validar por meio de asserção que o título ou a URL da página de resultados contém o termo pesquisado
        _wait.Until(d => d.Title.Contains("Selenium", StringComparison.OrdinalIgnoreCase));

        // Assert
        var tituloPagina = _driver.Title;
        var urlAtual = _driver.Url;

        Assert.IsTrue(
            tituloPagina.Contains("Selenium", StringComparison.OrdinalIgnoreCase) ||
            urlAtual.Contains("Selenium", StringComparison.OrdinalIgnoreCase),
            $"A página de resultados não contém o termo pesquisado. Título: '{tituloPagina}', URL: '{urlAtual}'");
    }

    #endregion

    #region Exercício 2: Interagindo com o Autocompletar (Suggestions)

    /// <summary>
    /// Exercício 2: Lidar com menus suspensos dinâmicos gerados via JavaScript enquanto o usuário digita.
    /// </summary>
    [TestMethod]
    [TestCategory("Exercicio2")]
    public void Exercicio2_Autocompletar_ClicarTerceiraOpcao()
    {
        // Arrange
        const string termoPesquisa = "C# unit testing";

        // Act
        // 1. Acessar a página inicial do DuckDuckGo (já feito no Setup)
        // 2. Digitar o termo "C# unit testing" no campo de pesquisa, sem pressionar Enter imediatamente
        var campoPesquisa = _wait.Until(d => d.FindElement(By.Name("q")));
        campoPesquisa.Clear();
        campoPesquisa.SendKeys(termoPesquisa);

        // 3. Utilizar uma espera explícita (WebDriverWait) para aguardar a listagem de sugestões
        // Seletores possíveis: .search-suggestions-list li, .sbox-suggestions li, [role='option']
        var listaSugestoes = _wait.Until(d =>
        {
            var elements = d.FindElements(By.CssSelector("[role='option'], .search-suggestions-list li, .sbox-suggestions li, .acp-wrap li"));
            return elements.Count >= 3 ? elements : null;
        });

        // Verificar se temos pelo menos 3 sugestões
        Assert.IsTrue(listaSugestoes.Count >= 3, "Não foram encontradas pelo menos 3 sugestões de autocompletar");

        // 4. Clicar na terceira opção sugerida pela lista dinâmica
        var terceiraOpcao = listaSugestoes[2]; // Índice 2 = terceira opção (base 0)
        var textoTerceiraOpcao = terceiraOpcao.Text;

        // Usar JavaScript click para evitar problemas de interceptação
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", terceiraOpcao);

        // 5. Garantir através de asserção que a navegação redirecionou corretamente
        _wait.Until(d => d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        // Assert
        var urlResultados = _driver.Url;
        Assert.IsTrue(
            urlResultados.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase) &&
            urlResultados.Contains("q=", StringComparison.OrdinalIgnoreCase),
            $"A navegação não redirecionou para a página de resultados. URL atual: '{urlResultados}'");

        // Verificar se o termo da terceira opção está presente na URL ou título
        Assert.IsTrue(
            urlResultados.Contains(textoTerceiraOpcao.Split(' ')[0], StringComparison.OrdinalIgnoreCase) ||
            _driver.Title.Contains(textoTerceiraOpcao.Split(' ')[0], StringComparison.OrdinalIgnoreCase),
            $"O resultado não corresponde à terceira opção selecionada ('{textoTerceiraOpcao}'). Título: '{_driver.Title}', URL: '{urlResultados}'");
    }

    #endregion

    #region Exercício 3: Navegação por Histórico e Botões de Ação

    /// <summary>
    /// Exercício 3: Simular interações complexas de navegação, utilizando os botões de "Voltar" e "Avançar" do navegador.
    /// </summary>
    [TestMethod]
    [TestCategory("Exercicio3")]
    public void Exercicio3_NavegacaoHistorico_BotaoVoltarAvancar()
    {
        // Act
        // 1. Realizar uma busca inicial por "Automated Testing"
        var campoPesquisa = _wait.Until(d => d.FindElement(By.Name("q")));
        campoPesquisa.Clear();
        campoPesquisa.SendKeys("Automated Testing");
        campoPesquisa.SendKeys(Keys.Enter);

        // Aguardar carregamento dos resultados
        _wait.Until(d => d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        var urlResultadosBusca = _driver.Url;

        // 2. Clicar no primeiro link orgânico relevante retornado na página de resultados
        // Seletor para links de resultados orgânicos do DuckDuckGo
        // Usar seletores mais específicos para a estrutura atual do DuckDuckGo
        var primeiroLink = _wait.Until(d =>
        {
            // Tentar múltiplos seletores em ordem de preferência
            var selectors = new[]
            {
                "article[data-testid='result'] a[data-testid='result-title-a']",
                ".result__snippet a.result__url",
                ".result__snippet a",
                "[data-testid='result-title-a']",
                ".web-result-title a",
                ".result a.result__snippet",
                "main .results a"
            };

            foreach (var selector in selectors)
            {
                var elements = d.FindElements(By.CssSelector(selector));
                if (elements.Count > 0)
                {
                    // Filtrar apenas links visíveis e com href
                    var visibleLinks = elements.Where(e => e.Displayed && !string.IsNullOrEmpty(e.GetAttribute("href"))).ToList();
                    if (visibleLinks.Count > 0)
                        return visibleLinks[0];
                }
            }
            return null;
        });

        var urlPrimeiroLink = primeiroLink.GetAttribute("href");
        Assert.IsNotNull(urlPrimeiroLink, "O primeiro link não possui atributo href");
        Assert.IsTrue(urlPrimeiroLink.StartsWith("http"), "O link não parece ser uma URL válida");

        // Navegar diretamente para a URL do link (mais confiável que clicar)
        _driver.Navigate().GoToUrl(urlPrimeiroLink);

        // 3. Validar que a página externa foi carregada com sucesso verificando a URL atual
        _wait.Until(d => d.Url != urlResultadosBusca);
        var urlPaginaExterna = _driver.Url;

        Assert.AreNotEqual(urlResultadosBusca, urlPaginaExterna, "A navegação não saiu da página de resultados");
        Assert.IsFalse(urlPaginaExterna.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase),
            "A página externa ainda é do DuckDuckGo");

        // 4. Utilizar o comando de navegação do Selenium (_driver.Navigation.Back()) para retornar à página de resultados
        _driver.Navigate().Back();

        // 5. Validar que a página de resultados foi restaurada corretamente
        _wait.Until(d => d.Url.Contains("q=", StringComparison.OrdinalIgnoreCase));

        var urlAposVoltar = _driver.Url;

        // Assert
        Assert.IsTrue(
            urlAposVoltar.Contains("duckduckgo.com", StringComparison.OrdinalIgnoreCase) &&
            urlAposVoltar.Contains("q=", StringComparison.OrdinalIgnoreCase),
            $"A página de resultados não foi restaurada corretamente. URL atual: '{urlAposVoltar}'");

        // Validar que o termo de busca original está presente
        Assert.IsTrue(
            urlAposVoltar.Contains("Automated+Testing", StringComparison.OrdinalIgnoreCase) ||
            urlAposVoltar.Contains("Automated%20Testing", StringComparison.OrdinalIgnoreCase) ||
            _driver.Title.Contains("Automated Testing", StringComparison.OrdinalIgnoreCase),
            $"O termo de busca original não está presente na URL restaurada. URL: '{urlAposVoltar}', Título: '{_driver.Title}'");
    }

    #endregion
}
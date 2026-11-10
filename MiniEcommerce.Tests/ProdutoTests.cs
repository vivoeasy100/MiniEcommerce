using MiniEcommerce.Console.Models;
using MiniEcommerce.Console.Services;
using Xunit;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void CriarProduto_ComDadosValidos_DeveInstanciarComSucesso()
    {
        // Arrange & Act
        var produto = new Produto(1, "Notebook", 3500.00m, 10);

        // Assert
        Assert.Equal(1, produto.Id);
        Assert.Equal("Notebook", produto.Nome);
        Assert.Equal(3500.00m, produto.Preco);
        Assert.Equal(10, produto.QuantidadeEstoque);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CriarProduto_ComNomeInvalido_DeveLancarArgumentException(string? nomeInvalido)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Produto(1, nomeInvalido!, 100m, 5));
        Assert.Contains("nome", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CriarProduto_ComPrecoNegativo_DeveLancarArgumentException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Produto(1, "Mouse", -50.00m, 10));
        Assert.Contains("preço", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CriarProduto_ComEstoqueNegativo_DeveLancarArgumentException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Produto(1, "Teclado", 150.00m, -1));
        Assert.Contains("estoque", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Catalogo_CadastrarProduto_ComPrecoNegativo_DeveLancarArgumentException()
    {
        // Arrange
        var catalogo = new Catalogo();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => catalogo.CadastrarProduto("Monitor", -200m, 5));
    }

    [Fact]
    public void Catalogo_CadastrarProduto_ComEstoqueNegativo_DeveLancarArgumentException()
    {
        // Arrange
        var catalogo = new Catalogo();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => catalogo.CadastrarProduto("Monitor", 800m, -3));
    }

    [Fact]
    public void Catalogo_CadastrarProduto_ComDadosValidos_DeveAdicionarAoCatalogo()
    {
        // Arrange
        var catalogo = new Catalogo();

        // Act
        var produto = catalogo.CadastrarProduto("Monitor Ultrawide", 1200m, 7);

        // Assert
        Assert.NotNull(produto);
        Assert.Equal(1, produto.Id);
        Assert.Single(catalogo.ListarProdutos());
        Assert.Equal(produto, catalogo.ObterPorId(1));
    }
}

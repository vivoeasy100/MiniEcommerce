using MiniEcommerce.Console.Models;
using Xunit;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    [Fact]
    public void AdicionarItem_ComEstoqueDisponivel_DeveAdicionarAoCarrinhoComSucesso()
    {
        // Arrange
        var produto = new Produto(1, "Mouse Gamer", 150.00m, 5);
        var carrinho = new CarrinhoDeCompras();

        // Act
        carrinho.AdicionarItem(produto, 3);

        // Assert
        Assert.Single(carrinho.Itens);
        Assert.Equal(3, carrinho.Itens[0].Quantidade);
        Assert.Equal(450.00m, carrinho.CalcularSubtotal());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void AdicionarItem_ComQuantidadeInvalida_DeveLancarArgumentException(int quantidadeInvalida)
    {
        // Arrange
        var produto = new Produto(1, "Teclado Mecânico", 250.00m, 10);
        var carrinho = new CarrinhoDeCompras();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => carrinho.AdicionarItem(produto, quantidadeInvalida));
        Assert.Contains("quantidade", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AdicionarItem_ComQuantidadeSuperiorAoEstoque_DeveLancarInvalidOperationException()
    {
        // Arrange
        var produto = new Produto(1, "Monitor 144Hz", 1200.00m, 2);
        var carrinho = new CarrinhoDeCompras();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => carrinho.AdicionarItem(produto, 5));
        Assert.Contains("excede o estoque", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AdicionarItem_MesmoProdutoMultiplasVezes_DentroDoEstoque_DeveIncrementarQuantidade()
    {
        // Arrange
        var produto = new Produto(1, "Headset Gamer", 300.00m, 10);
        var carrinho = new CarrinhoDeCompras();

        // Act
        carrinho.AdicionarItem(produto, 2);
        carrinho.AdicionarItem(produto, 3);

        // Assert
        Assert.Single(carrinho.Itens);
        Assert.Equal(5, carrinho.Itens[0].Quantidade);
        Assert.Equal(1500.00m, carrinho.CalcularSubtotal());
    }

    [Fact]
    public void AdicionarItem_MesmoProdutoMultiplasVezes_ExcedendoEstoque_DeveLancarInvalidOperationException()
    {
        // Arrange
        var produto = new Produto(1, "Cadeira Gamer", 800.00m, 3);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(produto, 2);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => carrinho.AdicionarItem(produto, 2));
        Assert.Contains("excede o estoque", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CalcularSubtotal_ComMultiplosItens_DeveRetornarValorTotalCorreto()
    {
        // Arrange
        var prod1 = new Produto(1, "Mouse", 100.00m, 10);
        var prod2 = new Produto(2, "Teclado", 200.00m, 5);
        var carrinho = new CarrinhoDeCompras();

        // Act
        carrinho.AdicionarItem(prod1, 2); // 200.00
        carrinho.AdicionarItem(prod2, 1); // 200.00

        // Assert
        Assert.Equal(400.00m, carrinho.CalcularSubtotal());
    }

    [Fact]
    public void RemoverItem_ItemExistente_DeveRemoverDoCarrinho()
    {
        // Arrange
        var produto = new Produto(1, "Mouse", 100.00m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(produto, 2);

        // Act
        var removido = carrinho.RemoverItem(1);

        // Assert
        Assert.True(removido);
        Assert.Empty(carrinho.Itens);
        Assert.Equal(0m, carrinho.CalcularSubtotal());
    }

    [Fact]
    public void Limpar_ComItensNoCarrinho_DeveEsvaziarCarrinho()
    {
        // Arrange
        var produto = new Produto(1, "Mouse", 100.00m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(produto, 2);

        // Act
        carrinho.Limpar();

        // Assert
        Assert.Empty(carrinho.Itens);
    }
}

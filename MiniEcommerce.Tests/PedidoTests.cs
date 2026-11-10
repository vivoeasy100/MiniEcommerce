using System.Globalization;
using MiniEcommerce.Console.Models;
using Xunit;

namespace MiniEcommerce.Tests;

public class PedidoTests
{
    [Fact]
    public void FecharPedido_ComCarrinhoValido_DeveDecrementarEstoqueDosProdutos()
    {
        // Arrange
        var prod1 = new Produto(1, "Notebook Gamer", 4500.00m, 5);
        var prod2 = new Produto(2, "Mouse Sem Fio", 120.00m, 10);

        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(prod1, 2);
        carrinho.AdicionarItem(prod2, 3);

        // Act
        var pedido = Pedido.FecharPedido(1, carrinho);

        // Assert
        Assert.Equal(3, prod1.QuantidadeEstoque); // 5 - 2 = 3
        Assert.Equal(7, prod2.QuantidadeEstoque); // 10 - 3 = 7
    }

    [Fact]
    public void FecharPedido_ComCarrinhoValido_DeveCalcularValorTotalCorretoECriarItens()
    {
        // Arrange
        var prod1 = new Produto(1, "Monitor 24pol", 900.00m, 4);
        var prod2 = new Produto(2, "Suporte Monitor", 150.00m, 5);

        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(prod1, 2); // 1800.00
        carrinho.AdicionarItem(prod2, 1); // 150.00

        // Act
        var pedido = Pedido.FecharPedido(10, carrinho);

        // Assert
        Assert.Equal(10, pedido.Id);
        Assert.Equal(1950.00m, pedido.ValorTotal);
        Assert.Equal(2, pedido.Itens.Count);
        Assert.Equal(2, pedido.Itens[0].Quantidade);
        Assert.Equal(1800.00m, pedido.Itens[0].Subtotal);
        Assert.Equal(1, pedido.Itens[1].Quantidade);
        Assert.Equal(150.00m, pedido.Itens[1].Subtotal);
    }

    [Fact]
    public void FecharPedido_ComCarrinhoValido_DeveLimparCarrinhoAposCheckout()
    {
        // Arrange
        var prod = new Produto(1, "Headset", 200.00m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(prod, 1);

        // Act
        var pedido = Pedido.FecharPedido(1, carrinho);

        // Assert
        Assert.Empty(carrinho.Itens);
        Assert.Equal(0m, carrinho.Subtotal);
    }

    [Fact]
    public void FecharPedido_ComCarrinhoVazio_DeveLancarInvalidOperationException()
    {
        // Arrange
        var carrinho = new CarrinhoDeCompras();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Pedido.FecharPedido(1, carrinho));
        Assert.Contains("vazio", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void FecharPedido_ComEstoqueInsuficienteNoMomentoDoCheckout_DeveLancarInvalidOperationExceptionENaoDeduzirEstoque()
    {
        // Arrange
        var prod = new Produto(1, "Placa de Vídeo", 3000.00m, 2);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(prod, 2);

        // Simula estoque diminuído por outra operação antes do checkout
        prod.QuantidadeEstoque = 1;

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Pedido.FecharPedido(1, carrinho));
        Assert.Contains("insuficiente", ex.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(1, prod.QuantidadeEstoque); // estoque permaneceu intacto
    }

    [Fact]
    public void GerarResumo_DeveConterResumoFinanceiroComDetalhesDoPedido()
    {
        // Arrange
        var prod = new Produto(1, "Câmera Web Full HD", 250.00m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(prod, 2);

        var pedido = Pedido.FecharPedido(100, carrinho);

        // Act
        var resumo = pedido.GerarResumo();

        // Assert
        Assert.Contains("#100", resumo);
        Assert.Contains("Câmera Web Full HD", resumo);
        Assert.Contains(500.00m.ToString("C", CultureInfo.CurrentCulture), resumo);
    }
}

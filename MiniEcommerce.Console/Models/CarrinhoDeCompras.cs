namespace MiniEcommerce.Console.Models;

/// <summary>
/// Gerencia os itens selecionados pelo cliente e calcula os subtotais e totais da compra.
/// </summary>
public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = new();

    /// <summary>
    /// Lista dos itens atualmente contidos no carrinho.
    /// </summary>
    public IReadOnlyList<ItemCarrinho> Itens => _itens.AsReadOnly();

    /// <summary>
    /// Valor acumulado dos subtotais dos itens contidos no carrinho.
    /// </summary>
    public decimal Subtotal => CalcularSubtotal();

    /// <summary>
    /// Adiciona um produto e quantidade ao carrinho, validando se a quantidade solicitada
    /// respeita a disponibilidade de estoque do produto.
    /// </summary>
    public virtual void AdicionarItem(Produto produto, int quantidade)
    {
        if (produto == null)
            throw new ArgumentNullException(nameof(produto));

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade adicionada deve ser maior que zero.", nameof(quantidade));

        var itemExistente = _itens.FirstOrDefault(i => i.Produto.Id == produto.Id);
        int quantidadeTotalDesejada = (itemExistente?.Quantidade ?? 0) + quantidade;

        if (quantidadeTotalDesejada > produto.QuantidadeEstoque)
        {
            throw new InvalidOperationException(
                $"A quantidade solicitada ({quantidadeTotalDesejada}) excede o estoque disponível ({produto.QuantidadeEstoque}) do produto '{produto.Nome}'.");
        }

        if (itemExistente != null)
        {
            itemExistente.Quantidade = quantidadeTotalDesejada;
        }
        else
        {
            _itens.Add(new ItemCarrinho(produto, quantidade));
        }
    }

    /// <summary>
    /// Remove um produto do carrinho a partir do seu identificador.
    /// </summary>
    public virtual bool RemoverItem(int produtoId)
    {
        var item = _itens.FirstOrDefault(i => i.Produto.Id == produtoId);
        if (item != null)
        {
            return _itens.Remove(item);
        }
        return false;
    }

    /// <summary>
    /// Calcula a soma dos subtotais de todos os itens do carrinho.
    /// </summary>
    public virtual decimal CalcularSubtotal()
    {
        return _itens.Sum(i => i.Subtotal);
    }

    /// <summary>
    /// Esvazia o carrinho de compras.
    /// </summary>
    public virtual void Limpar()
    {
        _itens.Clear();
    }
}

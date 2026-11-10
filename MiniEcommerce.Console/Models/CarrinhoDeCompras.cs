namespace MiniEcommerce.Console.Models;

public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = new();

    public IReadOnlyList<ItemCarrinho> Itens => _itens.AsReadOnly();

    public decimal Subtotal => CalcularSubtotal();

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

    public virtual bool RemoverItem(int produtoId)
    {
        var item = _itens.FirstOrDefault(i => i.Produto.Id == produtoId);
        if (item != null)
        {
            return _itens.Remove(item);
        }
        return false;
    }

    public virtual decimal CalcularSubtotal()
    {
        return _itens.Sum(i => i.Subtotal);
    }

    public virtual void Limpar()
    {
        _itens.Clear();
    }
}

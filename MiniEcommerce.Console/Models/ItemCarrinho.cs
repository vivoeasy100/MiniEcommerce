namespace MiniEcommerce.Console.Models;

public class ItemCarrinho
{
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }

    public ItemCarrinho(Produto produto, int quantidade)
    {
        Produto = produto ?? throw new ArgumentNullException(nameof(produto));
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        Quantidade = quantidade;
    }

    public decimal Subtotal => Produto.Preco * Quantidade;
}

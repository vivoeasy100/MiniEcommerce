namespace MiniEcommerce.Console.Models;

public class ItemPedido
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
    public decimal Subtotal => PrecoUnitario * Quantidade;

    public ItemPedido() { }

    public ItemPedido(int produtoId, string nomeProduto, decimal precoUnitario, int quantidade)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        PrecoUnitario = precoUnitario;
        Quantidade = quantidade;
    }
}

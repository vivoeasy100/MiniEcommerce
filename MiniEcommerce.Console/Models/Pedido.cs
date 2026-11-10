namespace MiniEcommerce.Console.Models;

using System.Globalization;
using System.Text;

public class Pedido
{
    public int Id { get; set; }
    public DateTime DataPedido { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
    public decimal ValorTotal { get; set; }

    public Pedido()
    {
        DataPedido = DateTime.Now;
    }

    public Pedido(int id, DateTime dataPedido, List<ItemPedido> itens, decimal valorTotal)
    {
        Id = id;
        DataPedido = dataPedido;
        Itens = itens ?? throw new ArgumentNullException(nameof(itens));
        ValorTotal = valorTotal;
    }

    public static Pedido FecharPedido(int id, CarrinhoDeCompras carrinho)
    {
        if (carrinho == null)
            throw new ArgumentNullException(nameof(carrinho));

        if (!carrinho.Itens.Any())
            throw new InvalidOperationException("Não é possível fechar o pedido: o carrinho de compras está vazio.");

        // Validação prévia de estoque para todos os itens antes de deduzir qualquer estoque
        foreach (var item in carrinho.Itens)
        {
            if (item.Quantidade > item.Produto.QuantidadeEstoque)
            {
                throw new InvalidOperationException(
                    $"Estoque insuficiente para o produto '{item.Produto.Nome}'. Disponível: {item.Produto.QuantidadeEstoque}, Solicitado: {item.Quantidade}.");
            }
        }

        var itensPedido = new List<ItemPedido>();
        decimal total = 0m;

        // Deduz estoque e cria snapshot dos itens
        foreach (var item in carrinho.Itens)
        {
            item.Produto.DeduzirEstoque(item.Quantidade);
            var itemPedido = new ItemPedido(
                item.Produto.Id,
                item.Produto.Nome,
                item.Produto.Preco,
                item.Quantidade
            );
            itensPedido.Add(itemPedido);
            total += itemPedido.Subtotal;
        }

        var pedido = new Pedido(id, DateTime.Now, itensPedido, total);

        // Limpa o carrinho após fechar o pedido com sucesso
        carrinho.Limpar();

        return pedido;
    }

    public virtual string GerarResumo()
    {
        var sb = new StringBuilder();
        sb.AppendLine("==================================================");
        sb.AppendLine($"           RESUMO DO PEDIDO #{Id}");
        sb.AppendLine("==================================================");
        sb.AppendLine($"Data/Hora: {DataPedido:dd/MM/yyyy HH:mm:ss}");
        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine("Itens do Pedido:");

        foreach (var item in Itens)
        {
            sb.AppendLine(string.Format(
                CultureInfo.CurrentCulture,
                " - {0} | Qtd: {1} | Unit: {2:C} | Subtotal: {3:C}",
                item.NomeProduto,
                item.Quantidade,
                item.PrecoUnitario,
                item.Subtotal));
        }

        sb.AppendLine("--------------------------------------------------");
        sb.AppendLine(string.Format(
            CultureInfo.CurrentCulture,
            "VALOR TOTAL: {0:C}",
            ValorTotal));
        sb.AppendLine("==================================================");

        return sb.ToString();
    }
}

namespace MiniEcommerce.Console.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }

    public Produto() { }

    public Produto(int id, string nome, decimal preco, int quantidadeEstoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do produto não pode ser vazio ou nulo.", nameof(nome));

        if (preco < 0)
            throw new ArgumentException("O preço do produto não pode ser negativo.", nameof(preco));

        if (quantidadeEstoque < 0)
            throw new ArgumentException("A quantidade em estoque não pode ser menor que zero.", nameof(quantidadeEstoque));

        Id = id;
        Nome = nome.Trim();
        Preco = preco;
        QuantidadeEstoque = quantidadeEstoque;
    }

    public virtual void DeduzirEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade a ser deduzida deve ser maior que zero.", nameof(quantidade));

        if (quantidade > QuantidadeEstoque)
            throw new InvalidOperationException($"Estoque insuficiente para o produto '{Nome}'. Quantidade disponível: {QuantidadeEstoque}, solicitada: {quantidade}.");

        QuantidadeEstoque -= quantidade;
    }

    public virtual void AdicionarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade a ser adicionada deve ser maior que zero.", nameof(quantidade));

        QuantidadeEstoque += quantidade;
    }
}

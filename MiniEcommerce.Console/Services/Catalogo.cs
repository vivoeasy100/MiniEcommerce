namespace MiniEcommerce.Console.Services;

using MiniEcommerce.Console.Models;

/// <summary>
/// Implementação do serviço de gerenciamento do catálogo e estoque de produtos.
/// </summary>
public class Catalogo : ICatalogo
{
    private readonly List<Produto> _produtos = new();
    private int _proximoId = 1;

    /// <inheritdoc />
    public IReadOnlyList<Produto> Produtos => _produtos.AsReadOnly();

    /// <inheritdoc />
    public Produto CadastrarProduto(string nome, decimal preco, int quantidadeEstoque)
    {
        var produto = new Produto(_proximoId++, nome, preco, quantidadeEstoque);
        _produtos.Add(produto);
        return produto;
    }

    /// <inheritdoc />
    public void AdicionarProduto(Produto produto)
    {
        if (produto == null)
            throw new ArgumentNullException(nameof(produto));

        if (_produtos.Any(p => p.Id == produto.Id))
            throw new InvalidOperationException($"Já existe um produto com o ID {produto.Id} no catálogo.");

        _produtos.Add(produto);
    }

    /// <inheritdoc />
    public Produto? ObterPorId(int id)
    {
        return _produtos.FirstOrDefault(p => p.Id == id);
    }

    /// <inheritdoc />
    public IReadOnlyList<Produto> ListarProdutos()
    {
        return _produtos.AsReadOnly();
    }
}

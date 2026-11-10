namespace MiniEcommerce.Console.Services;

using MiniEcommerce.Console.Models;

/// <summary>
/// Contrato para operações de gerenciamento de catálogo de produtos.
/// </summary>
public interface ICatalogo
{
    /// <summary>
    /// Lista somente leitura dos produtos cadastrados.
    /// </summary>
    IReadOnlyList<Produto> Produtos { get; }

    /// <summary>
    /// Cadastra um novo produto no catálogo validando os dados fornecidos.
    /// </summary>
    Produto CadastrarProduto(string nome, decimal preco, int quantidadeEstoque);

    /// <summary>
    /// Adiciona uma entidade de produto existente ao catálogo.
    /// </summary>
    void AdicionarProduto(Produto produto);

    /// <summary>
    /// Busca um produto pelo seu identificador único.
    /// </summary>
    Produto? ObterPorId(int id);

    /// <summary>
    /// Retorna todos os produtos disponíveis no catálogo.
    /// </summary>
    IReadOnlyList<Produto> ListarProdutos();
}

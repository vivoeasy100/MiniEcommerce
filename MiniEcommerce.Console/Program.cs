using System.Globalization;
using MiniEcommerce.Console.Models;
using MiniEcommerce.Console.Services;

namespace MiniEcommerce.Console;

public class Program
{
    private static readonly Catalogo catalogo = new();
    private static readonly CarrinhoDeCompras carrinho = new();
    private static int proximoPedidoId = 1;

    public static void Main(string[] args)
    {
        CarregarDadosIniciais();

        bool executando = true;
        while (executando)
        {
            ExibirMenuPrincipal();
            string? opcao = System.Console.ReadLine()?.Trim();

            System.Console.WriteLine();
            switch (opcao)
            {
                case "1":
                    CadastrarNovoProduto();
                    break;
                case "2":
                    ListarCatalogo();
                    break;
                case "3":
                    AdicionarProdutoAoCarrinho();
                    break;
                case "4":
                    ExibirCarrinho();
                    break;
                case "5":
                    RemoverProdutoDoCarrinho();
                    break;
                case "6":
                    FinalizarPedido();
                    break;
                case "0":
                    executando = false;
                    System.Console.WriteLine("Obrigado por utilizar o Mini E-Commerce. Até logo!");
                    break;
                default:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.WriteLine("Opção inválida! Escolha uma opção do menu.");
                    System.Console.ResetColor();
                    break;
            }

            if (executando)
            {
                System.Console.WriteLine("\nPressione qualquer tecla para continuar...");
                System.Console.ReadKey(true);
            }
        }
    }

    private static void ExibirMenuPrincipal()
    {
        System.Console.Clear();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("==================================================");
        System.Console.WriteLine("                MINI E-COMMERCE                   ");
        System.Console.WriteLine("==================================================");
        System.Console.ResetColor();
        System.Console.WriteLine("1 - Cadastrar Produto");
        System.Console.WriteLine("2 - Listar Catálogo de Produtos");
        System.Console.WriteLine("3 - Adicionar Produto ao Carrinho");
        System.Console.WriteLine("4 - Visualizar Carrinho de Compras");
        System.Console.WriteLine("5 - Remover Produto do Carrinho");
        System.Console.WriteLine("6 - Finalizar Pedido (Checkout)");
        System.Console.WriteLine("0 - Sair");
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("==================================================");
        System.Console.ResetColor();
        System.Console.Write("Escolha uma opção: ");
    }

    private static void CadastrarNovoProduto()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("--- [ Cadastro de Produto ] ---");
        System.Console.ResetColor();

        System.Console.Write("Nome do Produto: ");
        string? nome = System.Console.ReadLine();

        System.Console.Write("Preço Unitário (ex: 99,90): ");
        string? precoInput = System.Console.ReadLine();

        System.Console.Write("Quantidade em Estoque: ");
        string? estoqueInput = System.Console.ReadLine();

        if (!decimal.TryParse(precoInput, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal preco))
        {
            if (!decimal.TryParse(precoInput, NumberStyles.Any, CultureInfo.InvariantCulture, out preco))
            {
                ExibirMensagemErro("Preço inválido. Certifique-se de digitar um número válido.");
                return;
            }
        }

        if (!int.TryParse(estoqueInput, out int estoque))
        {
            ExibirMensagemErro("Quantidade em estoque inválida. Deve ser um número inteiro.");
            return;
        }

        try
        {
            var produto = catalogo.CadastrarProduto(nome ?? string.Empty, preco, estoque);
            ExibirMensagemSucesso($"Produto '{produto.Nome}' cadastrado com sucesso com ID {produto.Id}!");
        }
        catch (ArgumentException ex)
        {
            ExibirMensagemErro($"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            ExibirMensagemErro($"Erro ao cadastrar produto: {ex.Message}");
        }
    }

    private static void ListarCatalogo()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("--- [ Catálogo de Produtos ] ---");
        System.Console.ResetColor();

        var produtos = catalogo.ListarProdutos();
        if (!produtos.Any())
        {
            System.Console.WriteLine("Nenhum produto cadastrado no catálogo.");
            return;
        }

        System.Console.WriteLine(string.Format("{0,-6} | {1,-30} | {2,12} | {3,10}", "ID", "Nome", "Preço", "Estoque"));
        System.Console.WriteLine(new string('-', 66));

        foreach (var p in produtos)
        {
            System.Console.WriteLine(string.Format(
                CultureInfo.CurrentCulture,
                "{0,-6} | {1,-30} | {2,12:C} | {3,10}",
                p.Id,
                p.Nome.Length > 30 ? p.Nome.Substring(0, 27) + "..." : p.Nome,
                p.Preco,
                p.QuantidadeEstoque));
        }
    }

    private static void AdicionarProdutoAoCarrinho()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("--- [ Adicionar ao Carrinho ] ---");
        System.Console.ResetColor();

        ListarCatalogo();
        System.Console.WriteLine();

        System.Console.Write("Informe o ID do produto desejado: ");
        if (!int.TryParse(System.Console.ReadLine(), out int produtoId))
        {
            ExibirMensagemErro("ID de produto inválido.");
            return;
        }

        var produto = catalogo.ObterPorId(produtoId);
        if (produto == null)
        {
            ExibirMensagemErro($"Produto com ID {produtoId} não encontrado no catálogo.");
            return;
        }

        System.Console.Write($"Informe a quantidade desejada de '{produto.Nome}': ");
        if (!int.TryParse(System.Console.ReadLine(), out int quantidade) || quantidade <= 0)
        {
            ExibirMensagemErro("A quantidade deve ser um número inteiro positivo.");
            return;
        }

        try
        {
            carrinho.AdicionarItem(produto, quantidade);
            ExibirMensagemSucesso($"{quantidade}x '{produto.Nome}' adicionado(s) ao carrinho com sucesso!");
        }
        catch (InvalidOperationException ex)
        {
            ExibirMensagemErro($"Não foi possível adicionar ao carrinho: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            ExibirMensagemErro($"Erro de parâmetro: {ex.Message}");
        }
    }

    private static void ExibirCarrinho()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("--- [ Carrinho de Compras ] ---");
        System.Console.ResetColor();

        if (!carrinho.Itens.Any())
        {
            System.Console.WriteLine("O seu carrinho de compras está vazio.");
            return;
        }

        System.Console.WriteLine(string.Format("{0,-6} | {1,-28} | {2,12} | {3,6} | {4,14}", "ID", "Produto", "Unitário", "Qtd", "Subtotal"));
        System.Console.WriteLine(new string('-', 75));

        foreach (var item in carrinho.Itens)
        {
            System.Console.WriteLine(string.Format(
                CultureInfo.CurrentCulture,
                "{0,-6} | {1,-28} | {2,12:C} | {3,6} | {4,14:C}",
                item.Produto.Id,
                item.Produto.Nome.Length > 28 ? item.Produto.Nome.Substring(0, 25) + "..." : item.Produto.Nome,
                item.Produto.Preco,
                item.Quantidade,
                item.Subtotal));
        }

        System.Console.WriteLine(new string('-', 75));
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "SUBTOTAL DO CARRINHO: {0:C}", carrinho.CalcularSubtotal()));
        System.Console.ResetColor();
    }

    private static void RemoverProdutoDoCarrinho()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("--- [ Remover do Carrinho ] ---");
        System.Console.ResetColor();

        if (!carrinho.Itens.Any())
        {
            System.Console.WriteLine("O seu carrinho de compras já está vazio.");
            return;
        }

        ExibirCarrinho();
        System.Console.WriteLine();

        System.Console.Write("Informe o ID do produto que deseja remover: ");
        if (!int.TryParse(System.Console.ReadLine(), out int produtoId))
        {
            ExibirMensagemErro("ID de produto inválido.");
            return;
        }

        bool removido = carrinho.RemoverItem(produtoId);
        if (removido)
        {
            ExibirMensagemSucesso("Item removido do carrinho com sucesso!");
        }
        else
        {
            ExibirMensagemErro($"Nenhum item com ID {produtoId} encontrado no carrinho.");
        }
    }

    private static void FinalizarPedido()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("--- [ Fechamento de Pedido (Checkout) ] ---");
        System.Console.ResetColor();

        if (!carrinho.Itens.Any())
        {
            ExibirMensagemErro("O carrinho está vazio. Adicione produtos antes de finalizar o pedido.");
            return;
        }

        ExibirCarrinho();
        System.Console.WriteLine();

        System.Console.Write("Confirma o fechamento da compra? (S/N): ");
        string? confirmacao = System.Console.ReadLine()?.Trim().ToUpperInvariant();

        if (confirmacao != "S" && confirmacao != "SIM")
        {
            System.Console.WriteLine("Operação cancelada pelo usuário.");
            return;
        }

        try
        {
            var pedido = Pedido.FecharPedido(proximoPedidoId++, carrinho);
            ExibirMensagemSucesso("Pedido finalizado com sucesso!");
            System.Console.WriteLine();
            System.Console.WriteLine(pedido.GerarResumo());
        }
        catch (InvalidOperationException ex)
        {
            ExibirMensagemErro($"Falha ao finalizar o pedido: {ex.Message}");
        }
        catch (Exception ex)
        {
            ExibirMensagemErro($"Erro inesperado ao fechar pedido: {ex.Message}");
        }
    }

    private static void CarregarDadosIniciais()
    {
        catalogo.CadastrarProduto("Notebook Dell Inspiron", 4200.00m, 5);
        catalogo.CadastrarProduto("Mouse Sem Fio Logitech", 129.90m, 20);
        catalogo.CadastrarProduto("Teclado Mecânico RGB", 280.00m, 12);
        catalogo.CadastrarProduto("Monitor LG UltraWide 29\"", 1350.00m, 8);
        catalogo.CadastrarProduto("Headset HyperX Cloud", 349.99m, 15);
    }

    private static void ExibirMensagemSucesso(string mensagem)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"[SUCESSO] {mensagem}");
        System.Console.ResetColor();
    }

    private static void ExibirMensagemErro(string mensagem)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"[ERRO] {mensagem}");
        System.Console.ResetColor();
    }
}

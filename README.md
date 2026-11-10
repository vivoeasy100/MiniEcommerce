# MiniEcommerce

Aplicação de console para simulação de um e-commerce desenvolvida em **.NET / C#** com foco em **Garantia da Qualidade de Software**, seguindo as práticas de **TDD (Test-Driven Development)** e o padrão **Conventional Commits**.

---

## 📋 Contexto Acadêmico

- **Disciplina:** Garantia da Qualidade de Software / Gestão e Qualidade de Software
- **Instituição:** Ecossistema Ânima Educação
- **Professor:** Daniel Henrique Matos de Paiva
- **Tema:** Prática com Git, Conventional Commits, xUnit e Arquitetura de Software em .NET

---

## 🚀 Funcionalidades da Aplicação

1. **Cadastro de Produtos:**
   - Registro de produtos com Nome, Preço e Quantidade em Estoque.
   - Validações de domínio: impede nomes vazios, preços negativos e quantidades de estoque menores que zero.

2. **Catálogo e Consulta:**
   - Exibição tabular de todos os itens cadastrados no catálogo, com IDs, nomes, preços e estoques atualizados em tempo real.

3. **Gerenciamento de Carrinho de Compras:**
   - Adição de itens com controle estrito de estoque (não permite adicionar quantidades superiores ao estoque disponível).
   - Acúmulo de quantidades para o mesmo produto respeitando o limite físico.
   - Cálculo automático do subtotal do carrinho.
   - Remoção individual de produtos do carrinho.

4. **Fechamento de Pedido (Checkout):**
   - Validação atômica de disponibilidade de estoque antes da confirmação.
   - Baixa automática no estoque de cada produto adquirido.
   - Geração de recibo e resumo financeiro detalhado com data, itens, quantidades, valores unitários e total geral.
   - Limpeza automática do carrinho pós-venda.

---

## 🏗️ Arquitetura da Solução

A solução é organizada no padrão de separação de responsabilidades entre aplicação e testes unitários:

```text
MiniEcommerce/
├── MiniEcommerce.slnx
├── MiniEcommerce.Console/
│   ├── MiniEcommerce.Console.csproj
│   ├── Program.cs                      # Interface interativa de linha de comando
│   ├── Models/
│   │   ├── Produto.cs                  # Entidade de produto e regras de estoque
│   │   ├── ItemCarrinho.cs             # Modelo de item contido no carrinho
│   │   ├── CarrinhoDeCompras.cs        # Gerenciamento de carrinho e regras de adição
│   │   ├── ItemPedido.cs               # Snapshot histórico do item no pedido
│   │   └── Pedido.cs                   # Regras de checkout, baixa e resumo financeiro
│   └── Services/
│       ├── ICatalogo.cs                # Contrato do catálogo de produtos
│       └── Catalogo.cs                 # Gerenciamento em memória dos produtos
└── MiniEcommerce.Tests/
    ├── MiniEcommerce.Tests.csproj
    ├── ProdutoTests.cs                 # Testes unitários de validações de Produto e Catálogo
    ├── CarrinhoDeComprasTests.cs       # Testes unitários de regras do carrinho e limites
    └── PedidoTests.cs                  # Testes unitários de checkout e dedução de estoque
```

---

## 🛠️ Tecnologias Utilizadas

- **Plataforma:** .NET 10.0 SDK
- **Linguagem:** C# 13 / C# 14
- **Testes Unitários:** xUnit
- **Padrão de Versionamento:** Git Flow com Conventional Commits (`feat`, `fix`, `test`, `refactor`, `docs`, `chore`)

---

## ⚙️ Como Executar o Projeto

### Pré-requisitos
- [.NET SDK](https://dotnet.microsoft.com/download) instalado (versão 8.0, 9.0 ou 10.0).
- Git instalado.

### 1. Clonar o Repositório
```bash
git clone <URL_DO_REPOSITORIO>
cd MiniEcommerce
```

### 2. Restaurar e Compilar a Solução
```bash
dotnet restore
dotnet build
```

### 3. Executar os Testes Unitários
```bash
dotnet test
```

### 4. Executar a Aplicação de Console
```bash
dotnet run --project MiniEcommerce.Console/MiniEcommerce.Console.csproj
```

---

## 🧪 Estratégia de Testes (TDD)

O desenvolvimento seguiu os princípios de **Test-Driven Development (TDD)**:
1. **Red/Test:** Criação prévia de testes unitários xUnit cobrindo casos de sucesso e cenários de exceção (preços inválidos, estoque negativo, limites de adição ao carrinho, estoques insuficientes no checkout).
2. **Green/Feat:** Implementação da lógica de negócio e validações até a aprovação de 100% dos casos de teste.
3. **Refactor:** Organização do código, criação de interfaces, documentação XML e padronização visual da saída no console.

---

## 📜 Histórico de Commits (Conventional Commits)

A evolução do repositório seguiu estritamente o roteiro de etapas estipulado:

- `chore: initial project structure with console and xunit apps`
- `test(produto): add unit tests for product validation rules`
- `feat(produto): implement product entity and validation logic`
- `test(carrinho): add unit tests for cart addition and stock limits`
- `feat(carrinho): implement shopping cart logic and stock verification`
- `test(pedido): add unit tests for checkout and stock deduction`
- `feat(pedido): implement order processing and inventory update`
- `feat(console): implement interactive menu for e-commerce store`
- `refactor(core): improve code readability and organize namespaces`
- `docs: add comprehensive instructions in README.md`

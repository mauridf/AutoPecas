using AutoPecas.Core.Entities;

namespace AutoPecas.Core.Interfaces;

public interface IEstoqueService
{
    Task AtualizarEstoque(int idProduto, int quantidade);
    Task VerificarEstoqueBaixo();
}
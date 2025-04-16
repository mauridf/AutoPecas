namespace AutoPecas.Core.Interfaces;

public interface IClienteService
{
    Task ValidarDocumentoExistente(string documento);
}
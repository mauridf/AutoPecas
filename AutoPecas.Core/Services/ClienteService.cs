using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace AutoPecas.Core.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(
        IClienteRepository clienteRepository,
        ILogger<ClienteService> logger)
    {
        _clienteRepository = clienteRepository;
        _logger = logger;
    }

    public async Task ValidarDocumentoExistente(string documento)
    {
        try
        {
            var clienteExistente = await _clienteRepository.ObterPorDocumento(documento);
            if (clienteExistente != null)
            {
                throw new BusinessException("Já existe um cliente cadastrado com este documento");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar documento do cliente");
            throw;
        }
    }
}
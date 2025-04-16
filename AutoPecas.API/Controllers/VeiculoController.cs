using AutoPecas.Core.DTOs;
using AutoPecas.Core.Interfaces;
using AutoPecas.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoPecas.Core.Exceptions;

namespace AutoPecas.API.Controllers;

/// <summary>
/// Controller para gerenciamento de veiculos
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VeiculoController : BaseController
{
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly ILogger<VeiculoController> _logger;

    public VeiculoController(
        IVeiculoRepository veiculoRepository,
        ILogger<VeiculoController> logger)
    {
        _veiculoRepository = veiculoRepository;
        _logger = logger;
    }

    /// <summary>
    /// Listar Veiculos
    /// </summary>
    /// <returns>Listar Veiculos cadastrados</returns>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] PaginationDto pagination)
    {
        try
        {
            var veiculos = await _veiculoRepository.Listar();
            return HandleResult(veiculos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar veiculos");
            return HandleError("Erro ao listar veiculos");
        }
    }

    /// <summary>
    /// Obter Veiculo
    /// </summary>
    /// <returns>Obter Veiculo pelo Id</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        try
        {
            var veiculo = await _veiculoRepository.Obter(id);
            return HandleResult(veiculo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter veiculo {VeiculoId}", id);
            return HandleError($"Erro ao obter veiculo {id}");
        }
    }

    /// <summary>
    /// Obter Por Marca
    /// </summary>
    /// <returns>Obter Veiculo pela Marca</returns>
    [HttpGet("marca/{marca}")]
    public async Task<IActionResult> ObterPorMarca(string marca)
    {
        try
        {
            var veiculo = await _veiculoRepository.BuscarPorMarca(marca);
            return HandleResult(marca);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter veiculo por marca {Marca}", marca);
            return HandleError($"Erro ao obter veiculo por marca {marca}");
        }
    }

    /// <summary>
    /// Obter Por Nome
    /// </summary>
    /// <returns>Obter Veiculo pelo Nome</returns>
    [HttpGet("nome/{nome}")]
    public async Task<IActionResult> ObterPorNome(string nome)
    {
        try
        {
            var veiculo = await _veiculoRepository.BuscarPorNome(nome);
            return HandleResult(nome);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter veiculo por nome {Nome}", nome);
            return HandleError($"Erro ao obter veiculo por  nome {nome}");
        }
    }

    /// <summary>
    /// Adicionar Veiculo
    /// </summary>
    /// <returns>Cadastrar Veiculo</returns>
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] VeiculoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var veiculo = new Veiculo
            {
                Nome = dto.Nome,
                Marca = dto.Marca,
                AnoModelo = dto.AnoModelo
            };

            await _veiculoRepository.Adicionar(veiculo);
            return HandleResult(veiculo, "Veiculo adicionado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar veiculo");
            return HandleError("Erro ao adicionar veiculo");
        }
    }

    /// <summary>
    /// Atualizar Veiculo
    /// </summary>
    /// <returns>Atualizar informações do Veiculo</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] VeiculoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return HandleError("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            var veiculo = await _veiculoRepository.Obter(id);
            if (veiculo == null)
                return HandleError("Veiculo não encontrado");

            veiculo.Nome = dto.Nome;
            veiculo.Marca = dto.Marca;
            veiculo.AnoModelo = dto.AnoModelo;

            await _veiculoRepository.Atualizar(veiculo);
            return HandleResult(veiculo, "Veiculo atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar veiculo {VeiculoId}", id);
            return HandleError($"Erro ao atualizar veiculo {id}");
        }
    }

    /// <summary>
    /// Remover Veiculo
    /// </summary>
    /// <returns>Excluir Veiculo</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            var veiculo = await _veiculoRepository.Obter(id);
            if (veiculo == null)
                return HandleError("Veiculo não encontrado");

            await _veiculoRepository.Remover(veiculo);
            return HandleResult(true, "Veiculo removido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover veiculo {VeiculoId}", id);
            return HandleError($"Erro ao remover veiculo {id}");
        }
    }
}

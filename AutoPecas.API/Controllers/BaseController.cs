using AutoPecas.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AutoPecas.API.Controllers;

/// <summary>
/// Controller Base
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(T result, string successMessage = "Operação realizada com sucesso")
    {
        if (result == null)
            return NotFound(new ResultDto<T> { Success = false, Message = "Registro não encontrado" });

        return Ok(new ResultDto<T>
        {
            Success = true,
            Message = successMessage,
            Data = result
        });
    }

    protected IActionResult HandleError(string errorMessage, IEnumerable<string>? errors = null)
    {
        return BadRequest(new ResultDto<object>
        {
            Success = false,
            Message = errorMessage,
            Errors = errors
        });
    }
}
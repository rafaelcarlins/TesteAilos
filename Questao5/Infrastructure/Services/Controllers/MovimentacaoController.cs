using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.DTO;
using Questao5.Application.Commands.Interface;
using Questao5.Application.Commands.Mapper;
using Questao5.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Infrastructure.Services.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class MovimentosFinanceirosController : ControllerBase
    {
        private readonly IMovimentacaoFinanceiroService _movimentacaoService;

        public MovimentosFinanceirosController(IMovimentacaoFinanceiroService movimentacaoService)
        {
            _movimentacaoService = movimentacaoService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovimentacaoFinanceiroDto movimento)
        {
            try
            {
                var movimentacao = MovimentacaoFinanceiroMapper.MapToDomain(movimento);

                bool contaCorrenteExiste = await _movimentacaoService.VerificarContaCorrenteCadastrada(movimentacao);

                if (!contaCorrenteExiste)
                {
                    return BadRequest("Apenas contas correntes cadastradas podem receber movimentação - TIPO: INVALID_ACCOUNT");
                }

                bool contaCorrenteAtiva = await _movimentacaoService.VerificarContaCorrenteAtiva(movimentacao);

                if (!contaCorrenteAtiva)
                {
                    return BadRequest("Apenas contas correntes ativas podem receber movimentação - TIPO: INACTIVE_ACCOUNT");
                }

                if (movimentacao.valor < 0)
                {
                    return BadRequest("Apenas valores positivos podem ser recebidos - TIPO: INVALID_VALUE");
                }

                if (movimentacao.tipoMovimento.ToUpper() !="C" && movimentacao.tipoMovimento.ToUpper() != "D")
                {
                    return BadRequest("Apenas os tipos “débito” ou “crédito” podem ser aceitos - TIPO: INVALID_TYPE");
                }
                await _movimentacaoService.CadastrarMovimento(movimentacao);
                return StatusCode(201);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
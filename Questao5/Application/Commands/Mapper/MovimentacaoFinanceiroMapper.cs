using Questao5.Application.Commands.DTO;
using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Mapper
{
    public class MovimentacaoFinanceiroMapper
    {
        public static MovimentacaoFinanceiro MapToDomain(MovimentacaoFinanceiroDto movimentacaoDto)
        {
            return new MovimentacaoFinanceiro
            {
                requisicaoId = movimentacaoDto.RequisicaoId,
                contaCorrenteId = movimentacaoDto.ContaCorrenteId,
                valor = movimentacaoDto.Valor,
                tipoMovimento = movimentacaoDto.TipoMovimento
            };
        }
    }
}

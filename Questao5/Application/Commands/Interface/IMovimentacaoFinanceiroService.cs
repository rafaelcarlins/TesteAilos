using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Interface
{
    public interface IMovimentacaoFinanceiroService
    {
        public Task<bool> VerificarContaCorrenteCadastrada(MovimentacaoFinanceiro movimentacao);

        public Task<bool> VerificarContaCorrenteAtiva(MovimentacaoFinanceiro movimentacao);

        public Task CadastrarMovimento(MovimentacaoFinanceiro movimentacao);
    }
}

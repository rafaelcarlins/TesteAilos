using Questao5.Application.Commands.Interface;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoFinanceiroService: IMovimentacaoFinanceiroService
    {
        private readonly ContaCorrenteRepository _contaCorrenteRepository;

        public MovimentacaoFinanceiroService(ContaCorrenteRepository contaCorrenteRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task CadastrarMovimento(MovimentacaoFinanceiro movimentacao)
        {
            await _contaCorrenteRepository.CadastrarMovimentoAsync(movimentacao);
        }

        public async Task<bool> VerificarContaCorrenteAtiva(MovimentacaoFinanceiro movimentacao)
        {
            bool contaCorrenteAtiva = await _contaCorrenteRepository.ValidarContaCorrenteAtiva(movimentacao.contaCorrenteId);

            if (!contaCorrenteAtiva)
            {
                return contaCorrenteAtiva;
            }
            return contaCorrenteAtiva;
        }

        public async Task<bool> VerificarContaCorrenteCadastrada(MovimentacaoFinanceiro movimentacao)
        {
            bool contaCorrenteExiste = await _contaCorrenteRepository.ValidarContaCorrente(movimentacao.contaCorrenteId);

            if (!contaCorrenteExiste)
            {
                return contaCorrenteExiste;
            }
            return contaCorrenteExiste;
        }
    }
}

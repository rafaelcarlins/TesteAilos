using Questao1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao1.Interface
{
    public interface IBoContaCorrente
    {
        void CadastrarConta(int numero, string titular, double depositoInicial = 0);
        ContaBancaria ObterContaBancariaPorConta(int numero);
        void EfetuarDepositoSaldo(int numero, double valor);
        void EfetuarSaqueSaldo(int numero, double valor);
        void AlterarNome(int numero, string novoNome);
    }
}

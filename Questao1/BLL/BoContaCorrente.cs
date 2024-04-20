using Questao1.DAL;
using Questao1.Interface;
using Questao1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao1.BLL
{
    public  class BoContaCorrente : IBoContaCorrente
    {
        double taxaInstituicao = 3.5;
        RepositoryContaCorrente repository = new RepositoryContaCorrente();
        ContaBancaria conta = new ContaBancaria();
        public void CadastrarConta(int numero, string titular, double depositoInicial = 0)
        {
            repository.CadastrarConta(numero, titular, depositoInicial);
        }
        public ContaBancaria ObterContaBancariaPorConta(int numero)
        {
            
            conta =  repository.ObterContaBancariaPorConta(numero);
            if (conta == null)
            {
                return null;
            }
            return conta;
        }
        public void EfetuarDepositoSaldo(int numero, double valor)
        {
            repository.EfetuarDepositoSaldo(numero,valor);
        }
        public void EfetuarSaqueSaldo(int numero, double valor)
        {
            repository.EfetuarSaqueSaldo(numero, valor, taxaInstituicao);
        }
        public void AlterarNome(int numero, string novoNome)
        {
            repository.AlterarNome(numero, novoNome);   
        }
    }
}

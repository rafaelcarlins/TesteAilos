using Questao1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao1.DAL
{
    public class RepositoryContaCorrente
    {
        private static List<ContaBancaria> contas = new List<ContaBancaria>();
        public void CadastrarConta(int numero, string titular, double depositoInicial)
        {
            ContaBancaria contaBancaria = new ContaBancaria();
            contaBancaria.numero = numero;
            contaBancaria.titular = titular;
            contaBancaria.saldo = depositoInicial;
            contas.Add(contaBancaria);
        }

        public ContaBancaria ObterContaBancariaPorConta(int numero)
        {
            foreach (var conta in contas)
            {
                if (conta.numero == numero)
                {
                    return conta;
                }

            }
            return null;
        }

        public void EfetuarDepositoSaldo(int numero, double valor)
        {
            ContaBancaria conta = new ContaBancaria();
            conta = ObterContaBancariaPorConta(numero);

            conta.saldo = valor + conta.saldo;

        }
        public void EfetuarSaqueSaldo(int numero, double valor, double taxaInstituicao)
        {
            ContaBancaria conta = new ContaBancaria();
            conta = ObterContaBancariaPorConta(numero);

            conta.saldo =  conta.saldo - valor - taxaInstituicao;

        }

        public void AlterarNome(int numero, string novoNome)
        {
            ContaBancaria conta = new ContaBancaria();
            conta = ObterContaBancariaPorConta(numero);
            conta.titular = novoNome;
        }

    }
}

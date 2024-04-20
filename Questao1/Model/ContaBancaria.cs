using Questao1.BLL;
using Questao1.Interface;
using System.Globalization;

namespace Questao1.Model
{
    public class ContaBancaria
    {
        private readonly IBoContaCorrente _boConta;

        public ContaBancaria(IBoContaCorrente boConta)
        {
            _boConta = boConta;
        }
        public int numero;
        public string titular;
        public double depositoInicial;
        public double saldo;
        public ContaBancaria()
        {
        }
       
    }
}

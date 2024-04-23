using Questao5.Application.Commands.Interface;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Questao5.Domain.Entities
{
    public class MovimentacaoFinanceiro 
    {
        public string requisicaoId { get; set; }
        public int contaCorrenteId { get; set; }
        public decimal valor { get; set; }
        public string tipoMovimento { get; set; }

    }
}


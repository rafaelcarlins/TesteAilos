namespace Questao5.Application.Commands.DTO
{
    public class MovimentacaoFinanceiroDto
    {
        public string RequisicaoId { get; set; }
        public int ContaCorrenteId { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
       
    }
}

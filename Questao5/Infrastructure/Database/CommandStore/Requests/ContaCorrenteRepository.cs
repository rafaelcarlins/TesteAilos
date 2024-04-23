using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using System.Data;
using System.Data.SqlClient;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class ContaCorrenteRepository
    {
        private readonly string _connectionString;

        public ContaCorrenteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> ValidarContaCorrente(int contaCorrenteId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM contacorrente WHERE contaCorrenteId = @ContaCorrenteId";
                int count = await db.ExecuteScalarAsync<int>(sql, new { ContaCorrenteId = contaCorrenteId });
                return count > 0;
            }
        }
        public async Task<bool> ValidarContaCorrenteAtiva(int contaCorrenteId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM contacorrente WHERE contaCorrenteId = @ContaCorrenteId AND ativo = 1";
                int count = await db.ExecuteScalarAsync<int>(sql, new { ContaCorrenteId = contaCorrenteId });
                return count > 0;
            }
        }

        public async Task CadastrarMovimentoAsync(MovimentacaoFinanceiro movimentacao)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                           VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
                await db.ExecuteAsync(sql, movimentacao);
            }
        }
    }
}

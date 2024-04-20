using Microsoft.Extensions.DependencyInjection;
using Questao1.BLL;
using Questao1.Interface;
using Questao1.Model;
using System;
using System.ComponentModel.Design;
using System.Globalization;

namespace Questao1 {
    class Program {
        static ContaBancaria contaBancaria;
        static IBoContaCorrente boContaCorrente;
        static void Main(string[] args) {

            var serviceProvider = new ServiceCollection()
            .AddTransient<IBoContaCorrente, BoContaCorrente>()
            .BuildServiceProvider();

            contaBancaria = new ContaBancaria(serviceProvider.GetService<IBoContaCorrente>());
            boContaCorrente = serviceProvider.GetService<IBoContaCorrente>();
            
            Menu();
            
            /* Output expected:
            Exemplo 1:

            Entre o número da conta: 5447
            Entre o titular da conta: Milton Gonçalves
            Haverá depósito inicial(s / n) ? s
            Entre o valor de depósito inicial: 350.00

            Dados da conta:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

            Entre um valor para depósito: 200
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

            Entre um valor para saque: 199
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

            Exemplo 2:
            Entre o número da conta: 5139
            Entre o titular da conta: Elza Soares
            Haverá depósito inicial(s / n) ? n

            Dados da conta:
            Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

            Entre um valor para depósito: 300.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

            Entre um valor para saque: 298.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
            */
        }
        public static void Menu() 
        {
            string opcao = string.Empty;
            while (opcao !="1" && opcao != "2" && opcao != "4")
            {
                Console.WriteLine("Selecione uma opção");
                Console.WriteLine("1 - Cadastrar conta e saldo");
                Console.WriteLine("2 - Consultar conta e saldo");
                Console.WriteLine("4 - Sair");
                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        PrepararContaBancaria();
                        Menu();
                        break;
                    case "2":
                        ConsultarSaldoBancario();
                        Menu();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }
            }
            
            
        }
        public static void ConsultarSaldoBancario()
        {
            try
            {
                Console.Write("Entre o número da conta: ");
                int numero = int.Parse(Console.ReadLine());

                contaBancaria = boContaCorrente.ObterContaBancariaPorConta(numero);
                if (contaBancaria == null)
                {
                    Console.WriteLine("Conta inexistente ");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Dados da conta:");
                    Console.WriteLine("conta: " + contaBancaria.numero + ", Titular: " + contaBancaria.titular + ", Saldo: $" + contaBancaria.saldo);
                    Console.WriteLine();
                    Console.WriteLine("O que deseja fazer?");
                    Console.WriteLine("1 - Depositar");
                    Console.WriteLine("2 - Sacar");
                    Console.WriteLine("3 - Alterar nome");
                    Console.WriteLine("4 - Sair");

                    var opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            Depositar(contaBancaria.numero);
                            Menu();
                            break;
                        case "2":
                            Sacar(contaBancaria.numero);
                            Menu();
                            break;
                        case "3":
                            AlterarNome(contaBancaria.numero);
                            Menu();
                            return;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.Write("Erro ao entrar com informações, verifique se dados digitados estão corretos " + e.Message);
                Console.WriteLine();
                throw;
            }
            
        }
        public static void AlterarNome(int numero)
        {
            Console.WriteLine();
            Console.Write("Deseja realmente alterar no nome? ");
            char resp = char.Parse(Console.ReadLine());
            if (resp == 's' || resp == 'S')
            {
                Console.WriteLine();
                Console.Write("Informe o novo nome");
                var novoNome = Console.ReadLine();
                boContaCorrente.AlterarNome(numero, novoNome);
                Console.WriteLine();
                Console.Write("Nome alterado com sucesso");
            }
        }
        public static void Depositar(int numero)
        {
            Console.WriteLine();
            Console.Write("Entre um valor para depósito: ");
            double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            boContaCorrente.EfetuarDepositoSaldo(contaBancaria.numero, quantia);
            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine("conta: " + contaBancaria.numero + ", Titular: " + contaBancaria.titular + ", Saldo: $" + contaBancaria.saldo);
        }

        public static void Sacar(int numero)
        {
            Console.WriteLine();
            Console.Write("Entre um valor para saque: ");
            double quantia = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            boContaCorrente.EfetuarSaqueSaldo(contaBancaria.numero, quantia);
            Console.WriteLine("Dados da conta atualizados:");
            Console.WriteLine("conta: " + contaBancaria.numero + ", Titular: " + contaBancaria.titular + ", Saldo: $" + contaBancaria.saldo);
            Console.WriteLine("");
        }
        private static void PrepararContaBancaria()
        {
            int numero;
            try
            {
                string num = string.Empty;
                while (num == string.Empty  || num == string.Empty || (!int.TryParse(num, out numero)))
                {
                    Console.Write("Entre o número da conta: ");
                    num = Console.ReadLine();
                    if (num == string.Empty || (!int.TryParse(num, out numero)))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Entre somente com números");
                    }
                }

                numero = int.Parse(num);
                contaBancaria = boContaCorrente.ObterContaBancariaPorConta(numero);
                if (contaBancaria != null)
                {
                    Console.Write("Número de conta já cadastrada");
                    Console.WriteLine();
                    return;
                }
                Console.Write("Entre o titular da conta: ");
                string titular = Console.ReadLine();
                string resp = string.Empty;
                while (resp.ToUpper() != "S" && resp.ToUpper()  != "N")
                {
                    Console.Write("Haverá depósito inicial (s/n)? ");
                    resp = Console.ReadLine();
                    if (resp.ToUpper() != "S" && resp.ToUpper() != "N")
                    {
                        Console.Write("");
                        Console.WriteLine("opção inválida");
                    }
                }
                
                if (resp.ToUpper() == "S" )
                {
                    int numeroInteiro;
                    double numeroDouble;
                    
                    string vlrDep = string.Empty;
                    
                    while (vlrDep == string.Empty || !int.TryParse(vlrDep, out numeroInteiro) && !double.TryParse(vlrDep, out numeroDouble))
                    {
                        Console.Write("Entre o valor de depósito inicial: ");
                        vlrDep = Console.ReadLine();
                        if (!int.TryParse(vlrDep, out numeroInteiro) && !double.TryParse(vlrDep, out numeroDouble))
                        {
                            Console.Write("");
                            Console.WriteLine("Digite somente números");
                        }
                        
                    }

                    double depositoInicial = double.Parse(vlrDep, CultureInfo.InvariantCulture);

                    boContaCorrente.CadastrarConta(numero, titular, depositoInicial);
                    contaBancaria = boContaCorrente.ObterContaBancariaPorConta(numero);
                }
                else
                {
                    boContaCorrente.CadastrarConta(numero, titular);
                    contaBancaria = boContaCorrente.ObterContaBancariaPorConta(numero);
                }

                Console.WriteLine();
                Console.WriteLine("Dados da conta:");
                Console.WriteLine("conta: " + contaBancaria.numero + ", Titular: " + contaBancaria.titular + ", Saldo: $" + contaBancaria.saldo);
                Console.WriteLine();

            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.Write("Erro ao entrar com informações, verifique se dados digitados estão corretos " + e.Message);
                Console.WriteLine();
                throw;
            }
            
        }
    }
}
    
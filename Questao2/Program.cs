using Newtonsoft.Json;
using Questao2.Model;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.NetworkInformation;

public class Program
{
    public static async Task Main()
    {
        Filtro filtro = new Filtro();

        Console.Write("Deseja escolher um ano para pesquisa?(s/n) ");
        char resp = char.Parse(Console.ReadLine());
        if (resp == 's' || resp == 'S')
        {
            string ano = string.Empty;
            int anoDigitado;
            while (ano == string.Empty || ano == string.Empty || (!int.TryParse(ano, out anoDigitado)))
            {
                Console.WriteLine("Digite um ano para pesquisa: ");
                ano = Console.ReadLine();
                if (ano == string.Empty || (!int.TryParse(ano, out anoDigitado)))
                {
                    Console.WriteLine();
                    Console.WriteLine("Entre somente com números");
                }
            }
            filtro.ano = anoDigitado;
        }
        else
        {
            Console.WriteLine("Opção inválida");
            return;
        }

        Console.Write("Deseja escolher uma equipe para pesquisa?(s/n) ");
        resp = char.Parse(Console.ReadLine());
        if (resp == 's' || resp == 'S')
        {
            Console.WriteLine("Digite uma equipe para pesquisa: ");
            filtro.teamName1 = Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Opção inválida");
            return;
        }
        Console.Write("Deseja escolher outra equipe para pesquisa?(s/n) ");
        resp = char.Parse(Console.ReadLine());
        if (resp == 's' || resp == 'S')
        {
            Console.WriteLine("Digite a segunda equipe para pesquisa: ");
            filtro.teamName2 = Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Opção inválida");
            return;
        }
        Console.Write("Deseja escolher uma página para pesquisa?(s/n) ");
        resp = char.Parse(Console.ReadLine());
        if (resp == 's' || resp == 'S')
        {
            string pagina = string.Empty;
            int paginaDigitada;
            while (pagina == string.Empty || pagina == string.Empty || (!int.TryParse(pagina, out paginaDigitada)))
            {
                Console.WriteLine("Digite uma página para pesquisa: ");
                pagina = Console.ReadLine();
                if (pagina == string.Empty || (!int.TryParse(pagina, out paginaDigitada)))
                {
                    Console.WriteLine();
                    Console.WriteLine("Entre somente com números");
                }
            }
            filtro.pagina = paginaDigitada;
        }
        else
        {
            Console.WriteLine("Opção inválida");
            return;
        }

        int totalGoals = 0;
        string teamName = string.Empty;
        string link = await montarLinkServico(filtro.teamName1, filtro.teamName2, filtro.ano, filtro.pagina);
        List<DadosCampeonato> dados = await BuscarServico(link);
        teamName = filtro.teamName1;
        var totalGoalsTask = getTotalScoredGoalsAsync(dados, filtro.teamName1, filtro.teamName2, filtro.ano, filtro.pagina);
        if (filtro.teamName2 == string.Empty || filtro.teamName2 == null)
        {
            filtro.teamName2 = filtro.teamName1;
            filtro.teamName1 = string.Empty;
            teamName = filtro.teamName2;
            link = await montarLinkServico(filtro.teamName1, filtro.teamName2, filtro.ano, filtro.pagina);
            List<DadosCampeonato> dadosVisitante = await BuscarServico(link);
            var totalGoalsTaskVisitor = getTotalScoredGoalsAsync(dadosVisitante, filtro.teamName1, filtro.teamName2, filtro.ano, filtro.pagina);
            
            totalGoals = await totalGoalsTask + await totalGoalsTaskVisitor;
        }
        else
        {
            totalGoals = await totalGoalsTask;
        }
        if (totalGoals == 0)
        {
            Console.WriteLine("O filtro selecionado não retornou resultados");
            return;
        }
        if (filtro.teamName1 != string.Empty && filtro.teamName2 != string.Empty)
        {
            Console.WriteLine("Team1 " + filtro.teamName1 + " " + totalGoals.ToString() + " goals" + " x " +
               "Team2 " + filtro.teamName2 + " in "+ filtro.ano );
            return;
        }
        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + filtro.ano);

    }

    public static async Task<string> montarLinkServico(string team = "", string team2 = "", int year = 0, int pagina = 0)
    {
        string linkRetorno = "?";
        if (team != string.Empty && team != null)
        {
            linkRetorno += "&team1=" + team;
        }
        if (team2 != string.Empty && team2 != null) 
        {
            if (linkRetorno !="?")
            {
                linkRetorno += "&team2="+ team2;
            }
            else
            {
                linkRetorno += "team2=" + team2;
            }
        }
        if (year != 0)
        {
            if (linkRetorno != "?")
            {
                linkRetorno += "&year=" + year;
            }
            else
            {
                linkRetorno += "year=" + year;
            }
        }
        if (pagina != 0)
        {
            if (linkRetorno != "?")
            {
                linkRetorno += "&page=" + pagina;
            }
            else
            {
                linkRetorno += "page=" + pagina;
            }
            
        }

        return linkRetorno;
    }
    public static async Task<int> getTotalScoredGoalsAsync(List<DadosCampeonato> dadosCampeonato, string team = "", string team2 = "", int year = 0, int pagina = 0)
    {
        int totalGoals = 0;
        string currentTeam = string.Empty;
        if (team != string.Empty)
        {
            currentTeam = team;
            foreach (var item in dadosCampeonato)
            {
                foreach (var dado in item.data)
                {
                    if (dado.team1 == currentTeam)
                    {
                        totalGoals += int.Parse(dado.team1goals);
                    }
                }
            }
        }
        else
        {
            currentTeam = team2;
            foreach (var item in dadosCampeonato)
            {
                foreach (var dado in item.data)
                {
                    if (dado.team2 == currentTeam)
                    {
                        totalGoals += int.Parse(dado.team2goals);
                    }
                }
            }
        }
        
        return totalGoals;
    }

    public static async Task<List<DadosCampeonato>> BuscarServico(string link)
    {
        DadosCampeonato dadosCampeonatoTodasPaginas = new DadosCampeonato();
        DadosCampeonato dadosCampeonatoPaginaporPagina = new DadosCampeonato();
        List<DadosCampeonato> listaDadosCampeonato= new List<DadosCampeonato>();
        string linkPagina = string.Empty;

        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://jsonmock.hackerrank.com/api/football_matches" + link);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    dadosCampeonatoTodasPaginas =  JsonConvert.DeserializeObject<DadosCampeonato>(responseBody);
                    if (dadosCampeonatoTodasPaginas.total_pages>1)
                    {
                        for (int i = 1; i < dadosCampeonatoTodasPaginas.total_pages + 1; i++)
                        {
                            linkPagina = link + "&page=" + i;
                            dadosCampeonatoPaginaporPagina = await BuscarServicoTodasPaginas(dadosCampeonatoTodasPaginas, linkPagina);

                            listaDadosCampeonato.Add(dadosCampeonatoPaginaporPagina);
                            linkPagina = string.Empty;
                        }

                    }
                    else
                    {
                        listaDadosCampeonato.Add(dadosCampeonatoTodasPaginas);
                    }
                    
                    return listaDadosCampeonato;
                }
                else
                {
                    throw new Exception($"Erro ao fazer a solicitação: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro: {ex.Message}");
            }
        }
    }
    private static async Task<DadosCampeonato> BuscarServicoTodasPaginas(DadosCampeonato dadosCampeonatoTodasPaginas, string link)
    {
        DadosCampeonato dadosCampeonatoPaginado = new DadosCampeonato();
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://jsonmock.hackerrank.com/api/football_matches" + link);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    dadosCampeonatoTodasPaginas = JsonConvert.DeserializeObject<DadosCampeonato>(responseBody);

                    return dadosCampeonatoTodasPaginas;
                }
                else
                {
                    throw new Exception($"Erro ao fazer a solicitação: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro: {ex.Message}");
            }
        }

    }

}
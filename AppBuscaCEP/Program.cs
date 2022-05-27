using System.Net;
using System.IO;
using Newtonsoft.Json;
using AppBuscaCEP;
using RequisicoesWeb;

namespace RequisicoesWeb
{
    public class WebRequestGetExample
    {
        class Sample
        {
            public static void Main(string[] args)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine("   ------BUSCADOR------   ");
                Console.WriteLine("--------------------------");
                Console.WriteLine("");
                inicio();

            }

            private static void inicio()
            {
                Console.WriteLine("1 - Pesquisar CEP");
                Console.WriteLine("2 - Pesquisar CNPJ");
                Console.WriteLine("3 - Sair");
                var opcao = Console.ReadLine();


                switch (opcao)
                {
                    case "1":
                        PesquisaCEP();
                        break;
                     case "2":
                         PesquisaCNPJ();
                         break;
                    case "3":
                        Sair();
                        break;
                    default:
                        //Console.Clear();
                        Console.WriteLine("Opção inválida");
                        inicio();
                        break;
                }
            }
            static void PesquisaCEP()
            {

                Console.WriteLine("Digite o CEP: (apenas números)");
                string codigoPostal = "";

                codigoPostal = Console.ReadLine();
                codigoPostal = codigoPostal.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "").Replace(",", "");
                Console.WriteLine("Buscando dados...");
                Console.WriteLine("");

                if(codigoPostal.Length == 8)
                {
                    Endereco endereco = BuscarCEP(codigoPostal);
                    if (endereco.bairro == null)
                    {
                        Console.WriteLine("O CEP digitado não é válido");
                    }
                    else
                    {
                        Console.WriteLine($"{" \n Seu endereço é: "}{"\n "}{endereco.logradouro}{"; \n Bairro: "}{endereco.bairro}{"; \n Cidade: "}{endereco.localidade}{"; \n Estado: "}{endereco.uf}{";"}");
                    }
                    Console.WriteLine("--------------------");
                    Console.WriteLine("--------------------");
                    inicio();

                }
                else
                {
                    Console.WriteLine("CEP inválido");
                    Console.WriteLine("1 - Tentar novamente");
                    Console.WriteLine("2 - Voltar ao menu");
                    Console.WriteLine("3 - Sair");
                    var opcao_2 = Console.ReadLine();
                    Console.WriteLine("-------------------------");
                    switch (opcao_2)
                    {
                        case "1":
                            PesquisaCEP();
                            break;
                        case "2":
                            inicio();
                            break;
                        case "3":
                            Sair();
                            break;
                        default :
                            Console.WriteLine("Opção inválida");
                            inicio();
                            break;
                    }
                }
                

            }

            static void PesquisaCNPJ()
            {
                Console.WriteLine("Digite o CNPJ: (apenas números)");
                string cnpj = "";
                cnpj = Console.ReadLine();
                cnpj = cnpj.Replace("/", "").Replace(".", "").Replace("-", "").Replace(" ", "").Replace(",", "");

                Console.WriteLine("Buscando dados...");
                Console.WriteLine("");
                if(cnpj.Length == 14)
                {
                    PessoaJuridica pessoaJuridica = BuscarCNPJ(cnpj);

                    if (pessoaJuridica.razao_social == null)
                    {
                        Console.WriteLine("O CNPJ não é válido");
                    }
                    else
                    {
                        Console.WriteLine($"{" \n CNPJ raiz: "}{pessoaJuridica.cnpj_raiz}{"; \n Razão social: "}{pessoaJuridica.razao_social}{"; \n Capital Social: "}{pessoaJuridica.capital_social}");
                    }
                    Console.WriteLine("--------------------");
                    Console.WriteLine("--------------------");
                    inicio();

                }
                else
                {
                    Console.WriteLine("CNPJ inválido");
                    Console.WriteLine("1 - Tentar novamente");
                    Console.WriteLine("2 - Voltar ao menu");
                    Console.WriteLine("3 - Sair");
                    var opcao_3 = Console.ReadLine();
                    Console.WriteLine("-------------------------");
                    switch (opcao_3)
                    {
                        case "1":
                            PesquisaCNPJ();
                            break;
                        case "2":
                            inicio();
                            break;
                        case "3":
                            Sair();
                            break;
                        default:
                            Console.WriteLine("Opção inválida");
                            inicio();
                            break;
                    }
                }

            }
            static void Sair()
            {
                Console.WriteLine("Saindo...");
            }


        }

        //aplica uma requisição na URL
        static Endereco BuscarCEP(string codigoPostal)
        {
            Endereco endereco = new();

            try
            {
                var requisicaoWeb = WebRequest.CreateHttp($"https://viacep.com.br/ws/{codigoPostal}/json/");

                requisicaoWeb.Method = "GET";
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                //guarda os dados da web em variavel
                using (var resposta = requisicaoWeb.GetResponse())
                {
                    var streamDados = resposta.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados);
                    object objResponse = reader.ReadToEnd();
                    string retorno = objResponse.ToString();
                    ;
                    streamDados.Close();
                    resposta.Close();



                    endereco =
                    JsonConvert.DeserializeObject<Endereco>(retorno);
                }
            }
            catch
            {
                Console.WriteLine("");
            }
            return endereco;
            
        }

        //aplica uma requisição na URL
         static PessoaJuridica BuscarCNPJ(string cnpj)
        {
            PessoaJuridica pessoaJuridica = new();
            try
            {
                var requisicaoWeb = WebRequest.CreateHttp($"https://publica.cnpj.ws/cnpj/{cnpj}");

                requisicaoWeb.Method = "GET";
                requisicaoWeb.UserAgent = "RequisicaoWebDemo";

                //guarda os dados da web em variavel
                using (var resposta = requisicaoWeb.GetResponse())
                {
                    var streamDados = resposta.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados);
                    object objResponse = reader.ReadToEnd();
                    string retornar = objResponse.ToString();

                    streamDados.Close();
                    resposta.Close();


                    pessoaJuridica =
                    JsonConvert.DeserializeObject<PessoaJuridica>(retornar);
                    
                }
            }
            catch
            {
                Console.WriteLine("");
            }
            return pessoaJuridica;
        }
    }
}
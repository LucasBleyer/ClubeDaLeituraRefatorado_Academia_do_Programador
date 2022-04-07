using ClubeLeitura.ConsoleApp.ModuloAmigo;
using ClubeLeitura.ConsoleApp.ModuloCaixa;
using ClubeLeitura.ConsoleApp.ModuloCategoria;
using ClubeLeitura.ConsoleApp.ModuloEmprestimo;
using ClubeLeitura.ConsoleApp.ModuloReserva;
using ClubeLeitura.ConsoleApp.ModuloRevista;
using System;

namespace ClubeLeitura.ConsoleApp.Compartilhado
{
    public class TelaMenuPrincipal
    {
        private string opcaoSelecionada;

        private const int QUANTIDADE_REGISTROS = 10;

        private Notificador notificador;

        // Declaração de Caixas
        private RepositorioCaixa repositorioCaixa;

        private TelaCadastroCaixa telaCadastroCaixa;

        // Declaração de Categorias
        private RepositorioCategoria repositorioCategoria;

        private TelaCadastroCategoria telaCadastroCategoria;

        // Declaração de Revistas
        private RepositorioRevista repositorioRevista;

        private TelaCadastroRevista telaCadastroRevista;

        // Declaração de Amigos
        private RepositorioAmigo repositorioAmigo;
        private TelaCadastroAmigo telaCadastroAmigo;

        // Declaração de Empréstimos
        private RepositorioEmprestimo repositorioEmprestimo;

        private TelaCadastroEmprestimo telaCadastroEmprestimo;

        // Declaração de Reservas
        private RepositorioReserva repositorioReserva;

        private TelaCadastroReserva telaCadastroReserva;

        public TelaMenuPrincipal(Notificador notificador)
        {
            this.notificador = notificador;

            repositorioCaixa = new RepositorioCaixa();
            telaCadastroCaixa = new TelaCadastroCaixa(repositorioCaixa, notificador);
            repositorioCategoria = new RepositorioCategoria();
            telaCadastroCategoria = new TelaCadastroCategoria(repositorioCategoria, notificador);
            repositorioRevista = new RepositorioRevista();

            telaCadastroRevista = new TelaCadastroRevista(
                telaCadastroCategoria,
                repositorioCategoria,
                telaCadastroCaixa,
                repositorioCaixa,
                repositorioRevista,
                notificador
            );

            repositorioAmigo = new RepositorioAmigo();
            telaCadastroAmigo = new TelaCadastroAmigo(repositorioAmigo, notificador);
            repositorioEmprestimo = new RepositorioEmprestimo();

            telaCadastroEmprestimo = new TelaCadastroEmprestimo(
                notificador,
                repositorioEmprestimo,
                repositorioRevista,
                repositorioAmigo,
                telaCadastroRevista,
                telaCadastroAmigo
            );

            repositorioReserva = new RepositorioReserva();

            telaCadastroReserva = new TelaCadastroReserva(
                notificador,
                repositorioReserva,
                repositorioAmigo,
                repositorioRevista,
                telaCadastroAmigo,
                telaCadastroRevista,
                repositorioEmprestimo
            );
        }

        public string MostrarOpcoes()
        {
            Console.Clear();

            Console.WriteLine("Clube da Leitura 1.0");

            Console.WriteLine();

            Console.WriteLine("Digite 1 para Cadastrar Caixas");
            Console.WriteLine("Digite 2 para Cadastrar Categorias");
            Console.WriteLine("Digite 3 para Cadastrar Revistinhas");
            Console.WriteLine("Digite 4 para Cadastrar Amiguinhos");
            Console.WriteLine("Digite 5 para Gerenciar Empréstimos");
            Console.WriteLine("Digite 6 para Gerenciar Reservas");

            Console.WriteLine("Digite s para sair");

            opcaoSelecionada = Console.ReadLine();

            return opcaoSelecionada;
        }

        public TelaBase ObterTela()
        {
            string opcao = MostrarOpcoes();

            TelaBase tela = null;

            if (opcao == "1")
                tela = telaCadastroCaixa;

            else if (opcao == "2")
                tela = telaCadastroCategoria;

            else if (opcao == "3")
                tela = telaCadastroRevista;

            else if (opcao == "4")
                tela = telaCadastroAmigo;

            else if (opcao == "5")
                tela = telaCadastroEmprestimo;

            else if (opcao == "6")
                tela = telaCadastroReserva;

            return tela;
        }
    
    }
}
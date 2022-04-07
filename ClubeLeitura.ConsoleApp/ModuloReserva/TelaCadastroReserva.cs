using ClubeLeitura.ConsoleApp.Compartilhado;
using ClubeLeitura.ConsoleApp.ModuloAmigo;
using ClubeLeitura.ConsoleApp.ModuloEmprestimo;
using ClubeLeitura.ConsoleApp.ModuloRevista;
using System;
using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.ModuloReserva
{
    public class TelaCadastroReserva : TelaBase
    {
        private readonly Notificador notificador;
        private readonly RepositorioReserva repositorioReserva;
        private readonly RepositorioAmigo repositorioAmigo;
        private readonly RepositorioRevista repositorioRevista;
        private readonly TelaCadastroAmigo telaCadastroAmigo;
        private readonly TelaCadastroRevista telaCadastroRevista;
        private readonly RepositorioEmprestimo repositorioEmprestimo;

        public TelaCadastroReserva(
            Notificador notificador,
            RepositorioReserva repositorioReserva,
            RepositorioAmigo repositorioAmigo,
            RepositorioRevista repositorioRevista,
            TelaCadastroAmigo telaCadastroAmigo,
            TelaCadastroRevista telaCadastroRevista,
            RepositorioEmprestimo repositorioEmprestimo) : base("Cadastro de Reservas")
        {
            this.notificador = notificador;
            this.repositorioReserva = repositorioReserva;
            this.repositorioAmigo = repositorioAmigo;
            this.repositorioRevista = repositorioRevista;
            this.telaCadastroAmigo = telaCadastroAmigo;
            this.telaCadastroRevista = telaCadastroRevista;
            this.repositorioEmprestimo = repositorioEmprestimo;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Registrar Reserva");
            Console.WriteLine("Digite 2 para Visualizar");
            Console.WriteLine("Digite 3 para Visualizar Reservas em Aberto");
            Console.WriteLine("Digite 4 para Cadastrar um Empréstimo à partir de Reserva");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void RegistrarNovaReserva()
        {
            MostrarTitulo("Inserindo nova Reserva");

            // Validação do Amigo
            Amigo amigoSelecionado = ObtemAmigo();

            if (amigoSelecionado == null)
            {
                notificador.ApresentarMensagem("Nenhum amigo selecionado", TipoMensagem.Erro);
                return;
            }

            if (amigoSelecionado.TemMultaEmAberto())
            {
                notificador.ApresentarMensagem("Este amigo tem uma multa em aberto e não pode reservar.", TipoMensagem.Erro);
                return;
            }

            if (amigoSelecionado.TemReservaEmAberto())
            {
                notificador.ApresentarMensagem("Este amigo já possui uma reserva em aberto..", TipoMensagem.Erro);
                return;
            }

            if (amigoSelecionado.TemEmprestimoEmAberto())
            {
                notificador.ApresentarMensagem("Este amigo já possui uma reserva em aberto e não pode reservar.", TipoMensagem.Erro);
                return;
            }

            // Validação da Revista
            Revista revistaSelecionada = ObtemRevista();

            if (revistaSelecionada.EstaReservada())
            {
                notificador.ApresentarMensagem("A revista selecionada já está reservada!", TipoMensagem.Erro);
                return;
            }

            if (revistaSelecionada.EstaEmprestada())
            {
                notificador.ApresentarMensagem("A revista selecionada já foi emprestada.", TipoMensagem.Erro);
                return;
            }

            Reserva novaReserva = ObtemReserva(amigoSelecionado, revistaSelecionada);

            string statusValidacao = repositorioReserva.Inserir(novaReserva);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Reserva cadastrada com sucesso!", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);
        }

        public void RegistrarNovoEmprestimo()
        {
            MostrarTitulo("Registrando novo Empréstimo");

            Reserva reservaParaEmprestimo = ObtemReservaParaEmprestimo();

            reservaParaEmprestimo.Fechar();

            Emprestimo novoEmprestimo = new Emprestimo();

            novoEmprestimo.revista = reservaParaEmprestimo.revista;
            novoEmprestimo.amigo = reservaParaEmprestimo.amigo;

            string statusValidacao = repositorioEmprestimo.Inserir(novoEmprestimo);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Empréstimo cadastrado com sucesso", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);
        }

        public bool VisualizarReservas(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Reservas");

            List<EntidadeBase> reservas = repositorioReserva.SelecionarTodos();

            if (reservas.Count == 0)
                return false;

            for (int i = 0; i < reservas.Count; i++)
            {
                Reserva reserva = (Reserva)reservas[i];

                string statusReserva = reserva.estaAberta ? "Aberta" : "Fechada";

                Console.WriteLine("Número: " + reserva.numero);
                Console.WriteLine("Revista reservada: " + reserva.revista.Colecao);
                Console.WriteLine("Nome do amigo: " + reserva.amigo.Nome);
                Console.WriteLine("Data da reserva: " + reserva.dataInicialReserva.ToShortDateString());
                Console.WriteLine("Status da reserva: " + statusReserva);
                Console.WriteLine();
            }

            return true;
        }

        public bool VisualizarReservasEmAberto(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Reservas em Aberto");

            Reserva[] reservas = repositorioReserva.SelecionarReservasEmAberto();

            if (reservas.Length == 0)
                return false;

            for (int i = 0; i < reservas.Length; i++)
            {
                Reserva reserva = reservas[i];

                Console.WriteLine("Número: " + reserva.numero);
                Console.WriteLine("Revista reservada: " + reserva.revista.Colecao);
                Console.WriteLine("Nome do amigo: " + reserva.amigo.Nome);
                Console.WriteLine("Data de expiração da Reserva: " + reserva.dataInicialReserva.AddDays(2).ToShortDateString());
                Console.WriteLine();
            }

            return true;
        }

        private Reserva ObtemReserva(Amigo amigoSelecionado, Revista revistaSelecionada)
        {
            Reserva novaReserva = new Reserva(amigoSelecionado, revistaSelecionada);

            return novaReserva;
        }

        public Reserva ObtemReservaParaEmprestimo()
        {
            bool temReservasEmAberto = VisualizarReservasEmAberto("Pesquisando");

            if (!temReservasEmAberto)
            {
                notificador.ApresentarMensagem("Não há nenhuma reserva aberta disponível para cadastro.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número da reserva que irá emprestar: ");
            int numeroReserva = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Reserva reservaSelecionada = (Reserva)repositorioReserva.SelecionarRegistro(numeroReserva);

            return reservaSelecionada;
        }

        public Amigo ObtemAmigo()
        {
            bool temAmigosDisponiveis = telaCadastroAmigo.VisualizarRegistros("Pesquisando");

            if (!temAmigosDisponiveis)
            {
                notificador.ApresentarMensagem("Não há nenhum amigo disponível para cadastrar reservas.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número do amigo que irá fazer a reserva: ");
            int numeroAmigoEmprestimo = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Amigo amigoSelecionado = (Amigo)repositorioAmigo.SelecionarRegistro(numeroAmigoEmprestimo);

            return amigoSelecionado;
        }

        public Revista ObtemRevista()
        {
            bool temRevistasDisponiveis = telaCadastroRevista.VisualizarRegistros("Pesquisando");

            if (!temRevistasDisponiveis)
            {
                notificador.ApresentarMensagem("Não há nenhuma revista disponível para cadastrar reservas.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número da revista que será reservada: ");
            int numeroRevistaEmprestimo = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Revista revistaSelecionada = (Revista)repositorioRevista.SelecionarRegistro(numeroRevistaEmprestimo);

            return revistaSelecionada;
        }
    }
}

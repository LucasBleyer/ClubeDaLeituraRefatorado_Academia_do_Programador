using ClubeLeitura.ConsoleApp.Compartilhado;
using ClubeLeitura.ConsoleApp.ModuloAmigo;
using ClubeLeitura.ConsoleApp.ModuloEmprestimo;
using ClubeLeitura.ConsoleApp.ModuloReserva;

namespace ClubeLeitura.ConsoleApp
{
    internal class Program
    {
        static Notificador notificador = new Notificador();
        static TelaMenuPrincipal menuPrincipal = new TelaMenuPrincipal(notificador);

        static void Main(string[] args)
        {
            while (true)
            {
                TelaBase telaSelecionada = menuPrincipal.ObterTela();

                string opcaoSelecionada = telaSelecionada.MostrarOpcoes();

                if (telaSelecionada is ICadastroBasico)
                    GerenciarCadastroBasico(telaSelecionada, opcaoSelecionada);

                else if (telaSelecionada is TelaCadastroEmprestimo)
                    GerenciarCadastroEmprestimos(telaSelecionada, opcaoSelecionada);

                else if (telaSelecionada is TelaCadastroReserva)
                    GerenciarCadastroReservas(telaSelecionada, opcaoSelecionada);
            }
        }

        private static void GerenciarCadastroReservas(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            TelaCadastroReserva telaCadastroReserva = (TelaCadastroReserva)telaSelecionada;

            if (opcaoSelecionada == "1")
                telaCadastroReserva.RegistrarNovaReserva();

            else if (opcaoSelecionada == "2")
            {
                bool temRegistros = telaCadastroReserva.VisualizarReservas("Tela");

                if (!temRegistros)
                    notificador.ApresentarMensagem("Não há nenhuma reserva cadastrada!", TipoMensagem.Atencao);
            }
            else if (opcaoSelecionada == "3")
            {
                bool temRegistros = telaCadastroReserva.VisualizarReservasEmAberto("Tela");

                if (!temRegistros)
                    notificador.ApresentarMensagem("Não há nenhuma reserva em aberto!", TipoMensagem.Atencao);
            }
            else if (opcaoSelecionada == "4")
                telaCadastroReserva.RegistrarNovoEmprestimo();
        }

        private static void GerenciarCadastroEmprestimos(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            TelaCadastroEmprestimo telaCadastroEmprestimo = (TelaCadastroEmprestimo)telaSelecionada;

            if (opcaoSelecionada == "1")
                telaCadastroEmprestimo.RegistrarEmprestimo();
            else if (opcaoSelecionada == "2")
                telaCadastroEmprestimo.EditarEmprestimo();
            else if (opcaoSelecionada == "3")
                telaCadastroEmprestimo.ExcluirEmprestimo();
            else if (opcaoSelecionada == "4")
            {
                bool temRegistros = telaCadastroEmprestimo.VisualizarEmprestimos("Tela");

                if (!temRegistros)
                    notificador.ApresentarMensagem("Não há nenhum empréstimo cadastrado!", TipoMensagem.Atencao);
            }
            else if (opcaoSelecionada == "5")
            {
                bool temRegistros = telaCadastroEmprestimo.VisualizarEmprestimosEmAberto("Tela");

                if (!temRegistros)
                    notificador.ApresentarMensagem("Não há nenhum empréstimo em aberto!", TipoMensagem.Atencao);
            }
            else if (opcaoSelecionada == "6")
                telaCadastroEmprestimo.RegistrarDevolucao();
        }

        public static void GerenciarCadastroBasico(TelaBase telaSelecionada, string opcaoSelecionada)
        {
            ICadastroBasico telaCadastroBasico = (ICadastroBasico)telaSelecionada;

            if (opcaoSelecionada == "1")
                telaCadastroBasico.InserirRegistro();

            else if (opcaoSelecionada == "2")
                telaCadastroBasico.EditarRegistro();

            else if (opcaoSelecionada == "3")
                telaCadastroBasico.ExcluirRegistro();

            else if (opcaoSelecionada == "4")
            {
                bool temRegistros = telaCadastroBasico.VisualizarRegistros("Tela");

                if (!temRegistros)
                    notificador.ApresentarMensagem("Nenhum registro disponível!", TipoMensagem.Atencao);
            }

            if (telaSelecionada is TelaCadastroAmigo)
            {
                TelaCadastroAmigo telaCadastroAmigo = (TelaCadastroAmigo)telaSelecionada;

                if (opcaoSelecionada == "5")
                {
                    bool temRegistros = telaCadastroAmigo.VisualizarAmigosComMulta("Tela");

                    if (!temRegistros)
                        notificador.ApresentarMensagem("Não há nenhum amigo com multa aberta.", TipoMensagem.Atencao);
                }

                else if (opcaoSelecionada == "6")
                    telaCadastroAmigo.PagarMulta();
            }
        }
    }
}

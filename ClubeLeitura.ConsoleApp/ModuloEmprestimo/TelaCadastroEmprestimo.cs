using ClubeLeitura.ConsoleApp.Compartilhado;
using ClubeLeitura.ConsoleApp.ModuloAmigo;
using ClubeLeitura.ConsoleApp.ModuloRevista;
using System;
using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.ModuloEmprestimo
{
    public class TelaCadastroEmprestimo : TelaBase
    {
        private readonly Notificador notificador;
        private readonly RepositorioEmprestimo repositorioEmprestimo;
        private readonly RepositorioRevista repositorioRevista;
        private readonly RepositorioAmigo repositorioAmigo;
        private readonly TelaCadastroRevista telaCadastroRevista;
        private readonly TelaCadastroAmigo telaCadastroAmigo;

        public TelaCadastroEmprestimo(
            Notificador notificador,
            RepositorioEmprestimo repositorioEmprestimo,
            RepositorioRevista repositorioRevista,
            RepositorioAmigo repositorioAmigo,
            TelaCadastroRevista telaCadastroRevista,
            TelaCadastroAmigo telaCadastroAmigo) : base("Cadastro de Empréstimos")
        {
            this.notificador = notificador;
            this.repositorioEmprestimo = repositorioEmprestimo;
            this.repositorioRevista = repositorioRevista;
            this.repositorioAmigo = repositorioAmigo;
            this.telaCadastroRevista = telaCadastroRevista;
            this.telaCadastroAmigo = telaCadastroAmigo;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Registrar Empréstimo");
            Console.WriteLine("Digite 2 para Editar Empréstimo");
            Console.WriteLine("Digite 3 para Excluir Empréstimo");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Visualizar Empréstimos em Aberto");
            Console.WriteLine("Digite 6 para Devolver um empréstimo");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void RegistrarEmprestimo()
        {
            MostrarTitulo("Inserindo novo Empréstimo");

            // Validação do Amigo
            Amigo amigoSelecionado = ObtemAmigo();

            if (amigoSelecionado == null)
            {
                notificador.ApresentarMensagem("Nenhum amigo selecionado", TipoMensagem.Erro);
                return;
            }

            if (amigoSelecionado.TemMultaEmAberto())
            {
                notificador.ApresentarMensagem("Este amigo tem uma multa em aberto.", TipoMensagem.Erro);
                return;
            }

            if (amigoSelecionado.TemEmprestimoEmAberto())
            {
                notificador.ApresentarMensagem("Este amigo já tem um empréstimo em aberto.", TipoMensagem.Erro);
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

            Emprestimo emprestimo = ObtemEmprestimo(amigoSelecionado, revistaSelecionada);
            
            string statusValidacao = repositorioEmprestimo.Inserir(emprestimo);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Empréstimo cadastrado com sucesso!", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);
        }

        public void RegistrarDevolucao()
        {
            MostrarTitulo("Devolvendo Empréstimo");

            bool temEmprestimos = VisualizarEmprestimosEmAberto("Pesquisando");

            if (!temEmprestimos)
            {
                notificador.ApresentarMensagem("Nenhum empréstimo disponível para devolução.", TipoMensagem.Atencao);
                return;
            }

            int numeroEmprestimo = ObterNumeroEmprestimo();

            Emprestimo emprestimoParaDevolver = (Emprestimo)repositorioEmprestimo.SelecionarRegistro(numeroEmprestimo);

            if (!emprestimoParaDevolver.estaAberto)
            {
                notificador.ApresentarMensagem("O empréstimo selecionado não está mais aberto.", TipoMensagem.Atencao);
                return;
            }

            repositorioEmprestimo.RegistrarDevolucao(emprestimoParaDevolver);

            if (emprestimoParaDevolver.amigo.TemMultaEmAberto())
            {
                decimal multa = emprestimoParaDevolver.amigo.multa.Valor;

                notificador.ApresentarMensagem($"A devolução está atrasada, uma multa de R${multa} foi incluída.", TipoMensagem.Atencao);
            }

            notificador.ApresentarMensagem("Devolução concluída com sucesso!", TipoMensagem.Sucesso);
        }

        public void EditarEmprestimo()
        {
            MostrarTitulo("Editando Empréstimos");

            bool temEmprestimosCadastrados = VisualizarEmprestimos("Pesquisando");

            if (temEmprestimosCadastrados == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhuma empréstimo cadastrado para poder editar", TipoMensagem.Atencao);
                return;
            }
            int numeroEmprestimo = ObterNumeroEmprestimo();

            Amigo amigoSelecionado = ObtemAmigo();

            Revista revistaSelecionada = ObtemRevista();

            Emprestimo emprestimoAtualizado = ObtemEmprestimo(amigoSelecionado, revistaSelecionada);

            repositorioEmprestimo.Editar(numeroEmprestimo, emprestimoAtualizado);

            notificador.ApresentarMensagem("Empréstimo editado com sucesso", TipoMensagem.Sucesso);
        }

        public void ExcluirEmprestimo()
        {
            MostrarTitulo("Excluindo Empréstimo");

            bool temEmprestimosCadastrados = VisualizarEmprestimos("Pesquisando");

            if (temEmprestimosCadastrados == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhuma empréstimo cadastrado para poder excluir", TipoMensagem.Atencao);
                return;
            }

            int numeroEmprestimo = ObterNumeroEmprestimo();

            repositorioEmprestimo.Excluir(numeroEmprestimo);

            notificador.ApresentarMensagem("Revista excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarEmprestimos(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Empréstimos");

            List<EntidadeBase> emprestimos = repositorioEmprestimo.SelecionarTodos();

            if (emprestimos.Count == 0)
                return false;

            for (int i = 0; i < emprestimos.Count; i++)
            {
                Emprestimo emprestimo = (Emprestimo)emprestimos[i];

                string statusEmprestimo = emprestimo.estaAberto ? "Aberto" : "Fechado";

                Console.WriteLine("Número: " + emprestimo.numero);
                Console.WriteLine("Revista emprestada: " + emprestimo.revista.Colecao);
                Console.WriteLine("Nome do amigo: " + emprestimo.amigo.Nome);
                Console.WriteLine("Data do empréstimo: " + emprestimo.dataEmprestimo);
                Console.WriteLine("Status do empréstimo: " + statusEmprestimo);
                Console.WriteLine();
            }

            return true;
        }

        public bool VisualizarEmprestimosEmAberto(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Empréstimos em Aberto");

            List<Emprestimo> emprestimos = repositorioEmprestimo.SelecionarEmprestimosAbertos();

            if (emprestimos.Count == 0)
                return false;

            for (int i = 0; i < emprestimos.Count; i++)
            {
                Emprestimo emprestimo = emprestimos[i];

                Console.WriteLine("Número: " + emprestimo.numero);
                Console.WriteLine("Revista emprestada: " + emprestimo.revista.Colecao);
                Console.WriteLine("Nome do amigo: " + emprestimo.amigo.Nome);
                Console.WriteLine("Data do empréstimo: " + emprestimo.dataEmprestimo);
                Console.WriteLine();
            }

            return true;
        }

        #region Métodos privados
        private Emprestimo ObtemEmprestimo(Amigo amigo, Revista revista)
        {
            Emprestimo novoEmprestimo = new Emprestimo();

            novoEmprestimo.amigo = amigo;
            novoEmprestimo.revista = revista;

            return novoEmprestimo;
        }

        private Amigo ObtemAmigo()
        {
            bool temAmigosDisponiveis = telaCadastroAmigo.VisualizarRegistros("Pesquisando");

            if (!temAmigosDisponiveis)
            {
                notificador.ApresentarMensagem("Não há nenhum amigo disponível para cadastrar empréstimos.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número do amigo que irá pegar o empréstimo: ");
            int numeroAmigoEmprestimo = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Amigo amigoSelecionado = (Amigo)repositorioAmigo.SelecionarRegistro(numeroAmigoEmprestimo);

            return amigoSelecionado;
        }

        private Revista ObtemRevista()
        {
            bool temRevistasDisponiveis = telaCadastroRevista.VisualizarRegistros("Pesquisando");

            if (!temRevistasDisponiveis)
            {
                notificador.ApresentarMensagem("Não há nenhuma revista disponível para cadastrar empréstimos.", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número da revista que irá será emprestada: ");
            int numeroRevistaEmprestimo = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Revista revistaSelecionada = (Revista)repositorioRevista.SelecionarRegistro(numeroRevistaEmprestimo);

            return revistaSelecionada;
        }

        private int ObterNumeroEmprestimo()
        {
            int numeroEmprestimo;
            bool numeroEmprestimoEncontrado;

            do
            {
                Console.Write("Digite o número do empréstimo que deseja selecionar: ");
                numeroEmprestimo = Convert.ToInt32(Console.ReadLine());

                numeroEmprestimoEncontrado = repositorioEmprestimo.ExisteRegistro(numeroEmprestimo);

                if (numeroEmprestimoEncontrado == false)
                    notificador.ApresentarMensagem("Número de empréstimo não encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroEmprestimoEncontrado == false);
            return numeroEmprestimo;
        }
        #endregion
    }
}

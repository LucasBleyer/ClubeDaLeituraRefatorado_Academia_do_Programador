using ClubeLeitura.ConsoleApp.Compartilhado;
using System;
using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.ModuloAmigo
{
    public class TelaCadastroAmigo : TelaBase, ICadastroBasico
    {
        private readonly Notificador notificador;
        private readonly RepositorioAmigo repositorioAmigo;

        public TelaCadastroAmigo(RepositorioAmigo repositorioAmigo, Notificador notificador)
            : base ("Cadastro de Amigos")
        {
            this.repositorioAmigo = repositorioAmigo;
            this.notificador = notificador;
        }

        public override string MostrarOpcoes()
        {
            MostrarTitulo(Titulo);

            Console.WriteLine("Digite 1 para Inserir");
            Console.WriteLine("Digite 2 para Editar");
            Console.WriteLine("Digite 3 para Excluir");
            Console.WriteLine("Digite 4 para Visualizar");
            Console.WriteLine("Digite 5 para Visualizar Amigos com Multa");
            Console.WriteLine("Digite 6 para Pagar Multas");

            Console.WriteLine("Digite s para sair");

            string opcao = Console.ReadLine();

            return opcao;
        }

        public void PagarMulta()
        {
            MostrarTitulo("Pagamento de Multas");

            bool temAmigosComMulta = VisualizarAmigosComMulta("Pesquisando");

            if (!temAmigosComMulta)
            {
                notificador.ApresentarMensagem("Não há nenhum amigo com multas em aberto", TipoMensagem.Atencao);
                return;
            }

            int numeroAmigoComMulta = ObterNumeroAmigo();

            Amigo amigoComMulta = (Amigo)repositorioAmigo.SelecionarRegistro(numeroAmigoComMulta);

            amigoComMulta.PagarMulta();
        }

        public void InserirRegistro()
        {
            MostrarTitulo("Inserindo novo amigo");

            Amigo novoAmigo = ObterAmigo();

            string statusValidacao = repositorioAmigo.Inserir(novoAmigo);

            if (statusValidacao == "REGISTRO_VALIDO")
                notificador.ApresentarMensagem("Amigo cadastrado com sucesso!", TipoMensagem.Sucesso);
            else
                notificador.ApresentarMensagem(statusValidacao, TipoMensagem.Erro);
        }

        public void EditarRegistro()
        {
            MostrarTitulo("Editando Amigo");

            bool temAmigosCadastrados = VisualizarRegistros("Pesquisando");

            if (temAmigosCadastrados == false)
            {
                notificador.ApresentarMensagem("Nenhum amigo cadastrado para poder editar.", TipoMensagem.Atencao);
                return;
            }

            int numeroAmigo = ObterNumeroAmigo();

            Amigo amigoAtualizado = ObterAmigo();

            repositorioAmigo.Editar(numeroAmigo, amigoAtualizado);

            notificador.ApresentarMensagem("Amigo editado com sucesso", TipoMensagem.Sucesso);
        }

        public void ExcluirRegistro()
        {
            MostrarTitulo("Excluindo Amigo");

            bool temAmigosCadastrados = VisualizarRegistros("Pesquisando");

            if (temAmigosCadastrados == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhum amigo cadastrado para poder excluir", TipoMensagem.Atencao);
                return;
            }

            int numeroAmigo = ObterNumeroAmigo();

            repositorioAmigo.Excluir(numeroAmigo);

            notificador.ApresentarMensagem("Amigo excluído com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Amigos");

            List<EntidadeBase> amigos = repositorioAmigo.SelecionarTodos();

            if (amigos.Count == 0)
                return false;

            for (int i = 0; i < amigos.Count; i++)
            {
                Amigo a = (Amigo)amigos[i];

                Console.WriteLine("Número: " + a.numero);
                Console.WriteLine("Nome: " + a.Nome);
                Console.WriteLine("Nome do responsável: " + a.NomeResponsavel);
                Console.WriteLine("Onde mora: " + a.Endereco);

                Console.WriteLine();
            }

            return true;
        }

        public bool VisualizarAmigosComMulta(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Amigos com Multa");

            List<Amigo> amigos = repositorioAmigo.SelecionarAmigosComMulta();

            if (amigos.Count == 0)
                return false;

            for (int i = 0; i < amigos.Count; i++)
            {
                Amigo a = amigos[i];

                Console.WriteLine("Número: " + a.numero);
                Console.WriteLine("Nome: " + a.Nome);
                Console.WriteLine("Nome do responsável: " + a.NomeResponsavel);
                Console.WriteLine("Onde mora: " + a.Endereco);
                Console.WriteLine("Multa: R$" + a.multa.Valor);

                Console.WriteLine();
            }

            return true;
        }

        #region Métodos privados
        private int ObterNumeroAmigo()
        {
            int numeroAmigo;
            bool numeroAmigoEncontrado;

            do
            {
                Console.Write("Digite o número do amigo que deseja selecionar: ");
                numeroAmigo = Convert.ToInt32(Console.ReadLine());

                numeroAmigoEncontrado = repositorioAmigo.ExisteRegistro(numeroAmigo);

                if (numeroAmigoEncontrado == false)
                    notificador.ApresentarMensagem("Número do amigo não encontrado, digite novamente.", TipoMensagem.Atencao);

            } while (numeroAmigoEncontrado == false);
            return numeroAmigo;
        }

        private Amigo ObterAmigo()
        {
            Console.Write("Digite o nome do amigo: ");
            string nome = Console.ReadLine();

            Console.Write("Digite o nome do responsável: ");
            string nomeResponsavel = Console.ReadLine();

            Console.Write("Digite o número do telefone: ");
            string telefone = Console.ReadLine();

            Console.Write("Digite onde o amigo mora: ");
            string endereco = Console.ReadLine();

            Amigo amigo = new Amigo(nome, nomeResponsavel, telefone, endereco);

            return amigo;
        }
        #endregion
    }
}

using ClubeLeitura.ConsoleApp.Compartilhado;
using ClubeLeitura.ConsoleApp.ModuloCaixa;
using ClubeLeitura.ConsoleApp.ModuloCategoria;
using System;
using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.ModuloRevista
{
    public class TelaCadastroRevista : TelaBase, ICadastroBasico
    {
        private readonly TelaCadastroCategoria telaCadastroCategoria;
        private readonly RepositorioCategoria repositorioCategoria;
        private readonly TelaCadastroCaixa telaCadastroCaixa;
        private readonly RepositorioCaixa repositorioCaixa;
        private readonly RepositorioRevista repositorioRevista;
        private readonly Notificador notificador;

        public TelaCadastroRevista(
            TelaCadastroCategoria telaCadastroCategoria,
            RepositorioCategoria repositorioCategoria,
            TelaCadastroCaixa telaCadastroCaixa,
            RepositorioCaixa repositorioCaixa,
            RepositorioRevista repositorioRevista,
            Notificador notificador) : base("Cadastro de Revistas")
        {
            this.telaCadastroCategoria = telaCadastroCategoria;
            this.repositorioCategoria = repositorioCategoria;
            this.telaCadastroCaixa = telaCadastroCaixa;
            this.repositorioCaixa = repositorioCaixa;
            this.repositorioRevista = repositorioRevista;
            this.notificador = notificador;
        }

        public void InserirRegistro()
        {
            MostrarTitulo("Inserindo nova revista");

            Caixa caixaSelecionada = ObtemCaixa();

            Categoria categoriaSelecionada = ObtemCategoria();

            if (caixaSelecionada == null || categoriaSelecionada == null)
            {
                notificador
                    .ApresentarMensagem("Cadastre uma caixa e uma categoria antes de cadastrar revistas!", TipoMensagem.Atencao);
                return;
            }

            Revista novaRevista = ObterRevista();

            novaRevista.caixa = caixaSelecionada;
            novaRevista.categoria = categoriaSelecionada;

            repositorioRevista.Inserir(novaRevista);

            notificador.ApresentarMensagem("Revista inserida com sucesso", TipoMensagem.Sucesso);
        }

        public void EditarRegistro()
        {
            MostrarTitulo("Editando Revista");

            bool temRevistasCadastradas = VisualizarRegistros("Pesquisando");

            if (temRevistasCadastradas == false)
            {
                notificador.ApresentarMensagem("Nenhuma revista cadastrada para poder editar", TipoMensagem.Atencao);
                return;
            }

            int numeroRevista = ObterNumeroRevista();

            Console.WriteLine();
            
            Caixa caixaSelecionada = ObtemCaixa();

            Revista revistaAtualizada = ObterRevista();

            revistaAtualizada.caixa = caixaSelecionada;

            repositorioRevista.Editar(numeroRevista, revistaAtualizada);

            notificador.ApresentarMensagem("Revista editada com sucesso", TipoMensagem.Sucesso);
        }

        public void ExcluirRegistro()
        {
            MostrarTitulo("Excluindo Revista");

            bool temRevistasCadastradas = VisualizarRegistros("Pesquisando");

            if (temRevistasCadastradas == false)
            {
                notificador.ApresentarMensagem(
                    "Nenhuma revista cadastrada para poder excluir", TipoMensagem.Atencao);
                return;
            }

            int numeroRevista = ObterNumeroRevista();

            repositorioRevista.Excluir(numeroRevista);

            notificador.ApresentarMensagem("Revista excluída com sucesso", TipoMensagem.Sucesso);
        }

        public bool VisualizarRegistros(string tipo)
        {
            if (tipo == "Tela")
                MostrarTitulo("Visualização de Revistas");

            List<EntidadeBase> revistas = repositorioRevista.SelecionarTodos();

            if (revistas.Count == 0)
                return false;

            for (int i = 0; i < revistas.Count; i++)
            {
                Revista revista = (Revista)revistas[i];

                Console.WriteLine("Número: " + revista.numero);
                Console.WriteLine("Categoria: " + revista.categoria.Nome);
                Console.WriteLine("Coleção: " + revista.Colecao);
                Console.WriteLine("Edição: " + revista.Edicao);
                Console.WriteLine("Ano: " + revista.Ano);
                Console.WriteLine("Caixa que está guardada: " + revista.caixa.Cor);

                Console.WriteLine();
            }

            return true;
        }

        #region Métodos privados
        private Revista ObterRevista()
        {
            Console.Write("Digite a coleção da revista: ");
            string colecao = Console.ReadLine();

            Console.Write("Digite a edição da revista: ");
            int edicao = Convert.ToInt32(Console.ReadLine());
            
            Console.Write("Digite o ano da revista: ");
            int ano = Convert.ToInt32(Console.ReadLine());

            Revista novaRevista = new Revista(colecao, edicao, ano);

            return novaRevista;
        }

        private Categoria ObtemCategoria()
        {
            bool temCategoriasDisponiveis = telaCadastroCategoria.VisualizarRegistros("");

            if (!temCategoriasDisponiveis)
            {
                notificador.ApresentarMensagem("Você precisa cadastrar uma categoria antes de uma revista!", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número da categoria da revista: ");
            int numCategoriaSelecionada = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Categoria categoriaSelecionada = (Categoria)repositorioCategoria.SelecionarRegistro(numCategoriaSelecionada);

            return categoriaSelecionada;
        }

        private Caixa ObtemCaixa()
        {
            bool temCaixasDisponiveis = telaCadastroCaixa.VisualizarRegistros("");

            if (!temCaixasDisponiveis)
            {
                notificador.ApresentarMensagem("Não há nenhuma caixa disponível para cadastrar revistas", TipoMensagem.Atencao);
                return null;
            }

            Console.Write("Digite o número da caixa que irá inserir: ");
            int numCaixaSelecionada = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            Caixa caixaSelecionada = (Caixa)repositorioCaixa.SelecionarRegistro(numCaixaSelecionada);

            return caixaSelecionada;
        }

        private int ObterNumeroRevista()
        {
            int numeroRevista;
            bool numeroRevistaEncontrado;

            do
            {
                Console.Write("Digite o número da revista que deseja selecionar: ");
                numeroRevista = Convert.ToInt32(Console.ReadLine());

                numeroRevistaEncontrado = repositorioRevista.ExisteRegistro(numeroRevista);

                if (numeroRevistaEncontrado == false)
                    notificador.ApresentarMensagem("Número de revista não encontrado, digite novamente", TipoMensagem.Atencao);

            } while (numeroRevistaEncontrado == false);

            return numeroRevista;
        }
        #endregion
    }
}

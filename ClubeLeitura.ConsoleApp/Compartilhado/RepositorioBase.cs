using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.Compartilhado
{
    public class RepositorioBase
    {
        protected readonly List<EntidadeBase> registros;
        protected int contadorNumero;

        public RepositorioBase()
        {
            registros = new List<EntidadeBase>();
        }

        public virtual string Inserir(EntidadeBase entidade)
        {
            entidade.numero = ++contadorNumero;

            registros.Add(entidade);

            return "REGISTRO_VALIDO";
        }

        public void Editar(int numeroSelecionado, EntidadeBase entidade)
        {
            for (int i = 0; i < registros.Count; i++)
            {
                if (registros[i].numero == numeroSelecionado)
                {
                    entidade.numero = numeroSelecionado;
                    registros[i] = entidade;

                    break;
                }
            }
        }

        public bool Excluir(int numeroSelecionado)
        {
            EntidadeBase entidadeSelecionada = SelecionarRegistro(numeroSelecionado);

            if (entidadeSelecionada == null)
                return false;

            registros.Remove(entidadeSelecionada);

            return true;
        }

        public EntidadeBase SelecionarRegistro(int numeroRegistro)
        {
            foreach (EntidadeBase registro in registros)
                if (numeroRegistro == registro.numero)
                    return registro;

            return null;
        }

        public List<EntidadeBase> SelecionarTodos()
        {
            return registros;
        }

        public bool ExisteRegistro(int numeroRegistro)
        {
            foreach (EntidadeBase registro in registros)
                if (registro.numero == numeroRegistro)
                    return true;

            return false;
        }
    }
}

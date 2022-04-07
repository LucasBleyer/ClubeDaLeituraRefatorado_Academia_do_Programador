using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.Compartilhado
{
    public class RepositorioBase<T> where T : EntidadeBase
    {
        protected readonly List<T> registros = new List<T>();
        protected int contadorNumero;

        public RepositorioBase()
        {
            registros = new List<T>();
        }

        public virtual string Inserir(T entidade)
        {
            registros.Add(entidade);
            return "Inserido com Sucesso!";
        }

        public void Editar(int numeroSelecionado, EntidadeBase entidade)
        {
            registros.Remove(registros.Find(x => x.numero == numeroSelecionado));

        }
        public void Excluir(int numeroSelecionado)
        {
            registros.RemoveAt(numeroSelecionado);
        }

        public EntidadeBase SelecionarRegistro(int numeroRegistro)
        {
            return registros.Find(x => x.numero == numeroRegistro);
        }

        public List<T> SelecionarTodos()
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

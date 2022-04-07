using ClubeLeitura.ConsoleApp.Compartilhado;
using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.ModuloEmprestimo
{
    public class RepositorioEmprestimo : RepositorioBase<Emprestimo>
    {
        public bool RegistrarDevolucao(Emprestimo emprestimo)
        {
            emprestimo.Fechar();

            return true;
        }

        public List<Emprestimo> SelecionarEmprestimosAbertos()
        {
            return registros.FindAll(x => x.estaAberto);
        }
    }
}

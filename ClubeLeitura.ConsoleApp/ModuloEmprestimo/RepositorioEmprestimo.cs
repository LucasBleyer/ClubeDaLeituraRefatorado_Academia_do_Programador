using ClubeLeitura.ConsoleApp.Compartilhado;
using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.ModuloEmprestimo
{
    public class RepositorioEmprestimo : RepositorioBase
    {
        public RepositorioEmprestimo()
        {
        }

        public override string Inserir(EntidadeBase e)
        {
            Emprestimo emprestimo = (Emprestimo)e;

            emprestimo.numero = ++contadorNumero;

            emprestimo.Abrir();

            emprestimo.revista.RegistrarEmprestimo(emprestimo);
            emprestimo.amigo.RegistrarEmprestimo(emprestimo);

            registros.Add(e);

            return "REGISTRO_VALIDO";
        }

        public bool RegistrarDevolucao(Emprestimo emprestimo)
        {
            emprestimo.Fechar();

            return true;
        }

        public List<Emprestimo> SelecionarEmprestimosAbertos()
        {
            List<Emprestimo> emprestimosAbertos = new List<Emprestimo>();

            foreach (Emprestimo emprestimo in registros)
            {
                if (emprestimo.estaAberto)
                    emprestimosAbertos.Add(emprestimo);
            }

            return emprestimosAbertos;
        }
    }
}

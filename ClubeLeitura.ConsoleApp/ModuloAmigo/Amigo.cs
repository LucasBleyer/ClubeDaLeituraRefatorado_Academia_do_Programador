using ClubeLeitura.ConsoleApp.Compartilhado;
using ClubeLeitura.ConsoleApp.ModuloEmprestimo;
using ClubeLeitura.ConsoleApp.ModuloReserva;

namespace ClubeLeitura.ConsoleApp.ModuloAmigo
{
    public class Amigo : EntidadeBase
    {
        private readonly string nome;
        private readonly string nomeResponsavel;
        private readonly string telefone;
        private readonly string endereco;
        public Multa multa;

        private readonly Emprestimo[] historicoEmprestimos = new Emprestimo[10];
        private readonly Reserva[] historicoReservas = new Reserva[10];

        public string Nome => nome;

        public string NomeResponsavel => nomeResponsavel;

        public string Telefone => telefone;

        public string Endereco => endereco;

        public Amigo(string nome, string nomeResponsavel, string telefone, string endereco)
        {
            this.nome = nome;
            this.nomeResponsavel = nomeResponsavel;
            this.telefone = telefone;
            this.endereco = endereco;
        }

        public void RegistrarEmprestimo(Emprestimo emprestimo)
        {
            historicoEmprestimos[ObtemPosicaoVazia()] = emprestimo;
        }

        public void RegistrarReserva(Reserva reserva)
        {
            historicoReservas[ObtemPosicaoReservasVazia()] = reserva;
        }

        public bool TemReservaEmAberto()
        {
            bool temReservaEmAberto = false;

            foreach (Reserva reserva in historicoReservas)
            {
                if (reserva != null && reserva.estaAberta)
                {
                    temReservaEmAberto = true;
                    break;
                }
            }

            return temReservaEmAberto;
        }

        public bool TemEmprestimoEmAberto()
        {
            bool temEmprestimoEmAberto = false;

            foreach (Emprestimo emprestimo in historicoEmprestimos)
            {
                if (emprestimo != null && emprestimo.estaAberto)
                {
                    temEmprestimoEmAberto = true;
                    break;
                }
            }
            return temEmprestimoEmAberto;
        }

        public void RegistrarMulta(decimal valor)
        {
            Multa novaMulta = new Multa(valor);

            multa = novaMulta;
        }

        public void PagarMulta()
        {
            if (multa != null)
                multa = null;
        }

        public bool TemMultaEmAberto()
        {
            if (multa == null)
                return false;

            return true;
        }

        #region Métodos privados
        private int ObtemPosicaoVazia()
        {
            for (int i = 0; i < historicoEmprestimos.Length; i++)
            {
                if (historicoEmprestimos[i] == null)
                    return i;
            }

            return -1;
        }

        private int ObtemPosicaoReservasVazia()
        {
            for (int i = 0; i < historicoReservas.Length; i++)
            {
                if (historicoReservas[i] == null)
                    return i;
            }

            return -1;
        }

        public override string Validar()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
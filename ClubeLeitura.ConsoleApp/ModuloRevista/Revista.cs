using ClubeLeitura.ConsoleApp.Compartilhado;
using ClubeLeitura.ConsoleApp.ModuloCaixa;
using ClubeLeitura.ConsoleApp.ModuloCategoria;
using ClubeLeitura.ConsoleApp.ModuloEmprestimo;
using ClubeLeitura.ConsoleApp.ModuloReserva;
using System;

namespace ClubeLeitura.ConsoleApp.ModuloRevista
{
    public class Revista : EntidadeBase
    {
        private readonly string colecao;
        private readonly int edicao;
        private readonly int ano;
        public Caixa caixa;
        public Categoria categoria;

        public Emprestimo[] historicoEmprestimos = new Emprestimo[10];
        public Reserva[] historicoReservas = new Reserva[10];

        public string Colecao => colecao;

        public int Edicao => edicao;

        public int Ano => ano;

        public Revista(string colecao, int edicao, int ano)
        {
            this.colecao = colecao;
            this.edicao = edicao;
            this.ano = ano;
        }
        public override string Validar()
        {
            string validacao = "";

            if (string.IsNullOrEmpty(Colecao))
                validacao += "É necessário incluir uma coleção!\n";

            if (Edicao < 0)
                validacao += "A edição de uma revista não pode ser menor que zero!\n";

            if (Ano < 0 || Ano > DateTime.Now.Year)
                validacao += "O ano da revista precisa ser válido!\n";

            if (string.IsNullOrEmpty(validacao))
                return "REGISTRO_VALIDO";

            return validacao;
        }

        public void RegistrarReserva(Reserva reserva)
        {
            historicoReservas[ObtemPosicaoReservasVazia()] = reserva;
        }

        public void RegistrarEmprestimo(Emprestimo emprestimo)
        {
            historicoEmprestimos[ObtemPosicaoEmprestimosVazia()] = emprestimo;
        }

        public bool EstaReservada()
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

        public bool EstaEmprestada()
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


        public int ObtemPosicaoReservasVazia()
        {
            for (int i = 0; i < historicoReservas.Length; i++)
            {
                if (historicoReservas[i] == null)
                    return i;
            }

            return -1;
        }

        public int ObtemPosicaoEmprestimosVazia()
        {
            for (int i = 0; i < historicoEmprestimos.Length; i++)
            {
                if (historicoEmprestimos[i] == null)
                    return i;
            }

            return -1;
        }
    }
}
using ClubeLeitura.ConsoleApp.Compartilhado;
using ClubeLeitura.ConsoleApp.ModuloAmigo;
using ClubeLeitura.ConsoleApp.ModuloRevista;
using System;

namespace ClubeLeitura.ConsoleApp.ModuloReserva
{
    public class Reserva : EntidadeBase
    {
        public Amigo amigo;
        public Revista revista;
        public DateTime dataInicialReserva;
        public bool estaAberta;

        public Reserva(Amigo amigo, Revista revista)
        {
            this.amigo = amigo;
            this.revista = revista;
        }

        public void Abrir()
        {
            if (!estaAberta)
            {
                estaAberta = true;
                dataInicialReserva = DateTime.Today;

                amigo.RegistrarReserva(this);
                revista.RegistrarReserva(this);
            }
        }

        public void Fechar()
        {
            if (estaAberta)
                estaAberta = false;
        }

        public bool EstaExpirada()
        {
            bool ultrapassouDataReserva = dataInicialReserva.AddDays(2) > DateTime.Today;

            if (ultrapassouDataReserva)
                estaAberta = false;

            return ultrapassouDataReserva;
        }

        public override string Validar()
        {
            throw new NotImplementedException();
        }
    }
}

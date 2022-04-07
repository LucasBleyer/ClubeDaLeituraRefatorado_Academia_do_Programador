using ClubeLeitura.ConsoleApp.Compartilhado;
using System.Collections.Generic;

namespace ClubeLeitura.ConsoleApp.ModuloReserva
{
    public class RepositorioReserva : RepositorioBase<Reserva>
    {
        public Reserva[] SelecionarReservasEmAberto()
        {
            List<Reserva> reservasInseridas = new List<Reserva>();

            foreach (Reserva reserva in registros)
                if (reserva.estaAberta)
                    reservasInseridas.Add(reserva);

            return reservasInseridas.ToArray();
        }
    }
}

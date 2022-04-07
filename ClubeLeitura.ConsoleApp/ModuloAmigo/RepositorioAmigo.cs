using ClubeLeitura.ConsoleApp.Compartilhado;
using System.Collections.Generic;
using ClubeLeitura.ConsoleApp.ModuloAmigo;

namespace ClubeLeitura.ConsoleApp.ModuloAmigo
{
    public class RepositorioAmigo : RepositorioBase<Amigo>
    {
        public List<Amigo> SelecionarAmigosComMulta()
        {
            List<Amigo> amigosComMulta = new List<Amigo>();

            foreach (Amigo amigo in registros)
                if (amigo.TemMultaEmAberto())
                    amigosComMulta.Add(amigo);

            return amigosComMulta;
        }
    }
}

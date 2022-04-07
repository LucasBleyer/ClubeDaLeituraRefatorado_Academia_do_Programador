namespace ClubeLeitura.ConsoleApp.ModuloAmigo
{
    public class Multa
    {
        private readonly decimal valor;

        public decimal Valor => valor;

        public Multa(decimal valor)
        {
            this.valor = valor;
        }

    }
}

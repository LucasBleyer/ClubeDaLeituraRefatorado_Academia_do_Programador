namespace ClubeLeitura.ConsoleApp.Compartilhado
{
    public class ResultadoValidacao
    {
        public StatusValidacao status;
        public string[] erros;

        public ResultadoValidacao(StatusValidacao status, string[] erros)
        {
            this.status = status;
            this.erros = erros;
        }

        public override string ToString()
        {
            string strErros = "";

            for (int i = 0; i < erros.Length; i++)
            {
                if (erros[i] != "")
                    strErros += erros[i] + "\n";
            }

            return strErros;
        }
    }
}

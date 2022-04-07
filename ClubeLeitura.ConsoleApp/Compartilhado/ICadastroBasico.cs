namespace ClubeLeitura.ConsoleApp.Compartilhado
{
    public interface ICadastroBasico
    {
        void InserirRegistro();
        void EditarRegistro();
        void ExcluirRegistro();
        bool VisualizarRegistros(string tipoVisualizado);
    }
}

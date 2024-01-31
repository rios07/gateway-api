namespace Track3_Api_Interfaces_Core.Application.DATA
{
    public class ErrorApplication
    {
        public ErrorApplication() { }
        public ErrorApplication(string mensaje)
        {
            this.Mensaje = mensaje;
        }
        public int Codigo { get; set; } = 0;
        public string Mensaje { get; set; } = String.Empty;
        public object Exception { get; set; }
    }
}

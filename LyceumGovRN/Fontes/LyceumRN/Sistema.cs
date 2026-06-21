namespace Techne.Lyceum.RN
{
    using System.Web;

    public class Sistema
    {
        public static string ObterIP()
        {
            var current = HttpContext.Current;

            // Primeiro tenta pegar o cabeçalho do Akamai. Caso não exista, usamos o normal para obter o IP. 
            return current.Request.Headers["True-Client-IP"] ?? current.Request.UserHostAddress;
        }
    }
}
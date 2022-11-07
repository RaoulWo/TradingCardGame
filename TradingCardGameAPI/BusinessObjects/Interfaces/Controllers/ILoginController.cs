using System.Net;

namespace BusinessObjects.Interfaces.Controllers;

public interface ILoginController
{
    void Post(HttpListenerContext ctx);
}
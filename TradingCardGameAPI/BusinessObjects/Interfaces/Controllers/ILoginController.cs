using System.Net;

namespace BusinessObjects.Interfaces.Controllers;

public interface ILoginController
{
    void Register(HttpListenerContext ctx);
}
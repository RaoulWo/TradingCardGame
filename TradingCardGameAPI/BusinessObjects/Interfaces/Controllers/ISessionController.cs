using System.Net;

namespace BusinessObjects.Interfaces.Controllers;

public interface ISessionController
{
    void Register(HttpListenerContext ctx);
    void Login(HttpListenerContext ctx);
    void Logout(HttpListenerContext ctx);

}
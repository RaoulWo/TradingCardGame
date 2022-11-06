using System.Net;

namespace BusinessObjects.Interfaces.Controllers;

public interface IPlayerController
{
    void Get(HttpListenerContext ctx);
    void Post(HttpListenerContext ctx);
}
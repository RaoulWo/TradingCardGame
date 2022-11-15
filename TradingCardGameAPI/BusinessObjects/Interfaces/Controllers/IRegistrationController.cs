using System.Net;

namespace BusinessObjects.Interfaces.Controllers;

public interface IRegistrationController
{
    void Register(HttpListenerContext ctx);
}
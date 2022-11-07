using System.Net;

namespace BusinessObjects.Interfaces.Controllers;

public interface IRegistrationController
{
    void Post(HttpListenerContext ctx);
}
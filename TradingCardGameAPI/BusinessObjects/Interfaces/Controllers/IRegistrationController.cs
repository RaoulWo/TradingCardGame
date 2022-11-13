using System.Net;

namespace BusinessObjects.Interfaces.Controllers;

public interface IRegistrationController
{
    void SignUp(HttpListenerContext ctx);
}
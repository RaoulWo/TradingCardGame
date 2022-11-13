using System.Net;

namespace BusinessObjects.Interfaces.Controllers;

public interface IStoreController
{
    void Buy(HttpListenerContext ctx);
}
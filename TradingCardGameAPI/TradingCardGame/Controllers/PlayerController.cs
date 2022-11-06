using System.Net;
using BusinessObjects.Interfaces.Controllers;

namespace BusinessLogic.Controllers;

public class PlayerController : IPlayerController
{
    public static PlayerController Instance
    {
        get
        {
            _instance ??= new PlayerController();

            return _instance;
        }
    }

    private static PlayerController _instance = null;

    public void Get(HttpListenerContext ctx)
    {
        var req = ctx.Request;
        var res = ctx.Response;

        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "text/plain";

        string responseString = "GET IS CALLED";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        res.ContentLength64 = buffer.Length;

        // Send the response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }

    public void Post(HttpListenerContext ctx)
    {
        var req = ctx.Request;

        var body = new StreamReader(req.InputStream).ReadToEnd();

        var res = ctx.Response;

        res.StatusCode = (int)HttpStatusCode.OK;
        res.ContentType = "text/plain";

        string responseString = "POST IS CALLED " + body;
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

        res.ContentLength64 = buffer.Length;

        // Send the response
        res.OutputStream.Write(buffer, 0, buffer.Length);
        res.OutputStream.Close();
    }
}
using System.Dynamic;
using System.Net;

namespace BusinessLogic.Utils;

public class Response
{
    public static Response Instance
    {
        get
        {
            _instance ??= new Response();

            return _instance;
        }
    }

    private static Response _instance = null;

    public void BadRequest(HttpListenerResponse res)
    {
        // Construct response
        res.StatusCode = (int)HttpStatusCode.BadRequest;
        res.ContentType = "application/json";

        // Send response
        res.OutputStream.Write(new byte[] { }, 0, 0);
        res.OutputStream.Close();
    }

    public void Conflict(HttpListenerResponse res)
    {
        // Construct response
        res.StatusCode = (int)HttpStatusCode.Conflict;
        res.ContentType = "application/json";

        // Send response
        res.OutputStream.Write(new byte[] { }, 0, 0);
        res.OutputStream.Close();
    }

    public void InternalServerError(HttpListenerResponse res)
    {
        // Construct response
        res.StatusCode = (int)HttpStatusCode.InternalServerError;
        res.ContentType = "application/json";

        // Send response
        res.OutputStream.Write(new byte[] { }, 0, 0);
        res.OutputStream.Close();
    }

    public void Unauthorized(HttpListenerResponse res)
    {
        // Construct response
        res.StatusCode = (int)HttpStatusCode.Unauthorized;
        res.ContentType = "application/json";

        // Send response
        res.OutputStream.Write(new byte[] { }, 0, 0);
        res.OutputStream.Close();
    }
}
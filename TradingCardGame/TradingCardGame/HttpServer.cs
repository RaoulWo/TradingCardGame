using System.Net;

namespace BusinessLogic;

public class HttpServer
{
    public int Port = 10001;

    private HttpListener _listener;

    /// <summary>
    /// Starts the HTTP Server.
    /// </summary>
    public void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://localhost:" + Port.ToString() + "/");
        _listener.Start();

        // Start receiving an incoming request
        Receive();
    }

    /// <summary>
    /// Receives an incoming request asynchronously.
    /// </summary>
    public void Receive()
    {
        // Asynchronously retrieve an incoming request
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

    /// <summary>
    /// The callback function for when an incoming request is received.
    /// </summary>
    /// <param name="result"></param>
    private void ListenerCallback(IAsyncResult result)
    {
        if (_listener.IsListening)
        {
            var context = _listener.EndGetContext(result);
            var request = context.Request;

            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "text/plain";
            response.OutputStream.Write(new byte[] {}, 0, 0);
            response.OutputStream.Close();

            // TODO Handle incoming request

            // TODO Send response

            // Start receiving an incoming request
            Receive(); 
        }
    }
}
using System.Net;
using BusinessLogic.Controllers;
using BusinessObjects;

namespace BusinessLogic;

public class HttpServer
{
    public int Port = 10001;

    private HttpListener _listener;
    private Dictionary<RequestMap, Action<HttpListenerContext>> _dictionary 
        = new Dictionary<RequestMap, Action<HttpListenerContext>>();

    /// <summary>
    /// Starts the HTTP Server.
    /// </summary>
    public void Start()
    {
        // Add the url mappings
        _dictionary.Add(new RequestMap("GET", "players"), PlayerController.Instance.Get);
        _dictionary.Add(new RequestMap("POST", "players"), PlayerController.Instance.Post);

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
            // Store the context and request
            var ctx = _listener.EndGetContext(result);
            var req = ctx.Request;

            // Get the http method and the http target
            string httpMethod = req.HttpMethod.ToUpper();
            string httpTarget = req.Url.Segments[1].Replace("/", "");

            // Handle the request
            _dictionary[new RequestMap(httpMethod, httpTarget)](ctx);

            // Start receiving an incoming request
            Receive(); 
        }
    }
}
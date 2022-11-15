using System.Collections.Concurrent;
using System.Net;
using BusinessLogic.Controllers;
using BusinessLogic.Utils;
using BusinessObjects;

namespace BusinessLogic;

public class HttpServer
{
    public int Port = 10001;

    private HttpListener _listener;
    private readonly ConcurrentDictionary<RequestMap, Action<HttpListenerContext>> _dictionary 
        = new ConcurrentDictionary<RequestMap, Action<HttpListenerContext>>();

    /// <summary>
    /// Starts the HTTP Server.
    /// </summary>
    public void Start()
    {
        // Add the url mappings
        _dictionary.TryAdd(new RequestMap("POST", "registration"), RegistrationController.Instance.Register);
        _dictionary.TryAdd(new RequestMap("POST", "login"), SessionController.Instance.Login);
        _dictionary.TryAdd(new RequestMap("GET", "logout"), SessionController.Instance.Logout);
        _dictionary.TryAdd(new RequestMap("POST", "store"), StoreController.Instance.Buy);
    
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

            // Try to handle the request
            try
            {
                _dictionary[new RequestMap(httpMethod, httpTarget)](ctx);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Response.Instance.InternalServerError(ctx.Response);
            }

            // Start receiving an incoming request
            Receive(); 
        }
    }
}
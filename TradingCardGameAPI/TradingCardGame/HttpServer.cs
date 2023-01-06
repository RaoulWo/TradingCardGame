using System.Net;
using BusinessLogic.Controllers;
using BusinessLogic.Utils;
using BusinessObjects;

namespace BusinessLogic;

public class HttpServer
{
    public int Port = 10001;

    private HttpListener _listener;
    private readonly Dictionary<RequestMap, Action<HttpListenerContext>> _dictionary 
        = new Dictionary<RequestMap, Action<HttpListenerContext>>();

    /// <summary>
    /// Starts the HTTP Server.
    /// </summary>
    public void Start()
    {
        // Add the url mappings
        _dictionary.Add(new RequestMap("POST", "register"), SessionController.Instance.Register);
        _dictionary.Add(new RequestMap("POST", "login"), SessionController.Instance.Login);
        _dictionary.Add(new RequestMap("GET", "logout"), SessionController.Instance.Logout);
        _dictionary.Add(new RequestMap("GET", "store"), StoreController.Instance.BuyPack);
        _dictionary.Add(new RequestMap("GET", "collection"), CollectionController.Instance.ShowCollection);
        _dictionary.Add(new RequestMap("GET", "deck"), CollectionController.Instance.ShowDeck);
        _dictionary.Add(new RequestMap("POST", "deck"), CollectionController.Instance.ConfigureDeck);
        _dictionary.Add(new RequestMap("GET", "profile"), ProfileController.Instance.ShowProfile);
        _dictionary.Add(new RequestMap("PUT", "profile"), ProfileController.Instance.ConfigureProfile);
        _dictionary.Add(new RequestMap("GET", "score"), ScoreController.Instance.ShowLeaderBoard);
        _dictionary.Add(new RequestMap("GET", "play"), GameController.Instance.Play);

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
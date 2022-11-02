using System.Net;

namespace BusinessLogic;

public class HttpServer
{
    public int Port = 10001;

    private HttpListener _listener;

    public void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://localhost:" + Port.ToString() + "/");
        _listener.Start();
        Receive();
    }

    public void Receive()
    {
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

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

            Receive(); 
        }
    }
}
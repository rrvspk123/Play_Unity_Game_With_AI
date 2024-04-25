using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

public class HttpCodeListener
{
    private HttpListener listener;
    private Thread listenerThread;
    private Action<string> onCodeFetched;

    public HttpCodeListener(int port)
    {
        listener = new HttpListener();
        #if UNITY_WEBGL
            listener.Prefixes.Add($"?");
        #else
            listener.Prefixes.Add($"http://localhost:{port}/");
            listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        #endif
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
    }

    public void StartListening(Action<string> callback)
    {
        onCodeFetched = callback;
        
        listener.Start();

        listenerThread = new Thread(ListeningThread);
        listenerThread.Start();
    }

    public void StopListening()
    {
        listener.Stop();
        onCodeFetched = null;
    }

    private void ListeningThread()
    {
        while (listener.IsListening)
        {
            var result = listener.BeginGetContext(ListenerCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }
    }

    private void ListenerCallback(IAsyncResult result)
    {
        var context = listener.EndGetContext(result);

        if (!context.Request.QueryString.AllKeys.Contains("code")) return;
        UnityMainThreadDispatcher.Instance().Enqueue(() => onCodeFetched?.Invoke(context.Request.QueryString.Get("code")));

        context.Response.Close(); 
    }
}
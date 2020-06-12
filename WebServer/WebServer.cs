using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class WebServer
    {

        private HttpListener _listener;
        private string _baseFolder = "";


        public WebServer(string uriPrefix, string basefolder)
        {
            _baseFolder = basefolder;
            _listener = new HttpListener();
            if (uriPrefix == null || uriPrefix == "")
            {
                throw new ArgumentException("invaild prefix");
            }
            else
                _listener.Prefixes.Add(uriPrefix);
        }
        public void Stop()
        {
            _listener.Stop();
        }
        public async void Start()
        {

                _listener.Start();
                Console.WriteLine("Listening for incoming requests . . . \n");
            

            while (_listener.IsListening)
            {
                try
                {
                   var context = await _listener.GetContextAsync();
                     Task.Run(() =>
                     ProcessRequestAysnc(context)
                    );

                }

                catch (HttpListenerException ex)
                {
                    Console.WriteLine(ex.ErrorCode + "\n" + ex.Message);
                    break;
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message); 
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

       

        private async void ProcessRequestAysnc(HttpListenerContext context)
        {
            try
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string fileName = Path.GetFileName(request.RawUrl);
                string path = Path.Combine(_baseFolder, fileName);

                byte[] msg;
                if (!File.Exists(path))
                {

                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine($"{0}   not found", path);
                    msg = Encoding.UTF8.GetBytes("Sorry that file does not exist.");
                }

                else
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    msg = File.ReadAllBytes(path);
                }

                response.ContentLength64 = msg.Length;
                using (Stream stream = context.Response.OutputStream)
                {
                    await stream.WriteAsync(msg, 0, msg.Length);
                }

            }
            catch (HttpListenerException e)
            {
                Console.WriteLine($"{0} \n {1}", e.ErrorCode, e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine($"{0} \n {1}", e.InnerException, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

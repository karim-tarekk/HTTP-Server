using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            int myport = portNumber;
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket.Bind(hostEndPoint);

        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(100);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = this.serverSocket.Accept();
                Console.WriteLine("New client accepted: {0}", clientSocket.RemoteEndPoint);
                Thread newthread = new Thread(new ParameterizedThreadStart(HandleConnection));
                newthread.Start(clientSocket);

            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket clientSock = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSock.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] data = new byte[5000];
                    int receivedLength = clientSock.Receive(data);
                    // TODO: break the while loop if receivedLen==0
                    if (receivedLength == 0)
                    {
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    string req = Encoding.ASCII.GetString(data, 0, receivedLength);
                    Request Request = new Request(req);
                    // TODO: Call HandleRequest Method that returns the response

                    Response resp = HandleRequest(Request);
                    //Console.WriteLine(resp.ResponseString);
                    byte[] msg = Encoding.ASCII.GetBytes(resp.ResponseString);
                    // TODO: Send Response back to client
                   clientSock.Send(msg);

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSock.Close();

        }
     
        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content;
            try
            {
                //TODO: check for bad request 
                string uri, NDuri;
                HTTPVersion version;
                if (!request.ParseRequest(request))
                {
                    version = request.httpVersion;
                    content = LoadDefaultPage("BadRequest.html");
                    Response res = new Response(StatusCode.BadRequest, version, "text/html", content);
                    return res;
                }
                else
                {
                    uri = request.relativeURI;
                    NDuri = uri.Substring(1);
                    version = request.httpVersion;
                    //TODO: map the relativeURI in request to get the physical path of the resource.
                    string filePath = Path.Combine(Configuration.RootPath, NDuri);
                    //TODO: check for redirect
                    string redir = GetRedirectionPagePathIFExist(NDuri);
                    if (redir != "NOTFOUND")
                    {
                        content = LoadDefaultPage("Redirect.html");
                        Response res = new Response(StatusCode.Redirect, version, "text/html", content, redir);
                        return res;
                    }
                    else
                    {
                        //TODO: check file exists
                        if (File.Exists(filePath))
                        {
                            content = LoadDefaultPage(NDuri);
                            Response res = new Response(StatusCode.OK, version, "text/html", content);
                            return res;
                        }
                        else
                        {
                            content = LoadDefaultPage("Notfound.html");
                            Response res = new Response(StatusCode.NotFound, version, "text/html", content);
                            return res;

                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                HTTPVersion version = request.httpVersion;
                string xcontent = LoadDefaultPage("InternalError.html");
                Response res = new Response(StatusCode.InternalServerError, version, "text/html", xcontent);
                return res;
                // TODO: in case of exception, return Internal Server Error. 
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            string value, output;
            bool hasValue = Configuration.RedirectionRules.TryGetValue(relativePath, out value);
            if (hasValue)
            {
                output = value;
            }
            else
            {
                output = "NOTFOUND";
            }
            return output;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if (File.Exists(@filePath))
            {
                string html = File.ReadAllText(@filePath);
                return html;
            }
            // else read file and return its content
            else
            {
                return string.Empty;
                throw new FileNotFoundException();
            }
        }
        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        string s1 = line.Substring(0, line.LastIndexOf(','));
                        string s2 = line.Substring(line.LastIndexOf(',') + 1);
                        Configuration.RedirectionRules.Add(s1, s2);
                    }

                }
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}

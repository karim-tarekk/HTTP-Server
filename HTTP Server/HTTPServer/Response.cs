using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, HTTPVersion version, string contentType, string content, string redirectoinPath = "")
        {
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            string statusLine = GetStatusLine(code, version);
            int contLeng = content.Length;
            DateTime now = DateTime.Now;
            string time = now.ToString("F");
            if (code == StatusCode.Redirect)
            {
                responseString = statusLine + "\r\n" + "Content-Type: " + contentType + "\r\n" +
                    "Content-Length: " + contLeng + "\r\n" + "Date: " + time + "\r\n" + "location: " + redirectoinPath
                    + "\r\n" + "\r\n" + content;
            }
            else
            {
                responseString = statusLine + "\r\n" + "Content-Type: " + contentType + "\r\n" +
                    "Content-Length: " + contLeng + "\r\n" + "Date: " + time + "\r\n" + "\r\n" + content;
            }
            // TODO: Create the request string
        }

        private string GetStatusLine(StatusCode code, HTTPVersion version)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            if (version == HTTPVersion.HTTP11)
            {
                if (code == StatusCode.OK)
                {
                    statusLine = "HTTP/1.1 200 OK";
                }
                else if (code == StatusCode.InternalServerError)
                {
                    statusLine = "HTTP/1.1 500 InternalServerError";
                }
                else if (code == StatusCode.BadRequest)
                {
                    statusLine = "HTTP/1.1 400 BadRequest";
                }
                else if (code == StatusCode.NotFound)
                {
                    statusLine = "HTTP/1.1 404 NotFound";
                }
                else if (code == StatusCode.Redirect)
                {
                    statusLine = "HTTP/1.1 301 Found";
                }
            }
            else if (version == HTTPVersion.HTTP10)
            {
                if (code == StatusCode.OK)
                {
                    statusLine = "HTTP/1.0 200 OK";
                }
                else if (code == StatusCode.InternalServerError)
                {
                    statusLine = "HTTP/1.0 500 InternalServerError";
                }
                else if (code == StatusCode.BadRequest)
                {
                    statusLine = "HTTP/1.0 400 BadRequest";
                }
                else if (code == StatusCode.NotFound)
                {
                    statusLine = "HTTP/1.0 404 NotFound";
                }
                else if (code == StatusCode.Redirect)
                {
                    statusLine = "HTTP/1.0 301 Found";
                }
            }
            else
            {
                if (code == StatusCode.OK)
                {
                    statusLine = "HTTP/0.9 200 OK";
                }
                else if (code == StatusCode.InternalServerError)
                {
                    statusLine = "HTTP/0.9 500 InternalServerError";
                }
                else if (code == StatusCode.BadRequest)
                {
                    statusLine = "HTTP/0.9 400 BadRequest";
                }
                else if (code == StatusCode.NotFound)
                {
                    statusLine = "HTTP/0.9 404 NotFound";
                }
                else if (code == StatusCode.Redirect)
                {
                    statusLine = "HTTP/0.9 301 Found";
                }
            }
            return statusLine;
        }
    }
}

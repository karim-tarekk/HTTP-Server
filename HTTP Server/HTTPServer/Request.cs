using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines = new Dictionary<string, string>();

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        public HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest(Request req)
        {
           // throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter   
            string parsed = requestString + "\r\n";
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            int lines = parsed.Split('\n').Length;
            if (lines < 3) 
            {
                return false;
            }
            else
            {
                // Parse Request line
                string[] lin = parsed.Split('\n');
                int size = lin.Length;
                string reqLine = lin[0];
                // Validate blank line exists*/
                if (!ValidateBlankLine(lin) )
                {
                    return false;

                }
                // Load header lines into HeaderLines dictionary
                else
                {
                    if (!ParseRequestLine(reqLine))
                    {
                        return false;
                    }
                    else {
                        LoadHeaderLines(lin, size);
                        return true;
                    }
                }
            }
        }

        private bool ParseRequestLine(string reqline)
        {
            if (reqline.Contains("GET") && reqline.Contains(" ") && (reqline.Contains("HTTP/1.1") || reqline.Contains("HTTP/0.9") || reqline.Contains("HTTP/1.0")))
            {
                relativeURI = ReturnURL(reqline, ' ',' ');
                if (reqline.Contains("HTTP/1.1"))
                {
                    httpVersion = HTTPVersion.HTTP11;
                }
                else if (reqline.Contains("HTTP/1.0"))
                {
                    httpVersion = HTTPVersion.HTTP10;
                }
                else
                {
                    httpVersion = HTTPVersion.HTTP09;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string ReturnURL(string input, char charFrom, char charTo)
        {
            int posFrom = input.IndexOf(charFrom);
            if (posFrom != -1) //if found char
            {
                int posTo = input.IndexOf(charTo, posFrom + 1);
                if (posTo != -1) //if found char
                {
                    return input.Substring(posFrom + 1, posTo - posFrom - 1);
                }
            }

            return string.Empty;
        }
        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private void LoadHeaderLines(string[] lin, int size)
        {
            for (int i = 1; i < size-3 ; i++)
                headerLines.Add(lin[i].Substring(0, lin[i].LastIndexOf(':')), lin[i].Substring(lin[i].LastIndexOf(':') + 1));
        }

        private bool ValidateBlankLine(string[] lines)
        {
            //throw new NotImplementedException();
            int last = lines.Length;
            if (String.IsNullOrEmpty(lines[last-1]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

# HTTP-Server

HTTP Server project in Computer Networks

Starting the Server • Accept multiple clients by starting a thread for each accepted connection. • Keep on accepting requests from the remote client until the client closes the socket (sends a zero length message) • For each received request, the server must reply with a response.

Receiving Request • The received request must be a valid HTTP request, else return 400 Bad Request – Check single space separating the request line parameters. • Method URI HTTPVersion – Check blank line separating the header lines and the content, even if the content is empty – Check valid URI – Check at least request line and host header and blank lines exist.

Response Headers • The response should include the following headers: – Content-Type: • We will use only text/html – Content-Length: • The length of the content – Date: • Current DateTime of the server – Location: • Only if there is redirection.

Handling Request • Using Configuration.RootPath, map the URI to the physical path – Example: configuration.RootPath = “X:\Visual Studio 2019\HTTPserver\intepub\wwwroot\fcis1” and URI = “/aboutus.html”

Physical-Path = “\aboutus.html” ==> Redirection-To = "\aboutus2.html"

• If the URI exists in the configuration.RedirectionRules, then return 301 Redirection Error and add location header with the new redirected URI.

• The content should be the content of the static page “redirect.html” Not Found

• If the physical file is not found return 404 Not Found error.

• The content should be the content of the static page “Notfound.html” Bad Request

• If there is any parsing error in the request, return 400 Bad Request Error.

• The content should be loaded with the content of the static page “BadRequest.html” Internal Server Error

• If there is any unknown exception, return 500 Internal Server Error.

• The content should be the content of the static page “InternalError.html”

Project Running => SERVER DEMO • Run the server • Try the following URIs in the web browser:

http://localhost:1000/aboutus2.html should display aboutus2.html page

http://localhost:1000/aboutus.html should display aboutus2.html (redirection)

http://localhost:1000/main.html should display main page

http://localhost:1000/blabla.html should display 404 page

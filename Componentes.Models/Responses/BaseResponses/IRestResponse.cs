using System.Net;

namespace Componentes.Models.Responses.BaseResponses;

public interface IRestResponse
{
    public string? Content { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? StatusDescription { get; set; }
    public bool IsSuccessful { get; }
}
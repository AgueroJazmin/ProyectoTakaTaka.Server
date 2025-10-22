
namespace ProyectoTakaTaka.Servicio.ServicioHttp
{
    public interface IHttpServicio
    {
        Task<HttpRespuesta<object>> Delete(string url);
        Task<HttpRespuesta<T>> Get<T>(string url);
        Task<HttpRespuesta<TResp>> Post<T, TResp>(string url, T entidad);
    }
}
using Microsoft.EntityFrameworkCore;

namespace PeliculasAPI.Helpers
{
    public static class HttpContextExtension
    {
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpContext,
            IQueryable<T> queryable, int CantidadRegistrosxPagina)
        {
            double cantidad = await queryable.CountAsync();
            double cantidadPaginas = Math.Ceiling(cantidad / CantidadRegistrosxPagina);

            httpContext.Response.Headers.Add("cantidadPaginas",cantidadPaginas.ToString());

        }
    }
}

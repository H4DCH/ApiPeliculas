using PeliculasAPI.Entidades.DTO;
using System.Linq;

namespace PeliculasAPI.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
             // Asegurarse de que la resta se realice antes de la multiplicación
            int offset = (paginacionDTO.Pagina - 1) * paginacionDTO.CantidadRegistrosxPagina;
            return queryable.Skip(offset).Take(paginacionDTO.CantidadRegistrosxPagina);
        }
    }
}

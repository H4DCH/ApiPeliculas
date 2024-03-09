namespace PeliculasAPI.Entidades.DTO
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int CantidadxPagina { get; set; } = 10;
        private readonly int CantidadMaxPagina  = 50;  
        public int CantidadRegistrosxPagina {
            get => CantidadxPagina;
            set 
            {
                CantidadxPagina = (value > CantidadMaxPagina) ? CantidadMaxPagina : value; 
            
            }}   


    }
}

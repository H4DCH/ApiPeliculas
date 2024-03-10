using AutoMapper;
using PeliculasAPI.Entidades;
using PeliculasAPI.Entidades.DTO;

namespace PeliculasAPI.ProfileMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO,Actor>().
                ForMember(x=> x.Foto , options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>().
                ForMember(x => x.Poster, options => options.Ignore()).
                ForMember(x=>x.PeliculasGeneros, options => options.MapFrom(MapPeliculasGeneros)).
                ForMember(x=>x.PeliculasActores,options => options.MapFrom(MapPeliculasActores));
            CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();
        }

        private List<PeliculasGeneros> MapPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resul = new List<PeliculasGeneros>();
            if(peliculaCreacionDTO.GenerosId == null)
            {
                return resul;
            }
            foreach(var id in peliculaCreacionDTO.GenerosId)
            {
                resul.Add(new PeliculasGeneros()
                {
                    GeneroId = id
                });
            }
            return resul;
        }
        private List<PeliculasActores> MapPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resul = new List<PeliculasActores>();
            if (peliculaCreacionDTO.actores == null)
            {
                return resul;
            }
            foreach (var actor in peliculaCreacionDTO.actores)
            {
                resul.Add(new PeliculasActores()
                {
                    ActorId = actor.ActorId,
                    Personaje = actor.Personaje
                });
            }
            return resul;

        }
    }
}

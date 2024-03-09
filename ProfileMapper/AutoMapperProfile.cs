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
        }
    }
}

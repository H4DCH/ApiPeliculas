using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using PeliculasAPI.Entidades.DTO;
using System.ComponentModel;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public ActoresController( Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet(Name ="ListaActores")]
        public async Task<ActionResult<List<ActorDTO>>> GetActores()
        {
            var lista = await context.Actores.ToListAsync();

            return mapper.Map<List<ActorDTO>>(lista);
        }
        [HttpGet("{Id:int}",Name ="ActorId")]
        public async Task<ActionResult<ActorDTO>> GetxId(int Id)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(a => a.Id == Id);

            if (actor == null)
            {
                return BadRequest();
            }
            return mapper.Map<ActorDTO>(actor);

        }
        [HttpPost(Name = "CreacionActor")]
        public async Task<ActionResult> NewActor([FromForm]ActorCreacionDTO actorCreacionDTO)
        {
            var verifi = await context.Actores.AnyAsync(x => x.Nombre == actorCreacionDTO.Nombre && x.fechaNacimiento == actorCreacionDTO.fechaNacimiento);
            if (verifi)
            {
                return BadRequest("El actor ya ha sido registrado");
            }

            var actorNew = mapper.Map<Actor>(actorCreacionDTO);
            context.Add(actorNew);
           // await context.SaveChangesAsync();
            return CreatedAtRoute("ActorId",new {Id = actorNew.Id},actorCreacionDTO);
        }
        [HttpPut("{Id:Int}",Name ="ModiActor")]
        public async Task<ActionResult> UpdateActor(int Id, ActorCreacionDTO actorCreacionDTO)
        {
            var Actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == Id);
            if (Actor == null)
            {
                return BadRequest();
            }
            var actorDTO = mapper.Map<Actor>(actorCreacionDTO);
             actorDTO.Id = Id;

            context.Update(actorDTO);
            await context.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete("{Id:int}",Name ="DeleteActor")]
        public async Task<ActionResult> DeleteActor(int Id)
        {
            var act = await context.Actores.FirstOrDefaultAsync(a => a.Id == Id);
            if (act == null)
            {
                return NotFound();
            }
            context.Remove(act);
            await context.SaveChangesAsync();
            return NoContent();

        }
    }
}

using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using PeliculasAPI.Entidades.DTO;
using PeliculasAPI.Helpers;
using PeliculasAPI.Servicios;
using System.ComponentModel;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly Context context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActoresController( Context context, IMapper mapper,IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }
        [HttpGet(Name ="ListaActores")]
        public async Task<ActionResult<List<ActorDTO>>> GetActores([FromQuery]PaginacionDTO paginacionDTO)
        {
            var queryable =  context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable,paginacionDTO.CantidadRegistrosxPagina);

            var lista = await queryable.Paginar(paginacionDTO).ToListAsync();

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
            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorNew.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                        actorCreacionDTO.Foto.ContentType);
                }
            }
            context.Add(actorNew);
           await context.SaveChangesAsync();
            return CreatedAtRoute("ActorId",new {Id = actorNew.Id},actorCreacionDTO);
        }
        [HttpPut("{Id:Int}",Name ="ModiActor")]
        public async Task<ActionResult> UpdateActor(int Id, ActorCreacionDTO actorCreacionDTO)
        {
            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == Id);
            if (actor == null)
            {
                return BadRequest();
            }
            var actorDTO = mapper.Map(actorCreacionDTO,actor);

            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actor.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                        actor.Foto,
                        actorCreacionDTO.Foto.ContentType);
                }
            }
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
        [HttpPatch("{Id:int}")]
        public async Task<ActionResult> PatchActor(int Id, [FromBody]JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var actor = await context.Actores.FirstOrDefaultAsync(x => x.Id == Id);

            if (actor == null)
            {
                return NotFound();
            }

            var actorDTO = mapper.Map<ActorPatchDTO>(actor);
            patchDocument.ApplyTo(actorDTO,ModelState);

            var esValido = TryValidateModel(actorDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(actorDTO, actor);

            await context.SaveChangesAsync();
            return NoContent();

        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using PeliculasAPI.Entidades.DTO;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/genero")]
    public class GenerosController : ControllerBase
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public GenerosController(Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet(Name = "ListadeGeneros")]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var gen = await context.Generos.ToListAsync();
            if (gen == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<List<GeneroDTO>>(gen);


            return dto;

        }
        [HttpGet("{Id:int}",Name ="GeneroId")]
        public async Task<ActionResult<GeneroDTO>>GetId(int Id)
        {
            var gen = await context.Generos.FirstOrDefaultAsync(g => g.Id == Id);
            
            if (gen==null)
            {
                return NotFound();
            }
            var dtog = mapper.Map<GeneroDTO>(gen);

            return dtog;
        }

        [HttpPost]
        public async Task<ActionResult> NewGenero(GeneroCreacionDTO generoCreacionDTO)
        {
            var resul = await context.Generos.AnyAsync( g => g.Nombre == generoCreacionDTO.Nombre);
            if (resul)
            {
                return BadRequest();
            }

            var dtonew = mapper.Map<Genero>(generoCreacionDTO);

            if(dtonew == null)
            {
                return BadRequest();
            }
            context.Add(dtonew);
            await context.SaveChangesAsync();

            return CreatedAtRoute("GeneroId", new {Id = dtonew.Id},generoCreacionDTO);
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult>UpdateGenero(int Id, GeneroCreacionDTO generoCreacionDTO)
        {
            var res = await context.Generos.AnyAsync(g => g.Id==Id);
            if (!res)
            {
                return NotFound();
            }
            var dtoUp = mapper.Map<Genero>(generoCreacionDTO);
            dtoUp.Id = Id;

            context.Update(dtoUp);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id:int}",Name ="DeleteGen")]
        public async Task<ActionResult> DeleteGenero(int Id)
        {
            var res = await context.Generos.FirstOrDefaultAsync(g => g.Id==Id);
            if (res==null)
            {
                return NotFound();
            }
            context.Remove(res);
            await context.SaveChangesAsync();
            return NoContent();

        }
    }
}

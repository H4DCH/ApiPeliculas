using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using PeliculasAPI.Entidades.DTO;
using PeliculasAPI.Servicios;
using System.Runtime.CompilerServices;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/peliculas")]    
    public class PeliculasController : ControllerBase
    {
        private readonly Context context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(Context context, IMapper mapper,IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }
        [HttpGet(Name ="ListadePeliculas")]
        public async Task<ActionResult<List<PeliculaDTO>>> GetPeliculas()
        {
            var pelis = await context.Peliculas.ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(pelis);    
        }
        [HttpGet("{Id:int}",Name ="PeliculaId")]
        public async Task<ActionResult<PeliculaDTO>> GetxIdPelicula(int Id)
        {
            var peliId = await context.Peliculas.
                Include(x=>x.PeliculasActores).ThenInclude(x=>x.Actor).
                Include(x=>x.PeliculasGeneros).ThenInclude(x=>x.Genero).
                FirstOrDefaultAsync(p=>p.Id==Id);
            if (peliId == null){
                return NotFound();
            }

            var peliDTO =  mapper.Map<PeliculaDTO>(peliId);

            return peliDTO;
        }

        [HttpPost]
        public async Task<ActionResult> NewPelicula([FromForm]PeliculaCreacionDTO peliculaCreacionDTO)
        {
            
            var PeliVali = await context.Peliculas.AnyAsync(p => p.Titulo == peliculaCreacionDTO.Titulo);
            if (PeliVali)
            {
                return BadRequest("El titulo ya esta registrado");
            }
            var peliDTO = mapper.Map<Pelicula>(peliculaCreacionDTO);
            
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliDTO.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                        peliculaCreacionDTO.Poster.ContentType);
                }
            }

            context.Add(peliDTO);
            await context.SaveChangesAsync();

            return CreatedAtRoute("PeliculaId", new { Id = peliDTO.Id }, peliculaCreacionDTO); 
            
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> PutPelicula(int Id, [FromForm]PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peli = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == Id);
            if (peli == null)
            {
                return BadRequest();
            }
            var peliculaDTO = mapper.Map(peliculaCreacionDTO, peli);

            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peli.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor,
                        peli.Poster,
                        peliculaCreacionDTO.Poster.ContentType);
                }
            }
            context.Update(peliculaDTO);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{Id:Int}")]
        public async Task<ActionResult> PatchPelis(int Id,[FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var peli = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == Id);

            if (peli == null)
            {
                return NotFound();
            }

            var peliDTO = mapper.Map<PeliculaPatchDTO>(peli);
            patchDocument.ApplyTo(peliDTO, ModelState);

            var esValido = TryValidateModel(peliDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(peliDTO, peli);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeletePelicula(int Id)
        {
            var peli = await context.Peliculas.FirstOrDefaultAsync(p => p.Id == Id);

            if (peli==null)
            {
                return NotFound();
            }

            context.Remove(peli);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using ApiPersonajesAWS.Models;
using ApiPersonajesAWS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPersonajesAWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonajesController : ControllerBase
    {
        private PersonajesRepository repo;

        public PersonajesController(PersonajesRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Personaje>>> Personajes()
        {
            return Ok(await this.repo.GetPersonajesAsync());
        }

        [HttpGet]
        [Route("{idpersonaje}")]
        public async Task<ActionResult<Personaje>> Personaje(int idpersonaje)
        {
            return Ok(await this.repo.GetPersonajeAsync(idpersonaje));
        }

        [HttpPost]
        public async Task<ActionResult<Personaje>> Insertar(Personaje personaje)
        {
            return Ok(await this.repo.InsertarPersonajes(personaje.Nombre, personaje.Imagen));
        }

        [HttpDelete]
        [Route("{idpersonaje}")]
        public async Task<ActionResult> Eliminar(int idpersonaje)
        {
            await this.repo.Eliminar(idpersonaje);
            return Ok();
        }
    }
}

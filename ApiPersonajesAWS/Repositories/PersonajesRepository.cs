using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPersonajesAWS.Repositories
{
    public class PersonajesRepository
    {
        private TelevisionContext context;

        public PersonajesRepository(TelevisionContext context)
        {
            this.context = context;
        }


        public async Task<int> GetMaxIdPersonaje()
        {
            if (this.context.Personajes.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Personajes.MaxAsync(x => x.IdPersonajes) + 1;
            }
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> GetPersonajeAsync(int idpersonaje)
        {
            return await this.context.Personajes.FirstOrDefaultAsync(x => x.IdPersonajes == idpersonaje);
        }

        public async Task<Personaje> InsertarPersonajes(string personaje, string imagen)
        {
            Personaje per = new Personaje
            {
                IdPersonajes = await this.GetMaxIdPersonaje(),
                Nombre = personaje,
                Imagen = imagen
            };

            this.context.Personajes.Add(per);
            await this.context.SaveChangesAsync();

            return per;
        }

        public async Task Eliminar(int idpersonaje)
        {
            Personaje pers = await this.GetPersonajeAsync(idpersonaje);

            this.context.Personajes.Remove(pers);
            await this.context.SaveChangesAsync();
        }
    }
}

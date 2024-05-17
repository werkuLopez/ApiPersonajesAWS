using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using static System.Net.Mime.MediaTypeNames;


#region PROCEDURES
//DELIMITER $$
//DROP PROCEDURE IF EXISTS update_personaje$$
//CREATE PROCEDURE update_personaje(IN nombre VARCHAR(255), IN imagen VARCHAR(255), IN idpers INT)
//BEGIN

//update PERSONAJES
//	SET PERSONAJE=nombre, IMAGEN = imagen
//	WHERE IDPERSONAJE = idpers;
//END
//$$
#endregion
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
            List<Personaje> personajes = await this.context.Personajes.ToListAsync();
            return personajes;
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

        public async Task<Personaje> UpdatePersonaje(int idpersonaje, string nombre, string imagen)
        {
            string sql = "call update_personaje(@nombre, @imagen, @idpers)";
            MySqlParameter paramId = new MySqlParameter("@idpers", idpersonaje);
            MySqlParameter paramNombre = new MySqlParameter("@nombre", nombre);
            MySqlParameter paramImagen = new MySqlParameter("@imagen", imagen);

            await this.context.Database.ExecuteSqlRawAsync(sql, paramId, paramNombre, paramImagen);

            await this.context.SaveChangesAsync();


            Personaje empleado = await GetPersonajeAsync(idpersonaje);
            return empleado;
        }
    }
}

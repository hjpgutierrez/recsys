using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;



namespace WebApiRecSys.Controllers
{
    [Route("api/[controller]")]
    public class ComentarioController : ControllerBase
    {
        public AppDb Db { get; }

        public ComentarioController(AppDb db)
        {
            Db = db;
        }   

        
        [HttpPost]
        public async Task<RespuestaJson> agregar([FromBody]Comentario result)
        {
            try
            {
                await Db.Connection.OpenAsync();
                result.Db = Db;
                await result.Insertar();
                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }           
        }


        [HttpDelete("{IdComentario}")]
        public async Task<RespuestaJson> eliminar(int IdComentario)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new Comentario {
                    Db = this.Db,
                    IdComentario = IdComentario
                };

                await query.Eliminar();
                return new RespuestaJson(true, "Comentario eliminado", null);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            } 
        }
  
    }
}
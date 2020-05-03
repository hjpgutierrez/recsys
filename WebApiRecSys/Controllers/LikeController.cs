using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;



namespace WebApiRecSys.Controllers
{
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        public AppDb Db { get; }

        public LikeController(AppDb db)
        {
            Db = db;
        }   

        
        [HttpPost]
        public async Task<RespuestaJson> agregar([FromBody]Like result)
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


        [HttpDelete("{IdUsuario}/{IdReceta}")]
        public async Task<RespuestaJson> eliminar(int IdUsuario, int IdReceta)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new Like {
                    Db = this.Db,
                    IdUsuario = IdUsuario,
                    IdReceta = IdReceta
                };

                await query.Eliminar();
                return new RespuestaJson(true, "Like eliminado", null);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            } 
        }
  
    }
}
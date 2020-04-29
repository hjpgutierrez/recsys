using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace WebApiRecSys.Controllers
{
    [Route("api/[controller]")]
    public class RecetaController : ControllerBase
    {
        public AppDb Db { get; }
        public IWebHostEnvironment _env;

        public RecetaController(AppDb db, IWebHostEnvironment env)
        {
            Db = db;
            _env = env;
        }  

        [HttpPost]
        public async Task<RespuestaJson> agregar([FromBody]Receta result)
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

        [HttpGet]
        public async Task<RespuestaJson> cargar()
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new RecetaQuery(Db);
                var result = await query.BuscarRecetas();
                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }
        }

        [HttpGet("{id}")]
        public async Task<RespuestaJson> buscarRecetasUsuario(int id)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new RecetaQuery(Db);
                var result = await query.BuscarRecetasUsuario(id);
                if (result is null)
                    return new RespuestaJson(false, "Usuario no tiene recetas.", null);

                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }
        }
    }
}
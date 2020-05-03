using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiRecSys.Controllers
{
    [Route("api/[controller]")]
    public class RecetaDetalleController : ControllerBase
    {
        public AppDb Db { get; }

        public RecetaDetalleController(AppDb db)
        {
            Db = db;
        } 

        [HttpGet("{filtro}")]
        public async Task<RespuestaJson> CargarIngredientesFiltradoPorNombre(string filtro)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new RecetaDetalleQuery(Db);
                var result = await query.CargarIngredientesFiltradoPorNombre(filtro);
                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }
        }
    }
}
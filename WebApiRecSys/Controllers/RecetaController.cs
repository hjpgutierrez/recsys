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

        [HttpGet("{idusuario}")]
        public async Task<RespuestaJson> buscarRecetasUsuario(int idusuario)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new RecetaQuery(Db);
                var result = await query.CargarRecetasUsuario(idusuario);
                if (result is null)
                    return new RespuestaJson(false, "Usuario no tiene recetas.", null);

                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }
        }
    
        [HttpPut]
        [Route("ActualizarReceta")]
        public async Task<RespuestaJson> ActualizarReceta([FromBody]Receta result)
        {
            try
            {
                await Db.Connection.OpenAsync();
                result.Db = Db;
                await result.Actualizar();
                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }           
        } 

        [HttpPut]
        [Route("ImagenReceta")]
        public async Task<RespuestaJson> ImagenReceta([FromBody]Imagen filtro)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new RecetaQuery(Db);
                var result = await query.BuscarReceta(filtro.idactualizar);

                if (result is null)
                    return new RespuestaJson(false, "Receta no encontrada.", null);

                var bytes = Convert.FromBase64String(filtro.base64image);
                
                string raiz = _env.WebRootPath + "\\Upload\\";
                if(!Directory.Exists(raiz))
                {
                    Directory.CreateDirectory(raiz);
                }

                var uniqueFileName = FileUploadAPI.GetUniqueFileName(
                    FileUploadAPI.GenerarExtension(result.nombreReceta));
                var uploads = Path.Combine(_env.WebRootPath, "Upload");
                var filePath = Path.Combine(uploads,uniqueFileName);


                if (bytes.Length > 0)
                {
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Flush();

                        result.imagenReceta = uniqueFileName;
                        await result.ActualizarImagen();
                        return new RespuestaJson(true, null, result);
                    }
                }
                else
                {
                    return new RespuestaJson(false, "Archivo no encontrado.", null);
                }
        
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }  
        }

         
        [HttpDelete("{id}")]
        public async Task<RespuestaJson> eliminar(int id)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new RecetaQuery(Db);
                var result = await query.BuscarReceta(id);
                if (result is null)
                    return new RespuestaJson(false, "Receta no encontrada.", null);

                await result.Eliminar();
                return new RespuestaJson(true, "Receta eliminada", null);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            } 
        }
    
    }
}
using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;



namespace WebApiRecSys.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        public AppDb Db { get; }
        public IWebHostEnvironment _env;

        public UsuarioController(AppDb db, IWebHostEnvironment env)
        {
            Db = db;
            _env = env;
        }
        
             
        [HttpGet]
        public async Task<RespuestaJson> cargar()
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new UsuarioQuery(Db);
                var result = await query.BuscarUltimosUsuario();
                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }
        }

      
        [HttpGet("{id}")]
        public async Task<RespuestaJson> buscar(int id)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new UsuarioQuery(Db);
                var result = await query.BuscarUsuario(id);
                if (result is null)
                    return new RespuestaJson(false, "Usuario no encontrado.", null);

                    return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }
        }

        [HttpGet("{usuario}/{pass}")]
        public async Task<RespuestaJson> login(string usuario, string pass)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new UsuarioQuery(Db);
                var result = await query.BuscarUsuario(usuario, pass);
                if (result is null)
                    return new RespuestaJson(false, "Usuario no encontrado.", null);
                    
                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }
        }

        
        [HttpPost]
        public async Task<RespuestaJson> agregar([FromBody]Usuario result)
        {
            try
            {
                await Db.Connection.OpenAsync();
                result.Db = Db;
                await result.InsertarUsuario();
                return new RespuestaJson(true, null, result);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            }           
        }

         // DELETE api/usuario/5
        [HttpDelete("{id}")]
        public async Task<RespuestaJson> eliminar(int id)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new UsuarioQuery(Db);
                var result = await query.BuscarUsuario(id);
                if (result is null)
                    return new RespuestaJson(false, "Usuario no encontrado.", null);

                await result.Eliminar();
                return new RespuestaJson(true, "Usuario eliminado", null);
            } 
            catch (Exception ex)
            {
                return new RespuestaJson(false, ex.Message.ToString(), null);
            } 
        }

        [HttpPut]
        public async Task<RespuestaJson> ActualizarImagen(FileUploadAPI objFile, int id)
        {
            try
            {
                if(objFile.files.Length > 0)
                {
                    string raiz = _env.WebRootPath + "\\Upload\\";
                    if(!Directory.Exists(raiz))
                    {
                        Directory.CreateDirectory(raiz);
                    }

                    var uniqueFileName = FileUploadAPI.GetUniqueFileName(objFile.files.FileName);
                    var uploads = Path.Combine(_env.WebRootPath, "Upload");
                    var filePath = Path.Combine(uploads,uniqueFileName);

                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        objFile.files.CopyTo(fileStream);
                        fileStream.Flush();

                        await Db.Connection.OpenAsync();
                        var query = new UsuarioQuery(Db);
                        var result = await query.BuscarUsuario(id);

                        if (result is null)
                            return new RespuestaJson(false, "Usuario no encontrado.", null);

                        result.imagenUsuario = uniqueFileName;
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
       

        [HttpPut]
        [Route("ImagenUsuario")]
        public async Task<RespuestaJson> ImagenUsuario([FromBody]Imagen filtro)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new UsuarioQuery(Db);
                var result = await query.BuscarUsuario(filtro.idactualizar);

                if (result is null)
                    return new RespuestaJson(false, "Usuario no encontrado.", null);

                var bytes = Convert.FromBase64String(filtro.base64image);
                
                string raiz = _env.WebRootPath + "\\Upload\\";
                if(!Directory.Exists(raiz))
                {
                    Directory.CreateDirectory(raiz);
                }

                var uniqueFileName = FileUploadAPI.GetUniqueFileName(
                    FileUploadAPI.GenerarExtension(result.loginUsuario));
                var uploads = Path.Combine(_env.WebRootPath, "Upload");
                var filePath = Path.Combine(uploads,uniqueFileName);


                if (bytes.Length > 0)
                {
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Flush();

                        result.imagenUsuario = uniqueFileName;
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

       
  
    }
}
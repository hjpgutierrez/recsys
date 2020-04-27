using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        public class FileUploadAPI
        {
            public IFormFile files {get; set;}
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarImagen(FileUploadAPI objFile, int id)
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

                    var uniqueFileName = GetUniqueFileName(objFile.files.FileName);
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
                            return new NotFoundResult();

                        result.ImagenUsuario = uniqueFileName;
                        await result.ActualizarImagen();
                        return  new OkObjectResult(result);
                    }
                }
                else
                {
                    return new NotFoundResult();
                }
            } 
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            } 
        }

        // GET api/usuario
                 
        [HttpGet]
        public async Task<IActionResult> cargar()
        {
            await Db.Connection.OpenAsync();
            var query = new UsuarioQuery(Db);
            var result = await query.BuscarUltimosUsuario();
            return new OkObjectResult(result);
        }

        // GET api/usuario/5
        [HttpGet("{id}")]
        public async Task<IActionResult> buscar(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new UsuarioQuery(Db);
            var result = await query.BuscarUsuario(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        [HttpGet("{usuario}/{pass}")]
        public async Task<IActionResult> login(string usuario, string pass)
        {
            await Db.Connection.OpenAsync();
            var query = new UsuarioQuery(Db);
            var result = await query.BuscarUsuario(usuario, pass);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/usuario
        [HttpPost]
        public async Task<IActionResult> agregar([FromBody]Usuario body)
        {
            try
            {
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertarUsuario();
                return new OkObjectResult(body);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            }            
        }

        // DELETE api/usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> eliminar(int id)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new UsuarioQuery(Db);
                var result = await query.BuscarUsuario(id);
                if (result is null)
                    return new NotFoundResult();
                await result.Eliminar();
                return new OkResult();
            } 
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.ToString());
            } 
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return  Path.GetFileNameWithoutExtension(fileName)
                    + "_" 
                    + Guid.NewGuid().ToString().Substring(0, 4) 
                    + Path.GetExtension(fileName);
        }
    }
}
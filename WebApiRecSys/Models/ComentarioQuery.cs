using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class ComentarioQuery
    {
        public AppDb Db { get; }

        public ComentarioQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Comentario>> CargarComentariosReceta(int IdReceta)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT l.*,CONCAT_WS(' ',u.`NombreUsuario`,u.`ApellidoUsuario`)'usuario' FROM comentario l INNER JOIN usuario u ON l.`IdUsuario`=u.`IdUsuario` WHERE IdReceta=@IdReceta;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdReceta",
                DbType = DbType.Int32,
                Value = IdReceta,
            });
            return await cargarTodos(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Comentario>> cargarTodos(DbDataReader reader)
        {
            var lista = new List<Comentario>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var item = new Comentario(Db)
                    {
                        IdComentario = reader.GetInt32(0),
                        Observacion = reader.GetString(1),    
                        IdUsuario = reader.GetInt32(2),
                        IdReceta = reader.GetInt32(3),
                        nombreUsuario = reader.GetString(4),                      
                    };
                    lista.Add(item);
                }
            }
            return lista;
        }
    }
}
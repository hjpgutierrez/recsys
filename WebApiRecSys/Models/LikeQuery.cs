using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class LikeQuery
    {
        public AppDb Db { get; }

        public LikeQuery(AppDb db)
        {
            Db = db;
        }
        
        public async Task<List<Like>> CargarLikesReceta(int IdReceta)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT l.*,CONCAT_WS(' ',u.`NombreUsuario`,u.`ApellidoUsuario`)'usuario' FROM likes l INNER JOIN usuario u ON l.`IdUsuario`=u.`IdUsuario` WHERE IdReceta=@IdReceta;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdReceta",
                DbType = DbType.Int32,
                Value = IdReceta,
            });
            return await cargarTodos(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Like>> cargarTodos(DbDataReader reader)
        {
            var lista = new List<Like>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var item = new Like(Db)
                    {
                        IdLike = reader.GetInt32(0),
                        IdUsuario = reader.GetInt32(1),
                        IdReceta = reader.GetInt32(2),
                        nombreUsuario = reader.GetString(3),                      
                    };
                    lista.Add(item);
                }
            }
            return lista;
        }
    }
}
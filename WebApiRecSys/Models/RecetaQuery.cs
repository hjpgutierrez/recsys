using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class RecetaQuery
    {
        public AppDb Db { get; }

        public RecetaQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Receta> BuscarRecetasUsuario(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT r.*,CONCAT_WS(' ',u.`NombreUsuario`,u.`ApellidoUsuario`)'usuario' FROM receta r INNER JOIN usuario u ON r.`IdUsuario`=u.`IdUsuario` WHERE r.`IdUsuario`=@IdUsuario ORDER BY r.`IdReceta` DESC;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdUsuario",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await cargarTodos(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Receta>> BuscarRecetas()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT r.*,CONCAT_WS(' ',u.`NombreUsuario`,u.`ApellidoUsuario`)'usuario' FROM receta r INNER JOIN usuario u ON r.`IdUsuario`=u.`IdUsuario` ORDER BY r.`IdReceta` DESC;";
            var listaRecetas = await cargarTodos(await cmd.ExecuteReaderAsync());
            foreach (Receta item in listaRecetas)
            {
                item.listaIngredientes = await new RecetaDetalleQuery(Db)
                        .BuscarDetallesRecetas(item.idReceta);
            }
            return listaRecetas;
        }

        private async Task<List<Receta>> cargarTodos(DbDataReader reader)
        {
            var lista = new List<Receta>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var item = new Receta(Db)
                    {
                        idReceta = reader.GetInt32(0),
                        nombreReceta = reader.GetString(1),
                        descripcion = reader.GetString(2),
                        idUsuario = reader.GetInt32(3),
                        valorTotal = reader.GetDouble(4),                        
                        ciudad = reader.GetString(5),
                        imagenReceta = reader.IsDBNull(6) ? null : reader.GetString(6),
                        nombreUsuario =  reader.GetString(7),                         
                    };
                    lista.Add(item);
                }
            }
            return lista;
        }
    }
}
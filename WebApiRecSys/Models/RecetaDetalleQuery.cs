using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class RecetaDetalleQuery
    {
        public AppDb Db { get; }

        public RecetaDetalleQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<RecetaDetalle>> BuscarDetallesRecetas(int IdReceta)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM detallereceta WHERE IdReceta=@IdReceta;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdReceta",
                DbType = DbType.Int32,
                Value = IdReceta,
            });
            return await cargarTodos(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<RecetaDetalle>> cargarTodos(DbDataReader reader)
        {
            var lista = new List<RecetaDetalle>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var item = new RecetaDetalle(Db)
                    {
                        idDetalleReceta = reader.GetInt32(0),
                        idReceta = reader.GetInt32(1),
                        nombreIngrediente = reader.GetString(2),
                        marca = reader.GetString(3),
                        cantidadIngrediente = reader.GetDouble(4),
                        medida = reader.GetString(5),
                        valorIngrediente = reader.GetDouble(6),
                        subtotal= reader.GetDouble(7),
                        direccion = reader.GetString(8),                        
                    };
                    lista.Add(item);
                }
            }
            return lista;
        }
    }
}
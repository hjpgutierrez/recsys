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
                        IdDetalleReceta = reader.GetInt32(0),
                        IdReceta = reader.GetInt32(1),
                        NombreIngrediente = reader.GetString(2),
                        Marca = reader.GetString(3),
                        CantidadIngrediente = reader.GetDouble(4),
                        Medida = reader.GetString(5),
                        ValorIngrediente = reader.GetDouble(6),
                        Subtotal= reader.GetDouble(7),
                        Direccion = reader.GetString(8),                        
                    };
                    lista.Add(item);
                }
            }
            return lista;
        }
    }
}
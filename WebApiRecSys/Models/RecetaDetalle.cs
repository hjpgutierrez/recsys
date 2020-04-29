using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class RecetaDetalle
    {
        public int idDetalleReceta { get; set; }
        public int idReceta { get; set; }
        public string nombreIngrediente { get; set; }
        public string marca { get; set; }
        public double cantidadIngrediente { get; set; }
        public string medida { get; set; }
        public double valorIngrediente { get; set; }
        public double subtotal { 
            get ; 
            set ;
        }
        public string direccion { get; set; }

        internal AppDb Db { get; set; }

        public RecetaDetalle()
        {
        }

        internal RecetaDetalle(AppDb db)
        {
            Db = db;
        }

        public async Task Insertar()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `detallereceta`
                                        (`IdReceta`,
                                        `NombreIngrediente`,
                                        `Marca`,
                                        `CantidadIngrediente`,
                                        `Medida`,
                                        `ValorIngrediente`,
                                        `Subtotal`,
                                        `Direccion`)
                                            VALUES (@IdReceta,
                                                    @NombreIngrediente,
                                                    @Marca,
                                                    @CantidadIngrediente,
                                                    @Medida,
                                                    @ValorIngrediente,
                                                    @Subtotal,
                                                    @Direccion);";
            BindearParametros(cmd);
            await cmd.ExecuteNonQueryAsync();
            this.idDetalleReceta = (int) cmd.LastInsertedId;
        }

        private void BindearParametros(MySqlCommand cmd)        
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdReceta",
                DbType = DbType.Int32,
                Value = idReceta,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@NombreIngrediente",
                DbType = DbType.String,
                Value = nombreIngrediente,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Marca",
                DbType = DbType.Int32,
                Value = marca,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CantidadIngrediente",
                DbType = DbType.Double,
                Value = cantidadIngrediente,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Medida",
                DbType = DbType.String,
                Value = medida,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Subtotal",
                DbType = DbType.Double,
                Value = subtotal,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ValorIngrediente",
                DbType = DbType.Double,
                Value = valorIngrediente,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Direccion",
                DbType = DbType.String,
                Value = direccion,
            });
        }

    }
}
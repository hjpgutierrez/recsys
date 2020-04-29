using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class RecetaDetalle
    {
        public int IdDetalleReceta { get; set; }
        public int IdReceta { get; set; }
        public string NombreIngrediente { get; set; }
        public string Marca { get; set; }
        public double CantidadIngrediente { get; set; }
        public string Medida { get; set; }
        public double ValorIngrediente { get; set; }
        public double Subtotal { 
            get ; 
            set ;
        }
        public string Direccion { get; set; }

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
            this.IdDetalleReceta = (int) cmd.LastInsertedId;
        }

        private void BindearParametros(MySqlCommand cmd)        
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdReceta",
                DbType = DbType.Int32,
                Value = IdReceta,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@NombreIngrediente",
                DbType = DbType.String,
                Value = NombreIngrediente,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Marca",
                DbType = DbType.Int32,
                Value = Marca,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CantidadIngrediente",
                DbType = DbType.Double,
                Value = CantidadIngrediente,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Medida",
                DbType = DbType.String,
                Value = Medida,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Subtotal",
                DbType = DbType.Double,
                Value = Subtotal,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ValorIngrediente",
                DbType = DbType.Double,
                Value = ValorIngrediente,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Direccion",
                DbType = DbType.String,
                Value = Direccion,
            });
        }

    }
}
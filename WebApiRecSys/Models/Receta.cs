using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class Receta
    {
        public int IdReceta { get; set; }
        public string NombreReceta { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public double ValorTotal { 
            get ;
            set ;
        }
        public string Ciudad { get; set; }
        public string ImagenReceta { get; set; }
        public List<RecetaDetalle> ListaIngredientes { get; set; }

        public string NombreUsuario { get; set; }

        internal AppDb Db { get; set; }

        public Receta()
        {
        }

        internal Receta(AppDb db)
        {
            Db = db;
        }

        public async Task Insertar()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `receta`
                                (`NombreReceta`,
                                `Descripcion`,
                                `IdUsuario`,
                                `ValorTotal`,
                                `Ciudad`)
                                VALUES (@NombreReceta,
                                        @Descripcion,
                                        @IdUsuario,
                                        @ValorTotal,
                                        @Ciudad);";
            BindearParametros(cmd);
            await cmd.ExecuteNonQueryAsync();
            this.IdReceta = (int) cmd.LastInsertedId;
            if(IdReceta > 0){
                foreach(RecetaDetalle item in ListaIngredientes){
                    item.IdReceta = this.IdReceta;
                    item.Db = this.Db;
                    await item.Insertar();
                }
            }
        }

        private void BindearParametros(MySqlCommand cmd)        
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@NombreReceta",
                DbType = DbType.String,
                Value = NombreReceta,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Descripcion",
                DbType = DbType.String,
                Value = Descripcion,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdUsuario",
                DbType = DbType.Int32,
                Value = IdUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ValorTotal",
                DbType = DbType.Double,
                Value = ValorTotal,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Ciudad",
                DbType = DbType.String,
                Value = Ciudad,
            });
        }

    }
}
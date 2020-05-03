using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class Like
    {
        public int IdLike { get; set; }
        public int IdUsuario { get; set; }
        public int IdReceta { get; set; }
        public string nombreUsuario { get; set; }

        internal AppDb Db { get; set; }

        public Like()
        {
        }

        internal Like(AppDb db)
        {
            Db = db;
        }

        public async Task Insertar()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `likes`
                                                (`IdUsuario`,
                                                `IdReceta`)
                                            VALUES (@IdUsuario,
                                                    @IdReceta);";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdUsuario",
                DbType = DbType.Int32,
                Value = IdUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdReceta",
                DbType = DbType.Int32,
                Value = IdReceta,
            });
            await cmd.ExecuteNonQueryAsync();
            IdLike = (int) cmd.LastInsertedId;
        }
        

        public async Task Eliminar()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `likes` WHERE `IdUsuario` = @IdUsuario AND `IdReceta` = @IdReceta;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdUsuario",
                DbType = DbType.Int32,
                Value = IdUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdReceta",
                DbType = DbType.Int32,
                Value = IdReceta,
            });
            await cmd.ExecuteNonQueryAsync();
        }


        
    }
}
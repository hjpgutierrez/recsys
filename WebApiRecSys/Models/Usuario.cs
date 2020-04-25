using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string LoginUsuario { get; set; }
        public string PasswordUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string DocumentoUsuario { get; set; }
        public string ImagenUsuario { get; set; }
        public int IdPerfil { get; set; }

        internal AppDb Db { get; set; }

        public Usuario()
        {
        }

        internal Usuario(AppDb db)
        {
            Db = db;
        }

        public async Task InsertarUsuario()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `usuario`
                                (`LoginUsuario`,
                                `PasswordUsuario`,
                                `NombreUsuario`,
                                `ApellidoUsuario`,
                                `DocumentoUsuario`,
                                `IdPerfil`)
                                VALUES (@LoginUsuario,
                                        @PasswordUsuario,
                                        @NombreUsuario,
                                        @ApellidoUsuario,
                                        @DocumentoUsuario,
                                        @IdPerfil);";
            BindearParametros(cmd);
            await cmd.ExecuteNonQueryAsync();
            IdUsuario = (int) cmd.LastInsertedId;
        }
        

        public async Task Eliminar()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `usuario` WHERE `IdUsuario` = @IdUsuario;";
            BindearId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarImagen()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `usuario` SET `ImagenUsuario` = @ImagenUsuario WHERE `IdUsuario` = @IdUsuario;";
            BindearImagenUsuario(cmd);
            BindearId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindearId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdUsuario",
                DbType = DbType.Int32,
                Value = IdUsuario,
            });
        }

        private void BindearParametros(MySqlCommand cmd)        
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LoginUsuario",
                DbType = DbType.String,
                Value = LoginUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PasswordUsuario",
                DbType = DbType.String,
                Value = PasswordUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@NombreUsuario",
                DbType = DbType.String,
                Value = NombreUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ApellidoUsuario",
                DbType = DbType.String,
                Value = ApellidoUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DocumentoUsuario",
                DbType = DbType.String,
                Value = DocumentoUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdPerfil",
                DbType = DbType.Int32,
                Value = IdPerfil,
            });
        }

        private void BindearImagenUsuario(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ImagenUsuario",
                DbType = DbType.String,
                Value = ImagenUsuario,
            });
        }
    }
}
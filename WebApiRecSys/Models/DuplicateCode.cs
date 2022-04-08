using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class DuplicateCode
    {
        public int idUsuario { get; set; }
        public string loginUsuario { get; set; }
        public string passwordUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string apellidoUsuario { get; set; }
        public string documentoUsuario { get; set; }
        public string imagenUsuario { get; set; }
        public int idPerfil { get; set; }

        internal AppDb Db { get; set; }

        public DuplicateCode()
        {
        }

        internal DuplicateCode(AppDb db)
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
            idUsuario = (int) cmd.LastInsertedId;
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
                Value = idUsuario,
            });
        }

        private void BindearParametros(MySqlCommand cmd)        
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LoginUsuario",
                DbType = DbType.String,
                Value = loginUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PasswordUsuario",
                DbType = DbType.String,
                Value = passwordUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@NombreUsuario",
                DbType = DbType.String,
                Value = nombreUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ApellidoUsuario",
                DbType = DbType.String,
                Value = apellidoUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DocumentoUsuario",
                DbType = DbType.String,
                Value = documentoUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdPerfil",
                DbType = DbType.Int32,
                Value = idPerfil,
            });
        }

        private void BindearParametrosDuplicado(MySqlCommand cmd, bool agregar)        
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LoginUsuario",
                DbType = DbType.String,
                Value = loginUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PasswordUsuario",
                DbType = DbType.String,
                Value = passwordUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@NombreUsuario",
                DbType = DbType.String,
                Value = nombreUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ApellidoUsuario",
                DbType = DbType.String,
                Value = apellidoUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DocumentoUsuario",
                DbType = DbType.String,
                Value = documentoUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdPerfil",
                DbType = DbType.Int32,
                Value = idPerfil,
            });
        }

        private void BindearImagenUsuario(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ImagenUsuario",
                DbType = DbType.String,
                Value = imagenUsuario,
            });
        }
    }
}
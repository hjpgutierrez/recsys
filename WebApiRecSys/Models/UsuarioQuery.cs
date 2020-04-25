using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class UsuarioQuery
    {
        public AppDb Db { get; }

        public UsuarioQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Usuario> BuscarUsuario(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `usuario` WHERE `IdUsuario` = @IdUsuario";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdUsuario",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await cargarTodos(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Usuario> BuscarUsuario(string usuario, string password)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `usuario` WHERE LoginUsuario = @LoginUsuario AND PasswordUsuario = @PasswordUsuario;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@LoginUsuario",
                DbType = DbType.String,
                Value = usuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@PasswordUsuario",
                DbType = DbType.String,
                Value = password,
            });
            var result = await cargarTodos(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Usuario>> BuscarUltimosUsuario()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `usuario` ORDER BY `IdUsuario` DESC LIMIT 10;";
            return await cargarTodos(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Usuario>> cargarTodos(DbDataReader reader)
        {
            var lista = new List<Usuario>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var usuario = new Usuario(Db)
                    {
                        IdUsuario = reader.GetInt32(0),
                        LoginUsuario = reader.GetString(1),
                        PasswordUsuario = reader.GetString(2),
                        NombreUsuario = reader.GetString(3),
                        ApellidoUsuario = reader.GetString(4),                        
                        DocumentoUsuario = reader.GetString(5),
                        ImagenUsuario = reader.IsDBNull(6) ? null : reader.GetString(6),
                        IdPerfil = reader.GetInt32(7),
                    };
                    lista.Add(usuario);
                }
            }
            return lista;
        }
    }
}
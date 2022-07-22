using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
  public class Comentario
  {
    public int IdComentario { get; set; }
    public string Observacion { get; set; }
    public int IdUsuario { get; set; }
    public int IdReceta { get; set; }
    public string nombreUsuario { get; set; }

    internal AppDb Db { get; set; }

    public Comentario()
    {
    }

    internal Comentario(AppDb db)
    {
      Db = db;
    }

    public async Task Insertar()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"INSERT INTO `comentario`
                                                (Observacion,
                                                `IdUsuario`,
                                                `IdReceta`)
                                            VALUES (@Observacion,
                                                    @IdUsuario,
                                                    @IdReceta);";
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@Observacion",
        DbType = DbType.String,
        Value = Observacion,
      });
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
      IdComentario = (int)cmd.LastInsertedId;
    }


    public async Task Eliminar()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"DELETE FROM `comentario` WHERE `IdComentario` = @IdComentario;";
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@IdComentario",
        DbType = DbType.Int32,
        Value = IdComentario,
      });
      await cmd.ExecuteNonQueryAsync();
    }

    public async Task Editar()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"DELETE FROM `comentario` WHERE `IdComentario` = @IdComentario;";
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@IdComentario",
        DbType = DbType.Int32,
        Value = IdComentario,
      });
      await cmd.ExecuteNonQueryAsync();
    }


  }
}

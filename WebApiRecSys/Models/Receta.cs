using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApiRecSys
{
    public class Receta
    {
        public int idReceta { get; set; }
        public string nombreReceta { get; set; }
        public string descripcion { get; set; }
        public int idUsuario { get; set; }
        public double valorTotal { 
            get ;
            set ;
        }
        public string ciudad { get; set; }
        public string imagenReceta { get; set; }
        public List<RecetaDetalle> listaIngredientes { get; set; }

        public string nombreUsuario { get; set; }

        public DateTime fechaCreacion { get; set; }

        public int diasCreacion { get; set; }

        public int cantidadLike { get; set; }

        public int cantidadComentario { get; set; }

        public List<Like> listaLikes { get; set; }

        public List<Comentario> listaComentario { get; set; }

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
                                `Ciudad`,
                                `FechaCreacion`)
                                VALUES (@NombreReceta,
                                        @Descripcion,
                                        @IdUsuario,
                                        @ValorTotal,
                                        @Ciudad,
                                        NOW());";
            BindearParametros(cmd);
            await cmd.ExecuteNonQueryAsync();
            this.idReceta = (int) cmd.LastInsertedId;
            if(idReceta > 0){
                foreach(RecetaDetalle item in listaIngredientes){
                    item.idReceta = this.idReceta;
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
                Value = nombreReceta,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Descripcion",
                DbType = DbType.String,
                Value = descripcion,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@IdUsuario",
                DbType = DbType.Int32,
                Value = idUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ValorTotal",
                DbType = DbType.Double,
                Value = valorTotal,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Ciudad",
                DbType = DbType.String,
                Value = ciudad,
            });
        }

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using AddJson.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace AddJson
{
    class Program
    {
        public static SqlConnection _connection;
        public static SqlCommand _command;
        public static SqlDataReader _read;

        static void Main(string[] args)
        {
          
            var query = new List<string>();
            var strJson = File.ReadAllText("Seguro Vida.jsonc");
            var dataJson = JsonConvert.DeserializeObject<List<l>>(strJson);
             



            foreach (var obj in dataJson)
            {
                foreach (var capitulo in obj.Capitulo)
                {
                    capitulo.IdCapitulo = Guid.NewGuid().ToString();
                    query.Add($"INSERT INTO {capitulo.GetType().Name} (IdCapitulo,Designacao,NumCapitulo,LinhaProdutoId) VALUES('{capitulo.IdCapitulo}','{capitulo.Designacao}','{capitulo.NumCapitulo}','E7C1619E-0C4C-4CFB-AD5D-E6CBF8038F2B')");
                   executeSQL($"INSERT INTO {capitulo.GetType().Name} (IdCapitulo,Designacao,NumCapitulo,LinhaProdutoId) VALUES('{capitulo.IdCapitulo}','{capitulo.Designacao}','{capitulo.NumCapitulo}','E7C1619E-0C4C-4CFB-AD5D-E6CBF8038F2B')");
                    foreach (var artigo in capitulo.Artigo)
                    {
                        artigo.IdArtigo=Guid.NewGuid().ToString();
                        query.Add($"INSERT INTO {artigo.GetType().Name} (IdArtigo,Titulo,NumOrdemArtigo,CapituloId) VALUES('{artigo.IdArtigo}','{artigo.Titulo}','{artigo.NumOrdemArtigo}','{capitulo.IdCapitulo}')");
                        executeSQL($"INSERT INTO {artigo.GetType().Name} (IdArtigo,Titulo,NumOrdemArtigo,CapituloId) VALUES('{artigo.IdArtigo}','{artigo.Titulo}','{artigo.NumOrdemArtigo}','{capitulo.IdCapitulo}')");
                     foreach(var clausula in artigo.Clausula)
                     {
                         clausula.IdClausula=Guid.NewGuid().ToString();
                         query.Add($"INSERT INTO {clausula.GetType().Name} (IdClausula,Titulo,NumClausula,ArtigoId) VALUES('{clausula.IdClausula}','{clausula.Descricao}','{clausula.NumDescircao}','{artigo.IdArtigo}')");
                      executeSQL($"INSERT INTO {clausula.GetType().Name} (IdClausula,Titulo,NumClausula,ArtigoId) VALUES('{clausula.IdClausula}','{clausula.Descricao}','{clausula.NumDescircao}','{artigo.IdArtigo}')");
                       if(clausula.PontosClausula!=null)
                       {
                         foreach(var pontoClausula in clausula.PontosClausula)
                         {
                              pontoClausula.IdPontosClausula=Guid.NewGuid().ToString();
                           query.Add($"INSERT INTO {pontoClausula.GetType().Name} (IdPontosClausula,Conteudo,ClausulaID) VALUES('{pontoClausula.IdPontosClausula}','{clausula.Descricao}','{clausula.IdClausula}')");
                         executeSQL($"INSERT INTO {pontoClausula.GetType().Name} (IdPontosClausula,Conteudo,ClausulaID) VALUES('{pontoClausula.IdPontosClausula}','{clausula.Descricao}','{clausula.IdClausula}')");
                         }
                       }
                       
                     }
                    }

                }
            }
            
            var strQuery=JsonConvert.SerializeObject(query);
            Console.WriteLine(dataJson);
        }
        public static void Connection()
        {
            _connection = new SqlConnection("Data Source=172.16.16.53;Initial Catalog=DBIS_PRE_PROD;User ID=sa;Password=snirdb!2020;MultipleActiveResultSets=True;");
            _connection.Open();
        }
         public static SqlDataReader Exec(string exec)
        {
            Connection();
            _command = new SqlCommand($"{exec}", _connection);
            _command.Connection = _connection;
            _read = _command.ExecuteReader();
            //this.Desconectar();
            return _read;
        }
        public static void executeSQL(string query)
        {
             Connection();
            _command = new SqlCommand(query, _connection);
            //this.Desconectar();
            _command.ExecuteNonQuery();
            _connection.Close();
            
        }
        
    } 
}

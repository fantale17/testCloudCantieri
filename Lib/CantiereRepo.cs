using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Lib
{
    public class CantiereRepo
    {
        private readonly string _cs;

        public CantiereRepo(string cs)
        {
            _cs = cs;
        }


        public async Task<IEnumerable<Cantiere>> GetAllCantieri()
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = "SELECT Id, NomeCliente, EmailCliente, Luogo from Cantiere";

                return await connection.QueryAsync<Cantiere>(query);
            }
        }


        public async Task<Cantiere> Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = "SELECT Id, NomeCliente, EmailCliente, Luogo from Cantiere";

                return await connection.QueryFirstOrDefaultAsync<Cantiere>(query);
            }
        }


        public async Task CreateCantiere(Cantiere c)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = @"INSERT INTO [dbo].[Cantiere]
                        (
                        [NomeCliente]
                        ,[EmailCliente]
                        ,[Luogo])
                    VALUES
                        (
                        @NomeCliente,
                        @EmailCliente,
                        @Luogo);
                   SELECT SCOPE_IDENTITY()";

                c.Id = await connection.QueryFirstOrDefaultAsync<int>(query, c);
                foreach (var photoUri in c.UriPhotos)
                {
                    var query2 = "INSERT INTO dbo.FotoCantiere (IdCantiere, UriPhoto) VALUES (@IdCantiere, @UriPhoto)";
                    await connection.QueryAsync(query2, new {IdCantiere = c.Id, UriPhoto = photoUri});
                }
            }
        }


        public async Task<List<string>> GetPhotos(int idCantiere)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = "SELECT UriPhoto from FotoCantiere where IdCantiere = @Id";

                return (await connection.QueryAsync<string>(query, new { Id = idCantiere})).ToList();
            }
        }
    }
}

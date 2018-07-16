using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Lib
{
    public class InterventoRepo
    {
        private readonly string _cs;

        public InterventoRepo(string cs)
        {
            _cs = cs;
        }


        public async Task<IEnumerable<Intervento>> GetAllInterventi()
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = "SELECT Id, IdCantiere, Tipo, Note, StimaCosto, UriPhoto from Intervento";

                return await connection.QueryAsync<Intervento>(query);
            }
        }


        public async Task<IEnumerable<InterventoEsterno>> GetAllInterventiFiltered(int type)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = @"SELECT I.Id, C.NomeCliente, C.EmailCliente, C.Luogo
                from Intervento as I join Cantiere as C on C.Id = I.IdCantiere where Tipo = @Tipo";

                return await connection.QueryAsync<InterventoEsterno>(query, new {Tipo = type});
            }
        }

        public async Task InsertIntervento(Intervento i)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = @"INSERT INTO [dbo].[Intervento]
                        ([IdCantiere]
                            ,[Tipo]
                            ,[Note]
                            ,[StimaCosto]
                            ,[UriPhoto])
                        VALUES
                            (@IdCantiere
                            ,@Tipo
                            ,@Note
                            ,@StimaCosto
                            ,@UriPhoto)";

                await connection.QueryAsync<Intervento>(query, i);
            }
        }


        public async Task<InterventoEsterno> GetDatiInterventoEsterno(int id)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = @"SELECT I.Note, I.StimaCosto, C.NomeCliente, C.EmailCliente, C.Luogo
                            from Intervento as I join Cantiere as C on C.Id = I.IdCantiere where I.Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<InterventoEsterno>(query, new {Id = id});
            }
        }


        public async Task<Intervento> GetDetailsIntervento(int id)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = @"SELECT Id, IdCantiere, Tipo, Note, StimaCosto, UriPhoto
                            from Intervento where Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<Intervento>(query, new { Id = id });
            }
        }

    }
}

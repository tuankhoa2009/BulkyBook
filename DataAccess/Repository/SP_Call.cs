using BulkyBook.DataAccess.Data;
using Dapper;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataAccess.Repository
{
    public class SP_Call : ISP_Call
    {
        private readonly ApplicationDbContext _db;
        private static string ConnectionString = "";

        public SP_Call(ApplicationDbContext db)
        {
            _db = db;
        }

        public T Single<T>(string produreName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
               return (T)Convert.ChangeType(sqlCon.ExecuteScalar<T>(produreName, param, commandType: System.Data.CommandType.StoredProcedure),typeof(T));
            }
        }

        public void Execute(string produreName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(produreName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public T OneRecord<T>(string produreName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var value= sqlCon.Query<T>(produreName, param, commandType: System.Data.CommandType.StoredProcedure);
                return (T)Convert.ChangeType(value.FirstOrDefault(),typeof(T));
            }
        }

        public IEnumerable<T> List<T>(string produreName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(produreName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string produreName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                 var result = SqlMapper.QueryMultiple(sqlCon,produreName, param, commandType: System.Data.CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if(item1 != null && item2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }

            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new  List<T1>(),new List<T2>());
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

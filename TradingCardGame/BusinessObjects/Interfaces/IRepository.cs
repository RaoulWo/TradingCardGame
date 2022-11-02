using BusinessObjects.Base;
using Npgsql;

namespace BusinessObjects.Interfaces;

public interface IRepository<T> where T : Entity, IAggregateRoot
{
    IEnumerable<T> GetAll(string getAllSql);
    T GetById(int id, string getByIdSql);
    int Insert(T entity, string insertSql, NpgsqlTransaction sqlTransaction);
    int Update(T entity, string updateSql, NpgsqlTransaction sqlTransaction);
    int DeleteById(int id, string deleteSql, NpgsqlTransaction sqlTransaction);
}
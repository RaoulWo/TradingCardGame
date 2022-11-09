using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using Npgsql;

namespace DataAccess.Repositories;

public class CollectionRepository : Repository<CollectionEntity>
{
    public static CollectionRepository Instance
    {
        get
        {
            _instance ??= new CollectionRepository(UnitOfWork.Instance);

            return _instance;
        }
    }

    private static CollectionRepository _instance = null;

    public CollectionRepository(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    { }

    /// <summary>
    /// Maps data for populating all statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override List<CollectionEntity> Maps(NpgsqlDataReader reader)
    {
        List<CollectionEntity> collections = new List<CollectionEntity>();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                CollectionEntity collection = new CollectionEntity();

                collection.Id = new Guid(reader["Id"].ToString());
                collection.PlayerId = new Guid(reader["PlayerId"].ToString());
                collection.CardId = new Guid(reader["CardId"].ToString());

                collections.Add(collection);
            }
        }

        return collections;
    }

    /// <summary>
    /// Maps data for populating by key statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override CollectionEntity Map(NpgsqlDataReader reader)
    {
        CollectionEntity collection = new CollectionEntity();

        if (reader.HasRows)
        {
            while (reader.Read())
            { 
                collection.Id = new Guid(reader["Id"].ToString());
                collection.PlayerId = new Guid(reader["PlayerId"].ToString());
                collection.CardId = new Guid(reader["CardId"].ToString());
            }
        }

        return collection;
    }

    /// <summary>
    /// Passes the parameters for populating by key statement.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="cmd"></param>
    protected override void GetByGuidCommandParameters(Guid guid, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", guid.ToString());
    }

    /// <summary>
    /// Passes the parameters for insert statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void InsertCommandParameters(CollectionEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("@PlayerId", entity.PlayerId.ToString());
        cmd.Parameters.AddWithValue("@CardId", entity.CardId.ToString());
    }

    /// <summary>
    /// Passes the parameters for update statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void UpdateCommandParameters(CollectionEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", entity.Id.ToString());
        cmd.Parameters.AddWithValue("@PlayerId", entity.PlayerId.ToString());
        cmd.Parameters.AddWithValue("@CardId", entity.CardId.ToString());
    }

    /// <summary>
    /// Passes the parameters for delete statement.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="cmd"></param>
    protected override void DeleteByGuidCommandParameters(Guid guid, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", guid.ToString());
    }
}
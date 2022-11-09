using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using Npgsql;

namespace DataAccess.Repositories;

public class DeckRepository : Repository<DeckEntity>
{
    public static DeckRepository Instance
    {
        get
        {
            _instance ??= new DeckRepository(UnitOfWork.Instance);

            return _instance;
        }
    }

    private static DeckRepository _instance = null;

    public DeckRepository(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    { }

    /// <summary>
    /// Maps data for populating all statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override List<DeckEntity> Maps(NpgsqlDataReader reader)
    {
        List<DeckEntity> decks = new List<DeckEntity>();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                DeckEntity deck = new DeckEntity();

                deck.Id = new Guid(reader["Id"].ToString());
                deck.PlayerId = new Guid(reader["PlayerId"].ToString());
                deck.CollectionId = new Guid(reader["CollectionId"].ToString());

                decks.Add(deck);
            }
        }

        return decks;
    }

    /// <summary>
    /// Maps data for populating by key statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override DeckEntity Map(NpgsqlDataReader reader)
    {
        DeckEntity deck = new DeckEntity();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                deck.Id = new Guid(reader["Id"].ToString());
                deck.PlayerId = new Guid(reader["PlayerId"].ToString());
                deck.CollectionId = new Guid(reader["CollectionId"].ToString());
            }
        }

        return deck;
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
    protected override void InsertCommandParameters(DeckEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("@PlayerId", entity.PlayerId.ToString());
        cmd.Parameters.AddWithValue("@CollectionId", entity.CollectionId.ToString());
    }

    /// <summary>
    /// Passes the parameters for update statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void UpdateCommandParameters(DeckEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", entity.Id.ToString());
        cmd.Parameters.AddWithValue("@PlayerId", entity.PlayerId.ToString());
        cmd.Parameters.AddWithValue("@CollectionId", entity.CollectionId.ToString());
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
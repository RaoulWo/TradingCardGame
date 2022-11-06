using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using Npgsql;

namespace DataAccess.Repositories;

public class PlayerRepository : Repository<PlayerEntity>
{
    public static PlayerRepository Instance
    {
        get
        {
            _instance ??= new PlayerRepository(UnitOfWork.Instance);

            return _instance;
        }
    }

    private static PlayerRepository _instance = null;

    public PlayerRepository(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    { }

    /// <summary>
    /// Maps data for populating all statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override List<PlayerEntity> Maps(NpgsqlDataReader reader)
    {
        List<PlayerEntity> players = new List<PlayerEntity>();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                PlayerEntity player = new PlayerEntity();

                player.Id = new Guid(reader["Id"].ToString());
                player.Name = reader["Name"].ToString();
                player.Password = reader["Password"].ToString();
                player.Coins = Convert.ToInt32(reader["Coins"].ToString());

                players.Add(player);
            }
        }

        return players;
    }

    /// <summary>
    /// Maps data for populating by key statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override PlayerEntity Map(NpgsqlDataReader reader)
    {
        PlayerEntity player = new PlayerEntity();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                player.Id = new Guid(reader["Id"].ToString());
                player.Name = reader["Name"].ToString();
                player.Password = reader["Password"].ToString();
                player.Coins = Convert.ToInt32(reader["Coins"].ToString());
            }
        }

        return player;
    }

    /// <summary>
    /// Passes the parameters for populating by key statement.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="cmd"></param>
    protected override void GetByGuidCommandParameters(Guid guid, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", guid);
    }

    /// <summary>
    /// Passes the parameters for insert statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void InsertCommandParameters(PlayerEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Password", entity.Password);
        cmd.Parameters.AddWithValue("@Coins", entity.Coins);
    }

    /// <summary>
    /// Passes the parameters for update statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void UpdateCommandParameters(PlayerEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Password", entity.Password);
        cmd.Parameters.AddWithValue("@Coins", entity.Coins);
    }

    /// <summary>
    /// Passes the parameters for delete statement.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="cmd"></param>
    protected override void DeleteByGuidCommandParameters(Guid guid, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", guid);
    }
}
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
                player.Wins = Convert.ToInt32(reader["Wins"].ToString());
                player.Losses = Convert.ToInt32(reader["Losses"].ToString());
                player.Draws = Convert.ToInt32(reader["Draws"].ToString());
                player.Elo = Convert.ToInt32(reader["Elo"].ToString());

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
                player.Wins = Convert.ToInt32(reader["Wins"].ToString());
                player.Losses = Convert.ToInt32(reader["Losses"].ToString());
                player.Draws = Convert.ToInt32(reader["Draws"].ToString());
                player.Elo = Convert.ToInt32(reader["Elo"].ToString());
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
        cmd.Parameters.AddWithValue("@Id", guid.ToString());
    }

    /// <summary>
    /// Passes the parameters for insert statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void InsertCommandParameters(PlayerEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Password", entity.Password);
        cmd.Parameters.AddWithValue("@Coins", 20);
        cmd.Parameters.AddWithValue("@Wins", 0);
        cmd.Parameters.AddWithValue("@Losses", 0);
        cmd.Parameters.AddWithValue("@Draws", 0);
        cmd.Parameters.AddWithValue("@Elo", 100);
    }

    /// <summary>
    /// Passes the parameters for update statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void UpdateCommandParameters(PlayerEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", entity.Id.ToString());
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Password", entity.Password);
        cmd.Parameters.AddWithValue("@Coins", entity.Coins);
        cmd.Parameters.AddWithValue("@Wins", entity.Wins);
        cmd.Parameters.AddWithValue("@Losses", entity.Losses);
        cmd.Parameters.AddWithValue("@Draws", entity.Draws);
        cmd.Parameters.AddWithValue("@Elo", entity.Elo);
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
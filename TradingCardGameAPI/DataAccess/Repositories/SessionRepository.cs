using BusinessObjects.Entities;
using BusinessObjects.Interfaces;
using Npgsql;

namespace DataAccess.Repositories;

public class SessionRepository : Repository<SessionEntity>
{
    public static SessionRepository Instance
    {
        get
        {
            _instance ??= new SessionRepository(UnitOfWork.Instance);

            return _instance;
        }
    }

    private static SessionRepository _instance = null;

    public SessionRepository(IUnitOfWork unitOfWork)
        : base (unitOfWork) { }

    /// <summary>
    /// Maps data for populating all statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override List<SessionEntity> Maps(NpgsqlDataReader reader)
    {
        List<SessionEntity> sessions = new List<SessionEntity>();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                SessionEntity session = new SessionEntity();

                session.Id = new Guid(reader["Id"].ToString());
                session.FkPlayerId = new Guid(reader["FkPlayerId"].ToString());

                sessions.Add(session);
            }
        }

        return sessions;
    }

    /// <summary>
    /// Maps data for populating by key statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override SessionEntity Map(NpgsqlDataReader reader)
    {
        SessionEntity session = new SessionEntity();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                session.Id = new Guid(reader["Id"].ToString());
                session.FkPlayerId = new Guid(reader["FkPlayerId"].ToString());
            }
        }

        return session;
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
    protected override void InsertCommandParameters(SessionEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", entity.Id.ToString());
        cmd.Parameters.AddWithValue("@FkPlayerId", entity.FkPlayerId.ToString());
    }

    /// <summary>
    /// Passes the parameters for update statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void UpdateCommandParameters(SessionEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        cmd.Parameters.AddWithValue("@FkPlayerId", entity.FkPlayerId);
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
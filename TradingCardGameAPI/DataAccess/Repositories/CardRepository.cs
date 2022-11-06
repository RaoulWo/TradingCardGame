using BusinessObjects.Entities;
using BusinessObjects.Enums;
using BusinessObjects.Interfaces;
using Npgsql;

namespace DataAccess.Repositories;

public class CardRepository : Repository<CardEntity>
{
    public static CardRepository Instance
    {
        get
        {
            _instance ??= new CardRepository(UnitOfWork.Instance);

            return _instance;
        }
    }

    private static CardRepository _instance = null;

    public CardRepository(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    { }

    /// <summary>
    /// Maps data for populating all statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override List<CardEntity> Maps(NpgsqlDataReader reader)
    {
        List<CardEntity> cards = new List<CardEntity>();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                CardEntity card = new CardEntity();

                card.Id = new Guid(reader["Id"].ToString());
                card.Name = reader["Name"].ToString();

                Enum.TryParse(reader["Type"].ToString(), out CardType type);
                card.Type = type;

                Enum.TryParse(reader["Element"].ToString(), out CardElement element);
                card.Element = element;

                card.Damage = Convert.ToInt32(reader["Damage"].ToString());
                card.Health = Convert.ToInt32(reader["Health"].ToString());
                card.Cost = Convert.ToInt32(reader["Cost"].ToString());

                cards.Add(card);
            }
        }

        return cards;
    }

    /// <summary>
    /// Maps data for populating by key statement.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected override CardEntity Map(NpgsqlDataReader reader)
    {
        CardEntity card = new CardEntity();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                card.Id = new Guid(reader["Id"].ToString());
                card.Name = reader["Name"].ToString();

                Enum.TryParse(reader["Type"].ToString(), out CardType type);
                card.Type = type;

                Enum.TryParse(reader["Element"].ToString(), out CardElement element);
                card.Element = element;

                card.Damage = Convert.ToInt32(reader["Damage"].ToString());
                card.Health = Convert.ToInt32(reader["Health"].ToString());
                card.Cost = Convert.ToInt32(reader["Cost"].ToString());
            }
        }

        return card;
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
    protected override void InsertCommandParameters(CardEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Type", entity.Type.ToString());
        cmd.Parameters.AddWithValue("@Element", entity.Element.ToString());
        cmd.Parameters.AddWithValue("@Damage", entity.Damage.ToString());
        cmd.Parameters.AddWithValue("@Health", entity.Health.ToString());
        cmd.Parameters.AddWithValue("@Cost", entity.Cost.ToString());
    }

    /// <summary>
    /// Passes the parameters for update statement.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cmd"></param>
    protected override void UpdateCommandParameters(CardEntity entity, NpgsqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Type", entity.Type.ToString());
        cmd.Parameters.AddWithValue("@Element", entity.Element.ToString());
        cmd.Parameters.AddWithValue("@Damage", entity.Damage.ToString());
        cmd.Parameters.AddWithValue("@Health", entity.Health.ToString());
        cmd.Parameters.AddWithValue("@Cost", entity.Cost.ToString());
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
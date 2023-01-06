using BusinessObjects.Entities;

namespace BusinessObjects.Interfaces.Services;

public interface IPlayerService
{
    bool CheckIfUsernameIsAvailable(string username);
}
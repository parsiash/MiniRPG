using MiniRPG.Common;
using MiniRPG.Metagame;

namespace MiniRPG
{
    public interface IPlayerDataRepository
    {
        UserProfile LoadUserProfile();
        void SaveUserProfile(UserProfile profile);
    }

    public class PlayerDataRepository : IPlayerDataRepository
    {
        private IObjectStorage _objectStorage;
        private ILogger _logger;

        public PlayerDataRepository(IObjectStorage objectStorage, ILogger logger)
        {
            _objectStorage = objectStorage;
            _logger = logger;
        }

        public UserProfile LoadUserProfile()
        {
            return _objectStorage.LoadObject<UserProfile>();
        }

        public void SaveUserProfile(UserProfile profile)
        {
            _objectStorage.SaveObject<UserProfile>(profile);
        }
    }
} 
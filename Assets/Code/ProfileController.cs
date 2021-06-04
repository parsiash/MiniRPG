using System.Collections.Generic;
using MiniRPG.Common;
using MiniRPG.Metagame;

namespace MiniRPG
{
    public class ProfileController : IProfileController
    {
        private UserProfile _profile;
        public UserProfile Profile => _profile;
        private IPlayerDataRepository _playerDataRepository;
        private ILogger _logger;
        private IList<IProfileUpdateListener> _updateListeners;

        public ProfileController(UserProfile profile, IPlayerDataRepository playerDataRepository, ILogger logger)
        {
            _profile = profile;
            _playerDataRepository = playerDataRepository;
            _logger = logger;

            _updateListeners = new List<IProfileUpdateListener>();
        }

        public void AddListener(IProfileUpdateListener listener)
        {
            _updateListeners.Add(listener);
        }

        public void RemoveListener(IProfileUpdateListener listener)
        {
            _updateListeners.Remove(listener);
        }

        public bool Update(IProfileUpdate update)
        {
            var success = update.Apply(_profile);
            if(!success)
            {
                return false;
            }

            _playerDataRepository.SaveUserProfile(_profile);

            foreach(var updateListener in _updateListeners)
            {
                updateListener?.OnProfileUpdate(update);
            }
            return true;
        }
    }
} 
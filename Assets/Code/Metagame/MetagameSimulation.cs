using MiniRPG.Common;

namespace MiniRPG.Metagame
{
    public interface IMetagameSimulation
    {
        IUser User { get; }
    }

    public interface IUser
    {
        UserProfile Profile { get; }
    }

    public class User : IUser
    {
        private UserProfile _profile;
        public UserProfile Profile => _profile;

        public User(UserProfile profile)
        {
            _profile = profile;
        }
    }

    public class MetagameSimulation : IMetagameSimulation
    {
        private IUser _user;
        public IUser User => _user;

        private ILogger _logger;

        public MetagameSimulation(IUser user, ILogger logger)
        {
            _user = user;
            _logger = logger;
        }
    }
}
namespace MiniRPG.Logic.Metagame
{
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
}
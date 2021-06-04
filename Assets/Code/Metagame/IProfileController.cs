namespace MiniRPG.Metagame
{
    public interface IProfileController
    {
        UserProfile Profile { get; }
        bool Update(IProfileUpdate update);
        void AddListener(IProfileUpdateListener listener);
        void RemoveListener(IProfileUpdateListener listener);
    }
} 
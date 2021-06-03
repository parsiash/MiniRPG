namespace MiniRPG.Metagame
{
    public interface IProfileController
    {
        bool Update(IProfileUpdate update);
        void AddListener(IProfileUpdateListener listener);
        void RemoveListener(IProfileUpdateListener listener);
    }
} 
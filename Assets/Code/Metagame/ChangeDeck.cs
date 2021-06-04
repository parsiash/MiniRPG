namespace MiniRPG.Metagame
{
    public class ChangeDeck : IProfileUpdate
    {
        public ProfileDeck newDeck { get; private set; }

        public ChangeDeck(ProfileDeck newDeck)
        {
            this.newDeck = newDeck;
        }

        public bool Apply(UserProfile profile)
        {
            profile.deck = newDeck;
            return true;    
        }
    }
} 
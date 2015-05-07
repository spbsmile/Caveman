namespace Caveman.Players
{
    public class Player
    {
        public int kills;
        public int deaths;
        public int weapons;
        public readonly string name;

        public Player(string name)
        {
            this.name = name;
        }
    }
}

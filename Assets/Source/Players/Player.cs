namespace Caveman.Players
{
    public class Player
    {
        public int killed;
        public int death;
        public int weapons;
        public readonly string name;

        private int id;

        public Player(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
    }
}

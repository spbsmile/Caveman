public class SocketMessagePool
{
    private static SocketMessagePool instance;

    private SocketMessagePool()
    {
    }

    private static SocketMessagePool Instance
    {
        get
        {
            if (instance == null)
                instance = new SocketMessagePool();
            return instance;
        }
    }
}
public class SocketMessagePool {

    private SocketMessagePool()
    {
    }

    static private SocketMessagePool instance;
    static SocketMessagePool Instance
    {
        get
        {
            if (instance == null)
                instance = new SocketMessagePool();
            return instance;
        }
    }

}

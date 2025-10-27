using P2PPlugin.Network;

public class PongGame
{
    public static PongGame Instance = null;

    public long ID;


    public PongGame()
    {
        Instance = this;
    }
}

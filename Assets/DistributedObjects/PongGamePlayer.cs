using P2PPlugin.Network;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PongGamePlayer : P2PNetworkObject
{
    public static PongGamePlayer localPongGame = null;

    [P2PDistributed]
    public int pongGameUID;

    [P2PToPeer]
    public long toPeer;

    [P2PDistributed]
    public int playerNumber;   // 0 - left/primary, 1 - right/secondary

    public void AfterInsertRemote()
    {
        if (localPongGame == null)
        {
            localPongGame = new PongGamePlayer();
            localPongGame.pongGameUID = pongGameUID;
            localPongGame.toPeer = sourceComputerID;
            localPongGame.playerNumber = 1;  // right/secondary
            localPongGame.Insert();
        }
    }
    public void AfterDeleteRemote()
    {
        if (localPongGame != null)
        {
            BackToLobby.Instance.BackToLobbyAction();
        }
    }
    public PongGamePlayer()
    {
    }

    static public void CreateLocalPongGamePlayer(long toPeerID)
    {
        localPongGame = new PongGamePlayer();
        localPongGame.toPeer = toPeerID;
        localPongGame.playerNumber = 0;
        localPongGame.pongGameUID = (int)P2PObject.IntRandFunc();
        localPongGame.Insert();
    }
}

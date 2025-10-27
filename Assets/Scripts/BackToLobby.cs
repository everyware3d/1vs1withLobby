using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobby : MonoBehaviour
{
    public static BackToLobby Instance = null;

    public BackToLobby()
    {
        Instance = this;
    }
    private void Start()
    {
    }
    public void BackToLobbyAction()
    {
        PeerLobbyScript.isPlaying = false;
        if (PongGamePlayer.localPongGame != null)
        {
            PongGamePlayer.localPongGame.Delete();
            PongGamePlayer.localPongGame = null;
            SceneManager.LoadScene("Lobby");            
        }
    }
}

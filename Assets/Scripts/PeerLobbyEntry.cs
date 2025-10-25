using UnityEngine;

public class PeerLobbyEntry : MonoBehaviour
{
    public GameObject PeerNameTextGO;
    public GameObject RequestOrJoinButtonGO;
    public GameObject RequestOrJoinButtonTextGO;

    [HideInInspector]
    public long peerComputerID;

    public void RequestOrJoinButtonClicked()
    {
        PeerLobbyScript.Instance.RequestToPlayWithPeer(peerComputerID);
    }
}

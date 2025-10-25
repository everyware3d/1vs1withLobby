using System;
using System.Collections.Generic;
using P2PPlugin.Network;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyPeer : P2PNetworkObject
{

    public LobbyPeer()
    {
    }
    [P2PDistributed]
    public long timeCreated = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //only set for source, then distributed

    [P2PSkip]
    private long _requestedToPeer = 0;  // This LobbyPeer is requesting to the peerID
    [P2PDistributed]
    public long RequestedToPeer
    {
        get
        {
            return _requestedToPeer;
        }
        set
        {
            long prevPeerID = _requestedToPeer;
            _requestedToPeer = value;
            if (getIsLocal())
            {
                UpdateAllFields();
            } else
            {
                if (value == 0)
                {
                    if (prevPeerID == P2PObject.peerComputerID)
                    {
                        // if this is cancelling the request
                        PeerLobbyScript.Instance.CancelRequestFromPeerToPlay();
                    } else
                    {
                        PeerLobbyScript.Instance.UpdateRemotePeerButtons();
                    }
                } else if (value == P2PObject.peerComputerID)
                {
                    // request is coming from remote peer
                    PeerLobbyScript.Instance.RequestFromPeerToPlay(sourceComputerID);
                } else
                {
                    PeerLobbyScript.Instance.UpdateRemotePeerButtons();
                }
            }
        }
    }

    static private Dictionary<int, Action<bool, LobbyPeer>> LobbyPeerChanged = new Dictionary<int, Action<bool, LobbyPeer>>();
    static private int maxKeyForPeers = 0;
    static public void fireLobbyPeerChanged(bool addOrRemoved, LobbyPeer LobbyPeer)
    {
        foreach (Action<bool, LobbyPeer> callback in LobbyPeerChanged.Values)
        {
            callback(addOrRemoved, LobbyPeer);
        }
    }
    static public int addP2PChangeListener(Action<bool, LobbyPeer> callback)
    {
        int key = ++maxKeyForPeers;
        LobbyPeerChanged.Add(key, callback);
        return key;
    }
    static public int removeP2PChangeListener(int key)
    {
        LobbyPeerChanged.Remove(key);
        return key;
    }

    public void AfterInsertRemote()
    {

        fireLobbyPeerChanged(true, this);
    }
    public void AfterDeleteRemote()
    {
        fireLobbyPeerChanged(false, this);
    }

    [P2PDistributed(P2PMethodType.ToOwner)]
    public void DeclineRequest(long fromPeerID)
    {
        PeerLobbyScript.Instance.CancelRequestToPeerToPlay(fromPeerID);
        RequestedToPeer = 0;
    }
    public void DeclineRequestInvoke(long fromPeerID)
    {
        Invoke("DeclineRequest", new object[] { fromPeerID });
    }
    [P2PDistributed(P2PMethodType.ToOwner)]
    public void AcceptedRequest(long fromPeerID)
    {
        PeerLobbyScript.Instance.AcceptedRequestToPeerToPlay(fromPeerID);
    }

    public void AcceptedRequestInvoke(long fromPeerID)
    {
        Invoke("AcceptedRequest", new object[] { fromPeerID });
    }

}
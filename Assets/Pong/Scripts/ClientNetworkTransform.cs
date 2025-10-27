// using Unity.Netcode.Components;
using UnityEngine;

[DisallowMultipleComponent]
public class ClientNetworkTransform : MonoBehaviour
// public class ClientNetworkTransform : NetworkTransform
{
    protected bool OnIsServerAuthoritative()
    // protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
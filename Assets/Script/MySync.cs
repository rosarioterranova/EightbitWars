using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySync : MonoBehaviourPunCallbacks, IPunObservable //custom Lag Compensation for transform
{
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public Vector3 realPosition = Vector3.zero;
    public Vector3 positionAtLastPacket = Vector3.zero;
    public double currentTime = 0.0;
    public double currentPacketTime = 0.0;
    public double lastPacketTime = 0.0;
    public double timeToReachGoal = 0.0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        {
            if (stream.IsWriting) //if i'm local player, i send my info to sync my replicate in others clients...
            {
                stream.SendNext((Vector3)transform.position);
            }
            else //else if i'm a replicated of the local player, i set the data to te actuale replicated
            {
                currentTime = 0.0;
                positionAtLastPacket = transform.position; //last position received previously
                realPosition = (Vector3)stream.ReceiveNext(); //actual position from data pack
                lastPacketTime = currentPacketTime;
                currentPacketTime = info.timestamp;
            }
        }
    }

    void Update()
    {
        if (!pv.IsMine)
        {
            timeToReachGoal = currentPacketTime - lastPacketTime; //calc time to reach target
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal)); //lerp at real position
        }
    }
}

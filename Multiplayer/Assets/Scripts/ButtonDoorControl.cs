using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Photon.Pun.Demo.PunBasics
{
    public class ButtonDoorControl : MonoBehaviourPunCallbacks, IPunObservable
    {

        public bool active;

        private void OnTriggerEnter(Collider other)
        {
            if (photonView.IsMine)
            {
                if (other.tag == "Player")
                {
                    active = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (photonView.IsMine)
            {
                if (other.tag == "Player")
                {
                    active = false;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(active);
            }
            else
            {
                active = (bool)stream.ReceiveNext();
            }
        }
    }
}
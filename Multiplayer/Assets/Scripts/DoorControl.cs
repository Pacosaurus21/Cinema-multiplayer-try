using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Photon.Pun.Demo.PunBasics
{
    public class DoorControl : MonoBehaviourPunCallbacks, IPunObservable
    {
        public ButtonDoorControl[] allButtons;
        public Transform closePoint, openPoint;
        private bool activeDoor;
        public float speedDoor;

        
        void Start()
        {

        }

        
        void Update()
        {
            if(SetActiveDoor() == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, openPoint.position, 
                                                  speedDoor * Time.deltaTime);
                activeDoor = true;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, closePoint.position,
                                                  speedDoor * Time.deltaTime);
            }
        }

        bool SetActiveDoor()
        {
            if (activeDoor == true) return true;
            for (int i = 0; i < allButtons.Length; i++)
            {
                if (allButtons[i].active == false) return false;
            }
            return true;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {


        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Photon.Pun.Demo.PunBasics
{
    public class PlayerControlOnline : MonoBehaviourPunCallbacks, IPunObservable
    {
        public int life;
        private CharacterController control;
        public CameraControlOnline cameraControl;
        public Vector3 moveDir;
        public float speedMove, speedRot, gravity, jumpforce;

        public GameObject otherCanvas, mineCanvas;
        public TextMeshProUGUI nickname;
        public Image lifeBar, mineLifeBar;
        public Image background;
        private int initLife;

        private RaycastHit hitShoot;
        private Ray rayShoot;

        private Vector3 initPos;
        private Quaternion initRot;

        private Vector3 currentPosition;
        private Quaternion currentRotation;
        private bool setRespawn;

        public Transform player;
        public Transform eyesTransform;

        public float rotationXh;
        public float rotationYh;

        // Start is called before the first frame update
        void Start()
        {
            if (photonView.IsMine) LocalStart();
            else OtherStart();
        }

        void LocalStart()
        {
            control = GetComponent<CharacterController>();
            Camera.main.GetComponent<CameraControlOnline>().SetTransform(eyesTransform);
            cameraControl = Camera.main.GetComponent<CameraControlOnline>();
            nickname.text = "";
            initLife = life;
            otherCanvas.SetActive(false);
            initPos = transform.position;
            initRot = transform.rotation;
        }

        void OtherStart()
        {
            nickname.text = photonView.Owner.NickName;
            mineCanvas.SetActive(false);
            
        }
        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine) LocalUpdate();
            else OtherUpdate();
            lifeBar.transform.rotation = Camera.main.transform.rotation;
            background.transform.rotation = Camera.main.transform.rotation;

        }

        void LocalUpdate()
        {
            
            ShootSystem();

            if(control.isGrounded)
            {
                moveDir = new Vector3(Input.GetAxis("Horizontal")* speedMove, 0, Input.GetAxis("Vertical") * speedMove);
                moveDir = transform.TransformDirection(moveDir);
                rotationXh = cameraControl.rotationX;
                rotationYh = cameraControl.rotationY;
                transform.localEulerAngles = new Vector3(rotationXh, rotationYh, 0);

                if (Input.GetButtonDown("Jump"))
                {
                    moveDir.y = jumpforce;
                }

            }
            else
            {
                moveDir.y -= gravity * Time.deltaTime;
            }
            control.Move(moveDir * Time.deltaTime);

        }

        void OtherUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, currentPosition, 4 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, 4 * Time.deltaTime);
            if(setRespawn == true)
            {
                transform.position = currentPosition;
                transform.rotation = currentRotation;
                setRespawn = false;
            }

        }

        private void ShootSystem()
        {
            if (Input.GetMouseButtonDown(0))
            {
                rayShoot = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(rayShoot, out hitShoot, Mathf.Infinity, (1 << 6)))
                {
                    hitShoot.collider.GetComponent<PhotonView>().RPC("GetDamageRPC", RpcTarget.Others, 
                        Random.Range(10, 20)); 
                }
            }

        }
        [PunRPC]
        public void GetDamageRPC(int _damage)
        {
            GetDamage(_damage);
        }

        public void GetDamage (int _damage)
        {
            life -= _damage;
            float totalLife = (float)life / (float)initLife;
            lifeBar.fillAmount = totalLife;
            mineLifeBar.fillAmount = totalLife;
            if(life <= 0)
            {
                transform.position = initPos;
                transform.rotation = initRot;
                life = 100;
                mineLifeBar.fillAmount = 1;
                lifeBar.fillAmount = 1;
                setRespawn = true;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(life);//0
                stream.SendNext(lifeBar.fillAmount);//1
                stream.SendNext(transform.position);//2
                stream.SendNext(transform.rotation);//3
                stream.SendNext(setRespawn);

            }
            else
            {
                life = (int)stream.ReceiveNext();//0
                lifeBar.fillAmount = (float)stream.ReceiveNext();//1
                currentPosition = (Vector3)stream.ReceiveNext();//2
                currentRotation = (Quaternion)stream.ReceiveNext();//3
                setRespawn = (bool)stream.ReceiveNext();

            }

        }

    }
}

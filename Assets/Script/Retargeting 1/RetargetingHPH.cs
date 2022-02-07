using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetargetingHPH : MonoBehaviour
{
    [SerializeField]  public GameObject originGO;

   HumanPoseHandler originPoseHandler;
   HumanPoseHandler destinationPoseHandler;

    void Start()
    {
        //nombre del origen qeu tiene las animaciones 
        originGO = GameObject.Find("personaje1Baile");
        //crear leer y escribir el humanpose de un objeto
        originPoseHandler = new HumanPoseHandler(originGO.GetComponent<Animator>().avatar, originGO.transform);
        destinationPoseHandler = new HumanPoseHandler(this.GetComponent<Animator>().avatar, this.transform);

    }

    void LateUpdate()
    {
        HumanPose m_humanPose = new HumanPose();
        // GetHumanPose: Calcula una pose humana a partir del esqueleto del avatar, almacena la pose en el manejador y  la devuelve.
        //SetHumanPose: Almacena la pose dentro del manejador.
        //VOID
        originPoseHandler.GetHumanPose(ref m_humanPose);
        destinationPoseHandler.SetHumanPose(ref m_humanPose);

    }
}

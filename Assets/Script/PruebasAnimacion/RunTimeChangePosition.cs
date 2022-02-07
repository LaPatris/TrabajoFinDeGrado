using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeChangePosition : MonoBehaviour
{
    //el esqueleto 
    public Transform srcModel;
    //ambos animators
    //animator del que voy a leer
    Animator srcAnimator;
    //animator donde voy a meter la persona
    Animator selfAnimator;
    //en nuestro caso actual solo hace falta uno 
    Animator animat;
    //listas de los transforms que recibimos
    [SerializeField] List<Transform> srcJoints = new List<Transform>();
    //los mios 
    [SerializeField] List<Transform> selfJoints = new List<Transform>();
    //cuaternion para la rotacion inicial
    Quaternion srcInitRotation = new Quaternion();
    Quaternion selfInitRotation = new Quaternion();
    //lista de quaternions(supongo que servirá para todos los huesos??
    List<Quaternion> srcJointsInitRotation = new List<Quaternion>();
    List<Quaternion> selfJointsInitRotation = new List<Quaternion>();
    //guarda la root y la posicion de las
    Transform srcRoot;
    Transform selfRoot;
    Vector3 srcInitPosition = new Vector3();
    Vector3 selfInitPosition = new Vector3();

    [SerializeField]
    static HumanBodyBones[] bonesToUse = new[]{
        HumanBodyBones.Neck,
        HumanBodyBones.Head,

        HumanBodyBones.Hips,
        HumanBodyBones.Spine,
        HumanBodyBones.Chest,
        HumanBodyBones.UpperChest,

        HumanBodyBones.LeftShoulder,
        HumanBodyBones.LeftUpperArm,
        HumanBodyBones.LeftLowerArm,
        HumanBodyBones.LeftHand,

        HumanBodyBones.RightShoulder,
        HumanBodyBones.RightUpperArm,
        HumanBodyBones.RightLowerArm,
        HumanBodyBones.RightHand,

        HumanBodyBones.LeftUpperLeg,
        HumanBodyBones.LeftLowerLeg,
        HumanBodyBones.LeftFoot,
        HumanBodyBones.LeftToes,

        HumanBodyBones.RightUpperLeg,
        HumanBodyBones.RightLowerLeg,
        HumanBodyBones.RightFoot,
        HumanBodyBones.RightToes,
    };

    void Start()
    {
        //setea los animator
        animat =  gameObject.GetComponent<Animator>();

        //setea las rotaciones inciiales
        srcInitRotation = srcModel.transform.rotation;
        selfInitRotation = gameObject.transform.rotation;
        // guarda la root
        srcRoot = animat.GetBoneTransform(HumanBodyBones.Hips);
        selfRoot = animat.GetBoneTransform(HumanBodyBones.Hips);

        InitBones();
        SetJointsInitRotation();
        SetInitPosition();
    }

    void LateUpdate()
    {
        SetJointsRotation();
        SetPosition();
    }

    private void InitBones()
    {
        //inicializa los huesos tanto del origen como de la copia
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            //getBoneTransfor: devuelve el tranfor asignada al hueso seleccionado 
            srcJoints.Add(srcAnimator.GetBoneTransform(bonesToUse[i]));
            selfJoints.Add(selfAnimator.GetBoneTransform(bonesToUse[i]));
        }
    }

    private void SetJointsInitRotation()
    {
        //rotacion inciial de los joints
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            //añade la raotacion inicial de los huesos
            srcJointsInitRotation.Add(srcJoints[i].rotation);
            selfJointsInitRotation.Add(selfJoints[i].rotation);
        }
    }

    private void SetJointsRotation()
    {
        //setea todas las futuras rotaciones
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            selfJoints[i].rotation = selfInitRotation;// setea la rotacion inicial del destino
            selfJoints[i].rotation *= (srcJoints[i].rotation);// la multiplica por la rotacion del hueso del orgen
            selfJoints[i].rotation *= selfJointsInitRotation[i];// y la multiplica por la rotacion inicial del hueso destino
        }
    }

    private void SetInitPosition()
    {
        //seta las posiciones iniciales(de la root
        srcInitPosition = srcRoot.localPosition;
        selfInitPosition = selfRoot.localPosition;
    }

    private void SetPosition()
    {// setea la nueva posicion( posicion root local- la inicial del origen)+ la inicial del destino
        selfRoot.localPosition = (srcRoot.localPosition - srcInitPosition) + selfInitPosition;
    }
}
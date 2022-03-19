
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System;
public class animacion : MonoBehaviour
{

    //el esqueleto 
    public Transform srcModel;
    //ambos animators
    Animator selfAnimator;
    //listas con los joins tanto del origen como del destino
    [SerializeField] List<Transform> selfJoints = new List<Transform>();

    [SerializeField] List<Transform> srcJoints = new List<Transform>();
    [SerializeField] Dictionary<String, List<Quaternion>> totalRotation = new Dictionary<string, List<Quaternion>>();

    //cuaternion para la rotacion inicial
    [SerializeField] Quaternion srcInitRotation = new Quaternion();
    [SerializeField] Quaternion selfInitRotation = new Quaternion();
    //lista de quaternions(supongo que servirá para todos los huesos??
    [SerializeField] List<Quaternion> srcJointsInitRotation = new List<Quaternion>();
    [SerializeField] List<Quaternion> selfJointsInitRotation = new List<Quaternion>();
    //guarda la root y la posicion de las
    [SerializeField] Transform srcRoot;
    [SerializeField] Transform selfRoot;
    [SerializeField] Vector3 srcInitPosition = new Vector3();
    [SerializeField] Vector3 selfInitPosition = new Vector3();
    [SerializeField] bool ejecutar = false;
    [SerializeField] int posicion = 0;
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
        selfAnimator = this.GetComponent<Animator>();

        //setea las rotaciones inciiales
        
        selfInitRotation = gameObject.transform.rotation;
        // guarda la root
        selfRoot = selfAnimator.GetBoneTransform(HumanBodyBones.Hips);

    }
    private void InitBones()
    {
        //inicializa los huesos tanto del origen como de la copia
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            //getBoneTransfor: devuelve el tranfor asignada al hueso seleccionado 
            srcJoints.Add(selfAnimator.GetBoneTransform(bonesToUse[i]));
            selfJoints.Add(selfAnimator.GetBoneTransform(bonesToUse[i]));
        }
    }
    
    private void SetJointsInitRotation()
    {
        //rotacion inciial de los joints
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            //añade la raotacion inicial de los huesos
            if(selfJoints[i]!=null)
            selfJointsInitRotation.Add(selfJoints[i].rotation);

        }
    }

    private void SetInitPosition()
    {
        //seta las posiciones iniciales(de la root
        
        selfInitPosition = selfRoot.localPosition;
    }
    //se tiene que llamar desde fuera para inicializar los valores
    public void initAll(Dictionary<String, List<Vector3>> totalBody)
    { 
        InitBones();
        SetJointsInitRotation( );
        SetInitPosition();
        //initRotation and initPosition

        foreach (KeyValuePair<string, List<Vector3>> hueso in totalBody)
    {
            for (int i = 0; i < bonesToUse.Length; i++)
            {
                if (hueso.Key.Equals(bonesToUse[i].ToString()))
                {
                    float x = Vector3.Angle(hueso.Value[0], Vector3.left);

                    float y = Vector3.Angle(hueso.Value[0], Vector3.up);

                    float z = Vector3.Angle(hueso.Value[0], Vector3.forward);
                    srcJointsInitRotation.Add(new Quaternion(x, y, z, 0));
                    //creamos el diccionario de rotation
                    List<Quaternion> auxList = new List<Quaternion>();
                    foreach (Vector3 vec in hueso.Value)
                    {

                        float anglex = Vector3.Angle(vec, Vector3.left);

                        float angley = Vector3.Angle(vec, Vector3.up);

                        float anglez = Vector3.Angle(vec, Vector3.forward);
                        auxList.Add(new Quaternion(anglex, angley, anglez, 1));
                    }
                    //metemos en el diccionario de rotation
                    totalRotation.Add(bonesToUse[i].ToString(), auxList);
                }
               
            }
           
            //calculamos la posición y rotación inicial
            if (hueso.Key.Equals("Hips"))
            {
                //no queremos que se mueva en el eje Z
                Vector3 newPosition = new Vector3(hueso.Value[0].x, hueso.Value[0].y, 0);
                srcRoot.position = newPosition;
                float x = Vector3.Angle(hueso.Value[0], Vector3.left);

                float y = Vector3.Angle(hueso.Value[0], Vector3.up);

                float z = Vector3.Angle(hueso.Value[0], Vector3.forward);
                srcInitRotation = new Quaternion(x, y, z, 1);
                srcInitPosition = srcRoot.localPosition;
            }
     }

        ejecutar= true;
    }
    void LateUpdate()
    {
        if (ejecutar)
        {
            SetJointsRotation();
            SetPosition();
            posicion += 1;
        }
    }

   

   
    private void SetJointsRotation()
    {
        int j = 0;
        foreach (KeyValuePair<string, List<Quaternion>> hueso in totalRotation)
            
        {    //setea todas las futuras rotaciones
            if (posicion > hueso.Value.Count) posicion = 0;
            for (int i = 0; i < selfJoints.Count; i++)
            {
                if(selfJoints[i]!=null)
                if (selfJoints[i].ToString().Contains(hueso.Key))
                {
                    selfJoints[i].rotation = selfInitRotation;// setea la rotacion inicial del destino
                    selfJoints[i].rotation *= (hueso.Value[posicion]);// la multiplica por la rotacion del hueso del orgen
                    selfJoints[i].rotation *= selfJointsInitRotation[j];// y la multiplica por la rotacion inicial del hueso destino}
                        j++;
                }
            }
        }
    }


    private void SetPosition()
    {// setea la nueva posicion( posicion root local- la inicial del origen)+ la inicial del destino
        selfRoot.localPosition = (srcRoot.localPosition - srcInitPosition) + selfInitPosition;
    }
}
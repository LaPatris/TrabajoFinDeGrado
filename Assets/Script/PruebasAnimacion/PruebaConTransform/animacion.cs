
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
    List<Quaternion> auxList = new List<Quaternion>();
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
    bool terminado = false;
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
            //metemos a ambos los huesos
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
    private Quaternion calculateRotation(Vector3 vector1)
    {
        float x = Vector3.Angle(vector1, Vector3.left);
        float y = Vector3.Angle(vector1, Vector3.up);
        float z = Vector3.Angle(vector1, Vector3.forward);
        Vector3 aux = new Vector3(x, y, z).normalized;

        return new Quaternion(aux.x, aux.y, aux.z, 1);
    }
    public void initAll(Dictionary<String, List<Vector3>> totalBody)
    { 
        InitBones();
        Vector3 newPosition = new Vector3(totalBody["Hips"][0].x, totalBody["Hips"][0].y, totalBody["Hips"][0].z);
        srcRoot.position = newPosition;
        srcRoot.rotation = calculateRotation(totalBody["Hips"][0]);
        srcInitRotation = srcRoot.localRotation;
        srcInitPosition = srcRoot.localPosition;
        SetJointsInitRotation( );
        SetInitPosition();
        //initRotation and initPosition
        for (int i = 0; i < bonesToUse.Length; i++)
        {// poniendo antes bonesToUse tendrá su organización 

            String[] nombre = bonesToUse[i].ToString().Split(' ');
            foreach (KeyValuePair<string, List<Vector3>> hueso in totalBody)
            {          
                if (hueso.Key.Equals(nombre[0]))
                {
                    //seleccionamos la rotación inicial de cada uno de los huesos
                    srcJointsInitRotation.Add(calculateRotation(hueso.Value[0]));
                    foreach (Vector3 vec in hueso.Value)
                    {
                        auxList.Add(calculateRotation(vec));
                    }

                    //se lo metemos a total rotation
                    totalRotation.Add(nombre[0], auxList);
                    
                } 
            }                    
        }
        ejecutar= true;
    }
    void LateUpdate()
    {
        if (ejecutar)
        {
            if (terminado == false)
            {
                SetJointsRotation();
                SetPosition();
                posicion += 1;
            }
        }
    }


                        
                
    

    private void SetJointsRotation()
    {
        int j = 0;
        terminado = true;
       for (int i = 0; i < selfJoints.Count; i++)
         {    //setea todas las futuras rotaciones
            if (selfJoints[i] != null)
            {
                String[] nombre = selfJoints[i].ToString().Split(' ');
            
                List<Quaternion> aux = new List<Quaternion>();
                if (!nombre[0].Equals("Hips") && totalRotation.TryGetValue(nombre[0], out aux))
                {
                    if (posicion == totalRotation[nombre[0]].Count) posicion = 0;

                    if (totalRotation[nombre[0]].Count > 0)
                    {
                        Debug.Log("estoy en :" + nombre[0]);
                        selfJoints[i].rotation = selfInitRotation;// setea la rotacion inicial del destino
                                                                  // selfJoints[i].rotation *= (hueso.Value[posicion]);// la multiplica por la rotacion del hueso del orgen

                        selfJoints[i].rotation *= (srcJointsInitRotation[j] * Quaternion.Inverse(totalRotation[nombre[0]][posicion]));

                        selfJoints[i].rotation *= selfJointsInitRotation[j];// y la multiplica por la rotacion inicial del hueso destino}


                    }
                    
                }
                j++;
            }
        }
        terminado = false;
       

    }


    private void SetPosition()
    {// setea la nueva posicion( posicion root local- la inicial del origen)+ la inicial del destino
        selfRoot.localPosition = (srcRoot.localPosition - srcInitPosition) + selfInitPosition;
    }
}

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
        //uno
        HumanBodyBones.Hips,//0
        HumanBodyBones.LeftUpperLeg,
        HumanBodyBones.RightUpperLeg,
        HumanBodyBones.Spine,//3        
        HumanBodyBones.Chest,//4
        HumanBodyBones.UpperChest,//5
        //left up Leg

        HumanBodyBones.LeftLowerLeg,//6
        HumanBodyBones.LeftFoot,//7
        HumanBodyBones.LeftToes,
        //right up leg
        HumanBodyBones.RightLowerLeg,//9
        HumanBodyBones.RightFoot,
        HumanBodyBones.RightToes,
   
        HumanBodyBones.LeftShoulder,//12
        HumanBodyBones.Neck,//13    
        HumanBodyBones.RightShoulder,//14
        
        HumanBodyBones.LeftUpperArm,//15
        HumanBodyBones.LeftLowerArm,//16
        HumanBodyBones.LeftHand,//17

        HumanBodyBones.Head,//18

        HumanBodyBones.RightUpperArm,//19
        HumanBodyBones.RightLowerArm,
        HumanBodyBones.RightHand,

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
            selfJoints.Add(selfAnimator.GetBoneTransform(bonesToUse[i]));
        }
    }
    
    private void SetJointsInitRotation()
    {
        //rotacion inciial de los joints
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            //añade la raotacion inicial de los huesos
            if (selfJoints[i] != null)
            {
                selfJointsInitRotation.Add(selfJoints[i].rotation);

                Debug.Log("todos los huesos de los que hay animación " + bonesToUse[i].ToString());
            }

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
        //float y = Vector3.Angle(vector1, Vector3.forward);
        float z = Vector3.Angle(vector1, Vector3.forward);
        float y = Vector3.Angle(vector1, Vector3.up);
        Vector3 aux = new Vector3(x, y, z).normalized;

        return new Quaternion(aux.x, aux.y, aux.z, 1);
    }
    public void setParents()
    {
        for (int i = 0; i < selfJoints.Count; i++)
        {// poniendo antes bonesToUse tendrá su organización 

            if (selfJoints[i] != null)
            {
                if(i!=0)
                selfJoints[i].SetParent(selfJoints[0]);
            } 
        }
    }
    public void setParents2()
    {
        for (int i = 0; i < selfJoints.Count; i++)
        {// poniendo antes bonesToUse tendrá su organización 

            if (selfJoints[i] != null)
            {
                String[] nombre = selfJoints[i].ToString().Split(' ');
                switch (nombre[0])
                {
                    case "Hips":
                        selfJoints[i].localPosition = this.gameObject.transform.localPosition; //transform.InverseTransformPoint(totalBody["Hips"][0].x / scala, (totalBody["Hips"][0].y / scala) - scalaSelfJoints, totalBody["Hips"][0].z / scala);
                        break;
                    #region Hijos de hips 0
                    case "RightUpLeg":
                        selfJoints[i].SetParent(selfJoints[0]);
                        break;
                    case "LeftUpLeg":
                        selfJoints[i].SetParent(selfJoints[0]);
                        break;
                    case "Spine":
                        selfJoints[i].SetParent(selfJoints[0]);
                        break;
                    #endregion hijos de hips 0
                    #region hijos de Chest/spine 3/4
                    case "Neck":
                        //selfJoints[i].SetParent(selfJoints[3]);
                        selfJoints[i].SetParent(selfJoints[0]);
                        break;
                    case "RightShoulder":
                        //selfJoints[i].SetParent(selfJoints[3]);
                        selfJoints[i].SetParent(selfJoints[0]);
                        break;
                    case "LeftShoulder":
                       // selfJoints[i].SetParent(selfJoints[3]);
                        selfJoints[i].SetParent(selfJoints[0]);
                        break;
                    #endregion hijos chest/spine
                    #region HIJO DEleftUpLeg 1
                    case "LeftLeg":
                        //selfJoints[i].SetParent(selfJoints[1]);
                        selfJoints[i].SetParent(selfJoints[0]);
                        break;
                    #endregion
                    #region HIJO DE RIGHTLEG2
                    case "RightLeg":

                        //selfJoints[i].SetParent(selfJoints[2]);
                        selfJoints[i].SetParent(selfJoints[0]);
                        break;
                        #endregion
                      #region hijo neck 13
                          case "Head":
                             // selfJoints[i].SetParent(selfJoints[13]);
                              selfJoints[i].SetParent(selfJoints[0]);
                              break;
                          #endregion
                          #region HIJO LEFT SHOULDER 12
                          case "LeftArm": //equivale a nuestro leftArm

                              selfJoints[i].SetParent(selfJoints[12]);
                              break;
                          #endregion

                          #region hijo de rightshoulder 14
                          case "RightArm": //equivale a nuestro right Arm

                              selfJoints[i].SetParent(selfJoints[14]);
                              break;
                          #endregion
                          #region HIJO LEFTupper arm 15
                          case "LeftForeArm":

                              selfJoints[i].SetParent(selfJoints[15]);
                              break;
                          #endregion
                          #region HIJO de left lower arm 16
                          case "LeftHand":

                              selfJoints[i].SetParent(selfJoints[16]);
                              break;
                          case "LeftHandThumb1":

                              selfJoints[i].SetParent(selfJoints[16]);
                              break;
                          #endregion

                          #region HIJO right upper arm 19
                          case "RightForeArm":

                              selfJoints[i].SetParent(selfJoints[19]);
                              break;
                          #endregion
                          #region HIJO de lower arm 20
                          case "RightHand":

                              selfJoints[i].SetParent(selfJoints[20]);
                              break;
                          case "RightHandThumb1":

                              selfJoints[i].SetParent(selfJoints[20]);
                              break;
                          #endregion
                          #region HIIJO DE lowerr left leg 6
                          case "LeftFoot":

                              selfJoints[i].SetParent(selfJoints[6]);
                              break;

                          #endregion
                          #region HIIJO DE lowerr left leg 6
                          case "LeftToes":

                              selfJoints[i].SetParent(selfJoints[6]);
                              break;

                          #endregion
                          #region right lower leg 9
                          case "RightFoot"://9

                              selfJoints[i].SetParent(selfJoints[9]);
                              break;
                          #endregion
                          #region right lower leg 9
                          case "RightToes"://9

                              selfJoints[i].SetParent(selfJoints[9]);
                              break;
                              #endregion
                        
                }
            }
        }
    }
    public void initAll(Dictionary<String, List<Vector3>> totalBody)
    { 
        //inicializamos los huesos
        InitBones();

        Vector3 newPosition = transform.InverseTransformPoint(totalBody["Hips"][0].x, totalBody["Hips"][0].y, totalBody["Hips"][0].z);
        srcRoot.localPosition = newPosition;
        /*srcRoot.rotation = calculateRotation(totalBody["Hips"][0]);
        srcInitRotation = srcRoot.localRotation;
        srcInitPosition = srcRoot.localPosition;*/
        SetJointsInitRotation( );
        //SetInitPosition();
        //initRotation and initPosition
     
        float scalaSelfJoints = selfJoints[2].position.y;
        float scalatotal = totalBody["Hips"][0].y;
        float scala = scalatotal- scalaSelfJoints;
        Debug.Log("escala" + scala+"scala leida"+scalatotal+"scala mi personaje "+scalaSelfJoints);
        this.gameObject.transform.position = new Vector3(totalBody["Hips"][0].x / scala, totalBody["Hips"][0].y / scala, totalBody["Hips"][0].z / scala);
        //seteamos los padres de los huesos
        setParents();
        // seteamos la nueva posición /*
        for (int i = 0; i < selfJoints.Count; i++)
        {// poniendo antes bonesToUse tendrá su organización 
           
            if (selfJoints[i] != null)
            {
                String[] nombre = selfJoints[i].ToString().Split(' ');
              //  foreach (KeyValuePair<string, List<Vector3>> hueso in totalBody)
               // {
                    //mete todo bien

                   // if (hueso.Key.Equals(nombre[0]) )
                   switch (nombre[0])
                {
                  case "Hips":
                        //selfJoints[i].localPosition= transform.InverseTransformPoint(0 / scala, 0 / scala, 0 / scala);
                        // selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["Hips"][0].x / scala, (totalBody["Hips"][0].y / scala) - scalaSelfJoints, totalBody["Hips"][0].z / scala);
                        // selfJoints[i].localPosition = this.gameObject.transform.localPosition; //transform.InverseTransformPoint(totalBody["Hips"][0].x / scala, (totalBody["Hips"][0].y / scala) - scalaSelfJoints, totalBody["Hips"][0].z / scala);

                        //Debug.Log(nombre[0] + "posicionlocal" + selfJoints[i].localPosition + " posicion normal" + totalBody["Hips"][0]);
                        //selfJoints[i].rotation = calculateRotation(totalBody["Hips"][0]);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["Hips"][0]));
                        break;
                    #region Hijos de hips 0
                    case "LeftUpLeg":
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["LeftUpLeg"][0].x  / scala, totalBody["LeftUpLeg"][0].y / scala, totalBody["LeftUpLeg"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftUpLeg"][0]));
                         break;
                    case "RightUpLeg":
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["RightUpLeg"][0].x  / scala, totalBody["RightUpLeg"][0].y  / scala, totalBody["RightUpLeg"][0].z  / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightUpLeg"][0]));
                        break;
                    case "Spine":
                       selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["Spine"][0].x / scala, totalBody["Spine"][0].y  / scala, totalBody["Spine"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["Spine"][0]));
                        break;
                        #endregion hijos de hips 0
                       #region hijos de Chest/spine 3/4
                        case "Neck":
                       // selfJoints[i].localPosition =transform.InverseTransformPoint((totalBody["Neck"][0].x- selfJoints[3].localPosition.x )/ scala , (totalBody["Neck"][0].y -selfJoints[3].localPosition.y) / scala , (totalBody["Neck"][0].z - selfJoints[3].localPosition.z) / scala );
                        selfJoints[i].localPosition =transform.InverseTransformPoint((totalBody["Neck"][0].x )/ scala , (totalBody["Neck"][0].y ) / scala , (totalBody["Neck"][0].z) / scala );
                        srcJointsInitRotation.Add(calculateRotation(totalBody["Neck"][0]));
                        break;
                        case "LeftShoulder":
                        selfJoints[i].localPosition = transform.InverseTransformPoint((totalBody["LeftShoulder"][0].x) / scala, (totalBody["LeftShoulder"][0].y ) / scala, (totalBody["LeftShoulder"][0].z ) / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftShoulder"][0]));
                        //selfJoints[i].localPosition = transform.InverseTransformPoint((totalBody["LeftShoulder"][0].x- selfJoints[3].localPosition.x) / scala, (totalBody["LeftShoulder"][0].y - selfJoints[3].localPosition.y) / scala, (totalBody["LeftShoulder"][0].z - selfJoints[3].localPosition.z) / scala);
                        break;
                        case "RightShoulder":
                        //selfJoints[i].localPosition = transform.InverseTransformPoint((totalBody["RightShoulder"][0].x - selfJoints[3].localPosition.x) / scala, (totalBody["RightShoulder"][0].y - selfJoints[3].localPosition.y) / scala, (totalBody["RightShoulder"][0].z - selfJoints[3].localPosition.z) / scala);
                        selfJoints[i].localPosition = transform.InverseTransformPoint((totalBody["RightShoulder"][0].x ) / scala, (totalBody["RightShoulder"][0].y ) / scala, (totalBody["RightShoulder"][0].z ) / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightShoulder"][0]));
                        break;
                        #endregion hijos chest/spine
                       #region HIJO DEleftUpLeg 1
                        case "LeftLeg":

                        selfJoints[i].localPosition = transform.InverseTransformPoint((totalBody["LeftLeg"][0].x  )/ scala, (totalBody["LeftLeg"][0].y ) / scala, (totalBody["LeftLeg"][0].z ) / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftLeg"][0]));
                        break;
                    #endregion
                    #region HIJO DE RIGHTLEG2
                    case "RightLeg":

                       // selfJoints[i].localPosition = transform.InverseTransformPoint((totalBody["RightLeg"][0].x - selfJoints[2].localPosition.x) / scala, (totalBody["RightLeg"][0].y - selfJoints[2].localPosition.y) / scala, (totalBody["RightLeg"][0].z - selfJoints[2].localPosition.z) / scala);
                        selfJoints[i].localPosition = transform.InverseTransformPoint((totalBody["RightLeg"][0].x ) / scala, (totalBody["RightLeg"][0].y) / scala, (totalBody["RightLeg"][0].z ) / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightLeg"][0]));
                        break;
                    #endregion
                  #region hijo neck 13
                    case "Head":

                        //selfJoints[i].SetParent(selfJoints[13]);
                       selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["Head"][0].x / scala, totalBody["Head"][0].y / scala, totalBody["Head"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["Head"][0]));
                        break;
                        #endregion
                        
                    #region HIJO LEFT SHOULDER 12
                        case "LeftArm": //equivale a nuestro leftArm

                       // selfJoints[i].SetParent(selfJoints[12]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["LeftArm"][0].x / scala, totalBody["LeftArm"][0].y / scala, totalBody["LeftArm"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftArm"][0]));
                        break;
                    #endregion

                    #region hijo de rightshoulder 14
                    case "RightArm": //equivale a nuestro right Arm

                        //selfJoints[i].SetParent(selfJoints[14]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["RightArm"][0].x / scala, totalBody["RightArm"][0].y / scala, totalBody["RightArm"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightArm"][0]));
                        break;
                    #endregion
                    #region HIJO LEFTupper arm 15
                    case "LeftForeArm":

                        //selfJoints[i].SetParent(selfJoints[15]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["LeftForeArm"][0].x / scala, totalBody["LeftForeArm"][0].y / scala, totalBody["LeftForeArm"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftForeArm"][0]));
                        break;
                    #endregion
                    #region HIJO de left lower arm 16
                    case "LeftHand":

                        //selfJoints[i].SetParent(selfJoints[16]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["LeftHand"][0].x / scala, totalBody["LeftHand"][0].y / scala, totalBody["LeftHand"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftHand"][0]));
                        break;
                    case "LeftHandThumb1":

                       // selfJoints[i].SetParent(selfJoints[16]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["LeftHandThumb1"][0].x / scala, totalBody["LeftHandThumb1"][0].y / scala, totalBody["LeftHandThumb1"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftHandThumb1"][0]));
                        break;
                    #endregion

                    #region HIJO right upper arm 19
                    case "RightForeArm":

                        //selfJoints[i].SetParent(selfJoints[19]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["RightForeArm"][0].x / scala, totalBody["RightForeArm"][0].y / scala, totalBody["RightForeArm"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightForeArm"][0]));
                        break;
                    #endregion
                    #region HIJO de lower arm 20
                    case "RightHand":

                       // selfJoints[i].SetParent(selfJoints[20]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["RightHand"][0].x / scala, totalBody["RightHand"][0].y / scala, totalBody["RightHand"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightHand"][0]));
                        break;
                    case "RightHandThumb1":

                        //selfJoints[i].SetParent(selfJoints[20]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["RightHandThumb1"][0].x / scala, totalBody["RightHandThumb1"][0].y / scala, totalBody["RightHandThumb1"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightHandThumb1"][0]));
                        break;
                    #endregion
                    #region HIIJO DE lowerr left leg 6
                    case "LeftFoot":

                        //selfJoints[i].SetParent(selfJoints[6]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["LeftFoot"][0].x / scala, totalBody["LeftFoot"][0].y / scala, totalBody["LeftFoot"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftFoot"][0]));
                        break;

                    #endregion
                    #region HIIJO DE lowerr left leg 6
                    case "LeftToeBase":

                        //selfJoints[i].SetParent(selfJoints[6]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["LeftToeBase"][0].x / scala, totalBody["LeftToeBase"][0].y / scala, totalBody["LeftToeBase"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["LeftToeBase"][0]));
                        break;

                    #endregion
                    #region right lower leg 9
                    case "RightFoot"://9

                        //selfJoints[i].SetParent(selfJoints[9]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["RightFoot"][0].x / scala, totalBody["RightFoot"][0].y / scala, totalBody["RightFoot"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightFoot"][0]));
                        break;
                    #endregion
                    #region right lower leg 9
                    case "RightToeEnd"://9

                        //selfJoints[i].SetParent(selfJoints[9]);
                        selfJoints[i].localPosition = transform.InverseTransformPoint(totalBody["RightToeEnd"][0].x / scala, totalBody["RightToeEnd"][0].y / scala, totalBody["RightToeEnd"][0].z / scala);
                        srcJointsInitRotation.Add(calculateRotation(totalBody["RightToeEnd"][0]));
                        break;
                        #endregion
                                            }
                // }
                List<Quaternion> auxList = new List<Quaternion>();
                foreach (Vector3 vec in totalBody[nombre[0]])
                {
                    auxList.Add(calculateRotation(vec));
                    
                }

                totalRotation.Add(nombre[0], auxList);

                break;
            }
        }
        ejecutar = true;
    }
    void LateUpdate()
    {
        /*if (ejecutar)
        {
            if (terminado == false)
            {
               // SetJointsRotation();
                //SetPosition();
                posicion += 1;
            }
        }*/
    }


                        
                
    

    private void SetJointsRotation()
    {
        int j = 0;
        int k = 0;
        terminado = true;

       for (int i = 0; i < selfJoints.Count; i++)
         {   
            if (selfJoints[i] != null)
            {
                String[] nombre = selfJoints[i].ToString().Split(' ');
               
                List<Quaternion> aux = new List<Quaternion>();

              // Debug.Log("estoy en :" + selfJoints[i]+ "tiene en lista"+ totalRotation[selfJoints[i].ToString()].ToString());
                if (totalRotation.TryGetValue(nombre[0], out aux))
                {
                    if (posicion >= totalRotation[nombre[0]].Count) posicion = 0;

                    if (totalRotation[nombre[0]].Count > 0 && j<selfJointsInitRotation.Count && k<srcJointsInitRotation.Count)
                    {
                        if(nombre[0].Equals("RightHand"))
                        Debug.Log("entro en :" + nombre[0]);
                        selfJoints[i].rotation = selfInitRotation;// setea la rotacion inicial del destino
                                                                  // selfJoints[i].rotation *= (hueso.Value[posicion]);// la multiplica por la rotacion del hueso del orgen

                       // selfJoints[i].rotation *= (srcJointsInitRotation[j] * Quaternion.Inverse(totalRotation[nombre[0]][posicion]));
                        selfJoints[i].rotation *= srcJointsInitRotation[j];// el self es el que se rellena con los huesos qeu tiene el personaje no el txt
                        selfJoints[i].rotation *= Quaternion.Inverse(totalRotation[nombre[0]][posicion]);

                        selfJoints[i].rotation *= selfJointsInitRotation[j];// y la multiplica por la rotacion inicial del hueso destino}
                       
                    }

                }
            }

            if (i + 1 >= selfJoints.Count)
            { terminado = false; 
            }

        }
       
       

    }


    private void SetPosition()
    {// setea la nueva posicion( posicion root local- la inicial del origen)+ la inicial del destino
        selfRoot.localPosition = (srcRoot.localPosition - srcInitPosition) + selfInitPosition;
    }
}
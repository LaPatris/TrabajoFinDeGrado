﻿
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
            }

        }
    }

    private void SetInitPosition()
    {
        //seta las posiciones iniciales(de la root

        selfInitPosition = selfRoot.localPosition;
    }
    //se tiene que llamar desde fuera para inicializar los valores
    private Vector3 calculateRotation(Vector3 vector1, Transform vector2)
    {
        /* float ejeX = Vector3.Angle(vector2.right, vector1);
         float ejey = Vector3.Angle(vector2.up, vector1);
         float ejez = Vector3.Angle(vector2.forward , vector1);*/
        float angleX = (float)(Math.Atan2(vector2.right.y, vector2.right.z) - Math.Atan2(vector1.y, vector1.z));
        float angleY = (float)(Math.Atan2(vector2.up.x, vector2.up.z) - Math.Atan2(vector1.x, vector1.z));
        float angleZ = (float)(Math.Atan2(vector2.forward.x, vector2.forward.y) - Math.Atan2(vector1.x, vector1.y));
        return new Vector3(angleX, angleY, angleZ);
        //return new Vector3(ejeX, ejey, ejez);

    }
    public void setParents()
    {
        for (int i = 0; i < selfJoints.Count; i++)
        {// poniendo antes bonesToUse tendrá su organización 

            if (selfJoints[i] != null)
            {
                if (i != 0)
                    selfJoints[i].SetParent(selfJoints[0]);
            }
        }
    }
    public void setPosition(Vector3 posicion, float sca, Transform hueso)
    {
        Debug.Log("Hueso en posicion " + hueso.name);
        //hueso.localPosition = transform.InverseTransformPoint(posicion.x / sca, posicion.y / sca, posicion.z / sca);


    }
    public Quaternion setRotation(Transform hueso, Vector3 posicion1, Vector3 posicion2)
    {
        Debug.Log("Hueso en rotation " + hueso.name);
        return Quaternion.FromToRotation(posicion1, posicion2);

        /*Vector3 eulersAngulos2 = calculateRotation(hueso.localPosition, selfJoints[0].transform);
        Debug.Log(eulersAngulos2);
        hueso.transform.eulerAngles.Set(eulersAngulos2.x, eulersAngulos2.y, eulersAngulos2.z);//Quaternion.Euler(eulersAngulos2.x, eulersAngulos2.y, eulersAngulos2.z);
       /* hueso.transform.rotation *=Quaternion.LookRotation(posicion, Vector3.forward);
        hueso.transform.rotation *=Quaternion.LookRotation(posicion, Vector3.right);
        hueso.transform.rotation *=Quaternion.LookRotation(posicion, Vector3.up);*/
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
        SetJointsInitRotation();
        //SetInitPosition();
        //initRotation and initPosition
        int posicion = 0;
        float scalaSelfJoints = selfJoints[2].position.y;
        float scalatotal = totalBody["Hips"][0].y;
        float scala = scalatotal - scalaSelfJoints;
        Debug.Log("escala" + scala + "scala leida" + scalatotal + "scala mi personaje " + scalaSelfJoints);
        this.gameObject.transform.position = new Vector3(totalBody["Hips"][0].x / scala, totalBody["Hips"][0].y / scala, totalBody["Hips"][0].z / scala);
        //seteamos los padres de los huesos
        //setParents();
        // seteamos la nueva posición /*
        Vector3 posicionInicial = Vector3.zero;

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


                    case "LeftUpLeg":
                        List<Quaternion> LeftUpLeg = new List<Quaternion>();
                        for (int pos = 0; pos < totalBody["LeftUpLeg"].Count; pos += 2)
                        {
                            Vector3 direccion1 = totalBody["LeftLeg"][pos] - totalBody["LeftUpLeg"][pos];
                            Vector3 direccion2 = totalBody["LeftLeg"][pos + 1] - totalBody["LeftUpLeg"][pos + 1];
                            LeftUpLeg.Add(setRotation(selfJoints[i], direccion1, direccion2));
                            if (pos == 0)
                            {
                                srcJointsInitRotation.Add(setRotation(selfJoints[i], direccion1, direccion2));
                            }
                        }
                        totalRotation.Add("LeftUpLeg", LeftUpLeg);
                        break;
                    case "RightUpLeg":
                        //setPosition(totalBody["RightUpLeg"][0], scala, selfJoints[i]);
                        List<Quaternion> RightUpLeg = new List<Quaternion>();
                        for (int pos = 0; pos < totalBody["RightUpLeg"].Count; pos += 2)
                        {
                            //fin- inicial
                            Vector3 direccion1 = totalBody["RightLeg"][pos] - totalBody["RightUpLeg"][pos];
                            float distance1 = direccion1.magnitude;
                            Vector3 direction1 = direccion1 / distance1; // T
                            Vector3 direccion2 = totalBody["RightLeg"][pos + 1] - totalBody["RightUpLeg"][pos + 1];
                            float distance2 = direccion2.magnitude;
                            Vector3 direction2 = direccion2 / distance1; // T


                            RightUpLeg.Add(setRotation(selfJoints[i], direction1, direction2));
                            if (pos == 0)
                            {
                                srcJointsInitRotation.Add(setRotation(selfJoints[i], direccion1, direccion2));
                            }
                        }
                        totalRotation.Add("RightUpLeg", RightUpLeg);
                        break;
                }
            }
        }          
           
        
        ejecutar = true;

    }
    /* public void initAll(Dictionary<String, List<Vector3>> totalBody)
     {
         //inicializamos los huesos
         InitBones();

         Vector3 newPosition = transform.InverseTransformPoint(totalBody["Hips"][0].x, totalBody["Hips"][0].y, totalBody["Hips"][0].z);
         srcRoot.localPosition = newPosition;
         /*srcRoot.rotation = calculateRotation(totalBody["Hips"][0]);
         srcInitRotation = srcRoot.localRotation;
         srcInitPosition = srcRoot.localPosition;*
         SetJointsInitRotation();
         //SetInitPosition();
         //initRotation and initPosition
         int posicion = 0;
         float scalaSelfJoints = selfJoints[2].position.y;
         float scalatotal = totalBody["Hips"][0].y;
         float scala = scalatotal - scalaSelfJoints;
         Debug.Log("escala" + scala + "scala leida" + scalatotal + "scala mi personaje " + scalaSelfJoints);
         this.gameObject.transform.position = new Vector3(totalBody["Hips"][0].x / scala, totalBody["Hips"][0].y / scala, totalBody["Hips"][0].z / scala);
         //seteamos los padres de los huesos
         setParents();
         // seteamos la nueva posición /*
         Vector3 posicionInicial = Vector3.zero;

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

                         //aquí hemos hecho que hips sea la posicion 0 ,0 ,0 
                         //por lo que todo ahora va en función de hips

                         setPosition(totalBody["Hips"][0], scala, selfJoints[i]);
                         setRotation(totalBody["Hips"][0], selfJoints[i]);

                         break;
                     #region Hijos de hips 0
                     case "LeftUpLeg":
                         setPosition(totalBody["LeftUpLeg"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftUpLeg"][0],  selfJoints[i]);

                         break;
                     case "RightUpLeg":
                         setPosition(totalBody["RightUpLeg"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightUpLeg"][0], selfJoints[i]);
                         break;
                     case "Spine":
                         setPosition(totalBody["Spine"][0], scala, selfJoints[i]);
                         setRotation(totalBody["Spine"][0], selfJoints[i]);
                         break;
                     #endregion hijos de hips 0
                      #region hijos de Chest/spine 3/4
                     case "Neck":
                         setPosition(totalBody["Neck"][0], scala, selfJoints[i]);
                         setRotation(totalBody["Neck"][0], selfJoints[i]);
                         break;
                     case "LeftShoulder":

                         setPosition(totalBody["LeftShoulder"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftShoulder"][0], selfJoints[i]);
                         break;
                     case "RightShoulder":

                         setPosition(totalBody["RightShoulder"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightShoulder"][0], selfJoints[i]);
                         break;
                     #endregion hijos chest/spine
                     #region HIJO DEleftUpLeg 1
                     case "LeftLeg":
                         setPosition(totalBody["LeftLeg"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftLeg"][0], selfJoints[i]);
                         break;
                     #endregion
                     #region HIJO DE RIGHTLEG2
                     case "RightLeg":
                         setPosition(totalBody["RightLeg"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightLeg"][0], selfJoints[i]);
                         break;
                     #endregion
                     #region hijo neck 13
                     case "Head":
                         setPosition(totalBody["Head"][0], scala, selfJoints[i]);
                         setRotation(totalBody["Head"][0], selfJoints[i]);
                         break;
                     #endregion

                     #region HIJO LEFT SHOULDER 12
                     case "LeftArm": //equivale a nuestro leftArm

                         setPosition(totalBody["LeftArm"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftArm"][0], selfJoints[i]);
                         break;
                     #endregion

                     #region hijo de rightshoulder 14
                     case "RightArm": //equivale a nuestro right Arm

                         setPosition(totalBody["RightArm"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightArm"][0], selfJoints[i]);
                         break;
                     #endregion
                     #region HIJO LEFTupper arm 15
                     case "LeftForeArm":

                         setPosition(totalBody["LeftForeArm"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftForeArm"][0], selfJoints[i]);
                         break;
                     #endregion
                     #region HIJO de left lower arm 16
                     case "LeftHand":
                         setPosition(totalBody["LeftHand"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftHand"][0], selfJoints[i]);
                         break;
                     case "LeftHandThumb1":
                         setPosition(totalBody["LeftHandThumb1"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftHandThumb1"][0], selfJoints[i]);
                         break;
                     #endregion

                     #region HIJO right upper arm 19
                     case "RightForeArm":

                         setPosition(totalBody["RightForeArm"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightForeArm"][0], selfJoints[i]);
                         break;
                     #endregion
                     #region HIJO de lower arm 20
                     case "RightHand":
                         setPosition(totalBody["RightHand"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightHand"][0], selfJoints[i]);
                         break;
                     case "RightHandThumb1":

                         setPosition(totalBody["RightHandThumb1"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightHandThumb1"][0], selfJoints[i]);
                         break;
                     #endregion
                     #region HIIJO DE lowerr left leg 6
                     case "LeftFoot":
                         setPosition(totalBody["LeftFoot"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftFoot"][0], selfJoints[i]);
                         break;

                     #endregion
                     #region HIIJO DE lowerr left leg 6
                     case "LeftToeBase":
                         setPosition(totalBody["LeftToeBase"][0], scala, selfJoints[i]);
                         setRotation(totalBody["LeftToeBase"][0], selfJoints[i]);
                         break;

                     #endregion
                     #region right lower leg 9
                     case "RightFoot"://9
                         setPosition(totalBody["RightFoot"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightFoot"][0], selfJoints[i]);
                         break;
                     #endregion
                     #region right lower leg 9
                     case "RightToeEnd"://9

                         setPosition(totalBody["RightToeEnd"][0], scala, selfJoints[i]);
                         setRotation(totalBody["RightToeEnd"][0], selfJoints[i]);
                         break;
                         #endregion
                 }
                 // }
                 /* List<Quaternion> auxList = new List<Quaternion>();
                  int p = -1;

                  foreach (Vector3 vec in totalBody[nombre[0]])
                  {if (p == -1)
                      {
                          auxList.Add(calculateRotation(vec, selfJoints[i].localPosition));

                      }
                      else
                      {

                          auxList.Add(calculateRotation(vec, totalBody[nombre[0]][p]));
                      }
                      p++;
                  }

                  totalRotation.Add(nombre[0], auxList);

             }
         }
         ejecutar = true;

     }
     */
    void LateUpdate()
    {
     if (ejecutar)
        {
            if (terminado == false)
            {
               SetJointsRotation();
                //SetPosition();
                posicion += 1;
            }
        }
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
                    // se pone así el k porquie solo hay una posición inicial así que solo tiene que comprobar que sea menor que el numero de huesos, pero 
                    //como el numero de huesos hay algunos que estan en null, cogemos el selfjointinitRotation
                    if (totalRotation[nombre[0]].Count > 0 && j<selfJointsInitRotation.Count && k< srcJointsInitRotation.Count)
                    {
                       // if(nombre[0].Equals("RightUpLeg"))
                        Debug.Log("entro en :" + nombre[0]);
                        /* selfJoints[i].rotation = selfInitRotation;// setea la rotacion inicial del destino
                                                                   // selfJoints[i].rotation *= (hueso.Value[posicion]);// la multiplica por la rotacion del hueso del orgen

                        // selfJoints[i].rotation *= (srcJointsInitRotation[j] * Quaternion.Inverse(totalRotation[nombre[0]][posicion]));
                         selfJoints[i].rotation *= srcJointsInitRotation[j];// el self es el que se rellena con los huesos qeu tiene el personaje no el txt
                         selfJoints[i].rotation *= Quaternion.Inverse(totalRotation["RightUpLeg"][posicion]);

                         selfJoints[i].rotation *= selfJointsInitRotation[j];// y la multiplica por la rotacion inicial del hueso destino}*/
                        /* selfJoints[i].rotation = selfInitRotation;// setea la rotacion inicial del destino
                         selfJoints[i].rotation *= (srcJoints[i].rotation);// la multiplica por la rotacion del hueso del orgen
                         selfJoints[i].rotation *= selfJointsInitRotation[i];// y la multiplica por la rotacion inicial del hueso destino*/
                        selfJoints[i].rotation = srcJointsInitRotation[k]; ;// setea la rotacion inicial del destino
                        selfJoints[i].rotation *=  Quaternion.Inverse(totalRotation[nombre[0]][posicion]);// la multiplica por la rotacion del hueso del orgen
                        selfJoints[i].rotation *= selfJointsInitRotation[j];// y la multiplica por la rotacion inicial del hueso destino

                k++;
                        
                    }


                }
                j++;

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
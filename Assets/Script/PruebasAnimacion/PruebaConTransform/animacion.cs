
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System;
public class animacion : MonoBehaviour
{

    //nuestro modelo 
    public Transform srcModel;
    //el animator de nuestro modelo
    Animator selfAnimator;
    //listas con los joins tanto del origen como del destino
    [SerializeField] List<Transform> selfJoints = new List<Transform>();
    [SerializeField] List<Transform> srcJoints = new List<Transform>();
    //diccionario con: nombre hueso: lista de las rotaciones del hueso
    [SerializeField] Dictionary<String, List<Quaternion>> totalRotation = new Dictionary<string, List<Quaternion>>();

    //cuaternion para la rotacion inicial
    [SerializeField] Quaternion selfInitRotation = new Quaternion();
    //lista para las rotaciones en el frame 0 de nuestro modelo y de lo leido del txt
    [SerializeField] List<Quaternion> srcJointsInitRotation = new List<Quaternion>();
    [SerializeField] List<Quaternion> selfJointsInitRotation = new List<Quaternion>();
    //guarda la root y la posicion de las
    [SerializeField] Transform srcRoot;
    [SerializeField] Transform selfRoot;
    //guarda las posiciones iniciales
    [SerializeField] Vector3 srcInitPosition = new Vector3();
    [SerializeField] Vector3 selfInitPosition = new Vector3();
    // boolean para saber si se han caclulado ya todas las posiciones y ejecutar el retargeting
    [SerializeField] bool ejecutar = false;
    // los huesos van de 0,32 y hay que tenerlo en cuenta
    [SerializeField] int posicion = 0;
    //el retargeting se pone la rotación de todos los huesos, cuando se ha acabado se puede pasar al siguiente frame
    bool terminado = false;
    // los huesos que se van a utilziar en función del humanBody
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
            //como hay huesos que sobran, hay que comprobar aqullos que no están en null para meterlo en el arry
            //de rotaciones
            if (selfJoints[i] != null)
            {
                selfJointsInitRotation.Add(selfJoints[i].rotation);
            }

        }
    }

    //se tiene que llamar desde fuera para inicializar los valores
    //seteamos la jerarquiía de todos los huesos a 0n(no se usa actualmente)
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
   //calculo de la posicion en localposicition(no se usa de momento)
    public void setPosition(Vector3 posicion, float sca, Transform hueso)
    {
        Debug.Log("Hueso en posicion " + hueso.name);
        hueso.localPosition = transform.InverseTransformPoint(posicion.x / sca, posicion.y / sca, posicion.z / sca);


    }
    //calculo de las rotaciones actuales
    public Quaternion setRotation(Vector3 posicion1, Vector3 posicion2)
    {
       return Quaternion.FromToRotation(posicion1, posicion2);
    }
    
    public void generarRotacionExtremos(List<Vector3> totalB,String nombre,  Vector3 huesoSiguiente0, Vector3 huesoSiguiente1, List<Quaternion> hueso)
    {
        for (int pos = 0; pos < totalB.Count - 1; pos++)
        {

            Vector3 direccion1 = huesoSiguiente0 - totalB[pos];
            Vector3 direccion2 = huesoSiguiente1 - totalB[pos+1];
            hueso.Add(setRotation(direccion1, direccion2));
            if (posicion == 0)
            {
                srcJointsInitRotation.Add(setRotation(direccion1, direccion2));
            }
        }
        totalRotation.Add(nombre, hueso);
    }
    public void generarRotacion( String nombre, List<Vector3> huesoActual, List<Vector3> huesoSiguiente, List<Quaternion> hueso)
    {
        for (int pos = 0; pos < huesoActual.Count - 1; pos++)
        {

            Vector3 direccion1 = huesoSiguiente[pos] - huesoActual[pos];
            Vector3 direccion2 = huesoSiguiente[pos+1] - huesoActual[pos + 1];
            hueso.Add(setRotation(direccion1, direccion2));
            if (posicion == 0)
            {
                srcJointsInitRotation.Add(setRotation(direccion1, direccion2));
            }
        }
        totalRotation.Add(nombre, hueso);
    }
    //metodo que se llama desde el txtManager para empezar a setear la animación al objeto
    public void initAll(Dictionary<String, List<Vector3>> totalBody)
    {
        //inicializamos los huesos
        InitBones();
        Vector3 newPosition = transform.InverseTransformPoint(totalBody["Hips"][0].x, totalBody["Hips"][0].y, totalBody["Hips"][0].z);
        srcRoot.localPosition = newPosition;
        SetJointsInitRotation();
        
        float scalaSelfJoints = selfJoints[2].position.y;
        float scalatotal = totalBody["Hips"][0].y;
        float scala = scalatotal - scalaSelfJoints;
        this.gameObject.transform.position = new Vector3(totalBody["Hips"][0].x / scala, totalBody["Hips"][0].y / scala, totalBody["Hips"][0].z / scala);

        for (int i = 0; i < selfJoints.Count; i++)
        {// poniendo antes bonesToUse tendrá su organización 

            if (selfJoints[i] != null)
            {
                String[] nombre = selfJoints[i].ToString().Split(' ');
               
                    switch (nombre[0])
                    {
                        case "Hips":

                            List<Quaternion> Hips = new List<Quaternion>();
                            generarRotacionExtremos(totalBody["Hips"],"Hips", selfJoints[i].up, selfJoints[i].up, Hips);
                            break;

                        case "LeftUpLeg":
                            List<Quaternion> LeftUpLeg = new List<Quaternion>();
                            generarRotacion("LeftUpLeg", totalBody["LeftLeg"], totalBody["LeftLeg"] , LeftUpLeg);
                            break;
                        case "RightUpLeg":
                            //setPosition(totalBody["RightUpLeg"][0], scala);
                            List<Quaternion> RightUpLeg = new List<Quaternion>();

                            generarRotacion("RightUpLeg", totalBody["RightUpLeg"], totalBody["RightLeg"], RightUpLeg);

                            break;
                        case "Spine":
                            List<Quaternion> Spine = new List<Quaternion>();

                            generarRotacion("Spine", totalBody["Spine"], totalBody["Neck"], Spine);

                            break;
                        case "Neck":
                            List<Quaternion> Neck = new List<Quaternion>();
                            generarRotacion("Neck", totalBody["Neck"], totalBody["Head"], Neck);
                            break;
                        case "LeftShoulder":
                            List<Quaternion> LeftShoulder = new List<Quaternion>();
                            generarRotacion("LeftShoulder", totalBody["LeftShoulder"], totalBody["LeftArm"], LeftShoulder);
                            break;
                        case "RightShoulder":
                            List<Quaternion> RightShoulder = new List<Quaternion>();
                            generarRotacion("RightShoulder", totalBody["RightShoulder"], totalBody["RightArm"], RightShoulder);

                            break;
                        case "LeftLeg":
                            List<Quaternion> LeftLeg = new List<Quaternion>();
                            generarRotacion("LeftLeg", totalBody["LeftLeg"], totalBody["LeftFoot"], LeftLeg);
                            break;
                        case "RightLeg":
                            List<Quaternion> RightLeg = new List<Quaternion>();
                            generarRotacion("RightLeg", totalBody["RightLeg"],totalBody["RightFoot"], RightLeg);

                            break;
                        case "Head":
                            List<Quaternion> Head = new List<Quaternion>();
                        generarRotacionExtremos(totalBody["Head"], "Head", selfJoints[i].up, selfJoints[i].up, Head);

                        break;
                        case "LeftArm": //equivale a nuestro leftArm
                            List<Quaternion> LeftArm = new List<Quaternion>();
                            generarRotacion("LeftArm", totalBody["LeftArm"],totalBody["LeftForeArm"], LeftArm);

                            break;

                        case "RightArm": //equivale a nuestro right Arm
                            List<Quaternion> RightArm = new List<Quaternion>();
                            generarRotacion("RightArm", totalBody["RightArm"], totalBody["RightForeArm"], RightArm);

                            break;

                        case "LeftForeArm":
                            List<Quaternion> LeftForeArm = new List<Quaternion>();
                            generarRotacion("LeftForeArm", totalBody["LeftForeArm"], totalBody["LeftHand"], LeftForeArm);

                            break;

                        case "LeftHand":
                            List<Quaternion> LeftHand = new List<Quaternion>();
                            generarRotacion("LeftHand", totalBody["LeftHand"],totalBody["LeftHand_end"], LeftHand);

                            break;
                        case "LeftHand_end":
                            List<Quaternion> LeftHand_end = new List<Quaternion>();

                        generarRotacionExtremos(totalBody["LeftHand_end"], "LeftHand_end", selfJoints[i].up, selfJoints[i].up, LeftHand_end); 

                            break;
                        case "RightForeArm":
                            List<Quaternion> RightForeArm = new List<Quaternion>();
                            generarRotacion("RightForeArm", totalBody["RightForeArm"],totalBody["RightHand"], RightForeArm);

                            break;
                        case "RightHand":
                            List<Quaternion> RightHand = new List<Quaternion>();
                            generarRotacion("RightHand", totalBody["RightHand"], totalBody["RightHand_end"], RightHand);

                            break;
                        case "RightHand_end":
                            List<Quaternion> RightHand_end = new List<Quaternion>();

                        generarRotacionExtremos(totalBody["RightHand_end"], "RightHand_end", selfJoints[i].up, selfJoints[i].up, RightHand_end); break;
                        case "LeftFoot":
                            List<Quaternion> LeftFoot = new List<Quaternion>();
                            generarRotacion("LeftFoot", totalBody["LeftFoot"], totalBody["LeftToeBase"], LeftFoot);

                            break;

                        case "LeftToeBase":
                            List<Quaternion> LeftToeBase = new List<Quaternion>();

                        generarRotacionExtremos(totalBody["LeftToeBase"], "LeftToeBase", selfJoints[i].up, selfJoints[i].up, LeftToeBase);

                        break;

                        case "RightFoot"://9
                            List<Quaternion> RightFoot = new List<Quaternion>();
                            generarRotacion("RightFoot", totalBody["RightFoot"], totalBody["RightToeBase"], RightFoot);

                            break;

                        case "RightToeBase"://9
                            List<Quaternion> RightToeBase = new List<Quaternion>();


                        generarRotacionExtremos(totalBody["RightToeBase"], "RightToeBase", selfJoints[i].up, selfJoints[i].up, RightToeBase);

                        break;
                    }
                }
            }
                  
           
        //se pone a true para poder hacer ya el retargeting
        ejecutar = true;

    }
 /*case "LeftUpLeg":
            setPosition(totalBody["LeftUpLeg"][0], scala, selfJoints[i]);
            setRotation(totalBody["LeftUpLeg"][0],  selfJoints[i]);
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
                      
                        //internet
                        selfJoints[i].rotation = selfJointsInitRotation[j];
                        //la rotación iniail * la actual.
                        selfJoints[i].rotation *= (srcJointsInitRotation[k] * Quaternion.Inverse(totalRotation[nombre[0]][posicion]));
                        //selfJoints[i].rotation *= ( Quaternion.Inverse(totalRotation[nombre[0]][posicion]));
                        //selfJoints[i].rotation *= selfJointsInitRotation[j];

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

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
    public Vector3 posicionInicial;
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

    //rotaciones iniciales del esqueleto que se ve por pantalla
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
    
    // se utiliza en los extremos Ya uqe se hace la rotación respecto al vector Up de ese mismo hueso
    public void generarRotacionExtremos(List<Vector3> totalB,String nombre,  Vector3 huesoSiguiente0, Vector3 huesoSiguiente1, List<Quaternion> hueso)
    {
        for (int pos = 0; pos < totalB.Count - 1; pos++)
        {
            //se ccalcula la direccion
            Vector3 direccion1 = huesoSiguiente0 - totalB[pos];
            Vector3 direccion2 = huesoSiguiente1 - totalB[pos+1];
            //se calcula la rotación y se añade al hueso correspondiente
            hueso.Add(setRotation(direccion1, direccion2));
            if (posicion == 0)
            {
                // si estamos en el frame 0 se añade a la lista de rotaciones iniciales
                srcJointsInitRotation.Add(setRotation(direccion1, direccion2));
            }
        }
        // se añade a las rotaciones totales
                totalRotation.Add(nombre, hueso);
    }
    //se va a aplicar a todos los huesos a excepción de los extremos. 
    public void generarRotacion( String nombre, List<Vector3> huesoActual, List<Vector3> huesoSiguiente, List<Quaternion> hueso)
    {
        for (int pos = 0; pos < huesoActual.Count - 1; pos++)
        {
            //se caclula la dirección 
            Vector3 direccion1 = huesoSiguiente[pos] - huesoActual[pos];
            Vector3 direccion2 = huesoSiguiente[pos+1] - huesoActual[pos + 1];
            //se calcula la rotaicón y se añade en la lista del hueso actual
            hueso.Add(setRotation(direccion1, direccion2));
            if (posicion == 0)
            {//si estamos en el frame 0 se añade a la lista de rotaciones iniciales
                srcJointsInitRotation.Add(setRotation(direccion1, direccion2));
            }
        }
        // se añade al dicccionario 
        totalRotation.Add(nombre, hueso);
    }
    //metodo que se llama desde el txtManager para empezar a setear la animación al objeto
    public void posicioneEn0()
    {
        posicionInicial = this.transform.position;
        this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
    }
    public void initAll(Dictionary<String, List<Vector3>> totalBody)
    {
        //posicionInicial = this.transform.position;
        //inicializamos los huesos
        InitBones();
        //calculamos la nueva posicion para el esqueleto y la seteamos 
        //Vector3 newPosition = transform.InverseTransformPoint(totalBody["Hips"][0].x, totalBody["Hips"][0].y, totalBody["Hips"][0].z);
        //srcRoot.localPosition = newPosition;
        //calculamos las totaciones iniciales
        SetJointsInitRotation();
        //calculamos la escala 
        float scalaSelfJoints = selfJoints[0].position.y;
        float scalatotal = totalBody["Hips"][0].y;
        float scala = scalatotal - scalaSelfJoints;
        //this.gameObject.transform.position = new Vector3(newPosition.x / scala, newPosition.y / scala, newPosition.z / scala);
        //recorrems todos los huesos
        for (int i = 0; i < selfJoints.Count; i++)
        {   //si es distinto a null
            if (selfJoints[i] != null)
            {// seleccionamos el nombre
                String[] nombre = selfJoints[i].ToString().Split(' ');
               //switch con el nombre y llamada a generar rotación 
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


    void LateUpdate()
    {
        // si se ha leido todas las animaciones
     if (ejecutar)
        {// si se han recorrido todas los huesos
            if (terminado == false)
            {
                //llamamos  al retargeting
               Retargeting();
                //SetPosition();
                // aumentamos la posición del frame
                posicion += 1;
            }
        }
    }


                        
                
    
    //MÉTODO DE ROTACIÓN DE RETARGETING
    private void Retargeting()
    {
        //indicie de la lista de rotaciones del objeto que se ve en pantalla
        int j = 0;
        //indicie de la lista de rotaciones de lo que leemos
        int k = 0;
        // terminado a true
        terminado = true;
        //para recorreernos cada uno de los huesos
       for (int i = 0; i < selfJoints.Count; i++)
         {   //comprobamos que no sea null
            if (selfJoints[i] != null)
            {
                String[] nombre = selfJoints[i].ToString().Split(' ');
               
                // lista auxiliar  para el trygetvalue
                List<Quaternion> aux = new List<Quaternion>();
                // si existe ese elemento en la lista de rotaciones
                if (totalRotation.TryGetValue(nombre[0], out aux))
                {
                    //primero comprobar si hemos llagado a la ultima posucion de la lista de animación, si es así volver a poner a 0 par que esté en loop
                    if (posicion >= totalRotation[nombre[0]].Count) posicion = 0;
                    
                   // comprobamos que la lista de rotaciones sea mayor a 0, y que los indicies j y k estén dentro de rango
                    if (totalRotation[nombre[0]].Count > 0 && j<selfJointsInitRotation.Count && k< srcJointsInitRotation.Count)
                    {
                        // seteamos la rotacion inicial del objeto de pantalla
                
                            selfJoints[i].rotation = selfJointsInitRotation[j];
                        
                      
                        //le añadimos la rotación  inicial de nuestro objeto y la rotación en el momento
                        selfJoints[i].rotation *= (srcJointsInitRotation[k] * Quaternion.Inverse(totalRotation[nombre[0]][posicion]));
                        //aumentamos indice
                        k++;
                        
                    }
                }
                j++;

            }

            // si ha terminado poner a false para que pase al siguiente frame
            if (i + 1 >= selfJoints.Count)
            { terminado = false; 
            }

        }
       
       

    }


    //No podemos usar las rotaciones
    private void SetPosition()
    {// setea la nueva posicion( posicion root local- la inicial del origen)+ la inicial del destino

        selfRoot.localPosition = (srcRoot.localPosition - srcInitPosition) + selfInitPosition;
    }
    //metodo para parar la animación
    public void setEjecutar(bool ejecutar)
    {
        this.ejecutar = ejecutar;
    }
    public void borrado()
    {
        selfJoints.Clear();
        srcJoints.Clear();
        totalRotation.Clear();
        srcJointsInitRotation.Clear();
        selfJointsInitRotation.Clear();
        
        ejecutar = false;
        posicion = 0;
        terminado = false;
    }
    
}

/*case "LeftUpLeg":
           setPosition(totalBody["LeftUpLeg"][0], scala, selfJoints[i]);
           setRotation(totalBody["LeftUpLeg"][0],  selfJoints[i]);
*/
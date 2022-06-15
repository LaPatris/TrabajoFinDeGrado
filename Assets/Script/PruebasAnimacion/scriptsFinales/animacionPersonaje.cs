using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System;

public class animacionPersonaje : MonoBehaviour
{

    //nuestro modelo 
    public Transform srcModel;
    //el animator de nuestro modelo
    public Animator selfAnimator;
    //listas con los joins tanto del origen como del destino
    [SerializeField] List<Transform> selfJoints = new List<Transform>();
    [SerializeField] List<Quaternion> selfJointsInit = new List<Quaternion>();
    [SerializeField] List<Transform> srcJoints = new List<Transform>();
    //diccionario con: nombre hueso: lista de las rotaciones del hueso
   [SerializeField] Dictionary<String, List<Quaternion>> totalRotation = new Dictionary<string, List<Quaternion>>();
    //[SerializeField] Dictionary<String, List<Vector3>> totalRotation = new Dictionary<string, List<Vector3>>();

    //cuaternion para la rotacion inicial
    public Quaternion selfInitRotation = new Quaternion();
    //lista para las rotaciones en el frame 0 de nuestro modelo y de lo leido del txt
    [SerializeField] List<Quaternion> srcJointsInitRotation = new List<Quaternion>();
    [SerializeField] List<Quaternion> selfJointsInitRotation = new List<Quaternion>();
    [SerializeField] List<Vector3> posicionesHips = new List<Vector3>();
    //guarda la root y la posicion de las
    [SerializeField] Transform srcRoot;
   public Transform rotacion;
    //guarda las posiciones iniciales
    [SerializeField] Vector3 srcInitPosition = new Vector3();
    [SerializeField] Vector3 selfInitPosition = new Vector3();
    // boolean para saber si se han caclulado ya todas las posiciones y ejecutar el retargeting
    [SerializeField] bool ejecutar = false;
    // los huesos van de 0,32 y hay que tenerlo en cuenta
    [SerializeField] int posicion = 0;
    [SerializeField] float tiempoEscala = 1.0f;
    private float timer = 0.0f;
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

    private void Awake()
    {
        Time.fixedDeltaTime = 0.02f;
    }
    void Start()
    {
        //setea los animator
        selfAnimator = this.GetComponent<Animator>();

        //setea las rotaciones inciiales
        selfInitRotation = gameObject.transform.rotation;
        // guarda la root
        //selfRoot = selfAnimator.GetBoneTransform(HumanBodyBones.Hips);

    }
    public void InitBones()
    {
        //inicializa los huesos tanto del origen como de la copia
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            if(selfAnimator.GetBoneTransform(bonesToUse[i])==null)
            Debug.Log("NULL " + bonesToUse[i].ToString());
            selfJoints.Add(selfAnimator.GetBoneTransform(bonesToUse[i]));
        }
    }
    public void Init()
    {
        //inicializa los huesos tanto del origen como de la copia
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            if (selfAnimator.GetBoneTransform(bonesToUse[i]) != null)
                selfJointsInit.Add(selfAnimator.GetBoneTransform(bonesToUse[i]).rotation);
            
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
                selfJointsInitRotation.Add(selfJoints[i].localRotation);
            }

        }
    }


    private Quaternion fromEulerZXY(Vector3 euler)
    {//matriz de rotación 

        //EN PRINCIPIO EN X SE HA GUARDADO LAS Z ; y LAS X ; Z LAS Y
        //y*x*z
       return Quaternion.AngleAxis(euler.z, Vector3.forward) * Quaternion.AngleAxis(euler.y, Vector3.right) * Quaternion.AngleAxis(euler.x, Vector3.up);

        //return Quaternion.AngleAxis(euler.z, Vector3.forward) * Quaternion.AngleAxis(euler.x, Vector3.right) * Quaternion.AngleAxis(euler.y, Vector3.up);
    }

    private float wrapAngle(float a)
    {
        
        if (a > 180f)
        {
            return a - 360f;
        }
        if (a < -180f)
        {
           return  a +360;
           // return 360 + a;
        }
        return a;
    }

    public void posicioneEn0()
    {
       posicionInicial = this.transform.position;
        Init();
        this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        this.transform.rotation =  new Quaternion(0,0,0,0);
       
    }
    public void initAll(Dictionary<String, List<Vector3>> totalBody/*, List<Vector3>posiciones*/)
    { 
        //posicionesHips = posiciones;
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
        foreach (KeyValuePair<String, List<Vector3>> kvp in totalBody)
        {
            //Debug.Log("nombres" + kvp.Key);
            List<Quaternion> quats = new List<Quaternion>();
          foreach(Vector3 vec in kvp.Value)
            {
                Vector3 eulerBVH = new Vector3();
                //EN PRINCIPIO EN 0 SE HA GUARDADO LAS Z EN 1 LAS X Y EN 2 LAS Y
             /*   if (kvp.Key.Equals("LeftShoulder") || kvp.Key.Equals("LeftUpArm") || kvp.Key.Equals("LeftForeArm") || kvp.Key.Equals("LeftHand") || kvp.Key.Equals("LeftHandThumb") || kvp.Key.Equals("L_Wrist_End") ||
                    kvp.Key.Equals("RightShoulder") || kvp.Key.Equals("RightUpArm") || kvp.Key.Equals("RightForeArm") || kvp.Key.Equals("RightHand") || kvp.Key.Equals("RightHandThumb") || kvp.Key.Equals("R_Wrist_End"))
                {
                    eulerBVH = new Vector3(wrapAngle(-vec[2]), wrapAngle(vec[1]), wrapAngle(-vec[0]));
                    //eulerBVH = new Vector3(-vec[2], vec[1], -vec[0]);

                }
                else {
                    //eulerBVH = new Vector3(wrapAngle(vec[2]), wrapAngle(-vec[1]), wrapAngle(vec[0]));
                   if (kvp.Key.Equals("Neck") || kvp.Key.Equals("Head") )
                    {

                        eulerBVH = new Vector3(wrapAngle(-vec[2]), wrapAngle(vec[1]), wrapAngle(-vec[0]));
                        //eulerBVH = new Vector3(-vec[2], vec[1], -vec[0]);
                       // eulerBVH = new Vector3(-vec[0], vec[1], vec[2]);
                        
                    }*/
                    //else
                    if (kvp.Key.Equals("LeftUpLeg") || kvp.Key.Equals("LeftLowLeg") || kvp.Key.Equals("LeftFoot")  || kvp.Key.Equals("RightUpLeg") || kvp.Key.Equals("RightLoeLeg") 
                        || kvp.Key.Equals("RightFoot") || kvp.Key.Equals("LeftToeBase") || kvp.Key.Equals("RightToeBase"))
                    {

                    //eulerBVH = new Vector3(vec[2], -vec[1], vec[0]);

                    eulerBVH = new Vector3(wrapAngle(vec[2]), wrapAngle(-vec[1]), wrapAngle(-vec[0]));
                    //eulerBVH = new Vector3(vec[2], -vec[1], vec[0]);
                }
                    else
                    {
                  
                    eulerBVH = new Vector3(wrapAngle(-vec[2]), wrapAngle(vec[1]), wrapAngle(-vec[0]));

                    }
               // }
                //HIPS-168.579 69.5547 - 187.273
                //en principio esto ya esta colocado como Z,X,Y que es como se tienen que meter
                // offset = new Vector3(-node.offsetX, node.offsetZ, -node.offsetY);
                quats.Add(fromEulerZXY(eulerBVH));
                //quats.Add(Quaternion.Euler(eulerBVH[2], eulerBVH[1], eulerBVH[0]));
           }
            //sehan guardado la matriz de rotación de cada momento 
            totalRotation.Add(kvp.Key, quats);
            selfJoints[0].localRotation = totalRotation["Hips"][0];
        }
        //this.gameObject.transform.position = new Vector3(newPosition.x / scala, newPosition.y / scala, newPosition.z / scala);
        //recorrems todos los huesos
        //se pone a true para poder hacer ya el retargeting
        ejecutar = true;

    }


    void FixedUpdate()
    {
        // si se ha leido todas las animaciones
        if (ejecutar)
        {// si se han recorrido todas los huesos
            if (terminado == false)
            {
                //llamamos  al retargeting
                Retargeting();
                //SetPosition();
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
                if (selfJoints[i] != null )
                {
                    String[] nombre = selfJoints[i].ToString().Split(' ');
                    // lista auxiliar  para el trygetvalue
                    // List<Quaternion> aux = new List<Quaternion>();
                    List<Quaternion> aux = new List<Quaternion>();
                // si existe ese elemento en la lista de rotaciones
                //Debug.Log("nombre= " + nombre[0] + " selfJoin= " + selfJoints[i].name);
                if (totalRotation.TryGetValue(nombre[0], out aux))
                {
                    if (  nombre[0].Equals("LeftUpLeg")  || nombre[0].Equals("LeftLowLeg") || nombre[0].Equals("LeftFoot") ||  nombre[0].Equals("RightUpLeg") || nombre[0].Equals("RightLoeLeg") || nombre[0].Equals("RightFoot") ) {

                        //|| nombre[0].Equals("Neck")
                        selfJoints[i].localRotation = Quaternion.Lerp(selfJoints[i].localRotation, selfJointsInitRotation[j] * totalRotation[nombre[0]][posicion], 0.1f);
                        // ombre[0].Equals("Head") || selfJoints[i].localRotation= selfJointsInitRotation[j]* totalRotation[nombre[0]][posicion];

                    }
                    else
                    {
                        if (!nombre[0].Equals("Hips"))//&& !nombre[0].Equals("LeftToeBase") && !nombre[0].Equals("RightToeBase"))&& !nombre[0].Equals("Neck")
                        {
                            selfJoints[i].localRotation = Quaternion.Lerp(selfJoints[i].localRotation,  totalRotation[nombre[0]][posicion], 0.1f);

                           // selfJoints[i].localRotation = /*selfJointsInitRotation[j]* */totalRotation[nombre[0]][posicion];
                        }
                        // selfJoints[i].localRotation = totalRotation[nombre[0]][posicion];
                    }
                    j++;

                    }

                    // si ha terminado poner a false para que pase al siguiente frame
                    if (i + 1 >= selfJoints.Count)
                    {
                        terminado = false;
                    }

                }


             }
    }


    //No podemos usar las rotaciones
    private void SetPosition()
    {// setea la nueva posicion( posicion root local- la inicial del origen)+ la inicial del destino

       // selfRoot.localPosition = (srcRoot.localPosition - srcInitPosition) + selfInitPosition;
    }
    //metodo para parar la animación
    public void setEjecutar(bool ejecutar)
    {
        this.ejecutar = ejecutar;
    }
    public void borrado()
    {
        int j = 0;
        ejecutar = false;
        Debug.Log("ejecutar" + ejecutar);
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            if (selfAnimator.GetBoneTransform(bonesToUse[i]) != null)
            {
                selfAnimator.GetBoneTransform(bonesToUse[i]).rotation = selfJointsInit[j];
                j++;
            }
            
        }
        
        selfJoints.Clear();
        srcJoints.Clear();
        totalRotation.Clear();
        srcJointsInitRotation.Clear();
        selfJointsInitRotation.Clear();

        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        posicion = 0;
        terminado = false;
    }
   
}

/*case "LeftUpLeg":
           setPosition(totalBody["LeftUpLeg"][0], scala, selfJoints[i]);
           setRotation(totalBody["LeftUpLeg"][0],  selfJoints[i]);
*/
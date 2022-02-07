using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Autodesk.Fbx;
//using UnityEditorInternal;
using UnityEditor.Animations;
public class ReadAnim : MonoBehaviour
{



    //need for animation

    [Header("Animation")]
    [SerializeField] Animation animationReader;// an animation for the animation which want to read
    [SerializeField] public AnimationClip animationClip; // an animationclip because we are going to generate one
    [SerializeField] AnimationCurve curve; // animation curve IDK
    [SerializeField] int numberBonesAnim;// number of bone just for control
    [SerializeField] public bool conditionChange = false; // to make the change of states
    [SerializeField] AnimatorState currentState; //  state what i want to change
    [SerializeField] AnimatorStateTransition newTransition; // transition of states
    [SerializeField] AnimatorController animatorController; // an animator controller due to is necesary to run a animation
    [SerializeField] AnimatorState defaultState; // the first state

    //double[] KEY_TIME = new double[3];
    //IDK
    static readonly double[] KEY_TIME = new double[] { 0.0, 0.5, 1.0 };
    static readonly float[] KEY_VALUE = new float[] { 0.0f, 90.0f, 180.0f };


    [SerializeField] public bool creadoStado = false; // TO control the create of states
                                                      //object prueba idk
    PruebaFBX prueba;



    // need for obj how 
    [Header("Reader object ")]
    [SerializeField] FbxManager managerFBX; // to read a fbx is necesary
    [SerializeField] FbxObject fbxObjOrigin; //idk
    [SerializeField] FbxObject fbxObjFinal; //idk
    [SerializeField] List<FbxNode> ListOfBonesOrigen = new List<FbxNode>(); // list of nodes( of the fbx i want to read)
    [SerializeField] List<FbxAnimCurveNode> listOfCurvesNodes = new List<FbxAnimCurveNode>(); // list of curve nodes
    [SerializeField] List<FbxAnimCurve> listOfCurves = new List<FbxAnimCurve>(); // list of animation curve
    [SerializeField] List<FbxAnimLayer> listOfLayer = new List<FbxAnimLayer>(); //list of animation layer
    [SerializeField] Dictionary<string, List<FbxAnimCurveKey>> keysWithName = new Dictionary<string, List<FbxAnimCurveKey>>(); //dict of animcurves with names
    [SerializeField] Dictionary<string, FbxNode> nameBonesOrigen = new Dictionary<string, FbxNode>(); // name of bones
    [SerializeField] Dictionary<string, AnimationCurve> translateCurves = new Dictionary<string, AnimationCurve>(); // dic of tranlatecurves with the name
    [SerializeField] FbxDouble3 initReaderObj; //position init hips
    [SerializeField] Vector3 changeToV3Init;// change fbxdouble3 to vector3
    [Header("MyObj ")]
    [SerializeField] Vector3 initPosition; //init myobj position
    [SerializeField] bool readyToChange;
    // Start is called before the first frame update
    void Start()
    {

        prueba = GetComponent<PruebaFBX>();
        animationReader = GetComponent<Animation>();
        curve = AnimationCurve.Linear(0, 0, 2, 2);
        animationClip = new AnimationClip();
        animationClip.name = "NuevaAnim";
        CreateAnimationClip();
        initPosition = this.transform.position;
        ImportCharacterToCopy();
        // ChangeAnimation();
    }
    //Create de anim Clip of MyObj
    public void CreateAnimationClip()
    {
        foreach (Transform bone in prueba.MyBonesInit)
        {
            //reading each bones
            animationClip.SetCurve("Bone:  " + bone.name.ToString(), typeof(Transform), " TransformClip", curve);

        }
        //ading to the animaiton layers
        animationReader.AddClip(animationClip, " TransformClip");
        animationReader.clip = animationClip;
        prueba.animacion = animationClip;
        prueba.nuevo = false;
    }

    // to change the state of the animation graph
    public void ChangeStateAnimator()
    {
        if (conditionChange)
        {
            animatorController = prueba.anim;
            defaultState = animatorController.layers[0].stateMachine.states[0].state;
            Debug.Log("Layer " + animatorController.layers[0].name);
            currentState = new AnimatorState();
            currentState = animatorController.layers[0].stateMachine.AddState(animationClip.name);

            newTransition = new AnimatorStateTransition();
            newTransition.destinationState = animatorController.layers[0].stateMachine.states[1].state;

            defaultState.AddTransition(newTransition);


            //EditorUtility.SetDirty(animatorController);
            AssetDatabase.SaveAssets();
            conditionChange = false;
            creadoStado = true;
        }

    }

    //to delete the new state
    public void RemoveState()
    {
        //remove transition
        defaultState.RemoveTransition(newTransition);
        //remove state
        animatorController.layers[0].stateMachine.RemoveState(currentState);


    }
    // Update is called once per frame
    void Update()
    {
        /*if (prueba.nuevo)
            CreateAnimationClip();*/
    }
    //importamos el personaje que queremos copiar
    protected void ImportCharacterToCopy()
    {
        // create FBX Manager
        FbxManager managerFBX = FbxManager.Create();
        // create an IOSettings object
        FbxIOSettings ios = FbxIOSettings.Create(managerFBX, Globals.IOSROOT);
        // set some IOSettings options 
        // create  importer 
        FbxImporter importerP1 = FbxImporter.Create(managerFBX, "importerP1");
        /*This class facilitates the testing/reporting of errors. It encapsulates the status code and the internal FBXSDK error code as returned by the API functions. */
        FbxStatus fbxStatus = importerP1.GetStatus();
        if (fbxStatus != null)
            ios.SetBoolProp(Globals.IMP_FBX_MATERIAL, false);
        ios.SetBoolProp(Globals.IMP_FBX_TEXTURE, false);
        ios.SetBoolProp(Globals.IMP_FBX_ANIMATION, true);
        ios.SetBoolProp(Globals.IMP_FBX_EXTRACT_EMBEDDED_DATA, false);
        ios.SetBoolProp(Globals.IMP_FBX_GLOBAL_SETTINGS, true);

        //create a scene
        FbxScene escenaP1 = FbxScene.Create(managerFBX, "escenaP1");
        //select the object that i want to read
        // donde se encuentra, formato( si pones -1 lo detecta sol, el iosetting)
        importerP1.Initialize("Assets/Personaje/personaje1.fbx", -1, ios);
        //and  import scene
        importerP1.Import(escenaP1);
        //importerP1.Destroy();//idk why i should do it

        //read  the character and save it 
        FbxNode nodoPersonaje1 = FbxNode.Create(managerFBX, "personaje1");
        //initialize
        nodoPersonaje1 = escenaP1.GetRootNode();
        //same but just the model  with the skeleton( easier to read the bones)
        FbxNode nodeHips1 = FbxNode.Create(managerFBX, "");

        nodeHips1 = nodoPersonaje1.GetChild(0);
        //get the transalation of the hip
        //init de initReaderObje
        initReaderObj = nodeHips1.LclTranslation.Get();
        //cinvert Fbxd3 to vector 3 Init
        changeToV3Init = new Vector3((float)initReaderObj.X, (float)initReaderObj.Y, (float)initReaderObj.Z);
        valuesNode(nodeHips1);
        //guardamos sus huesos
        getBoneOrigin2(nodeHips1, escenaP1, managerFBX);
        //simulate read position of a bone
        // readList(ListOfBonesOrigen);
        readyToChange = true;
        changeHipsPosition();
        getAnim(nodeHips1, escenaP1, managerFBX);

    }

    //to change the position to Myobj
    void changeHipsPosition()
    {
        this.transform.localPosition = (changeToV3Init + initPosition) - changeToV3Init;
    }

    //values of the node
    void valuesNode(FbxNode pNode)
    {
        string nodeName = pNode.GetName();
        FbxDouble3 translation = pNode.LclTranslation.Get();
        FbxDouble3 rotation = pNode.LclRotation.Get();
        FbxDouble3 scaling = pNode.LclScaling.Get();
        Debug.Log("Nombre: " + nodeName + " traslación: " + translation.ToString() + " rotation: " + rotation.ToString() + " scaling: " + scaling.ToString());
    }

    void getAnim(FbxNode rootNode, FbxScene escena, FbxManager man)
    {
        /*
         Structure of Anim
        The FBX SDK uses these data structures for animation: 
        animation stacks (FbxAnimStack), 
        animation layers (FbxAnimLayer),
        animation curve nodes (FbxAnimCurveNode),
        animation curves (FbxAnimCurve),
        and animation curve keys (FbxAnimCurveKey).
        The data structures are connected to each other through object-to-object (OO) connections and object-to-property (OP) connections in a FBX scene (FbxScene).
         */
        //Create a animStack

        FbxAnimStack stackForAnim = FbxAnimStack.Create(man, "stack");
        Debug.Log("Creamos la stack" + stackForAnim.GetName());
        // escena.AddMember(stackForAnim);

        //layer if this node
        FbxAnimLayer layer = FbxAnimLayer.Create(stackForAnim, "layer");

        //add the layer to de stack

        stackForAnim.AddMember(layer);

        Debug.Log("añadimos  la layer a la stack " + stackForAnim.GetMemberCount().ToString());
        //create anime curve node

        Debug.Log("Creamos la curveNode");
        // Get the hips’s curve node for local translation.
        // The second parameter to GetCurveNode() is "true" to ensure
        // that the curve node is automatically created, if it does not exist.
        FbxAnimCurveNode curveNode = FbxAnimCurveNode.Create(rootNode, "curveNode1");

        layer.AddMember(curveNode);

        Debug.Log("Añadimos a  la layer" + layer.GetMemberCount().ToString());

        //  curveNode =rootNode.GetFirstProperty().GetCurveNode(layer,true);
        Debug.Log("Cuantos canales tenemos1 " + curveNode.GetName() + " canales " + curveNode.GetChannelsCount().ToString());

        curveNode.AddChannel("X", rootNode.GetFirstProperty().GetCurveNode(true).GetChannelValue("X", 0));
        Debug.Log("Cuantos canales tenemos2 " + curveNode.GetChannelsCount().ToString());
        // anim curve create
        FbxAnimCurve curveTX = null;// transalation X
        FbxTime myTime;  //  time for the start and stop keys.
        int myKeyIndex = 0;// Index for the keys that define the curve


        // curveTX = curveNode.CreateCurve("transX", "X");
        //  rootNode.LclTranslation.GetCurve(layer, "X", true); // LAYER, NAME chanel , TRUE
        curveTX = rootNode.LclTranslation.GetCurve(layer, "X", true);
        Debug.Log("CUrvetx que hay " + curveTX.KeyGetCount());
        // curveTX = rootNode.LclTranslation.GetCurve(layer, "X", true); // LAYER, NAME chanel , TRUE

        /*
                //Define the starting key of the animation
                curveTX.KeyModifyBegin();

                myTime.SetSecondDouble(0.0);             // Starting time
                myKeyIndex = curveTX.KeyAdd(myTime); // Add the start key; returns 0

                curveTX.KeySet(myKeyIndex,           // Set the zero’th key
                                    myTime,               // Starting time
                                    0.0,                 // Starting X value
                FbxAnimCurveDef::eINTERPOLATION_LINEAR);// Straight line between 2 points
        */


    }

    void getBoneOrigin2(FbxNode rootNode, FbxScene escena, FbxManager man)
    {
        //HACER UNA LISTA DE STACKS Y OTRA DE LAYERS
        // no comprendo el orden de lectura, en el historial está como left, right y spine sin embargo esto lo lee así: spine, left, right
        for (int i = 0; i < rootNode.GetChildCount(); i++)
        {

            //Debug.Log("Estoy en el hueso " + rootNode.GetChild(i).GetName() + " que tiene " + rootNode.GetChild(i).GetChildCount() + " hijos");
            if (!nameBonesOrigen.ContainsKey(rootNode.GetName().ToString()))
            {
                //getAnim(rootNode, escena, man);
                numberBonesAnim += 1;
                nameBonesOrigen.Add(rootNode.GetName().ToString(), rootNode);
                ListOfBonesOrigen.Add(rootNode);
            }
            getBoneOrigin2(rootNode.GetChild(i), escena, man);
            // }
        }

    }



    void readList(List<FbxNode> nodes)
    {
        Debug.Log("**ESTAMOS LEYENDO LA LISTA **");

        Debug.Log("Orden en el que lee los datos: ");
        int aux = 0;
        foreach (FbxNode node in nodes)
        {
            Debug.Log(aux + " " + node.GetName());
            aux += 1;
        }

    }

    /*    void getBoneOrigin(FbxNode rootNode, FbxScene escena, List<FbxNode> auxiliarL, Dictionary<string, FbxNode> auxiliarD)
    {
        // no comprendo el orden de lectura, en el historial está como left, right y spine sin embargo esto lo lee así: spine, left, right
        for (int i = 0; i < rootNode.GetChildCount(); i++)
        {

            // Debug.Log("Estoy en el hueso " + rootNode.GetChild(i).GetName() + " que tiene " + rootNode.GetChild(i).GetChildCount() + " hijos");
            if (!auxiliarD.ContainsKey(rootNode.GetName().ToString()))
            {
                FbxAnimCurveNode animeCNP1 = FbxAnimCurveNode.CreateTypedCurveNode(rootNode.LclTranslation, escena);
                // KeyCode keu animeCNP1.GetCurve((uint) 0 , (uint) 0, rootNode.GetFirstProperty().GetCurveNode().GetChannelName(0)).KeyGet(0);
                listOfCurvesNodes.Add(animeCNP1);


                List<FbxAnimCurveKey> listOfKeys = new List<FbxAnimCurveKey>();

                numberBonesAnim += 1;
                auxiliarD.Add(rootNode.GetName().ToString(), rootNode);
                auxiliarL.Add(rootNode);

                //getBoneOrigin(rootNode.GetChild(i), escena, auxiliarL, auxiliarD);
            }
        }
    }
     /* NO HACER CASO DE MOMENTO
     * void ChangeAnimation(FbxNode rootNode, FbxDouble3 INITpOS)
      {


           for (int i = 0; i < rootNode.GetChildCount(); i++)
          {

              // Debug.Log("Estoy en el hueso " + rootNode.GetChild(i).GetName() + " que tiene " + rootNode.GetChild(i).GetChildCount() + " hijos");
              if (!nameBonesOrigen.ContainsKey(rootNode.GetName().ToString()))
              {
                  for (int b = 0; b <prueba.MyBones.Count; b++)
                      if (prueba.MyBones[b].name == rootNode.GetName() && (rootNode.GetName().Equals("Hips") || rootNode.GetName().Equals("LeftUpLeg")))
                      {
                          changePositions(rootNode.LclTranslation.Get());

                      }
              }
          }
      }*/

}

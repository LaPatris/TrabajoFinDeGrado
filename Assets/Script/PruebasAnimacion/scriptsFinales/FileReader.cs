using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Globalization;

public class FileReader : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Loader settings")]
   public string pathFile;
    public bool terminado = false;
    [SerializeField] List<string> Offsets = new List<string>();
    [SerializeField] List<Vector3> posicionesHips = new List<Vector3>();
    [SerializeField] List<Vector3> distanciasPadres = new List<Vector3>();
    [SerializeField] List<float> escala = new List<float>();
    public Dictionary<String, List<Vector3>> rotaciones = new Dictionary<String, List<Vector3>>();
    
    
    [SerializeField] List<Vector3> Hips = new List<Vector3>();
    [SerializeField] List<Vector3> Spine = new List<Vector3>();
    [SerializeField] List<Vector3> Spine1 = new List<Vector3>();
    [SerializeField] List<Vector3> Neck = new List<Vector3>();
    [SerializeField] List<Vector3> Head = new List<Vector3>();
    
    
    [SerializeField] List<Vector3> LeftShoulder = new List<Vector3>();
    [SerializeField] List<Vector3> LeftUpArm = new List<Vector3>();
    [SerializeField] List<Vector3> LeftForeArm = new List<Vector3>();
    [SerializeField] List<Vector3> LeftHand = new List<Vector3>();
    [SerializeField] List<Vector3> LeftHandThumb = new List<Vector3>();
    [SerializeField] List<Vector3> L_Wrist_End = new List<Vector3>();
    
    [SerializeField] List<Vector3> RightShoulder = new List<Vector3>();
    [SerializeField] List<Vector3> RightUpArm = new List<Vector3>();
    [SerializeField] List<Vector3> RightForeArm = new List<Vector3>();
    [SerializeField] List<Vector3> RightHand = new List<Vector3>();
    [SerializeField] List<Vector3> RightHandThumb = new List<Vector3>();
    [SerializeField] List<Vector3> R_Wrist_End = new List<Vector3>();
    
    
    [SerializeField] List<Vector3> LeftUpLeg = new List<Vector3>();
    [SerializeField] List<Vector3> LeftLowLeg = new List<Vector3>();
    [SerializeField] List<Vector3> LeftFoot = new List<Vector3>();
    [SerializeField] List<Vector3> LeftToeBase = new List<Vector3>();
    //HombroD 
    [SerializeField] List<Vector3> RightUpLeg = new List<Vector3>();
    [SerializeField] List<Vector3> RightLowLeg = new List<Vector3>();
    [SerializeField] List<Vector3> RightFoot = new List<Vector3>();
    [SerializeField] List<Vector3> RightToeBase = new List<Vector3>();
    public animacionPersonaje animP = new animacionPersonaje();
    
    void Start()

    {
        //obtenerDatosFichero();
        //obtenerEscalaHueso();
      //  animP.initAll(rotaciones, posicionesHips);
        
    }
    public void obtenerDatosFichero()
    {
        string[] lineas = File.ReadAllLines(pathFile);
        Offsets = lineas.Where(l => l.Contains("OFFSET")).ToList();
        //para leer las distancias a los padres de local
        for (int l = 0; l < Offsets.Count(); l++)
        {
            if (l != 5 && l != 11 && l != 13 && l != 19 && l != 21 && l != 26 && l != 31)
            { //nos quedamos con el offset para delante
                string lineaFinal = Offsets[l].Substring(Offsets[l].IndexOf('T') + 2);
                //sustituimos los \t por espacios
                string lin = lineaFinal.Replace('\t', ' ');
                //separamos por espacions
                string[] lineaFINAL = lin.Split(' ');
                List<string> valores = new List<string>();
                for (int posi = 0; posi < lineaFINAL.Length; posi++)
                {

                    if (!lineaFINAL[posi].All(char.IsDigit))
                    {
                        valores.Add(lineaFINAL[posi]);

                    }


                }
                if (valores.Count > 0)
                {
                    float x = (float)Convert.ToDouble(valores[0]);
                    
                    float y = (float)Convert.ToDouble(valores[1]);
                    float z = (float)Convert.ToDouble(valores[2]);
                    Vector3 newVector = new Vector3(x, y, z);
                   
                    distanciasPadres.Add(newVector);
                }
            }

        }
        //para leer las rotaciones
        //empieza en la:158 por lo que cogemos desde las 157
        for (int lin = 157; lin < lineas.Length-1; lin++)
        {
            string linAux="";
            string[] valorLineaRotacion;
            if (lin == 157)
            {
                linAux = lineas[lin].Substring(1);
               // Debug.Log(linAux);
                valorLineaRotacion = linAux.Split(' ');
                //Debug.Log("valores" + valorLineaRotacion[0] + " *1* " + valorLineaRotacion[2] + " *2* " + valorLineaRotacion[3]);
            }
            else
            {
                linAux = lineas[lin].Substring(2);
                valorLineaRotacion = linAux.Split(' ');

                //Debug.Log("valor 0*" + valorLineaRotacion[0] + " *1* " + valorLineaRotacion[2] + " *2* " + valorLineaRotacion[3]);
            }
            for(int vl=0; vl < valorLineaRotacion.Length-3; vl+=3)
            { 
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = ".";
                float x = (float)Convert.ToDouble(valorLineaRotacion[vl], provider);
                float y = (float)Convert.ToDouble(valorLineaRotacion[vl + 1], provider);
                float z = (float)Convert.ToDouble(valorLineaRotacion[vl + 2], provider);

                Vector3 newVector = new Vector3(x,y,z);
                //uzquierda y derecha cambiado de signo 
                switch (vl)
                {
                    case 0:
                        posicionesHips.Add(newVector);
                        break;
                    case 3:
                        Hips.Add(newVector);
                        break;
                    case 6:
                        Spine.Add(newVector);
                        break;
                    case 9:
                        Spine1.Add(newVector);
                        break;
                    case 12:
                        Neck.Add(newVector);
                        break;
                    case 15:
                        Head.Add(newVector);
                        break;
                    case 18:
                        LeftShoulder.Add(newVector);
                        break;
                    case 21:
                        //7
                        LeftUpArm.Add(newVector);
                        break;
                    case 24:
                        //8
                        LeftForeArm.Add(newVector);
                        break;
                    case 27:
                        //9
                        LeftHand.Add(newVector);
                        break;
                    case 30:
                        //10
                        LeftHandThumb.Add(newVector);
                        break;
                    case 33:
                        //11
                        L_Wrist_End.Add(newVector);
                        break;
                    case 36:
                        RightShoulder.Add(newVector);
                        break;
                    case 39:
                        RightUpArm.Add(newVector);
                        break;
                    case 42:
                        RightForeArm.Add(newVector);
                        break;
                    case 45:
                        RightHand.Add(newVector);
                        break;
                    case 48:
                        //18
                        RightHandThumb.Add(newVector);
                        break;
                    case 51:
                        //19
                        R_Wrist_End.Add(newVector);
                        break;
                    case 54:
                        //20
                        LeftUpLeg.Add(newVector);
                        break;
                    case 57:
                        //22
                        LeftLowLeg.Add(newVector);
                        break;
                    case 60:
                        //23
                        LeftFoot.Add(newVector);
                        break;
                    case 63:
                        //26
                        LeftToeBase.Add(newVector);
                        break;
                    case 66:
                        //27
                        RightUpLeg.Add(newVector);
                        break;
                    case 69:
                        //28
                        RightLowLeg.Add(newVector);
                        break;
                    case 72:
                        RightFoot.Add(newVector);
                        break;
                   case 75:
                        //30

                        RightToeBase.Add(newVector);
                        break;



                }
            }
        }
        /*rotaciones.Add("Hips", Hips);
          rotaciones.Add("Spine", Spine);
          rotaciones.Add("Spine1", Spine1);
          rotaciones.Add("Neck", Neck);
          rotaciones.Add("Head", Head);
          rotaciones.Add("LeftShoulder", RightShoulder);
          rotaciones.Add("LeftUpArm", RightUpArm);
          rotaciones.Add("LeftForeArm", RightForeArm);
          rotaciones.Add("LeftHand", RightHand);
          rotaciones.Add("LeftHandThumb", RightHandThumb);
          rotaciones.Add("L_Wrist_End", R_Wrist_End);
          rotaciones.Add("RightShoulder", LeftShoulder);
          rotaciones.Add("RightUpArm", LeftUpArm);
          rotaciones.Add("RightForeArm", LeftForeArm);
          rotaciones.Add("RightHand", LeftHand);
          rotaciones.Add("RightHandThumb", LeftHandThumb);
          rotaciones.Add("R_Wrist_End", L_Wrist_End);
          rotaciones.Add("LeftUpLeg", LeftUpLeg);
          rotaciones.Add("LeftLowLeg", LeftLowLeg);
          rotaciones.Add("LeftFoot", LeftFoot);
          rotaciones.Add("LeftToeBase", LeftToeBase);
          rotaciones.Add("RightUpLeg", RightUpLeg);
          rotaciones.Add("RightLoeLeg", RightLowLeg);
          rotaciones.Add("RightFoot", RightFoot);
          rotaciones.Add("RightToeBase", RightToeBase);*/
       rotaciones.Add("Hips", Hips);
        rotaciones.Add("Spine", Spine);
        rotaciones.Add("Spine1", Spine1);
        rotaciones.Add("Neck", Neck);
        rotaciones.Add("Head", Head);
        rotaciones.Add("LeftShoulder", LeftShoulder);
        rotaciones.Add("LeftUpArm", LeftUpArm);
        rotaciones.Add("LeftForeArm", LeftForeArm);
        rotaciones.Add("LeftHand", LeftHand);
        rotaciones.Add("LeftHandThumb", LeftHandThumb);
        rotaciones.Add("L_Wrist_End", L_Wrist_End);
        rotaciones.Add("RightShoulder", RightShoulder);
        rotaciones.Add("RightUpArm", RightUpArm);
        rotaciones.Add("RightForeArm", RightForeArm);
        rotaciones.Add("RightHand", RightHand);
        rotaciones.Add("RightHandThumb", RightHandThumb);
        rotaciones.Add("R_Wrist_End", R_Wrist_End);
        rotaciones.Add("LeftUpLeg", LeftUpLeg);
        rotaciones.Add("LeftLowLeg", LeftLowLeg);
        rotaciones.Add("LeftFoot", LeftFoot);
        rotaciones.Add("LeftToeBase", LeftToeBase);
        rotaciones.Add("RightUpLeg", RightUpLeg);
        rotaciones.Add("RightLoeLeg", RightLowLeg);
        rotaciones.Add("RightFoot", RightFoot);
        rotaciones.Add("RightToeBase", RightToeBase);
        terminado = true;
    }

    public void obtenerEscalaHueso()
    {
        //hips
        escala.Add(1);

        //spine
        escala.Add(Vector3.Distance(distanciasPadres[1], distanciasPadres[0]));

        //spine1
        escala.Add(Vector3.Distance(distanciasPadres[2], distanciasPadres[1]));
        
        //neck
        escala.Add(Vector3.Distance(distanciasPadres[3], distanciasPadres[2]));
        
        //head
        escala.Add(Vector3.Distance(distanciasPadres[4], distanciasPadres[3]));

        //leftShoulder
        escala.Add(Vector3.Distance(distanciasPadres[5], distanciasPadres[2]));
        
        //leftUpArm
        escala.Add(Vector3.Distance(distanciasPadres[6], distanciasPadres[5]));        //leftUpArm
       
        //leftForeArm
        escala.Add(Vector3.Distance(distanciasPadres[7], distanciasPadres[6]));

        //leftHand
        escala.Add(Vector3.Distance(distanciasPadres[8], distanciasPadres[7]));

        //leftHandThumb
        escala.Add(Vector3.Distance(distanciasPadres[9], distanciasPadres[8]));

        //L_Wrist_End
        escala.Add(Vector3.Distance(distanciasPadres[10], distanciasPadres[8]));

        //RightShoulder
        escala.Add(Vector3.Distance(distanciasPadres[11], distanciasPadres[2]));

        //RightUpArm
        escala.Add(Vector3.Distance(distanciasPadres[12], distanciasPadres[11]));        //leftUpArm

        //RightForeArm
        escala.Add(Vector3.Distance(distanciasPadres[13], distanciasPadres[12]));

        //RightHand
        escala.Add(Vector3.Distance(distanciasPadres[14], distanciasPadres[13]));

        //RightHandThumb
        escala.Add(Vector3.Distance(distanciasPadres[15], distanciasPadres[14]));

        //R_Wrist_End
        escala.Add(Vector3.Distance(distanciasPadres[16], distanciasPadres[14]));

        //LeftUpLeg
        escala.Add(Vector3.Distance(distanciasPadres[17], distanciasPadres[0]));
        //LeftLowLeg
        escala.Add(Vector3.Distance(distanciasPadres[18], distanciasPadres[17]));
        //LeftFoot
        escala.Add(Vector3.Distance(distanciasPadres[19], distanciasPadres[18]));
        //LeftToeBase
        escala.Add(Vector3.Distance(distanciasPadres[20], distanciasPadres[19]));
        //RightUpLeg
        escala.Add(Vector3.Distance(distanciasPadres[21], distanciasPadres[0]));
        //RightLowLeg
        escala.Add(Vector3.Distance(distanciasPadres[22], distanciasPadres[21]));
        //RightFoot
        escala.Add(Vector3.Distance(distanciasPadres[23], distanciasPadres[22]));
        //RightToeBase
        escala.Add(Vector3.Distance(distanciasPadres[24], distanciasPadres[23]));

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void borrado()
    {
        //se va a setear a null todo
        rotaciones.Clear();
         Hips.Clear();
        Spine.Clear();
         Spine1.Clear();
         Neck.Clear();
         Head.Clear();
         LeftShoulder.Clear();
        LeftUpArm.Clear();
        LeftForeArm.Clear();
        LeftHand.Clear();
        LeftHandThumb.Clear();
        L_Wrist_End.Clear();
        RightShoulder.Clear();
         RightUpArm.Clear();
         RightForeArm.Clear();
        RightHand.Clear();
        RightHandThumb.Clear();
        R_Wrist_End.Clear();
        LeftUpLeg.Clear();
         LeftLowLeg.Clear();
         LeftFoot.Clear();
        LeftToeBase.Clear();
         RightUpLeg.Clear();
        RightLowLeg.Clear();
        RightFoot.Clear();
      RightToeBase.Clear();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

using UnityEditor;

using System.IO;
using System.Globalization;

public class OrganizarDatosFile
{
    #region list of bones
    //Cadera
    [SerializeField] List<Vector3> cadera =new List<Vector3>();
    //pierna derecha
    [SerializeField] List<Vector3> caderaD =new List<Vector3>();
    [SerializeField] List<Vector3> rodillaD = new List<Vector3>();
    [SerializeField] List<Vector3> tobilloD = new List<Vector3>();
    [SerializeField] List<Vector3> empeineD = new List<Vector3>();
    [SerializeField] List<Vector3> puntaD = new List<Vector3>();
    //pierna izquierda
    [SerializeField] List<Vector3> caderaI = new List<Vector3>();
    [SerializeField] List<Vector3> rodillaI = new List<Vector3>();
    [SerializeField] List<Vector3> tobilloI = new List<Vector3>();
    [SerializeField] List<Vector3> empeineI = new List<Vector3>();
    [SerializeField] List<Vector3> puntaI = new List<Vector3>();
    //pecho 
    [SerializeField] List<Vector3> pecho = new List<Vector3>();
    //cuellobajo 
    [SerializeField] List<Vector3> cuellobajo = new List<Vector3>();
    //barbilla 
    [SerializeField] List<Vector3> barbilla = new List<Vector3>();
    //cabeza 
    [SerializeField] List<Vector3> cabeza = new List<Vector3>();
    //HombroIz 
    [SerializeField] List<Vector3> hombroI = new List<Vector3>();
    [SerializeField] List<Vector3> codoI = new List<Vector3>();
    [SerializeField] List<Vector3> muñecaI = new List<Vector3>();
    [SerializeField] List<Vector3> pulgarI = new List<Vector3>();
    [SerializeField] List<Vector3> dedosI = new List<Vector3>();
    //HombroD 
    [SerializeField] List<Vector3> hombroD = new List<Vector3>();
    [SerializeField] List<Vector3> codoD = new List<Vector3>();
    [SerializeField] List<Vector3> muñecaD = new List<Vector3>();
    [SerializeField] List<Vector3> pulgarD = new List<Vector3>();
    [SerializeField] List<Vector3> dedosD = new List<Vector3>();
    #endregion
    //total
    [SerializeField]  Dictionary<string, List<Vector3> > totalBody = new Dictionary<string, List<Vector3>>();
    //frames
    [SerializeField] List<float> timesPerFrame = new List<float>();
    [SerializeField] public bool finalizado = false;
      public void SetListBones(TextAsset TXT, AngleCurveCreator curv, GameObject personaje)
        {


        string pathTxt = AssetDatabase.GetAssetPath(TXT);
        StreamReader myTXT = new StreamReader(pathTxt);
        string[] auxValue;
        int i = 0; 
            while (!myTXT.EndOfStream )
            {
                string inp_ln = myTXT.ReadLine();
                inp_ln = inp_ln.Replace("*\n", "");
                inp_ln = inp_ln.Replace("*\r", "");
            //sustituimos la coma del tiempo por un punto
            inp_ln = inp_ln.Replace(",", ".");

            auxValue = inp_ln.Split(' ');
            int hueso = int.Parse(auxValue[0], CultureInfo.InvariantCulture);
            //el tiempo se encuentra en la posición 0, sñolo cambiará al siguiente valor cuando ya haya hecho todos los huesos del primer frame
            float time = float.Parse(auxValue[1], CultureInfo.InvariantCulture);
            float x = float.Parse(auxValue[2], CultureInfo.InvariantCulture);/// 1500;
            float y = float.Parse(auxValue[3], CultureInfo.InvariantCulture);/// 1500;
            float z = float.Parse(auxValue[4], CultureInfo.InvariantCulture);/// 1500;
            Vector3 valores = new Vector3(x, y, z);
            switch (hueso)
            {
                case 1:
                    cadera.Add(valores);
                break;
                case 2:
                    caderaD.Add(valores);
                    break;
                case 3:
                   rodillaD.Add(valores);
                    break;
                case 4:
                    tobilloD.Add(valores);
                    break;
                case 5:
                    empeineD.Add(valores);
                    break;
                case 6:
                    puntaD.Add(valores);
                    break;
                case 7:
                    caderaI.Add(valores);
                    break;
                case 8:
                    rodillaI.Add(valores);
                    break;
                case 9:
                    tobilloI.Add(valores);
                    break;
                case 10:
                    empeineI.Add(valores);
                    break;
                case 11:
                    puntaI.Add(valores);
                    break;
                case 13:
                    pecho.Add(valores);
                    break;
                case 14:
                    cuellobajo.Add(valores);
                    break;
                case 15:
                    barbilla.Add(valores);
                    break;
                case 16:
                    cabeza.Add(valores);
                    break;
                case 18:
                    hombroI.Add(valores);
                    break;
                case 19:
                    codoI.Add(valores);
                    break;
                case 20:
                    muñecaI.Add(valores);
                    break;
                case 22:
                    pulgarI.Add(valores);
                    break;
                case 23:
                    dedosI.Add(valores);
                    break;
                case 26:
                    hombroD.Add(valores);
                    break;
                case 27:
                    codoD.Add(valores);
                    break;
                case 28:
                    muñecaD.Add(valores);
                    break;
                case 30:
                    pulgarD.Add(valores);
                    break;
                case 31:
                    dedosD.Add(valores);
                    i++;
                    timesPerFrame.Add(time);
                   
                    break;



            }
                 
                auxValue = null;
                valores = Vector3.zero;
            }

        myTXT.Close();
        SetDiccionario( curv, personaje);
    }
      public void SetDiccionario(AngleCurveCreator curv, GameObject personaje)
    {
        totalBody.Add("Root", cadera);
        totalBody.Add("RightUpLeg", caderaD);
        totalBody.Add("RightLeg", rodillaD);
        totalBody.Add("RightFoot", tobilloD);
        totalBody.Add("RightToe", empeineD);
        totalBody.Add("RightEnd", puntaD);
        totalBody.Add("LeftUpLeg", caderaI);
        totalBody.Add("LeftLeg", rodillaI);
        totalBody.Add("LeftFoot", tobilloI);
        totalBody.Add("LeftToe", empeineI);
        totalBody.Add("LeftEnd", puntaI);
        totalBody.Add("Spine1", pecho);
        totalBody.Add("Spine2", cuellobajo);
        totalBody.Add("Neck", barbilla);
        totalBody.Add("Head", cabeza);
        totalBody.Add("LeftShoulder", hombroI);
        totalBody.Add("LeftArm", codoI);
        totalBody.Add("LeftForeArm", muñecaI);
        totalBody.Add("LeftHand", pulgarI);
        totalBody.Add("LeftHandIndex1", dedosI);
        totalBody.Add("RightShoulder", hombroD);
        totalBody.Add("RightArm", codoD);
        totalBody.Add("RightForeArm", muñecaD);
        totalBody.Add("RightHand", pulgarD);
        totalBody.Add("RightHanIndex1", dedosD);
        callBezierCurve( curv, personaje);
    }
        
    public void callBezierCurve(AngleCurveCreator curv, GameObject personaje)
    {

       
        foreach (KeyValuePair<string, List<Vector3>> hueso in totalBody)
        {
                if (curv.inicializarBezier(hueso.Value, timesPerFrame[0], timesPerFrame[timesPerFrame.Count - 1], personaje));
                    curv.Ready(hueso.Key, hueso.Value, timesPerFrame) ;
                if (curv.curveDone) curv.SetNull();
          
        }
        finalizado = true;
    }
       
   }

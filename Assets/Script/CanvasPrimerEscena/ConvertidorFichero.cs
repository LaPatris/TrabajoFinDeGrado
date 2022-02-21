using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ConvertidorFichero : MonoBehaviour
{

    [Header("TXT")]
    [SerializeField] string myPath;
    [SerializeField] String firstFileName;
    [SerializeField] String firstFilePath;
    [SerializeField] TextAsset newFile;
    [SerializeField] bool  terminado;
    [Header("Animación")]
    [SerializeField] int bones;
    [SerializeField] float duracionAnim;
    [SerializeField] float timePerFrame;
    [SerializeField] int totalFrames;
    [SerializeField] int lineas;
    [SerializeField] int actualFrame;





    // Start is called before the first frame update
    void Start()
    {
        duracionAnim = 3745;
        actualFrame = 1;
    }
    void Update()
    {
       
    }
    public bool nameAndFile(TextAsset fileRead)
    {
        firstFileName = fileRead.name;
        firstFilePath = AssetDatabase.GetAssetPath(fileRead);
        lineas = File.ReadAllLines(firstFilePath).Length;
        timePerFrame = timeXframe();

          bool sol=CreateFile();
        return sol;
    }

    public bool CreateFile()
    {

        //name file
        string fileName = firstFileName + "RtR.txt";//RtR ready to read
        myPath = "Assets/TXT/";

        // path+name
        myPath = Path.Combine(myPath, fileName);

        // Verify the paath
        Console.WriteLine("Path to my file: {0}\n", myPath);
        if (!File.Exists(myPath)) { 
            File.Create(myPath);
          string[] lineas = File.ReadAllLines(firstFilePath);
           for (int linea = 0; linea < lineas.Length; linea++)
             {
                            bones++;
                if (bones == 33) {
                    bones = 1;
                    actualFrame += 1;
                        };
                if (bones != 12 && bones != 17 && bones != 21 && bones != 24 && bones != 25 && bones != 29 && bones != 32)
                {
                   
                    lineas[linea] = bones +" " + timePerFrame*(actualFrame-1) +" "+ lineas[linea];
                }
                else
                {
                    lineas[linea] = "";
                }
             }
        File.WriteAllLines(myPath, lineas.Where(l => l != "").ToList());
            
           return true;
        }
        else
        {
            Debug.Log("File \"{0}\" already exists."+ fileName);
            return false;
        }

    }
    public float timeXframe()
    {
        totalFrames = lineas / 32;
        timePerFrame = duracionAnim / totalFrames;
        return timePerFrame;
    }
}
/*
 * IMPORTANTE SOBRE EL FICHERO
Las 32 primeras líneas del fichero son los 32 puntos del esqueleto del primer fotograma.
Por lo tanto sabemos que la linea 0 es del hueso cadera y la linea 31 es de una de la mano
la 32 por lo tanto será otra vez la cadera.
Esquema que siguen:
Lo que vamos a hacer en esta clase es organizar el fichero como nosotros queremos
1/12: Cadera(eliminar las lineas repetidaso no guardarlas directamente)
2: Cadera der
3: Rodilla der
4: tobillo der
5:empeine der
6: punta der
7: Cadera izq
8: Rodilla izq
9: tobillo izq
10:empeine izq
11: punta izq
13: pecho
14:/17/25: cuello bajo
15: barbilla
16:cabeza
18: hombro izq
19: codo izq
20/21: muñeca izq
22: pulgar izq
23/24: dedos izq
26: hombro der
27: codo der
28/29: muñeca der
30: pulgar der
31/32: dedos der

 */
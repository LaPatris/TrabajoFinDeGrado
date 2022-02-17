
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;
public class PruebaFBX : MonoBehaviour
{


   // [SerializeField] int numerOfBonesAnimation = 0;

    [Header("Animacion")]
    [SerializeField] public Animator animator;
    [SerializeField] Avatar avatar;
    [SerializeField] public AnimationClip animacion;
    [SerializeField] public bool nuevo;
    public AnimatorController anim;


    public static HumanBodyBones[] bonesToUse = new[]{
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
    //como no podemos modificar como tal estos huesos, lo que tenemos que hacer es asignar los huesos a un array, lista de tranformación 
   

    [Header("Bones Avatar")]
    [SerializeField] public List<Transform> MyBones = new List<Transform>();
    [SerializeField] public List<Transform> MyBonesInit = new List<Transform>();
    Transform initPos;
    [SerializeField] Dictionary<string, Transform> bienHuesos = new Dictionary<string, Transform>();

    [SerializeField] Transform hips;
    Transform srcRoot;
    [SerializeField] HumanDescription description = new HumanDescription();
    
    void Awake()
    {

       // hips = this.transform.Find("Hips");
        //RestartNames(hips);
      // CreateAvatar();
       // SetAnimator();

    }
    void Update()
    {
    }
    #region crearAvatar
    /*void CreateAvatar(){

        description = AvatarUtils.CreateHumanDescription(gameObject);
         avatar = AvatarBuilder.BuildHumanAvatar(gameObject, description);
         avatar.name = gameObject.name;
         SetAnimator();
        
     

    }*/
    void SetAnimator()
    {
        animator = GetComponent<Animator>();
        animator.avatar = avatar;
        animator.Play("aux");
        InitBones();
    }
   /* public void RestartNames(Transform child)
    {
        for (int i = 0; i < child.transform.childCount; i++)
        {
            if (child.name.Contains("mixamorig:"))
            {
               string newname= child.name.Remove(0, 10);
                child.gameObject.name = newname;
            }
            RestartNames(child.GetChild(i));

        }
    }*/
    private void InitBones()
    {
        for (int i = 0; i < bonesToUse.Length; i++)
        {
            if (!bienHuesos.ContainsKey(animator.GetBoneTransform(bonesToUse[i]).name))
           {  bienHuesos.Add(animator.GetBoneTransform(bonesToUse[i]).name, animator.GetBoneTransform(bonesToUse[i]));
                MyBones.Add(animator.GetBoneTransform(bonesToUse[i]));
                MyBonesInit.Add(animator.GetBoneTransform(bonesToUse[i])); }
        }
        //inicializa los huesos tanto del origen como de la copia
        nuevo = true;
    }
    #endregion
   
}
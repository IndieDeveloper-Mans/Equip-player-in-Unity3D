using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPlayer : MonoBehaviour
{
    [Header("Player")]
    // attach your player
    public GameObject player;
    // the SkinnedMeshRenderers will be used for blendshapes
    public SkinnedMeshRenderer playerBodySkinMesh;
    public SkinnedMeshRenderer playerHairSkinMesh;

    [Header("Equipment")]
    public GameObject[] Gear = new GameObject[10];
    [Space]
    public GameObject[] WeaponAndShield = new GameObject[2];
    
    [Header("Attachments")]
    public GameObject weaponAttach;
    public GameObject shieldAttach;
    string itemType;
    
    public void EquipWeaponAndShield(GameObject itemModel, string typeOfItem) 
    {
        if (typeOfItem == "Weapon")
        {
            WeaponAndShield[0] = itemModel;
            GameObject model = Instantiate(itemModel) as GameObject;
            model.transform.SetParent(weaponAttach.transform, false);
        }
        else
        {
            WeaponAndShield[1] = itemModel;
            GameObject model = Instantiate(itemModel) as GameObject;
            model.transform.SetParent(shieldAttach.transform, false);
        }
    }

    public void UnequipWeaponAndShield(GameObject itemModel, string typeOfItem)
    {
        if (typeOfItem == "Weapon")
        {
            Destroy(WeaponAndShield[0].gameObject);
            WeaponAndShield.SetValue(null, 0);

            Destroy(weaponAttach.transform.GetChild(0).gameObject);
        }
        else
        {
            Destroy(WeaponAndShield[1].gameObject);
            WeaponAndShield.SetValue(null, 1);

            Destroy(shieldAttach.transform.GetChild(0).gameObject);
        }
    }

    public void EquipItem(GameObject itemModel, string typeOfItem)
    {   
        itemType = typeOfItem;
        AddLimb(itemModel, player);
         
    }

    public void UnequipItem(GameObject itemModel, string typeOfItem)
    {
        switch (typeOfItem)
        {
            case "Helmet":
            Destroy(Gear[0].gameObject);
            Gear.SetValue(null, 0);
            break;

            case "Chest":
            Destroy(Gear[1].gameObject);
            Gear.SetValue(null, 1);
            break;

            case "Gloves":
            Destroy(Gear[2].gameObject);
            Gear.SetValue(null, 2);
            break;

            case "Pants":
            Destroy(Gear[3].gameObject);
            Gear.SetValue(null, 3);
            break;

            case "Boots":
            Destroy(Gear[4].gameObject);
            Gear.SetValue(null, 4);
            break;
           
        }
    }

    public void AddLimb(GameObject BonedObj, GameObject RootObj)
    {
        var BonedObjects = BonedObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer SkinnedRenderer in BonedObjects)
        {
            ProcessBonedObject(SkinnedRenderer, RootObj);
        }
    }

    private void ProcessBonedObject(SkinnedMeshRenderer ThisRenderer, GameObject RootObj)
    {
        /*      Create the SubObject        */
        var NewObj = new GameObject(ThisRenderer.gameObject.name);
        NewObj.transform.parent = RootObj.transform;
        /*      Add the renderer        */
        NewObj.AddComponent<SkinnedMeshRenderer>();
        var NewRenderer = NewObj.GetComponent<SkinnedMeshRenderer>();
        /*      Assemble Bone Structure     */
        var MyBones = new Transform[ThisRenderer.bones.Length];
        for (var i = 0; i < ThisRenderer.bones.Length; i++)
            MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, RootObj.transform);
        /*      Assemble Renderer       */
        NewRenderer.bones = MyBones;
        NewRenderer.sharedMesh = ThisRenderer.sharedMesh;
        NewRenderer.materials = ThisRenderer.sharedMaterials;
        
        NewObj.layer = 8; //you can delete this. I use this for rendering player in inventory

        switch (itemType)
        {
            case "Helmet":
            playerHairSkinMesh.SetBlendShapeWeight(0, 100f);
            Gear[0] = NewObj;
            break;

            case "Chest":
            playerBodySkinMesh.SetBlendShapeWeight(2, 100f);
            Gear[1] = NewObj;
            break;

            case "Gloves":
            playerBodySkinMesh.SetBlendShapeWeight(3, 100f);
            Gear[2] = NewObj;
            break;

            case "Pants":
            playerBodySkinMesh.SetBlendShapeWeight(1, 100f);
            Gear[3] = NewObj;
            break;

            case "Boots":
            playerBodySkinMesh.SetBlendShapeWeight(0, 100f);
            Gear[4] = NewObj;
            break;
 
        }
    }

    private Transform FindChildByName(string ThisName,Transform ThisGObj)
    {
        Transform ReturnObj;
        if( ThisGObj.name == ThisName )
            return ThisGObj.transform;
        foreach (Transform child in ThisGObj)
        {
            ReturnObj = FindChildByName( ThisName, child );
            if( ReturnObj )
                return ReturnObj;
        }
        return null;
    }
}

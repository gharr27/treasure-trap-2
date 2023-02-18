using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Settings")]

public class Settings : ScriptableObject
{
   
   [SerializeField]
   private string gameVersion = "0.0.0";
   public string GameVersion {get {return gameVersion;}}
   [SerializeField]
   private string nickName = "Treasure";

   public string NickName{

        get{

            int value = Random.Range(0,9999);
            return nickName + value.ToString();
        }
   }

}

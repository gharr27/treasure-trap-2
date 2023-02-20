using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singletons/MasterManager")]

public class MasterManager : ObjectSingleton<MasterManager>
{
  
  [SerializeField]
  private Settings gameSettings;
  public static Settings GameSettings { get { return Instance.gameSettings; } }

}

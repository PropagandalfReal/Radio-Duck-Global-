using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapDatabase : ScriptableObject
{
     public Map[] map;

     public int MapCount{
          get
          {
               return map.Length;
          }
     }

     public Map GetMap(int index){
          return map[index];
     }
}

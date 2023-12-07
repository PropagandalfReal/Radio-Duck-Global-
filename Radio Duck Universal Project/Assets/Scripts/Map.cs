using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
     public CharacterDatabase MapDB;

    public SpriteRenderer artworkSprite;

    private int selectedMap = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("selectedMap")){
            selectedMap = 0;
        }
        else{
            Load();
        }
        UpdateCharacter(selectedMap);
    }
    private void UpdateCharacter(int selectedMap){
        //Character character = characterDB.GetCharacter(selectedOption);
        //artworkSprite.sprite = character.characterSprite;
        Debug.Log("Map: "+selectedMap);
    }

    private void Load(){
        selectedMap = PlayerPrefs.GetInt("selectedMap");
    }
}

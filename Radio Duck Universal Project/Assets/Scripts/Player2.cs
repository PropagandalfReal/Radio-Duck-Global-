using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
     public CharacterDatabase characterDB;

    public SpriteRenderer artworkSprite;

    private int selectedOptionP2 = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("selectedOptionP2")){
            selectedOptionP2 = 0;
        }
        else{
            Load();
        }
        UpdateCharacter(selectedOptionP2);
    }
    private void UpdateCharacter(int selectedOptionP2){
        //Character character = characterDB.GetCharacter(selectedOption);
        //artworkSprite.sprite = character.characterSprite;
        Debug.Log("P2: "+selectedOptionP2);
    }

    private void Load(){
        selectedOptionP2 = PlayerPrefs.GetInt("selectedOptionP2");
    }
}

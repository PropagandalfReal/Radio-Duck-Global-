using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterManagerP2 : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public TextMeshProUGUI nameText;
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

    public void NextOption(){
        selectedOptionP2++;

        if(selectedOptionP2 >= characterDB.CharacterCount){
            selectedOptionP2 = 0;
        }

        UpdateCharacter(selectedOptionP2);
        Save();
    }

    public void BackOption(){
        selectedOptionP2--;

        if(selectedOptionP2 < 0){
            selectedOptionP2 = characterDB.CharacterCount - 1;
        }

        UpdateCharacter(selectedOptionP2);
        Save();
    }


    private void UpdateCharacter(int selectedOptionP2){
        Character character = characterDB.GetCharacter(selectedOptionP2);
        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;
    }

    private void Load(){
        selectedOptionP2 = PlayerPrefs.GetInt("selectedOptionP2");
    }

    private void Save(){
        PlayerPrefs.SetInt("selectedOptionP2", selectedOptionP2);
    }

    public void ChangeScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

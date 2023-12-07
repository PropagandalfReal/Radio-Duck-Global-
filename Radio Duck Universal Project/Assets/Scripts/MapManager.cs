using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public TextMeshProUGUI nameText;
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

    public void NextOption(){
        selectedMap++;

        if(selectedMap >= characterDB.CharacterCount){
            selectedMap = 0;
        }

        UpdateCharacter(selectedMap);
        Save();
    }

    public void BackOption(){
        selectedMap--;

        if(selectedMap < 0){
            selectedMap = characterDB.CharacterCount - 1;
        }

        UpdateCharacter(selectedMap);
        Save();
    }


    private void UpdateCharacter(int selectedMap){
        Character character = characterDB.GetCharacter(selectedMap);
        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;
    }

    private void Load(){
        selectedMap = PlayerPrefs.GetInt("selectedMap");
    }

    private void Save(){
        PlayerPrefs.SetInt("selectedMap", selectedMap);
    }

    public void ChangeScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

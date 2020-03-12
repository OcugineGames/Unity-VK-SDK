using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VK.SDK;

//===================================================
//  VK Standalone SDK
//  Данный SDK разработан для использования API
//  социальной сети вконтакте для Standalone
//  приложений, разработанных с использованием
//  Unity
//
//  @name           VK Standalone SDK
//  @developer      Ocugine Games
//  @version        0.4.2
//  @build          402
//  @url            https://vk.com/ocugine
//  @license        MIT
//===================================================
//===================================================
//  VK Scene Logic Component
//===================================================
public class SceneLogic : MonoBehaviour{
    // Get Object References
    [Header("General")]
    public GameObject SDK = null; // SDK Instance

    [Header("UI Panels")]
    public GameObject AuthenticationPanel; // Authentication Panel
    public GameObject GameMenu; // Game Menu
    public GameObject GameUI; // Game UI

    [Header("Game Menu UI Instances")]
    public GameObject AvatarUI; // Avatar UI
    public GameObject NameUI;   // Name UI
    public GameObject StatusUI; // Status UI
    public GameObject LocationUI; // Location UI
    public GameObject AboutUI;  // About UI

    [Header("Game UI Instances")]
    public GameObject ScoresUI; // Scores UI

    // Private Variables
    private VKSDK VK = null;
    private int current_scores = 0;

    // Before Scene Started
    void Awake(){
        // Check SDK Instance
        try{
            if (SDK != null){ // Has SDK Instance
                VK = SDK.GetComponent<VKSDK>(); // Get Component
            } else {
                SDK = GameObject.Find("VKSDK").transform.gameObject;
                VK = SDK.GetComponent<VKSDK>(); // Get Component
            }
        } catch(Exception ex){
            throw new Exception("На сцене отсутствует объект VK SDK");
        }
    }

    // Start is called before the first frame update
    void Start(){
        // Add Handlers
        VK.OnAuthenticationComplete += CompleteAuthentication;

        // Work With Game UI
        _HideAllGamePanels(); // Hide All Game Panels

        // Check Authentication
        if (VK.authentication.access_token.Length < 1){ // Authentication Error
            AuthenticationPanel.SetActive(true); // Show Authentication Panel
        } else { // Already Authenticated
            ShowGameMenu(); // Show Game Menu
        }
    }

    // Authenticate
    public void Authenticate(){
        VK.ShowLoginWindow(); // Authenticate
    }
    private void CompleteAuthentication(string access_token){
        Debug.Log("VK API Access Token: " + access_token);
        AuthenticationPanel.SetActive(false); // Deactivate
        ShowGameMenu();
    }

    // Show Game Menu
    public void ShowGameMenu(){
        // Request User Profile
        WWWForm _data = new WWWForm();
        _data.AddField("fields", "about,sex,bdate,city,country,photo_max,online");
        VK.Call("users.get", _data, (string data) => { // OK
            ProfileResponse profile = JsonUtility.FromJson<ProfileResponse>(data); // Convert
            _loadProfile(profile);
            GameMenu.SetActive(true);
        }, (BaseErrorModel error) => { // Error
            Debug.Log("VK API Error: " + error.error_msg);
        });
    }
    private void _loadProfile(ProfileResponse profile){
        // Load Profile Image
        StartCoroutine(VK.LoadImage(profile.response[0].photo_max, AvatarUI.GetComponent<Image>(), ()=> {
        }, (string err)=> {
            Debug.Log(err);
        }));

        // Load Other Profile Data
        NameUI.GetComponent<Text>().text = profile.response[0].first_name + " " + profile.response[0].last_name;
        StatusUI.GetComponent<Text>().text = (profile.response[0].online == 1) ? "Сейчас в сети" : "Сейчас не в сети";
        AboutUI.GetComponent<Text>().text = (profile.response[0].about.Length>0)? profile.response[0].about:"Пользователь ничего не указал о себе :(";
        LocationUI.GetComponent<Text>().text = profile.response[0].city.title + ", " + profile.response[0].country.title;
    }

    // Play
    public void Play(){
        GameMenu.SetActive(false);
        GameUI.SetActive(true);
    }
    public void AddScores(){
        current_scores += 10;
        ScoresUI.GetComponent<Text>().text = "Мои очки: " + current_scores.ToString();
    }
    public void Share(){
        WWWForm _data = new WWWForm();
        _data.AddField("message", "Я набрал максимальный счет в игре "+Application.productName+" в размере "+current_scores+" очков!");
        VK.Call("wall.post", _data, (string data) => { // OK
            Debug.Log("Запись вконтакте успешно размещена!");
        }, (BaseErrorModel error) => { // Error
            Debug.Log("VK API Error: " + error.error_msg);
        });
    }

    // Hide All Game Panels
    private void _HideAllGamePanels(){
        GameMenu.SetActive(false); // Hide Game Menu
        AuthenticationPanel.SetActive(false); // Hide Authentication Panel
        GameUI.SetActive(false); // Hide Game UI
    }

    // VK API Test
    private void _testVKAPI(){
        // Create Form Data
        WWWForm _data = new WWWForm(); // Create WWWForm
        _data.AddField("owner_id", 146332935); // Add Field "owner_id"

        // Call VK API Method
        VK.Call("wall.get", _data, (string data) => { // OK
            Debug.Log(data); // Returns string with JSON response
        }, (BaseErrorModel error) => { // Error
            Debug.Log("VK API Error: " + error.error_msg); // Returns Error Message
        });
    }
}

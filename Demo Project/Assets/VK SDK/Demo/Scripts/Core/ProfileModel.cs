using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
//  Profile Model Example
//===================================================
[System.Serializable]
public class ProfileResponse{
    public ProfileModel[] response;
}

[System.Serializable]
public class ProfileModel{
    // Base Params
    public int id; // User ID
    public string first_name;           // First Name
    public string last_name;            // Last Name
    public string deactivated;          // Deactivated Status
    public bool is_closed;              // Page is Closed?
    public bool can_access_closed;      // Can Access to this Page?

    // Advanced Params
    public string about;                // About
    public string bdate;                // Birth Date
    public CityModel city;              // City
    public CountryModel country;        // Country
    public int online;                  // Is Online? 1 = true, 0 = false
    public string photo_max;            // Max Photo URL
    public int sex;                     // Sex 1 - female, 2 - male, 0 - none
}

[System.Serializable]
public class CityModel{
    public int id;                      // City ID
    public string title;                // City Title
}

[System.Serializable]
public class CountryModel{
    public int id;                      // Country ID
    public string title;                // Country Title
}
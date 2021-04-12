using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Options : MonoBehaviour
{
    //Add Script to Canvas and drag options to eschandler. drag resolution dropdown to canvas script
    //Options shouild be off initially
    #region Variables
    private Resolution[] resolutions;

    public Dropdown resolutionDropdown;
    #endregion

    #region Start Methods
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resostr = new List<string>();

        int curr_reso = 0;

        foreach (Resolution rs in resolutions)
        {

            resostr.Add(rs.width + "x" + rs.height);

            if (rs.width != Screen.currentResolution.width &&
                rs.height != Screen.currentResolution.height)
            {
                curr_reso += 1;
            }
        }

        resolutionDropdown.AddOptions(resostr);
        resolutionDropdown.value = curr_reso;
        resolutionDropdown.RefreshShownValue();
    }
    #endregion

    #region Settings
    public void SetQuality(int qualityindex)
    {
        QualitySettings.SetQualityLevel(qualityindex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }

    public void ExitGame()
    {
        bool yn = EditorUtility.DisplayDialog("Are you sure you want to exit the app?", "You will lose any unsaved progresses", "Yes", "No");
        
        if (yn)
        {
            Application.Quit();
        }
        
    }
    #endregion
}

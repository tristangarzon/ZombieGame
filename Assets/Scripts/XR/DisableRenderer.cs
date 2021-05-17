/*
* Made by: Tristan Garzon
* 
* Script Summary:
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableRenderer : XRGrabNetworkInteractable
{
    #region Variables
    private Renderer rend;
    public bool renderSwitch;
    #endregion

    #region Unity Methods

    private void Start()
    {
        //rend = GetComponent<Renderer>();
        rend.enabled = true;
    }


    public void RenderOff()
    {
        renderSwitch = false;
    }


    public void RenderOn()
    {
        renderSwitch = true;
    }



    #endregion
}

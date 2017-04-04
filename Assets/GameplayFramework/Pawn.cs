using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn
{
    private Controller _controller;

    public Controller Controller
    {
        get
        {
            return _controller;
        }
    }



    public event EventHandler BecamePossessed;
    public event EventHandler BecameUnPossessed;



    public void OnBecamePossessed(Controller controller)
    {
        if(controller == null)
            throw new ArgumentNullException();

        if(_controller != null)
        {
            if(_controller.GetHashCode() == controller.GetHashCode())
                return;

            _controller.UnPossess();
        }

        _controller = controller;

        EventHandler subscribers = BecamePossessed;
        if(subscribers != null)
            subscribers(this, EventArgs.Empty);
    }


    public void OnBecameUnPossessed()
    {
        if(_controller == null)
            return;

        _controller = null;

        EventHandler subscribers = BecameUnPossessed;
        if(subscribers != null)
            subscribers(this, EventArgs.Empty);
    }
}
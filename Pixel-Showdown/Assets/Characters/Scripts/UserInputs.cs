using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class UserInputs : MonoBehaviour
{
    private string userInputName;
    private int controllerIndex;
    // accessible data
    public static float SoundVolume;
    public static float MusicVolume;

    public Dictionary<string, KeyCode> currentInputs;

    public Dictionary<string, KeyCode> Controller = new Dictionary<string, KeyCode>();
    public Dictionary<string, KeyCode> Keyboard = new Dictionary<string, KeyCode>();

    public Dictionary<string, KeyCode> DefaultKeyboard = new Dictionary<string, KeyCode>
    {
        {"+X", KeyCode.D},
        {"-X", KeyCode.A},
        {"+Y", KeyCode.W},
        {"-Y", KeyCode.S},
        {"Start", KeyCode.Escape},
        {"Jump", KeyCode.Space},
        {"Dash", KeyCode.LeftShift},
        {"Crouch", KeyCode.LeftControl},
        {"Attack", KeyCode.E},
        {"SpeAttack", KeyCode.Q},
        {"Accept", KeyCode.Return},
        {"Decline", KeyCode.Backspace},
    };

    public Dictionary<string, KeyCode> DefaultController = new Dictionary<string, KeyCode>
    {
        {"+X", KeyCode.None},
        {"-X", KeyCode.None},
        {"+Y", KeyCode.None},
        {"-Y", KeyCode.None},
        {"Start", KeyCode.JoystickButton7},
        {"Jump", KeyCode.JoystickButton0}, // A
        {"Dash", KeyCode.JoystickButton5}, // RB
        {"Crouch", KeyCode.JoystickButton2}, // X
        {"Attack", KeyCode.JoystickButton1}, // B
        {"SpeAttack", KeyCode.JoystickButton3}, // Y
        {"Accept", KeyCode.JoystickButton0}, // A
        {"Decline", KeyCode.JoystickButton1}, // B
    };

    private string[] keys = { "Start", "Jump", "Dash", "Crouch", "Attack", "SpeAttack", "Accept", "Decline" };
    private KeyCode[] joystick1Buttons = { KeyCode.Joystick1Button7, KeyCode.Joystick1Button0, KeyCode.Joystick1Button5, KeyCode.Joystick1Button2, KeyCode.Joystick1Button1, KeyCode.Joystick1Button3, KeyCode.Joystick1Button0, KeyCode.Joystick1Button1 };
    private KeyCode[] joystickButtons = { KeyCode.JoystickButton7, KeyCode.JoystickButton0, KeyCode.JoystickButton5, KeyCode.JoystickButton2, KeyCode.JoystickButton1, KeyCode.JoystickButton3, KeyCode.JoystickButton0, KeyCode.JoystickButton1 };

    void Awake()
    {
        userInputName = "/Player" + gameObject.name[gameObject.name.Length - 1]  + "Inputs.dat";
        controllerIndex = int.Parse(gameObject.name[gameObject.name.Length - 1].ToString());
        LoadInput();
    }

    // load the input file  /  if it doesn't exist, it creates it
    public void LoadInput()
    {
        if (File.Exists(Application.dataPath + userInputName))
        {
            Keyboard.Clear();
            Controller.Clear();

            string[] content = File.ReadAllLines(Application.dataPath + userInputName);

            bool KeyboardSetup = true;

            for (int i = 0; i < content.Length; i++)
            {
                KeyCode k;

                switch (content[i])
                {
                    case "[Keyboard]":
                        KeyboardSetup = true;
                        break;

                    case "[Controller]":
                        KeyboardSetup = false;
                        break;
                }

                if (Enum.TryParse<KeyCode>(content[i], out k))
                {

                    if (KeyboardSetup)
                    {
                        Keyboard.Add(content[i - 1], (KeyCode)Enum.Parse(typeof(KeyCode), content[i]));
                    }
                    else
                    {
                        Controller.Add(content[i - 1], (KeyCode)Enum.Parse(typeof(KeyCode), content[i]));
                    }

                }
            }
            
            SetInputController();

            currentInputs = Controller;
        }
        else
        {
            CreateInput();

            SetInputController();
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            for (KeyCode k = KeyCode.None; k < KeyCode.JoystickButton19; k++)
            {
                if (Input.GetKeyDown(k))
                {
                    currentInputs = k < KeyCode.JoystickButton0 ? Keyboard : Controller;
                }
            }
        }
    }

    // set the inputs depending on the controller the player should use
    public void SetInputController()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            Controller[keys[i]] = (KeyCode)(Controller[keys[i]] + (joystick1Buttons[i] - joystickButtons[i]) * controllerIndex);
        }
    }

    // create the input file with default settings
    public void CreateInput()
    {
        Keyboard = DefaultKeyboard;
        Controller = DefaultController;

        using (StreamWriter SW = File.CreateText(Application.dataPath + userInputName))
        {
            SW.WriteLine("Music");
            SW.WriteLine("1");
            SW.WriteLine("Sound");
            SW.WriteLine("1");

            SW.WriteLine("[Keyboard]");

            foreach (string s in DefaultKeyboard.Keys)
            {
                SW.WriteLine(s);
                SW.WriteLine(DefaultKeyboard[s]);
            }

            SW.WriteLine("");
            SW.WriteLine("[Controller]");
            SW.WriteLine("");

            foreach (string s in DefaultController.Keys)
            {
                SW.WriteLine(s);
                SW.WriteLine(DefaultController[s]);
            }
        }

        currentInputs = Keyboard;
    }

    // change a keybind
    public void ChangeInput(string ControllerName, string InputName, KeyCode Key)
    {

        try
        {
            string[] content = File.ReadAllLines(Application.dataPath + userInputName);

            bool ControllerSelected = false;

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == "[" + ControllerName + "]")
                {
                    ControllerSelected = true;
                }

                if (content[i] == InputName && ControllerSelected)
                {
                    content[i + 1] = Key.ToString();
                    break;
                }
            }

            File.WriteAllLines(Application.dataPath + userInputName, content);

            LoadInput();
        }
        catch
        {
            LoadInput();
        }
    }
}
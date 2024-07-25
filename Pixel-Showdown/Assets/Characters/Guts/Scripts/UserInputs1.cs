using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class UserInputs1 : MonoBehaviour
{
    // accessible data
    public static float SoundVolume;
    public static float MusicVolume;

    public static Dictionary<string, KeyCode> currentInputs;

    public static Dictionary<string, KeyCode> Controller = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> Keyboard = new Dictionary<string, KeyCode>();

    public static Dictionary<string, KeyCode> DefaultKeyboard = new Dictionary<string, KeyCode>
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

    public static Dictionary<string, KeyCode> DefaultController = new Dictionary<string, KeyCode>
    {
        {"+X", KeyCode.None},
        {"-X", KeyCode.None},
        {"+Y", KeyCode.None},
        {"-Y", KeyCode.None},
        {"Start", KeyCode.JoystickButton7},
        {"Jump", KeyCode.JoystickButton0}, // A
        {"Dash", KeyCode.JoystickButton5}, // RB
        {"Crouch", KeyCode.JoystickButton8}, // left stick
        {"Attack", KeyCode.JoystickButton1}, // B
        {"SpeAttack", KeyCode.JoystickButton3}, // Y
        {"Accept", KeyCode.JoystickButton0}, // A
        {"Decline", KeyCode.JoystickButton1}, // B
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void Main()
    {
        LoadInput();
    }

    // load the input file  /  if it doesn't exist, it creates it
    public static void LoadInput()
    {
        if (File.Exists(Application.dataPath + "/Player1Inputs.dat"))
        {
            Keyboard.Clear();
            Controller.Clear();

            string[] content = File.ReadAllLines(Application.dataPath + "/Player1Inputs.dat");

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

            currentInputs = Controller;
        }
        else
        {
            CreateInput();
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

    // create the input file with default settings
    public static void CreateInput()
    {
        Keyboard = DefaultKeyboard;
        Controller = DefaultController;

        using (StreamWriter SW = File.CreateText(Application.dataPath + "/Player1Inputs.dat"))
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
    public static void ChangeInput(string ControllerName, string InputName, KeyCode Key)
    {

        try
        {
            string[] content = File.ReadAllLines(Application.dataPath + "/Player1Inputs.dat");

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

            File.WriteAllLines(Application.dataPath + "/Player1Inputs.dat", content);

            LoadInput();
        }
        catch
        {
            LoadInput();
        }
    }
}
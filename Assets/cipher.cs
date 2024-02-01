using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using TMPro;

public class CipherController : MonoBehaviour
{
    public TMP_Text Ciphered_Text;
    public TMP_Text Key_Deciphered;
    public InputDevice leftCon;
    public InputDevice rightCon;
    //public bool _solved;

    void Start()
    {
        
        List<InputDevice> devicesRight = new List<InputDevice>();
        List<InputDevice> devicesLeft = new List<InputDevice>();
        Debug.Log(devicesRight.ToString());
        InputDeviceCharacteristics rightControllerChar = InputDeviceCharacteristics.Right; // | InputDeviceCharacteristics.Controller;
        InputDeviceCharacteristics leftControllerChar = InputDeviceCharacteristics.Left; // | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerChar, devicesRight);
        InputDevices.GetDevicesWithCharacteristics(leftControllerChar, devicesLeft);
        Debug.Log(devicesRight.ToString());
        Debug.Log(devicesLeft.ToString());
        rightCon = devicesRight[0];
        leftCon = devicesLeft[0];
        

        Ciphered_Text = GameObject.FindWithTag("cipher_text").GetComponent<TextMeshPro>();
        Key_Deciphered = GameObject.FindWithTag("decipher_key").GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        // SprawdŸ, czy nast¹pi³o klikniêcie na kontrolerze VR
        //if (Mouse.current.leftButton.wasPressedThisFrame) 
        if (rightCon.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightConValue) && rightConValue)
        {
            int shiftAmount = 1;  // Iloœæ miejsc do przesuniêcia

            // Pobierz aktualny tekst z obiektu TextMesh
            string currentText = Ciphered_Text.text;

            // Zaszyfruj tekst i zaktualizuj obiekt TextMesh
            string encryptedText = Encrypt(currentText, shiftAmount);
            Ciphered_Text.text = encryptedText;

            //Debug.Log("Encrypted Text: " + encryptedText);
        }


        //if (Mouse.current.rightButton.wasPressedThisFrame)
        if (leftCon.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftConValue) && leftConValue)
        {
            int shiftAmount = -1;  // Iloœæ miejsc do przesuniêcia

            // Pobierz aktualny tekst z obiektu TextMesh
            string currentText = Ciphered_Text.text;

            // Zaszyfruj tekst i zaktualizuj obiekt TextMesh
            string encryptedText = Encrypt(currentText, shiftAmount);
            Ciphered_Text.text = encryptedText;

            //Debug.Log("Encrypted Text: " + encryptedText);
        }

        Ciphered_Text = GameObject.FindWithTag("cipher_text").GetComponent<TextMeshPro>();
        Key_Deciphered = GameObject.FindWithTag("decipher_key").GetComponent<TextMeshPro>();
        if (Ciphered_Text.text == "O cztery znaki w prawo")
        {
            Key_Deciphered.text = "Rozwi¹zano zagadkê - liczba: 432";
            Key_Deciphered.color = new UnityEngine.Color(1, 0, 0, 1);
            //_solved = true;
        }
        else
        {
            Key_Deciphered.text = "Rozwi¹¿ zagadkê";
            Key_Deciphered.color = new UnityEngine.Color(0, 0, 0, 1);
            //_solved = false;
        }
    }

    private string Encrypt(string text, int shift)
    {
        string result = "";

        foreach (char letter in text)
        {
            if (char.IsLetter(letter))
            {
                char baseChar = char.IsUpper(letter) ? 'A' : 'a';
                char encryptedChar = (char)(((letter - baseChar + shift) % 26 + 26) % 26 + baseChar);
                result += encryptedChar;
            }
            else
            {
                result += letter;
            }
        }

        return result;
    }

}


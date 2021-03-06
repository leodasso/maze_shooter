﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;



public class TalkyText : MonoBehaviour
{
    public TextFormattingData formattingData;
    
    [MinValue(1), HorizontalGroup("chars")]
    public int charactersPerSecond;
    public float delay;

    public float Interval => 1f / charactersPerSecond;

    /// <summary>
    /// The full text to display
    /// </summary>
    [MultiLineProperty(5), HideLabel, BoxGroup("Text")]
    public string inputText = "";

    [Tooltip("Enter this character to have the talky text pause.")]
    public string pauseCharacter = ".";
    public float pauseTime = .2f;

    public bool FullyShowing => formattedOutput.Length == inputText.Length;

    /// <summary>
    /// A single symbol. Formatting tags like <b>, </i>, and \n are all considered single symbols
    /// </summary>
    [ReadOnly, ShowInInspector]
    string _nextSymbol;

    [ReadOnly, ShowInInspector]
    int _nextSymbolIndex = 0;

    [ReadOnly, ShowInInspector]
    List<string> _expectedClosingTags = new List<string>();

    /// <summary>
    /// The text being shown this frame
    /// </summary>
    [MultiLineProperty(5), ReadOnly, HideLabel, BoxGroup("Text")]
    public string formattedOutput = "";

    float _intervalTimer;
    TextMeshProUGUI _textBoxUGUI;
    TextMeshPro _textBox;
    Text _textBoxLegacy;
    string _outputText = "";
    
    /// <summary>
    /// How many characters have appeared since the last sfx
    /// </summary>
    int _charCount;
    /// <summary>
    /// How many characters appear between sfx
    /// </summary>
    int _sfxChars = 1;

    float _t;
    float _audioT;

    void Awake()
    {
        _textBox = GetComponent<TextMeshPro>();
        _textBoxUGUI = GetComponent<TextMeshProUGUI>();
        _textBoxLegacy = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        if (_textBox) _textBox.parseCtrlCharacters = true;
        if (_textBoxUGUI) _textBoxUGUI.parseCtrlCharacters = true;
        
        Clear();
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        // If fully showing, only update if the input text changes. 
        if (FullyShowing)
        {
            // If input text changes, clear the output text.
            if (_outputText != inputText) Clear();
            return;
        }

        if (inputText == null)
        {
            Clear();
            return;
        }
        
        // If the input has changed, clear the text
        if (inputText.Length < _outputText.Length) Clear();

        // delay
        if (_t < delay)
        {
            _t += Time.unscaledDeltaTime;
            return;
        }

        //Determine when to add a new (text) character to active dialogue box
        _intervalTimer += Time.unscaledDeltaTime;

        if (_intervalTimer >= Interval)
        {
            // add multiple characters per frame if the interval is less than the framerate
            int charTimes = Mathf.CeilToInt(Time.unscaledDeltaTime / Interval);
            for (int i = 0; i < charTimes; i++) UpdateText();

            _intervalTimer = 0;
              
            DoSfx();
        }
    }

    void DoSfx()
    {
        // TODO
        //if (_charCount >= _sfxChars)
            
        
        _charCount++;
        if (_charCount > _sfxChars) _charCount = 0;
    }

    public void SetTextColor(Color color)
    {
        if (_textBox) _textBox.color = color;
        if (_textBoxUGUI) _textBoxUGUI.color = color;
        if (_textBoxLegacy) _textBoxLegacy.color = color;
    }

    /// <summary>
    /// Clears output text, and resets stuff to begin typing again.
    /// </summary>
    [Button]
    public void Clear()
    {
        _outputText = formattedOutput = "";
        _t = 0;
        _nextSymbolIndex = 0;
        _expectedClosingTags.Clear();
    }

    /// <summary>
    /// Clears output text and the textbox components
    /// </summary>
    public void FullClear()
    {
        Debug.Log("Doing a full clear!");
        inputText = "";
        Clear();
        PushTextToComponents("");
    }

    /// <summary>
    /// Some sets of characters are considered one Symbol, such as formatting tags. Here, we'll
    /// return the next symbol, as well as output what the next index should be.
    /// </summary>
    /// <param name="nextIndex">The next index after the returned symbol</param>
    static string SymbolAtIndex(int startIndex, string fullText, out int nextIndex)
    {
        nextIndex = startIndex + 1;
        if (startIndex >= fullText.Length)
        {
            Debug.LogWarning("Index " + startIndex + " is beyond the bounds of the given string.");
            return "";
        }

        string nextSymbol = string.Empty;
        nextSymbol += fullText[startIndex];
        
        // Check if the next symbol is either of the formatting tags
        if (nextSymbol != "<") return nextSymbol;
        while (true)
        {
            nextSymbol += fullText[nextIndex];
            if (fullText[nextIndex] == '>') break;
            nextIndex++;
            if (nextIndex >= fullText.Length) break;
        }

        nextIndex++;
        return nextSymbol;
    }

    /// <summary>
    /// Adds a new character to the currently displayed inputText to give the effect of the text appearing gradually.
    /// Supports rich character tags '<>'
    /// </summary>
    [Button]
    void UpdateText()
    {
        if (FullyShowing) return;
        _nextSymbol = string.Empty;

        // Can't update text if the next symbol index is outside the range of our input text!
        if (_nextSymbolIndex >= inputText.Length) return;

        // Find the next symbol to parse
        _nextSymbol = SymbolAtIndex(_nextSymbolIndex, inputText, out _nextSymbolIndex);
        
        // check for formatting
        foreach (var set in formattingData.formattingSets)
        {
            if (_nextSymbol.Contains(set.opener))
                _expectedClosingTags.Add(set.closer);

            if (_nextSymbol.Contains(set.closer))
                _expectedClosingTags.Remove(set.closer);
        }
        
        // pausing characters
        if (_nextSymbol.Contains(pauseCharacter)) _intervalTimer += pauseTime;

        _outputText += _nextSymbol;

        formattedOutput = _outputText;
        // add the formatters back into the text
        foreach (var s in _expectedClosingTags)
            formattedOutput += s;
        
        PushTextToComponents(formattedOutput);
    }

    void PushTextToComponents(string text)
    {
        if (_textBox) _textBox.text = text;
        if (_textBoxUGUI) _textBoxUGUI.text = text;
        if (_textBoxLegacy) _textBoxLegacy.text = text;
    }

    [Button]
    public void ShowFull()
    {
        if (FullyShowing) return;
        _outputText = formattedOutput = inputText;
        PushTextToComponents(formattedOutput);
    }
}
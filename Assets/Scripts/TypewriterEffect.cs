using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
	private TMP_Text _textBox;

	// Basic Typewriter Functionality
	private int _currentVisibleCharacterIndex;
	private Coroutine _typewriterCoroutine;
	private bool _readyForNewText = true;

	private WaitForSeconds _simpleDelay;
	private WaitForSeconds _interpunctuationDelay;

	[Header("Typewriter Settings")] 
	[SerializeField] private float charactersPerSecond = 20;
	[SerializeField] private float interpunctuationDelay = 0.5f;

	// Event Functionality
	private WaitForSeconds _textboxFullEventDelay;
	[SerializeField] [Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f; // In testing, I found 0.25 to be a good value

	public static event Action CompleteTextRevealed;
	public static event Action<char> CharacterRevealed;

	// [SerializeField] private string testText;

	private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();

        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
	}

	private void Start()
	{
		// SetText(testText);
	}

	public void SetText(string text, bool skip = false)
	{
		_textBox.text = text;
		_textBox.maxVisibleCharacters = skip ? text.Length : 0;
		_currentVisibleCharacterIndex = 0;
	}

	private void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
    }

	private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
    }

	private void PrepareForNewText(UnityEngine.Object obj)
    {
        if (obj != _textBox || !_readyForNewText || _textBox.maxVisibleCharacters >= _textBox.textInfo.characterCount)
            return;

        _readyForNewText = false;
            
        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = _textBox.textInfo;

        while (_currentVisibleCharacterIndex < textInfo.characterCount + 1)
        {
            var lastCharacterIndex = textInfo.characterCount - 1;

            if (_currentVisibleCharacterIndex >= lastCharacterIndex)
            {
            	_textBox.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;
                CompleteTextRevealed?.Invoke();
                _readyForNewText = true;
                yield break;
            }

            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

            _textBox.maxVisibleCharacters++;
                
            if (character == '?' || character == '.' || character == ',' || character == ':' ||
				character == ';' || character == '!' || character == '-')
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return _simpleDelay;
            }
                
            CharacterRevealed?.Invoke(character);
            _currentVisibleCharacterIndex++;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Color _backgroundColor;
    [SerializeField] private Image _title;
    [SerializeField] private Sprite _titleSprite;

    private void Awake()
    {
        _background.color = _backgroundColor;
        _title.sprite = _titleSprite;
    }
}

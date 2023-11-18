using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryElements;
using UnityEngine;

[CreateAssetMenu(fileName = "ChemistryElement", menuName = "AddChemistryElement")]
public class ChemistryElementData : ScriptableObject
{
    [Header("Element Params")]
    public string elementName;
    public OtherElements elementFormula;
    public AcidicEnvironment elementEnvironment;
    public Color elementColor;

    [Header("Element Reactions")] 
    [EditInline] public ChemistryReactionsParameters[] chemistryReactionsParameter;
}

public enum AcidicEnvironment
{
    Sour,
    Alkaline,
    Neutral
}

public enum Colors
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
    Pink,
    White,
    Black,
    Brown,
    Gray,
    Violet
}

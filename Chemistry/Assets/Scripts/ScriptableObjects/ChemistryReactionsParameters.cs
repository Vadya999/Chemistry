using UnityEngine;

[CreateAssetMenu(fileName = "ChemistryReaction", menuName = "AddChemistryReaction")]
public class ChemistryReactionsParameters : ScriptableObject
{
    [Header("General Settings")]
    public OtherElements thisInteractElement;
    public OtherElements interactElementTwo;
    public AcidicEnvironment jointBeenElementsEnvironment;

    [Header("Effects")] 
    public bool changeColor;
    public Color color;
    public bool gasEvolution;
    public bool fireEvolution;
}

public enum OtherElements
{
    H2O,
    BR2,
    HG,
    N2,
    CL2,
    I2,
    AS,
    GL,
    CS,
    CL,
    H10
}


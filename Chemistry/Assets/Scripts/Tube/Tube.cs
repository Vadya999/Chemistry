using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class Tube : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private int _capacityElements;
    [Header("Pipette Settings")]
    [SerializeField] private Transform _correctPipettePos;
    [SerializeField] private float _speedPipette;
    [SerializeField] private float _delayEffectToFillTube;
    [Header("Develop Settings")]
    [SerializeField] private List<ChemistryElementData> _chemistryElementsData;
    [Header("Canvas Settings")]
    [SerializeField] private Transform _elementsCanvas;
    [SerializeField] private TextMeshProUGUI _textElements;
    [Header("VFX Settings")]
    [SerializeField] private Material _material;
    [SerializeField] private ParticleSystem _smokeParticleSystem;
    [SerializeField] private ParticleSystem _fireParticleSystem;
    [SerializeField] private  MeshRenderer _meshRenderer;
    
    
    private byte _currentFillTube;
    private bool _isTubeFilled;
    private AcidicEnvironment _acidicEnvironment;

    private void Awake()
    {
        _meshRenderer.material.SetFloat("_test", 0);
    }

    public void TryFilledTube(ChemistryElementData _chemistryElement, Transform pipette,Vector3 initPipettePos)
    {
        if (_isTubeFilled)
        {
            pipette.transform.position = initPipettePos;
        }
        else
        {
           StartCoroutine(FillTube(_chemistryElement, pipette, initPipettePos));
        }
    }

    public void TryFilledTubeWithIndicator(IndicatorElementData indicatorElementData, Transform pipette,Vector3 initPipettePos)
    {
        if (_currentFillTube == 0)
        {
            pipette.transform.position = initPipettePos;
        }
        else
        {
            StartCoroutine(FillTubeWithIndicator(indicatorElementData,pipette, initPipettePos));
        }
    }

    private IEnumerator FillTubeWithIndicator(IndicatorElementData indicatorElementData,Transform pipette, Vector3 initPipettePos)
    {
        pipette.transform.DOMove(_correctPipettePos.position, _speedPipette);
        yield return new WaitForSeconds(_speedPipette);
        
        yield return new WaitForSeconds(_delayEffectToFillTube);

        pipette.transform.DOMove(initPipettePos, _speedPipette);
        yield return new WaitForSeconds(_speedPipette);

        Color correctColor = default;
        switch (_acidicEnvironment)
        {
            case AcidicEnvironment.Sour:
                correctColor = indicatorElementData.colorIndicatorWithSourEnvironment;
                break;
            case AcidicEnvironment.Alkaline:
                correctColor = indicatorElementData.colorIndicatorWithAlkalineEnvironment;
                break;
            case AcidicEnvironment.Neutral:
                correctColor = indicatorElementData.colorIndicatorWithNeutralEnvironment;
                break;
        }

        ChangeColorElementsWithIndicator(correctColor);
        
        yield return null;
    }

    private IEnumerator FillTube(ChemistryElementData _chemistryElement, Transform pipette, Vector3 initPipettePos)
    {
        pipette.transform.DOMove(_correctPipettePos.position, _speedPipette);
        yield return new WaitForSeconds(_speedPipette);
        
        yield return new WaitForSeconds(_delayEffectToFillTube);

        pipette.transform.DOMove(initPipettePos, _speedPipette);
        yield return new WaitForSeconds(_speedPipette);

        for (int i = 0; i < _chemistryElementsData.Count; i++)
        {
            if (_currentFillTube == 1)
            {
                if (_chemistryElementsData[0] == _chemistryElement)
                {
                    yield break;
                }
            }

            if (_chemistryElementsData[i] == null)
            {
                _chemistryElementsData[i] = _chemistryElement;
                _currentFillTube++;
                _acidicEnvironment = _chemistryElementsData[i].elementEnvironment;
                if (_currentFillTube == _capacityElements)
                {
                    _isTubeFilled = true;
                }
                break;
            }
        }

        CheckReactionElementsInTube();
        
        yield break;
    }

    private void CheckReactionElementsInTube()
    {
        if (_currentFillTube == 1)
        {
            ViewFilledTubeWithOneElement();
        }
        else
        {
            ViewFilledTubeWithTwoElement();
        }
    }

    private void ViewFilledTubeWithOneElement()
    {
        _meshRenderer.material.SetFloat("_test", 2);
        _meshRenderer.material.SetColor("_Color", _chemistryElementsData[0].elementColor); 
    }

    private void ViewFilledTubeWithTwoElement()
    {

        byte countReactions = 0;
        
        _meshRenderer.material.SetFloat("_test", 4);
        
        for (int i = 0; i < _chemistryElementsData[0].chemistryReactionsParameter.Length; i++)
        {
            
            if (_chemistryElementsData[0].chemistryReactionsParameter[i].interactElementTwo == _chemistryElementsData[1].elementFormula)
            {
                if (_chemistryElementsData[0].chemistryReactionsParameter[i].changeColor)
                {
                    ChangeColorElementsInTube(_chemistryElementsData[0].chemistryReactionsParameter[i].color);
                    countReactions++;
                    _acidicEnvironment = _chemistryElementsData[0].chemistryReactionsParameter[i].jointBeenElementsEnvironment;
                }

                if (_chemistryElementsData[0].chemistryReactionsParameter[i].fireEvolution)
                {
                    PlayFireEvolution();
                    countReactions++;
                    _acidicEnvironment = _chemistryElementsData[0].chemistryReactionsParameter[i].jointBeenElementsEnvironment;
                }

                if (_chemistryElementsData[0].chemistryReactionsParameter[i].gasEvolution)
                {
                    PlayGasEvolution();
                    countReactions++;
                    _acidicEnvironment = _chemistryElementsData[0].chemistryReactionsParameter[i].jointBeenElementsEnvironment;
                }
            }
            
        }
        
        for (int i = 0; i < _chemistryElementsData[1].chemistryReactionsParameter.Length; i++)
        {
            
            if (_chemistryElementsData[1].chemistryReactionsParameter[i].interactElementTwo == _chemistryElementsData[0].elementFormula)
            {
                if (_chemistryElementsData[1].chemistryReactionsParameter[i].changeColor)
                {
                    ChangeColorElementsInTube(_chemistryElementsData[1].chemistryReactionsParameter[i].color);
                    countReactions++;
                    _acidicEnvironment = _chemistryElementsData[1].chemistryReactionsParameter[i].jointBeenElementsEnvironment;
                }

                if (_chemistryElementsData[1].chemistryReactionsParameter[i].fireEvolution)
                {
                    PlayFireEvolution();
                    countReactions++;
                    _acidicEnvironment = _chemistryElementsData[1].chemistryReactionsParameter[i].jointBeenElementsEnvironment;
                }

                if (_chemistryElementsData[1].chemistryReactionsParameter[i].gasEvolution)
                {
                    PlayGasEvolution();
                    countReactions++;
                    _acidicEnvironment = _chemistryElementsData[1].chemistryReactionsParameter[i].jointBeenElementsEnvironment;
                }
            }
            
        }

        if (countReactions == 0)
        {
            NoReactionWithTwoElements();
        }
    }

    private void ViewTextElements()
    {
        _elementsCanvas.gameObject.SetActive(true);
        _textElements.text = _chemistryElementsData[0].elementFormula.ToString() +
                             _chemistryElementsData[1].elementFormula.ToString();
    }

    private void NoReactionWithTwoElements()
    {
        Debug.Log("No Reaction");
        ViewTextElements();
        _acidicEnvironment = AcidicEnvironment.Neutral;
    }

    private void ChangeColorElementsInTube(Color color)
    {
        _meshRenderer.material.SetColor("_Color", color);
        ViewTextElements();
    }

    private void PlayFireEvolution()
    {
        ViewTextElements();
        _fireParticleSystem.Play();
    }

    private void PlayGasEvolution()
    {
        ViewTextElements();
        _smokeParticleSystem.Play();
    }
    
    private void ChangeColorElementsWithIndicator(Color color)
    {
        _meshRenderer.material.SetColor("_Color", color);
    }
}
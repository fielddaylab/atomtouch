using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LevelGuide : MonoBehaviour
{
  public TriggerOperator triggerOperator;
  public GameObject rigPanel;
  public GameObject contentPanel;
  public LevelGuideScroll levelGuideScroll;
  public Button backButton;
  public Button nextButton;
  public Text nextButtonText;
  public Text heading;
  public Text instruction;
  public AtomTouchGUI controller;
  public LevelsPanel levelsPanel;
  public LevelInstructionSet LIS;
  public GameObject yesNoButtonSet;
  public GameObject multipleChoiceButtonSet;
  public GameObject[] hideObjects;
  public AudioSource activityComplete;

  private int levelNumber;
  private int instructionNumber;
  private LevelInstructions linstrs;
  private Instructions instrs;
  private GameObject target;
  private int buttonTriggerCount;
  private Button[] buttonTrggers;
  private GameObject yesNo;
  private GameObject multipleChoice;
  private int answer;
  private string[] buttonAnswers;

  private AudioSource buttonSound;

  void Start()
  {
    buttonSound = GetComponent<AudioSource>();
  }

  public void SetLevelGuide(int levelNumber, int instructionNumber)
  {
    this.levelNumber = levelNumber;
    this.instructionNumber = instructionNumber;
    if(levelNumber < LIS.levelInstructions.Length && instructionNumber < LIS.levelInstructions[levelNumber].instructions.Length)
    {
      this.linstrs = LIS.levelInstructions[levelNumber];
      this.instrs = linstrs.instructions[instructionNumber];
      this.target        = this.instrs.target;
      this.buttonTrggers = this.instrs.buttonTriggers;
      heading.text       = this.instrs.heading;
      if(target != null) target.SetActive(true);
      if(buttonTrggers != null)
      {
        if(!this.instrs.cleared) nextButton.interactable = false;
        foreach(Button button in buttonTrggers)
          button.onClick.AddListener(delegate { ButtonTriggered(); });
      }

      if(instructionNumber != 0 && instructionNumber-1 < this.linstrs.instructions.Length)
      {
        Instructions prev_instrs = linstrs.instructions[instructionNumber-1];
             if(prev_instrs.hasYesNo)          instruction.text = this.instrs.instructions[answer];
        else if(prev_instrs.hasMultipleChoice) instruction.text = this.instrs.instructions[answer];
        else                                   instruction.text = this.instrs.instruction;
      }
      else                                     instruction.text = this.instrs.instruction;

      this.buttonAnswers = this.instrs.buttonAnswers;

      if(this.instrs.hasYesNo) yesNoButtonSet.SetActive(true);

      if(this.instrs.hasMultipleChoice)
      {
        this.instrs.cleared = false;
        multipleChoiceButtonSet.SetActive(true);
        Button[] buttons = multipleChoiceButtonSet.GetComponentsInChildren<Button>();
        for(int i = 0; i < buttonAnswers.Length; i++)
        {
          buttons[i].GetComponentInChildren<Text>().text = buttonAnswers[i];
        }
      }

      if(this.instrs.zone > 0) ChangeZones(this.instrs.zone);
      else                     ChangeZones(100);

      if(this.instrs.cleared) nextButton.interactable = true;
      else                    nextButton.interactable = false;

      triggerOperator.triggerID = this.instrs.TriggerID;

      if(instructionNumber == 0) backButton.interactable = false;
      else                       backButton.interactable = true;

      if(instructionNumber == this.linstrs.instructions.Length-1) nextButtonText.text = "Finish";
      else                                                        nextButtonText.text = "Next";

      if(this.instrs.blockerOn)
      {
        foreach(GameObject ob in hideObjects)
        {
          ob.SetActive(false);
        }
        StaticVariables.canSelectAtoms = false;
      }
      else
      {
        foreach(GameObject ob in hideObjects)
        {
          ob.SetActive(true);
        }
        StaticVariables.canSelectAtoms = true;
      }

      if(this.instrs.showObjects.Length > 0)
      {
        foreach(GameObject ob in this.instrs.showObjects)
        {
          ob.SetActive(true);
        }
        StaticVariables.canSelectAtoms = false;
      }

      buttonTriggerCount = 0;
    }
    else //no level data- sandbox
    {
      this.linstrs = null;
      this.instrs = null;
      this.target = null;
      this.buttonTrggers = null;
      heading.text = "";
      instruction.text = "";
      this.buttonAnswers = null;
      yesNoButtonSet.SetActive(false);
      multipleChoiceButtonSet.SetActive(false);
      ChangeZones(100);
      nextButton.interactable = false;
      triggerOperator.triggerID = 0; //?
      backButton.interactable = false;

      foreach(GameObject ob in hideObjects)
      {
        ob.SetActive(true);
      }
      StaticVariables.canSelectAtoms = true;

      buttonTriggerCount = 0; //?
      controller.LevelGuideClose();
    }
  }

  public void ButtonTriggered()
  {
    if(this.instrs == null) return;
    buttonTriggerCount++;
    if(buttonTriggerCount == this.instrs.buttonTriggerCount)
    {
      nextButton.interactable = true;
      foreach(Button button in buttonTrggers)
      {
        button.onClick.RemoveListener(delegate
        {
          ButtonTriggered();
        });
      }
      this.instrs.cleared = true;
      if(target != null)
        target.SetActive(false);
      buttonTriggerCount = 0;
    }
  }

  public void EventTriggered()
  {
    if(this.instrs == null) return;
    nextButton.interactable = true;
    if(target != null) target.SetActive(false);
    this.instrs.cleared = true;
  }

  public void NextButton()
  {
    if(this.instrs == null) return;
    buttonSound.Play();

    // reset answer buttons - MUST STAY ABOVE LIS CHANGE!
    if(this.instrs.hasYesNo) yesNoButtonSet.SetActive(false);

    if(this.instrs.hasMultipleChoice) multipleChoiceButtonSet.SetActive(false);

    if(target != null) target.SetActive(false);

    // Activity Complete
    if(instructionNumber == this.linstrs.instructions.Length-1)
    {
      activityComplete.Play();
      controller.hudController.activityCompletePanel.SetActive(true);
      controller.LevelGuideClose();
      //controller.LevelsOpen();
      levelsPanel.LevelComplete(levelNumber);
      return;
    }
    else
    {
      // Continue the level guide
      SetLevelGuide(levelNumber, this.instructionNumber + 1);
    }
  }

  public void BackButton()
  {
    if(this.instrs == null) return;

    buttonSound.Play();
    // reset answer buttons - MUST STAY ABOVE LIS CHANGE!
    if(this.instrs.hasYesNo) yesNoButtonSet.SetActive(false);

    if(this.instrs.hasMultipleChoice) multipleChoiceButtonSet.SetActive(false);

    if(target != null) target.SetActive(false);

    SetLevelGuide(levelNumber, this.instructionNumber-1);
  }

  public void OnMouseDown()
  {
    controller.cameraScript.enabled = false;
  }

  public void OnMouseUp()
  {
    controller.cameraScript.enabled = true;
  }

  public void LevelGuideQuite()
  {
    if(this.instrs == null) return;

    // reset answer buttons - MUST STAY ABOVE LIS CHANGE!
    if(this.instrs.hasYesNo)          yesNoButtonSet.SetActive(false);
    if(this.instrs.hasMultipleChoice) multipleChoiceButtonSet.SetActive(false);
    if(target != null)                target.SetActive(false);
  }

  // No = 0, Yes = 1
  public void YesNo(int answer)
  {
    this.answer = answer;
    EventTriggered();
  }

  // A = 0, B = 1, C = 2, D = 3
  public void MultipleChoice(int answer)
  {
    this.answer = answer;
    EventTriggered();
  }

  public void ChangeZones(int zone)
  {
    for(int i = 0; i < Atom.AllAtoms.Count; i++)
    {
      Atom currAtom = Atom.AllAtoms[i];

      if(currAtom.selectionZone == zone)
      {
        currAtom.SetSelected(true);
        Debug.Log(i);
      }
      else
      {
        currAtom.SetSelected(false);
      }
    }
  }
}


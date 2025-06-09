using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

// Define basic structures
[Serializable]
public class Word { public string name; public string animationClipName; }

[Serializable]
public class LectureText { public string content; public List<Word> words = new List<Word>(); }

[Serializable]
public class Course
{
    public int id; public string name; public string content;
    public List<Word> words = new List<Word>();
    public List<LectureText> lectureTexts = new List<LectureText>();
}

[Serializable]
public class Quiz { public int id; public string content; }

public class AdminPanelManager : MonoBehaviour
{
    [Header("UI Manager")]
    public UIManager uiManager;

    [Header("Main Tabs and Add Button")]
    public Button showLectureBtn;
    public Button showQuizBtn;
    public Button addItemBtn;
    public Text addItemBtnText;

    [Header("Panels")]
    public GameObject lecturePanel;
    public GameObject quizPanel;
    public GameObject editLecturePanel;
    public GameObject wordEditPanel;
    public GameObject grammarEditPanel;
    public GameObject lectureTextEditPanel;

    [Header("View Containers")]
    public RectTransform lectureListContent;
    public RectTransform wordListContent;
    public RectTransform grammarListContent;
    public RectTransform lectureTextListContent;
    public RectTransform constructContainer;

    [Header("Prefabs")]
    public GameObject lectureItemPrefab;
    public GameObject wordItemPrefab;
    public GameObject grammarItemPrefab;
    public GameObject lectureTextItemPrefab;
    public GameObject wordSelectPrefab;

    [Header("Edit Lecture Sub-Tabs")]
    public Button showWordsBtn;
    public Button showGrammarBtn;
    public Button showLectureTextsBtn;
    public GameObject wordsSection;
    public GameObject grammarSection;
    public GameObject lectureTextsSection;

    [Header("Lecture Edit UI")]
    public InputField lectureNameInput;

    [Header("Word Edit UI")]
    public Button addWordBtn;
    public InputField wordNameInput;
    public InputField animClipInput;
    public Button saveWordBtn;
    public Button cancelWordBtn;

    [Header("Grammar Edit UI")]
    public Button addGrammarBtn;
    public InputField grammarImageInput;
    public Button saveGrammarBtn;
    public Button cancelGrammarBtn;

    [Header("Lecture Text Edit UI")]
    public Button addLectureTextBtn;
    public InputField lectureTextContentInput;
    public Button saveLectureTextBtn;
    public Button cancelLectureTextBtn;
    public Button addConstructWordBtn;

    // Internal data
    private List<Course> lectures = new List<Course>();
    private Course editingLecture;
    private Word editingWord;
    private LectureText editingLectureText;

    void Start()
    {
        showLectureBtn.onClick.AddListener(() => SwitchMainTab(true));
        showQuizBtn.onClick.AddListener(() => SwitchMainTab(false));
        addItemBtn.onClick.AddListener(OnAddItemClicked);

        showWordsBtn.onClick.AddListener(() => uiManager.ShowOnly(wordsSection));
        showGrammarBtn.onClick.AddListener(() => uiManager.ShowOnly(grammarSection));
        showLectureTextsBtn.onClick.AddListener(() => uiManager.ShowOnly(lectureTextsSection));

        addWordBtn.onClick.AddListener(() => ShowWordEdit(null));
        saveWordBtn.onClick.AddListener(SaveWord);
        cancelWordBtn.onClick.AddListener(() => uiManager.ShowOnly(editLecturePanel));

        addGrammarBtn.onClick.AddListener(() => ShowGrammarEdit(null));
        saveGrammarBtn.onClick.AddListener(SaveGrammar);
        cancelGrammarBtn.onClick.AddListener(() => uiManager.ShowOnly(editLecturePanel));

        addLectureTextBtn.onClick.AddListener(() => ShowLectureTextEdit(null));
        saveLectureTextBtn.onClick.AddListener(SaveLectureText);
        cancelLectureTextBtn.onClick.AddListener(() => uiManager.ShowOnly(editLecturePanel));
        addConstructWordBtn.onClick.AddListener(() => AddWordSelect(null));

        uiManager.ShowOnly(lecturePanel);
        LoadLectures();
    }

    void SwitchMainTab(bool lectureTab)
    {
        uiManager.ShowOnly(lectureTab ? lecturePanel : quizPanel);
        addItemBtnText.text = lectureTab ? "Add Lecture" : "Add Quiz";
    }

    void OnAddItemClicked()
    {
        if (lecturePanel.activeSelf) AddLecture();
        else Debug.Log("Add Quiz not implemented yet");
    }

    void LoadLectures()
    {
        foreach (Transform c in lectureListContent) Destroy(c.gameObject);

        foreach (var lec in lectures)
        {
            var go = Instantiate(lectureItemPrefab, lectureListContent);
            var item = go.GetComponent<LectureItems>();
            item.titleText.text = lec.name;
            item.editButton.onClick.RemoveAllListeners();
            item.editButton.onClick.AddListener(() => EditLecture(lec));
            item.deleteButton.onClick.RemoveAllListeners();
            item.deleteButton.onClick.AddListener(() => { lectures.Remove(lec); LoadLectures(); });
        }
    }

    void AddLecture()
    {
        lectures.Add(new Course { id = lectures.Count + 1, name = "New Lecture", content = "" });
        LoadLectures();
    }

    void EditLecture(Course lec)
    {
        editingLecture = lec;
        
        uiManager.ShowOnly(editLecturePanel);
        lectureNameInput.text = editingLecture.name; // <-- set the name
        LoadWords(); LoadGrammar(); LoadLectureTexts();
    }

    void LoadWords()
    {
        foreach (Transform c in wordListContent) Destroy(c.gameObject);
        foreach (var w in editingLecture.words)
        {
            var go = Instantiate(wordItemPrefab, wordListContent);
            var item = go.GetComponent<WordItem>();
            item.wordNameText.text = w.name;
            item.editButton.onClick.RemoveAllListeners();
            item.editButton.onClick.AddListener(() => ShowWordEdit(w));
            item.deleteButton.onClick.RemoveAllListeners();
            item.deleteButton.onClick.AddListener(() => { editingLecture.words.Remove(w); LoadWords(); });
        }
    }
    public void SaveLectureName()
    {
        editingLecture.name = lectureNameInput.text;
        LoadLectures(); // refresh the list to show updated name
    }
    void ShowWordEdit(Word w)
    {
        editingWord = w ?? new Word();
        wordNameInput.text = editingWord.name;
        animClipInput.text = editingWord.animationClipName;
        uiManager.ShowOnly(wordEditPanel);
    }

    void SaveWord()
    {
        editingWord.name = wordNameInput.text;
        editingWord.animationClipName = animClipInput.text;
        if (!editingLecture.words.Contains(editingWord))
            editingLecture.words.Add(editingWord);
        uiManager.ShowOnly(editLecturePanel);
        uiManager.ShowOnly(wordsSection);
        LoadWords();
    }

    void LoadGrammar()
    {
        foreach (Transform c in grammarListContent) Destroy(c.gameObject);
        foreach (var g in editingLecture.lectureTexts)
        {
            var go = Instantiate(grammarItemPrefab, grammarListContent);
            var item = go.GetComponent<GrammarItem>();
            item.grammarText.text = g.content;
            item.editButton.onClick.RemoveAllListeners();
            item.editButton.onClick.AddListener(() => ShowGrammarEdit(g));
            item.deleteButton.onClick.RemoveAllListeners();
            item.deleteButton.onClick.AddListener(() => { editingLecture.lectureTexts.Remove(g); LoadGrammar(); });
        }
    }

    void ShowGrammarEdit(LectureText g)
    {
        editingLectureText = g ?? new LectureText();
        grammarImageInput.text = editingLectureText.content;
        uiManager.ShowOnly(grammarEditPanel);
    }

    void SaveGrammar()
    {
        editingLectureText.content = grammarImageInput.text;
        if (!editingLecture.lectureTexts.Contains(editingLectureText))
            editingLecture.lectureTexts.Add(editingLectureText);
        uiManager.ShowOnly(editLecturePanel);
        uiManager.ShowOnly(grammarSection);
        LoadGrammar(); // now refreshes UI
    }

    void LoadLectureTexts()
    {
        foreach (Transform c in lectureTextListContent) Destroy(c.gameObject);
        foreach (var t in editingLecture.lectureTexts)
        {
            var go = Instantiate(lectureTextItemPrefab, lectureTextListContent);
            var item = go.GetComponent<SentenceItem>(); // <-- use SentenceItem!
            item.sentenceText.text = t.content;

            item.editButton.onClick.RemoveAllListeners();
            item.editButton.onClick.AddListener(() => ShowLectureTextEdit(t));

            item.deleteButton.onClick.RemoveAllListeners();
            item.deleteButton.onClick.AddListener(() => {
                editingLecture.lectureTexts.Remove(t);
                LoadLectureTexts();
            });
        }
    }


    void ShowLectureTextEdit(LectureText t)
    {
        editingLectureText = t ?? new LectureText();
        lectureTextContentInput.text = editingLectureText.content;
        foreach (Transform c in constructContainer) Destroy(c.gameObject);
        foreach (var w in editingLectureText.words) AddWordSelect(w);
        uiManager.ShowOnly(lectureTextEditPanel);
    }

    void SaveLectureText()
    {
        editingLectureText.content = lectureTextContentInput.text;
        if (!editingLecture.lectureTexts.Contains(editingLectureText))
            editingLecture.lectureTexts.Add(editingLectureText);
        uiManager.ShowOnly(editLecturePanel);
        uiManager.ShowOnly(lectureTextsSection);
        LoadLectureTexts();
    }

    void AddWordSelect(Word w)
    {
        var go = Instantiate(wordSelectPrefab, constructContainer);
        var slot = go.GetComponent<WordSelectSlot>(); // <- use WordSelectSlot
        slot.wordDropdown.options.Clear();
        foreach (var word in editingLecture.words)
        {
            slot.wordDropdown.options.Add(new Dropdown.OptionData(word.name));
        }
        if (w != null)
        {
            int idx = editingLecture.words.FindIndex(x => x.name == w.name);
            slot.wordDropdown.value = idx >= 0 ? idx : 0;
        }
        slot.removeButton.onClick.AddListener(() => Destroy(go));
    }

}

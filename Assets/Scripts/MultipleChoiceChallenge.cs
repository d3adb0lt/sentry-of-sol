using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MultipleChoiceChallenge : MonoBehaviour, ISignChallenge
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Button[] answerButtons;
    [SerializeField] float feedbackSeconds = 0.8f;
    [SerializeField] Color correctColor = new(0.3f, 1f, 0.4f);
    [SerializeField] Color wrongColor = new(1f, 0.35f, 0.35f);

    public event Action<bool> Answered;

    readonly List<SignData> choices = new();
    SignData target;
    bool locked;   // thiss prevents double-taps during feedback

    void Awake()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => OnChoice(index));
        }
    }

    public void Present(SignData target, IReadOnlyList<SignData> pool)
    {
        this.target = target;
        locked = false;

        // Choices = the target + random wrong answers, shuffled
        choices.Clear();
        choices.Add(target);
        var others = new List<SignData>(pool);
        others.Remove(target);
        Shuffle(others);
        for (int i = 0; i < others.Count && choices.Count < answerButtons.Length; i++)
            choices.Add(others[i]);
        Shuffle(choices);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            bool used = i < choices.Count;
            answerButtons[i].gameObject.SetActive(used);
            if (!used) continue;

            answerButtons[i].GetComponentInChildren<TMP_Text>().text = choices[i].word;
            answerButtons[i].image.color = Color.white;
        }

        videoPlayer.clip = target.video;
        videoPlayer.Play();
    }

    void OnChoice(int index)
    {
        if (locked) return;
        locked = true;

        bool correct = choices[index] == target;
        answerButtons[index].image.color = correct ? correctColor : wrongColor;

        if (!correct)   // reveal the right answer
            answerButtons[choices.IndexOf(target)].image.color = correctColor;

        StartCoroutine(FinishAfterDelay(correct));
    }

    IEnumerator FinishAfterDelay(bool correct)
    {
        yield return new WaitForSeconds(feedbackSeconds);
        Answered?.Invoke(correct);
    }

    static void Shuffle<T>(IList<T> list)   // Fisher–Yates my goat, every order equally likely
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
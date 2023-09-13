using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameProcessController : MonoBehaviour
    {
        public int curDay { get; private set; } = 1;

        public int maxDay = 4;

        public DayActivities Activities = DayActivities.None;

        public List<int> dayFristDialogs = new();

        public Image Curtain;

        public float inOutTimer = 0.8f;

        public AudioSource closeDoor;

        public AudioSource bgm;

        public AudioSource end_1;

        public AudioSource end_2;


        public int overDialogId = -1;

        private void Awake()
        {
            Activities = DayActivities.None;
        }

        public void OnEnable()
        {
            ToNextDayAction(false);
        }

        private void ToNextDayAction(bool showHide = true)
        {
            float inoutTimer = inOutTimer;
            Curtain.gameObject.SetActive(true);
            Main.Get().MouseOpter.DisableMouseOpter();
            if (showHide)
            {
                StartCoroutine(OnChangeCurtain(0, 1, inoutTimer, () =>
                {
                    closeDoor.Play();
                    StartCoroutine(WaitTimer(closeDoor.clip.length, () =>
                    {
                        StartCoroutine(OnChangeCurtain(1, 0, inoutTimer, () =>
                        {
                            Curtain.gameObject.SetActive(false);
                            Main.Get().MouseOpter.EnableMouseOpter();
                            TryShowFristDialog();
                        }));
                    }));
                }));
            }
            else
            {
                StartCoroutine(OnChangeCurtain(1, 0, inoutTimer, () =>
                {
                    Curtain.gameObject.SetActive(false);
                    Main.Get().MouseOpter.EnableMouseOpter();
                    TryShowFristDialog();
                }));
            }
        }

        private System.Collections.IEnumerator WaitTimer(float doTimer, Action onEnd)
        {
            float time = doTimer;
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            onEnd?.Invoke();
        }

        private System.Collections.IEnumerator OnChangeCurtain(float start, float end, float doTimer, Action onEnd)
        {
            float time = doTimer;
            while (time > 0)
            {
                time -= Time.deltaTime;
                var color = Curtain.color;
                color.a = Mathf.Clamp01(Mathf.Lerp(start, end, 1f - time / doTimer));
                Curtain.color = color;
                yield return null;
            }
            onEnd?.Invoke();
        }


        public void TryShowFristDialog()
        {
            int dayIndex = curDay - 1;
            if (dayIndex >= 0 && dayIndex < dayFristDialogs.Count)
            {
                Main.Get().UIManager.ShowDialog(dayFristDialogs[dayIndex]);
            }
        }

        public bool TodayFinishAll()
        {
            return Activities == DayActivities.All;
        }

        public void ForceToNextDay()
        {
            if (maxDay <= curDay)
                return;

            Activities = DayActivities.None;
            curDay++;
        }

        public void FinishAll()
        {
            Activities = DayActivities.All;
        }

        public void TryToNextDay()
        {
            if (Activities != DayActivities.All)
            {
                Debug.Log($"To Next Day Failed.Activities {Activities}");
                return;
            }

            if (maxDay <= curDay)
                return;

            curDay++;
            Activities = DayActivities.None;
            ToNextDayAction();
        }

        public void FinishActivities(DayActivities activeities)
        {
            Activities |= activeities;
        }


        private void ResetDay()
        {
            bgm.Play();
            curDay = 1;
            Activities = DayActivities.None;
            UI.ComputerDesktopWindow.Instance.CloseAllWindows();
        }

        public void TryGameOver()
        {
            if (curDay == maxDay)
            {
                Main.Get().CloseBtn.SetActive(false);
                Curtain.gameObject.SetActive(true);
                Main.Get().MouseOpter.DisableMouseOpter();
                StartCoroutine(OnChangeCurtain(0, 1, inOutTimer, () =>
                {
                    StartCoroutine(WaitTimer(2f, () =>
                    {
                        Main.Get().UIManager.ShowDialog(overDialogId, () =>
                        {
                            bgm.Stop();
                            end_1.Play();
                            end_2.PlayDelayed(end_1.clip.length);
                            float waitEndTimer = end_1.clip.length + end_2.clip.length;
                            StartCoroutine(WaitTimer(waitEndTimer, () =>
                            {
                                Main.Get().StateManager.ChangeToState(StateType.Scene);
                                StartCoroutine(OnChangeCurtain(1, 0, inOutTimer,
                                    () =>
                                    {
                                        Curtain.gameObject.SetActive(false);
                                        Main.Get().MouseOpter.EnableMouseOpter();
                                        ResetDay();
                                    }
                                    ));
                            }));
                        }, true);
                    }));
                }));
            }
        }
    }

    [System.Flags]
    public enum DayActivities
    {
        None,
        Dialog = 1 << 0,
        Game = 1 << 1,

        All = Dialog | Game,
    }
}
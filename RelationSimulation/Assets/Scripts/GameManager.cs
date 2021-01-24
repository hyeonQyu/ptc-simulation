using Nextwin.Client.Util;
using UnityEngine;

public enum EFlow
{
    Call,
    Situation,
    Hint,
    ComeIn,
    SelectRiceBall,
    GiveRiceBall,
    SelectAgain,
    SelectCandy,
    GiveCandy,
    RearrangeReport1,
    GiveReport1,
    RearrangeReport2,
    GiveReport2,
    Finish
}

public class GameManager : Singleton<GameManager>
{
    public bool CanGoNextFlow { get; set; }
    public EFlow Flow { get; private set; }
    private bool _isOnFlow = false;

    [SerializeField]
    private Npc _director;
    [SerializeField]
    private Npc[] _leaders;
    [SerializeField]
    private Npc _member;
    [SerializeField]
    private Npc _girlFriend;
    [SerializeField]
    private Npc _me;

    private MidasianController _player;

    [SerializeField]
    private Collider _selectionBox;
    [SerializeField]
    private Collider _reportBox;
    private GameObject _particleSelection;
    private GameObject _particleReport;

    private void Start()
    {
        _player = _me.gameObject.GetComponent<MidasianController>();

        _particleSelection = _selectionBox.transform.GetChild(0).gameObject;
        _particleSelection.SetActive(false);

        _particleReport = _reportBox.transform.GetChild(0).gameObject;
        _particleReport.SetActive(false);

        OtherCollisionChecker selectionBoxCollisionChecker = _selectionBox.gameObject.AddComponent<OtherCollisionChecker>();
        selectionBoxCollisionChecker.AddCollisionEvent(CollisionEvent.OnTriggerEnter, (collider) =>
        {
            OnEnterSelectionBox(collider);
        });

        OtherCollisionChecker reportBoxCollisionChecker = _reportBox.gameObject.AddComponent<OtherCollisionChecker>();
        reportBoxCollisionChecker.AddCollisionEvent(CollisionEvent.OnTriggerEnter, (collider) =>
        {
            OnEnterReportBox(collider);
        });
    }

    // Update is called once per frame
    void Update()
    {
        GoFlow();
    }

    private void GoFlow()
    {
        if(_isOnFlow)
        {
            return;
        }
        _isOnFlow = true;

        switch(Flow)
        {
            case EFlow.Call:
                Call();
                break;

            case EFlow.Situation:
                Situation();
                break;

            case EFlow.Hint:
                Hint();
                break;

            case EFlow.ComeIn:
                ComeIn();
                break;

            case EFlow.SelectRiceBall:
                SelectRiceBall();
                break;

            case EFlow.GiveRiceBall:
                GiveRiceBall();
                break;

            case EFlow.SelectAgain:
                SelectAgain();
                break;

            case EFlow.SelectCandy:
                SelectCandy();
                break;

            case EFlow.GiveCandy:
                GiveCandy();
                break;

            case EFlow.RearrangeReport1:
                RearrangeReport1();
                break;

            case EFlow.GiveReport1:
                GiveReport1();
                break;

            case EFlow.RearrangeReport2:
                RearrangeReport2();
                break;

            case EFlow.GiveReport2:
                GiveReport2();
                break;

            case EFlow.Finish:
                Finish();
                break;
        }
    }

    private void Call()
    {
        _girlFriend.Tell(EScript.Call1, () =>
        {
            _girlFriend.Tell(EScript.Call2, () =>
            {
                _director.Move(EDestination.Entrance, ELookAt.Player, () =>
                {
                    _director.Tell(EScript.DontCall, () =>
                    {
                        _director.Move(EDestination.Exit, ELookAt.None, () =>
                        {
                            ToNextFlow();
                        });
                    });
                });
            });
        });
    }

    private void Situation()
    {
        _leaders[0].Tell(EScript.TodayWhy, () =>
        {
            _leaders[1].Tell(EScript.Broken, () =>
            {
                _leaders[0].Tell(EScript.Why, () =>
                {
                    _leaders[1].Tell(EScript.Forgot, () =>
                    {
                        _leaders[1].Tell(EScript.NoOffWork, () =>
                        {
                            _me.Tell(EScript.Omg, () =>
                            {
                                ToNextFlow();
                            });
                        });
                    });
                });
            });
        });
    }

    private void Hint()
    {
        _member.Tell(EScript.Hey, () =>
        {
            _me.Tell(EScript.Pardon, () =>
            {
                _member.Tell(EScript.Busy, () =>
                {
                    _me.Tell(EScript.Ok, () =>
                    {
                        _member.Move(EDestination.SeatMember, ELookAt.ComputerMember, () =>
                        {
                            _particleSelection.SetActive(true);
                            ToNextFlow();
                        });
                    });
                });
            });
        });
    }

    private void ComeIn()
    {
        _director.Move(EDestination.SeatDirector, ELookAt.ComputerDirector, () =>
        {
            ToNextFlow();
        });
    }
    
    private void SelectRiceBall()
    {
        if(!CanGoNextFlow)
        {
            _isOnFlow = false;
            return;
        }

        _me.Tell(EScript.Which, () =>
        {
            UIManager.Instance.GetDialog(EDialog.Selection).Show();
            ToNextFlow();
        });
    }
    
    private void GiveRiceBall()
    {
        if(!CanGoNextFlow || _player.IsGrabbing)
        {
            _isOnFlow = false;
            return;
        }

        // 주먹밥 건내야 아래 실행
        ActionManager.Instance.ExecuteWithDelay(() =>
        {
            // 화남 표시
            ActionManager.Instance.ExecuteWithDelay(() =>
            {
                _director.Move(EDestination.SeatDirector, ELookAt.Player, () =>
                {
                    // 3초 후 부장이 주먹밥을 집어던짐
                    ActionManager.Instance.ExecuteWithDelay(() =>
                    {
                        _director.Tell(EScript.NoRice, () =>
                        {
                            _me.Tell(EScript.Sorry, () =>
                            {
                                ToNextFlow();
                            });
                        });
                    }, 1.5f);
                });
            }, 1f);
        }, 1.5f);
    }
    
    private void SelectAgain()
    {
        _me.Tell(EScript.SelectAgain, () =>
        {
            _particleSelection.SetActive(true);
            ToNextFlow();
        });
    }

    private void SelectCandy()
    {
        if(!CanGoNextFlow)
        {
            _isOnFlow = false;
            return;
        }

        _me.Tell(EScript.Which2, () =>
        {
            ToNextFlow();
        });
    }
    
    private void GiveCandy()
    {
        if(!CanGoNextFlow || _player.IsGrabbing)
        {
            _isOnFlow = false;
            return;
        }

        // 기뻐함
        _director.Tell(EScript.Happy, () =>
        {
            _director.Tell(EScript.Finish, () =>
            {
                _me.Tell(EScript.Ok, () =>
                {
                    _particleReport.SetActive(true);
                    ToNextFlow();
                });
            });
        });
    }
    
    private void RearrangeReport1()
    {
        if(!CanGoNextFlow)
        {
            _isOnFlow = false;
            return;
        }

        ActionManager.Instance.ExecuteWithDelay(() =>
        {
            UIManager.Instance.GetDialog(EDialog.Report).Show();

            ActionManager.Instance.ExecuteWithDelay(() =>
            {
                _me.Tell(EScript.Order, () =>
                {
                    ToNextFlow();
                });
            }, 1f);
        }, 3f);
    }

    private void GiveReport1()
    {
        if(!CanGoNextFlow || _player.IsGrabbing)
        {
            _isOnFlow = false;
            return;
        }

        ActionManager.Instance.ExecuteWithDelay(() =>
        {
            _director.Tell(EScript.Wtf, () =>
            {
                // 부장이 보고서를 던짐
                _particleReport.SetActive(true);
                ToNextFlow();
            });
        }, 2f);
    }
    
    private void RearrangeReport2()
    {
        if(!CanGoNextFlow)
        {
            _isOnFlow = false;
            return;
        }

        ActionManager.Instance.ExecuteWithDelay(() =>
        {
            UIManager.Instance.GetDialog(EDialog.Report).Show();

            ActionManager.Instance.ExecuteWithDelay(() =>
            {
                _me.Tell(EScript.Order, () =>
                {
                    ToNextFlow();
                });
            }, 1f);
        }, 3f);
    }

    private void GiveReport2()
    {
        if(!CanGoNextFlow || _player.IsGrabbing)
        {
            _isOnFlow = false;
            return;
        }

        ActionManager.Instance.ExecuteWithDelay(() =>
        {
            _director.Tell(EScript.Good, () =>
            {
                ToNextFlow();
            });
        }, 2f);
    }

    private void Finish()
    {

    }

    private void ToNextFlow()
    {
        Flow++;
        _isOnFlow = false;
        CanGoNextFlow = false;
    }

    private void OnEnterSelectionBox(Collider collider)
    {
        if(!collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            return;
        }

        if(!_particleSelection.activeSelf)
        {
            return;
        }

        if(Flow == EFlow.ComeIn || Flow == EFlow.SelectRiceBall || Flow == EFlow.SelectCandy)
        {
            _particleSelection.SetActive(false);
            CanGoNextFlow = true;
        }
    }

    private void OnEnterReportBox(Collider collider)
    {
        if(!collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            return;
        }

        if(!_particleReport.activeSelf)
        {
            return;
        }

        if(Flow == EFlow.RearrangeReport1 || Flow == EFlow.RearrangeReport2)
        {
            _particleReport.SetActive(false);
            CanGoNextFlow = true;
        }
    }
}

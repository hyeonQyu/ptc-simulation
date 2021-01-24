using Nextwin.Client.Util;
using UnityEngine;

public enum EFlow
{
    Call,
    Situation,
    Hint,
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
    private DirectorNpc _director;
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

    [SerializeField]
    private Transform _reportObject;
    [SerializeField]
    private GameObject[] _reports;
    private Vector3[] _reportPositions;

    private int _frame;

    private void Start()
    {
        _player = _me.gameObject.GetComponent<MidasianController>();

        SetParticles();
        AddCollisionCheckers();
        InitReportsPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if(_frame < 100)
        {
            _frame++;
            return;
        }

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
        _leaders[0].Move(EDestination.LobbyLeader1, ELookAt.Entrance);
        _leaders[1].Move(EDestination.LobbyLeader2, ELookAt.LobbyLeader1, () =>
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
                                _leaders[0].Move(EDestination.SeatLeader1, ELookAt.SeatLeader2);
                                _leaders[1].Move(EDestination.SeatLeader2, ELookAt.SeatLeader1);

                                _me.Tell(EScript.Omg, () =>
                                {
                                    ToNextFlow();
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    private void Hint()
    {
        _member.Move(EDestination.Player, ELookAt.Player, () =>
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

                        _director.Move(EDestination.SeatDirector, ELookAt.ComputerDirector);
                    });
                });
            });
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

        ActionManager.Instance.ExecuteWithDelay(() =>
        {
            ActionManager.Instance.ExecuteWithDelay(() =>
            {
                _director.Move(EDestination.SeatDirector, ELookAt.Player, () =>
                {
                    _director.ThrowItem();

                    ActionManager.Instance.ExecuteWithDelay(() =>
                    {
                        _director.Tell(EScript.NoRice, () =>
                        {
                            _director.Move(EDestination.SeatDirector, ELookAt.ComputerDirector);

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
            UIDialogSelection uiDialogSelection = UIManager.Instance.GetDialog(EDialog.Selection) as UIDialogSelection;
            uiDialogSelection.RemoveRiceBallButton();
            uiDialogSelection.Show();
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

        _director.Move(EDestination.SeatDirector, ELookAt.Player, () =>
        {
            _director.Tell(EScript.Happy, () =>
            {
                GameObject.Find("Candy").SetActive(false);
                _director.Tell(EScript.Finish, () =>
                {
                    _director.Move(EDestination.SeatDirector, ELookAt.ComputerDirector);

                    _me.Tell(EScript.Ok, () =>
                    {
                        _particleReport.SetActive(true);
                        ToNextFlow();
                    });
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
            _director.Move(EDestination.SeatDirector, ELookAt.Player, () =>
            {
                _director.Tell(EScript.Wtf, () =>
                {
                    _director.ThrowItems();
                    _particleReport.SetActive(true);

                    ActionManager.Instance.ExecuteWithDelay(() =>
                    {
                        _director.Move(EDestination.SeatDirector, ELookAt.ComputerDirector);
                    }, 2f);
                    ToNextFlow();
                });
            });
        }, 4f);
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
            _director.Move(EDestination.SeatDirector, ELookAt.Player, () =>
            {
                for(int i = 0; i < _reports.Length; i++)
                {
                    _reports[i].SetActive(false);
                }

                _director.Tell(EScript.Good, () =>
                {
                    ToNextFlow();
                });
            });
        }, 2f);
    }

    private void Finish()
    {
        AudioManager.Instance.PlayAudio(EAudioClip.Success, EAudioSource.Paper1);
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

        if(Flow == EFlow.SelectRiceBall || Flow == EFlow.SelectCandy)
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
            SpawnReports();
            _particleReport.SetActive(false);
            CanGoNextFlow = true;
        }
    }

    private void SpawnReports()
    {
        for(int i = 0; i < _reports.Length; i++)
        {
            _reports[i].transform.parent = _reportObject;
            _reports[i].transform.localRotation = Quaternion.Euler(0, 90, 0);
            _reports[i].transform.position = _reportPositions[i];
        }
    }

    private void SetParticles()
    {
        _particleSelection = _selectionBox.transform.GetChild(0).gameObject;
        _particleSelection.SetActive(false);

        _particleReport = _reportBox.transform.GetChild(0).gameObject;
        _particleReport.SetActive(false);
    }

    private void AddCollisionCheckers()
    {
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

    private void InitReportsPositions()
    {
        _reportPositions = new Vector3[_reports.Length];

        for(int i = 0; i < _reports.Length; i++)
        {
            _reportPositions[i] = _reports[i].transform.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Prefabs.Actors.Guard;
using Assets.Scripts.Prefabs.Actors.Player;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    public class RoomManager : MonoBehaviour {
        static GameObject[] _guards;
        public static GameObject Player { get; set; }

        public static Dictionary<string, bool> ItemsAquired { get; set; }
        public string RoomName;

        void Start()
        {
            _guards = GameObject.FindGameObjectsWithTag("Guard");
            Player = GameObject.FindGameObjectWithTag("PlayerObject");
        }

        public void SetGlobalAlert()
        {
            if (_guards != null)
            {
                foreach (GameObject guard in _guards)
                {
                    var guardController = guard.GetComponent<Guard>();
                    if (!guardController.State.Equals(GuardState.Alerted))
                        guardController.SetAlert(Player.transform.position);

                }
            }

        }

        public void CheckForOtherGuardsState()
        {

            if (AnyGuardSeesPlayer())
            {
                SetGlobalAlert();
            }
            else
            {
                SetGlobalSuspicious();
            }

        }
        bool AnyGuardSeesPlayer()
        {
            if (_guards != null)
            {
                foreach (GameObject guard in _guards)
                {
                    var guardController = guard.GetComponent<Guard>();
                    if (guardController.FieldOfView.PlayerInRange)
                    {
                        return true;

                    }
                }
            }
            return false;
        }



        public void SetGlobalSuspicious()
        {
            StartCoroutine("Evasion");
            if (_guards != null)
            {
                foreach (GameObject guard in _guards)
                {
                    var guardController = guard.GetComponent<Guard>();
                    if (!guardController.State.Equals(GuardState.Suspicious))
                        guardController.SetSuspicious(Player.transform.position, true);

                }
            }
        }
        public void SetGlobalCalm()
        {
            if (_guards != null)
            {
                foreach (GameObject guard in _guards)
                {
                    var guardController = guard.GetComponent<Guard>();
                    if (!guardController.State.Equals(GuardState.Calm))
                        guardController.SetCalm(guardController.NavPoint != null ? guardController.NavPoint.transform.position
                            : guardController.transform.position);

                }
            }
        }
        IEnumerator Evasion()
        {
            yield return new WaitForSeconds(5f);
            if (AnyGuardSeesPlayer())
            {
                SetGlobalAlert();

            }
            else
            {
                SetGlobalCalm();
            }
        }

        public SaveDataDto GetRoomData()
        {
            var dto = new SaveDataDto();
            dto.RoomName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            dto.PlayerPosition = Player.transform.position;
            dto.Items = Player.GetComponent<Player>().Keys;
            return dto;
        }

        public void SetRoomData(SaveDataDto dto)
        {
            Player.transform.position = dto.PlayerPosition;
            Player.GetComponent<Player>().Keys = dto.Items;
        }
    }
}

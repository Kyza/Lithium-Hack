using RemoteAdmin;
using UnityEngine;
using System.Collections.Generic;
using Dissonance;
using System;
using System.Linq;
using UnityEngine.Networking;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dissonance.Integrations.UNet_HLAPI;
//using Dissonance.Integrations.UNet_HLAPI;


namespace Cheat
{
    public class Cheat : NetworkBehaviour
    {
        public Dictionary<string, KeyCode> CheatKeys = new Dictionary<string, KeyCode>
        {
            ["SpeedHack"] = KeyCode.B,
            ["Trace"] = KeyCode.LeftAlt,
            ["Noclip"] = KeyCode.V,
            ["AntiDoor"] = KeyCode.Z,
            ["HideMenu"] = KeyCode.I,
            ["ListenAll"] = KeyCode.L,
            ["Headless"] = KeyCode.H,
            ["SCPMode"] = KeyCode.End,
            ["LocationESP"] = KeyCode.Insert,
            ["PlayerESP"] = KeyCode.Delete,
            ["ItemsESP"] = KeyCode.Home,
            ["WeaponsPro"] = KeyCode.F1,
            ["Electrician"] = KeyCode.F2,
            ["Eject"] = KeyCode.F10,
            ["OpenAllDoors"] = KeyCode.F8,
            ["MicroAim"] = KeyCode.U,
            ["GlobalModWarning"] = KeyCode.X,
            ["AmmoMagnet"] = KeyCode.O,
            ["Stomp"] = KeyCode.C,
            ["Aimbot"] = KeyCode.F,
            ["ZAP"] = KeyCode.Y

        };
        private static PlyMovementSync PlayerMovement;

        private static string security = "";

        private static string error = "";
        static private void OpenAllDoors ()
        {
            PlyMovementSync pms = PlayerManager.localPlayer.GetComponent<PlyMovementSync>();
            var interact = PlayerManager.localPlayer.GetComponent<PlayerInteract>();
            var doors = UnityEngine.Object.FindObjectsOfType<Door>();
            if (PlayerManager.localPlayer.GetComponent<CharacterClassManager>().curClass == -1)
            {
                error = " ";
                if (CheatsOn["OpenAllDoors"])
                {
                    foreach (Door door2 in doors)
                    {
                        if (!door2.isOpen && (door2.permissionLevel == string.Empty))
                        {
                            try
                            {
                                pms.CallCmdSyncData(0, new Vector3(door2.transform.position.x, door2.transform.position.y + 1, door2.transform.position.z -2), 0);
                                security = door2.permissionLevel;
                                interact.CallCmdOpenDoor(door2.gameObject);
                            }
                            catch { }
                        }
                    }
                }
            }
            else
            {
                error = "You need to be waiting for players to do this";
            }
            

        } 
        public static Dictionary<string, bool> CheatsOn = new Dictionary<string, bool>
        {
            
        };
        public static Dictionary<string, Action> Cheats = new Dictionary<string, Action>
        {
            ["SpeedHack"] = SpeedHack,
            ["Trace"] = Trace,
            ["Noclip"] = Noclip,
            ["AntiDoor"] = AntiDoor,
            ["HideMenu"] = HideMenu,
            ["ListenAll"] = ListenAll,
            ["Headless"] = Headless,
            ["SCPMode"] = SCPMode,
            ["LocationESP"] = LocationESP, //
            ["PlayerESP"] = PlayerESP,
            ["ItemsESP"] = ItemsESP,
            ["WeaponsPro"] = WeaponsPro, //
            ["Electrician"] = Electrician,
            ["Eject"] = Eject, 
            ["OpenAllDoors"] = OpenAllDoors,
            ["MicroAim"] = MicroAim,
            ["GlobalModWarning"] = GlobalModWarning,
            ["AmmoMagnet"] = AmmoMagnet,
            ["Stomp"] = Stomp,
            ["Aimbot"] = Aimbot,
            ["ZAP"] = ZAP


        };

        private static bool AimbotCheck(GameObject target, string hitboxType, Vector3 dir, Vector3 sourcePos, Vector3 targetPos)
        {
            var ccm = target.GetComponent<CharacterClassManager>();
            var mywm = PlayerManager.localPlayer.GetComponent<WeaponManager>();
            if (Math.Abs(Camera.main.transform.position.y - ccm.transform.position.y) > 40f) return false;
            if (!(ccm != null) || !mywm.GetShootPermission(ccm, false)) return false;
            if (Math.Abs(Camera.main.transform.position.y - ccm.transform.position.y) > 40f) return false;
            if (Vector3.Distance(Camera.main.transform.position, sourcePos) > 6.5f) return false;
            if (Vector3.Distance(ccm.transform.position, targetPos) > 6.5f) return false;
            if (Physics.Linecast(sourcePos, targetPos, mywm.raycastServerMask)) return false;
            return true;
        }

        private static void Aimbot()
        {
            foreach (GameObject enemy in PlayerManager.singleton.players)
            {
;
                if ((enemy!= PlayerManager.localPlayer) && (Vector3.Angle(Camera.main.transform.forward, enemy.transform.position - Camera.main.transform.position) < 15)) 
                {
                    Camera.main.transform.LookAt(enemy.transform.position);
                    var range = Camera.main.transform.forward * 5.4f;
                    var mypos = Camera.main.transform.position;
                    var enemypos = enemy.transform.position;
                    if (AimbotCheck(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, new Vector3(mypos.x, mypos.y - 6, mypos.z), new Vector3(enemypos.x, enemypos.y-6, enemypos.z)))
                    {
                        PlayerManager.localPlayer.GetComponent<WeaponManager>().CallCmdShoot(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, new Vector3(mypos.x, mypos.y - 6, mypos.z), new Vector3(enemypos.x, enemypos.y - 6, enemypos.z));
                    }
                    if (AimbotCheck(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position + range, enemy.transform.position - range))
                    {
                        PlayerManager.localPlayer.GetComponent<WeaponManager>().CallCmdShoot(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position + range, enemy.transform.position - range);
                    }
                    else if (AimbotCheck(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position, enemy.transform.position - range))
                    {
                        PlayerManager.localPlayer.GetComponent<WeaponManager>().CallCmdShoot(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position, enemy.transform.position - range);
                    }
                    else if (AimbotCheck(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position + range, enemy.transform.position))
                    {
                        PlayerManager.localPlayer.GetComponent<WeaponManager>().CallCmdShoot(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position + range, enemy.transform.position);
                    }
                    else if (AimbotCheck(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position, enemy.transform.position))
                    {
                        PlayerManager.localPlayer.GetComponent<WeaponManager>().CallCmdShoot(enemy, "HEAD", enemy.transform.position - Camera.main.transform.position, PlayerManager.localPlayer.transform.position, enemy.transform.position);
                    }
                }
            }
        }
        private static bool _isGlobalMod (string steamid)
        {
            switch (steamid)
            {
                case "76561198056374428": //4rae
                case "76561198139232244": //TheRealBimo
                case "76561198071934271": //zabszk
                case "76561197988477565": //maverick
                case "76561198116439744": //shingeni
                case "76561198067357008": //Takail
                case "76561198828758576": // gravegravegrave
                case "76561198068408917": //KingCobra70
                case "76561198187711341": //mozeman
                case "76561198328956236": //MrRunt
                case "76561198011844757": //jordanlol633
                case "76561198194513838": //peperownik
                case "76561198163284469": //Phoenix--Pronject
                case "76561198137216316": //rahveeohleee
                case "76561198349022866": //rintheweeb
                case "76561198049556261": //RealRomlyn
                case "76561198026973262": //WGSHark
                case "76561198040425824": //ICANHASNUKES
                case "76561198116188982": //AgentBlackout
                case "76561197963326920": //WinterBeyond
                case "76561198035190456": //hlchan
                case "76561198118254410": //androxanik
                case "76561198101074567": //Blizzard098
                case "76561198135154430": //BlueTheKing
                case "76561198078737562": //Dankrushen
                case "76561198113143090": //DjNathann
                case "76561198204575388": //ericthe1234
                case "76561198047770015": //erykol
                case "76561198049611738": //ilysen
                case "76561198019213377": //EvanGames
                case "76561198098290255": //ffian
                case "76561198010095857": //iD4NG3Rs
                case "76561198078443796": //InsaneRed?
                case "76561198093809845": //Keygano
                case "76561198082888468": //klaepek
                case "76561198071607345": //LordOfKhaos
                case "76561198289651857": //joseph_the_electrician
                case "76561198184948159": //MultiverseUncle
                case "76561198041787473": //191123
                case "76561198276465358": //Sinon1
                case "76561198170429887": //TheeRider
                case "76561198059219967": //TheHyde
                case "76561198269333290": //Tr00n11x
                case "76561198074568861": //Voidus
                case "76561198076837733": //wavepoole
                case "76561198219259740": //xEnded_
                case "76561198046639503": //nicku?
                case "76561198071735320": //aHarmlessSpoon?
                case "76561198091199713": //SirMeepington
                case "76561198202123521": //killer_1001
                    return true;
                default:
                    return false;
            }
        }
        static private void GlobalModWarning ()
        {
            int Rect2Order = 0;
            GUI.color = Color.red;
           // if (PlayerList.anyAdminOnServer)
          //  {
            //    GUI.Label(new Rect(Screen.width - 300, RectOrder * 20 + 10, 500, 30), "Admin is on");
           //     RectOrder++;
          //  }
            
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                ServerRoles SR = player.GetComponent<ServerRoles>();
                NicknameSync nickname = player.GetComponent<NicknameSync>();
                PermissionsHandler pm = player.GetComponent<PermissionsHandler>();
                CharacterClassManager ccm = player.GetComponent<CharacterClassManager>();
                SteamManager SM = player.GetComponent<SteamManager>();


                if (_isGlobalMod(ccm.SteamId))
                {
                    WarningMessage = nickname.myNick + " is a global moderator !!";
                }


                
                try
                {
                    if (pm.ManagersAccess) GUI.Label(new Rect(Screen.width - 300, Rect2Order * 20, 500, 30), nickname.myNick + " has global Managers Access");
                }
                catch { }

                

                try
                {
                    if (SR.Permissions != 0)
                    {
                        GUI.Label(new Rect(Screen.width - 300, Rect2Order * 20, 500, 30), nickname.myNick + " has permission number " + SR.Permissions);
                        Rect2Order++;
                    }
                }
                catch { }



                if (SR.RaEverywhere)
                {
                    GUI.Label(new Rect(Screen.width - 300, Rect2Order * 20, 500, 30), nickname.myNick + " is global admin");
                    Rect2Order++;
                }
                if (SR.Staff)
                {
                    GUI.Label(new Rect(Screen.width - 300, Rect2Order * 20, 500, 30), nickname.myNick + " is staff");
                    Rect2Order++;
                }
               
                if ((bool)SR.GetType().GetField("OverwatchEnabled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(SR)) GUI.Label(new Rect(Screen.width - 300, Rect2Order * 20, 500, 30), nickname.myNick + " has entered Overwatch Mode!!");

            }
        }
        private static Pickup current;
        private static float now = Time.time;
        static private void AmmoMagnet ()
        {
            foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
            {
                if((item.info.itemId == 28 || item.info.itemId == 29 || item.info.itemId == 22) && (Vector3.Distance(PlayerManager.localPlayer.transform.position, item.transform.position) < 3.5f)) 
                {
                    if (current == null || ((Time.time - now) > 1))
                    {
                        PlayerManager.localPlayer.GetComponent<Searching>().CallCmdStartPickup(item.gameObject);
                        current = item;
                        now = Time.time;
                    }
                    
                    if (current != null) PlayerManager.localPlayer.GetComponent<Searching>().CallCmdPickupItem(current.gameObject);
                }
            }
        }
        static private void Stomp ()
        {
            PlayerManager.localPlayer.GetComponent<FootstepSync>().CallCmdSyncFoot(true);
        }
        static private void ZAP ()
        {
            //  foreach (GameObject thing in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
            //  {
            //      PlayerManager.localPlayer.GetComponent<Scp173PlayerScript>().CallCmdHurtPlayer(thing);
            // }
            //    PlayerManager.localPlayer.GetComponent<Radio>().CallCmdUpdatePreset(999999999);
            PlayerManager.localPlayer.GetComponent<CharacterClassManager>().CallCmdConfirmDisconnect();
            //var death = new PlayerStats.HitInfo(12, "kkk", DamageTypes.RagdollLess, 12);
            //PlayerManager.localPlayer.GetComponent<RagdollManager>().SpawnRagdoll(PlayerManager.localPlayer.transform.position, PlayerManager.localPlayer.transform.rotation, 1, death , true, "123", "nigger", 13);
        }
        static private void MicroAim()
        {
            foreach (GameObject player in PlayerManager.singleton.players)
            {
                if (player != PlayerManager.localPlayer) PlayerManager.localPlayer.GetComponent<MicroHID_GFX>().CallCmdHurtPlayersInRange(player);
            }
        }

        static private Dictionary<int, Tuple<float, float, float, float, float, float, float>> DefaultWeapons = new Dictionary<int, Tuple<float, float, float, float, float, float, float>>
        {
            [0] = new Tuple<float, float, float, float, float, float, float>(6f, 5f, 1.8f, 1.3f, 1f, 0.12f, 0.13f),
            [1] = new Tuple<float, float, float, float, float, float, float>(10f, 5f, 0.5f, 1.5f, 1f, 0.1f, 0.11f),
            [2] = new Tuple<float, float, float, float, float, float, float>(8f, 5f, 0.2f, 1.5f, 1f, 0.1f, 0.11f),
            [3] = new Tuple<float, float, float, float, float, float, float>(10f, 5f, 0f, 1.5f, 1f, 0.1f, 0.11f),
            [4] = new Tuple<float, float, float, float, float, float, float>(11f, 10f, 0.7f, 1.5f, 1.5f, 0.08f, 0.13f),
            [5] = new Tuple<float, float, float, float, float, float, float>(5f, 10f, 2.5f, 1.5f, 1.5f, 0.15f, 0.15f)
        };
        static private void WeaponsPro ()
        {
            WeaponManager MyWM = PlayerManager.localPlayer.GetComponent<WeaponManager>();
            if (CheatsOn["WeaponsPro"])
            {
                for(int i = 0; i < MyWM.weapons.Length; i++)
                {
                    MyWM.weapons[i].shotsPerSecond = 30;
                    MyWM.weapons[i].unfocusedSpread = 0f;
                    MyWM.weapons[i].recoil.fovKick = 0;
                    MyWM.weapons[i].recoil.backSpeed = 0;
                    MyWM.weapons[i].recoil.lerpSpeed = 0;
                    MyWM.weapons[i].recoil.shockSize = 0;
                    MyWM.weapons[i].recoil.upSize = 0;
                }
            }
            else
            {
                for (int i = 0; i < MyWM.weapons.Length; i++)
                {
                    MyWM.weapons[i].shotsPerSecond = DefaultWeapons[i].Item1;
                    MyWM.weapons[i].unfocusedSpread = DefaultWeapons[i].Item2;
                    MyWM.weapons[i].recoil.fovKick = DefaultWeapons[i].Item3;
                    MyWM.weapons[i].recoil.backSpeed = DefaultWeapons[i].Item4;
                    MyWM.weapons[i].recoil.lerpSpeed = DefaultWeapons[i].Item5;
                    MyWM.weapons[i].recoil.shockSize = DefaultWeapons[i].Item6;
                    MyWM.weapons[i].recoil.upSize = DefaultWeapons[i].Item7;
                }
            }
        }
        private static void DisplayLocation(string name, Vector3 thePosition, Color colour)
        {
            var pos2d = Camera.main.WorldToScreenPoint(thePosition);
            GUI.color = colour;
            if (!(pos2d.z > 0f)) return;
            int distance = (int)Vector3.Distance(Camera.main.transform.position, thePosition);
            var textDimensions = GUI.skin.box.CalcSize(new GUIContent(name + distance.ToString() + " [m]"));
            GUI.Box(new Rect(pos2d.x - 20f, Screen.height - pos2d.y - 20f, textDimensions.x, textDimensions.y), name + " [" + distance + "m]");
        }
        private static void SpeedHack()
        {
            var ccm = PlayerManager.localPlayer.GetComponent<CharacterClassManager>();
            var fpc = PlayerManager.localPlayer.GetComponent<FirstPersonController>();
            if (CheatsOn["SpeedHack"])
            {

                fpc.m_RunSpeed = ccm.klasy[ccm.curClass].runSpeed * 1.21f;
                fpc.m_JumpSpeed = ccm.klasy[ccm.curClass].runSpeed * 1.32f;
                fpc.m_WalkSpeed = ccm.klasy[ccm.curClass].runSpeed * 1.30f;
            }
            else
            {
                fpc.m_RunSpeed = ccm.klasy[ccm.curClass].runSpeed;
                fpc.m_JumpSpeed = ccm.klasy[ccm.curClass].jumpSpeed;
                fpc.m_WalkSpeed = ccm.klasy[ccm.curClass].walkSpeed;
            }


        }
        private static void Trace()
        {
            if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out var hit))
            {
                GUI.color = Color.cyan;
                GUI.Label(new Rect(Screen.width / 2f, Screen.height / 2f, 500, 150),
                    "Tag:" + hit.transform.gameObject.tag + " and name is: " + hit.transform.gameObject.name);
                var firstParent = hit.transform.gameObject.transform.parent;
                if (firstParent)
                    GUI.Label(new Rect(Screen.width / 2f, Screen.height / 2f + 50, 500, 150),
                        "parent tag & name & type: " + firstParent.parent.tag + " & " + firstParent.name + " & " +
                        firstParent.GetType());
            }
        }
        private static void Noclip()
        {
            Vector3 MyPos = Vector3.zero; 
            if (Input.GetKey(KeyCode.W)) {
                MyPos += Camera.main.transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                MyPos -= Camera.main.transform.forward;
            }
            if (Input.GetKey(KeyCode.D))
            {
                MyPos += Camera.main.transform.right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                MyPos -= Camera.main.transform.right;
            }
            var speed = 5f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 9f;
            }
            PlayerManager.localPlayer.transform.position += MyPos.normalized * speed * Time.deltaTime;

            if (Input.GetKey(KeyCode.Space))
            {
                PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 5f;
                if (Input.GetKey(KeyCode.LeftShift)){
                    PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 400f;
                }               
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 5f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 400f;
                }
            }
            var playerMovement = PlayerManager.localPlayer.GetComponent<PlyMovementSync>();
            if (playerMovement.enabled)
            {
                playerMovement.enabled = false;
                playerMovement.GetType().GetMethod("TransmitData", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(playerMovement, new object[0]);
                realPosition = PlayerManager.localPlayer.transform.position;
            }

            PlyMovementSync component3 = PlayerManager.localPlayer.GetComponent<PlyMovementSync>();
            component3.CallCmdSyncData(PlayerManager.localPlayer.transform.rotation.eulerAngles.y, PlayerManager.localPlayer.transform.position, PlayerManager.localPlayer.transform.rotation.x);
        } 
        private static Vector3 realPosition;
        private static List<Door> ClosedDoors = new List<Door>();
        private static void AntiDoor()
        {
            foreach (Door door2 in ClosedDoors)
            {
                if (door2 != null)
                {
                    float distance = Vector3.Distance(PlayerManager.localPlayer.transform.position, door2.transform.position);
                    if (distance >= 4f)
                    {
                        door2.gameObject.SetActive(true);
                      //  door2.isOpen = false;
                    }
                }
            }
            if (CheatsOn["AntiDoor"])
            {
                foreach (Door door2 in UnityEngine.Object.FindObjectsOfType<Door>())
                {
                    float distance = Vector3.Distance(PlayerManager.localPlayer.transform.position, door2.transform.position);
                    if (distance < 4f)
                    {
                        ClosedDoors.Add(door2);
                        door2.gameObject.SetActive(false);
                        
                    }
                }
      
            }
            else
            {
                foreach (Door door2 in ClosedDoors)
                {
                    if (door2 != null) door2.gameObject.SetActive(true);
                    //door2.isOpen = false;
                }
                ClosedDoors.Clear();
            }
        }
        private static void HideMenu()
        {

        }
        private static void ListenAll()
        {
            Memory.SetRadio(!Memory._bAllRadio);
        }
        private static void Headless()
        {
            PlyMovementSync movesync = PlayerManager.localPlayer.GetComponent<PlyMovementSync>();
            movesync.CallCmdSyncData(PlayerManager.localPlayer.transform.rotation.eulerAngles.y, PlayerManager.localPlayer.transform.position, -590);
            //FACE DIRECTION???
        }



        private static void SCPMode()
        {
            var ccm = PlayerManager.localPlayer.GetComponent<CharacterClassManager>();
            Scp173PlayerScript peanut = ccm.GetComponent<Scp173PlayerScript>();
            Scp049PlayerScript doctor = ccm.GetComponent<Scp049PlayerScript>();
            Scp049_2PlayerScript zombie = ccm.GetComponent<Scp049_2PlayerScript>();
            Scp939PlayerScript dogo = ccm.GetComponent<Scp939PlayerScript>();
            Scp096PlayerScript shyguy = ccm.GetComponent<Scp096PlayerScript>();
            Scp106PlayerScript larry = ccm.GetComponent<Scp106PlayerScript>();
            Scp079PlayerScript computer = ccm.GetComponent<Scp079PlayerScript>();


            if (peanut.iAm173)
            {
                foreach (GameObject player in PlayerManager.singleton.players)
                {
                    if (Vector3.Distance(PlayerManager.localPlayer.transform.position, player.transform.position) < 4f + peanut.boost_teleportDistance.Evaluate(PlayerManager.localPlayer.GetComponent<PlayerStats>().GetHealthPercent())) 
                    {
                        peanut.GetType().GetMethod("HurtPlayer", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(peanut, new object[]
                        {
                            player,
                            "123"
                        });
                    }
                }
            }
            if (doctor.iAm049)
            {
                foreach (GameObject player in PlayerManager.singleton.players)
                {
                    if ((player != PlayerManager.localPlayer) && (Vector3.Distance(PlayerManager.localPlayer.transform.position, player.transform.position) < 3.5f))
                    {
                        doctor.GetType().GetMethod("InfectPlayer", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(doctor, new object[]
                                        {
                                            player,
                                            "123"
                                        });
                    }
                }
            }
            if (zombie.iAm049_2)
            {
                foreach (GameObject player in PlayerManager.singleton.players)
                {
                    if ((player!= PlayerManager.localPlayer) && (Vector3.Distance(PlayerManager.localPlayer.transform.position, player.transform.position) < 4f)) zombie.CallCmdHurtPlayer(player, "123");
                }
            }
            if (dogo.iAm939)
            {
                foreach (GameObject player in PlayerManager.singleton.players)
                {
                   // ccm.klasy[player.GetComponent<CharacterClassManager>().curClass].team != Team.SCP
                    if ((player != PlayerManager.localPlayer) && (Vector3.Distance(Camera.main.transform.position, player.transform.position) < 1.3 * dogo.attackDistance))
                    {
                        dogo.CallCmdShoot(player);
                    }

                }
            }
            if (shyguy.iAm096)
            {
                foreach (GameObject player in PlayerManager.singleton.players)
                {
                    if ((Vector3.Distance(Camera.main.transform.position, player.transform.position) < 3.5f))
                    shyguy.GetType().GetMethod("CallCmdHurtPlayer", BindingFlags.Instance | BindingFlags.Public).Invoke(shyguy, new object[]
                    {
                        player

                    });
                }
            }
            if (larry.iAm106)
            {
                foreach (GameObject player in PlayerManager.singleton.players)
                {
                    if (Vector3.Distance(Camera.main.transform.position, player.transform.position) < 3.5f)
                    {
                        larry.CallCmdMovePlayer(player, ServerTime.time);
                    }
                }
            }
            if (computer.iAm079)
            {

            }
        }
        private static List<Tuple<string, Vector3, Color>> LocationsToRender = new List<Tuple<string, Vector3, Color>>();
        private static void LocationESP()
        {

            Color LocationColour = new Color(1f, 0.65f, 1f, 0.6f);
            LocationsToRender.Clear();
            if (CheatsOn["LocationESP"])
            {
                foreach (TeslaGate teslaGate in UnityEngine.Object.FindObjectsOfType<TeslaGate>())
                {
                    LocationsToRender.Add(new Tuple<string, Vector3, Color>("Tesla Gate", teslaGate.transform.position, LocationColour));
                }
                foreach (PocketDimensionTeleport exit in UnityEngine.Object.FindObjectsOfType<PocketDimensionTeleport>())
                {
                    if (exit.GetTeleportType() == PocketDimensionTeleport.PDTeleportType.Exit) LocationsToRender.Add(new Tuple<string, Vector3, Color>("Exit (probably)", exit.transform.position, LocationColour));
                }
                foreach (Generator079 generator in UnityEngine.Object.FindObjectsOfType<Generator079>())
                {
                    LocationsToRender.Add(new Tuple<string, Vector3, Color>("Computer Generator", generator.transform.position, LocationColour));
                }
                foreach (GameObject elivator in UnityEngine.GameObject.FindGameObjectsWithTag("LiftTarget"))
                {
                    if (elivator.name.Contains("ElevatorChamber"))
                    {
                        LocationsToRender.Add(new Tuple<string, Vector3, Color>("Lift ", elivator.transform.position, LocationColour));
                    }
                }
                LocationsToRender.Add(new Tuple<string, Vector3, Color>("914", GameObject.FindGameObjectWithTag("914_use").transform.position, LocationColour));
                LocationsToRender.Add(new Tuple<string, Vector3, Color>("Intercom", UnityEngine.Object.FindObjectOfType<Intercom>().transform.position, LocationColour));
            }
        }
        private static List<Tuple<string, Vector3, Color>> PlayersToRender = new List<Tuple<string, Vector3, Color>>();
        private static void PlayerESP()
        {
            PlayersToRender.Clear();
            if (CheatsOn["PlayerESP"])
            {
                Color PlayerColor = Color.gray;
                foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    var nickname = player.transform.GetComponent<NicknameSync>();
                    var role = nickname.GetComponent<CharacterClassManager>().curClass;
                    if (role != -1)
                    {
                        Vector3 position = player.GetComponent<NetworkIdentity>().transform.position;
                        var rolename = nickname.GetComponent<CharacterClassManager>().klasy[role].fullName;
                        switch (role)
                        {
                            case 1:
                                PlayerColor = new Color(1f, 0.6f, 0f, 1f);
                                break;
                            case 15:
                                PlayerColor = Color.grey;
                                break;
                            case 0:
                            case 3:
                            case 5:
                            case 10:
                            case 9:
                            case 16:
                            case 17:
                            case 7:
                                PlayerColor = Color.red;
                                break;
                            case 14:
                                PlayerColor = new Color(1f, 1f, 0.7f, 1);
                                break;
                            case 8:
                                PlayerColor = Color.green;
                                break;
                            case 6:  // scientist
                                PlayerColor = Color.white;
                                break;
                            case 4:
                            case 11:
                            case 12:
                            case 13:
                                PlayerColor = Color.blue;
                                break;
                        }
                        var name = string.Format(nickname.myNick + "{0}" + rolename, Environment.NewLine);
                        PlayersToRender.Add(new Tuple<String, Vector3, Color>(name, new Vector3(position.x, position.y + 1, position.z), PlayerColor));
                       // PlayersToRender.Add(new Tuple<String, Vector3, Color>(" (" + rolename + ") ", new Vector3(position.x, position.y + 3, position.z), PlayerColor));
                    }
                }
            }
        }
        private static List<Tuple<string, Vector3, Color>> ItemsToRender = new List<Tuple<string, Vector3, Color>>();
        private static void ItemsESP()
        {
            ItemsToRender.Clear();
            if (CheatsOn["ItemsESP"])
            {
                Color ItemsColour = new Color(1f, 1f, 0.7f, 0.5f);
                foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
                {
                    float distance = Vector3.Distance(PlayerManager.localPlayer.transform.position, item.transform.position);
                    ItemsToRender.Add(new Tuple<string, Vector3, Color>(PlayerManager.localPlayer.GetComponent<Inventory>().availableItems[item.info.itemId].label, item.transform.position, ItemsColour));
                }
            }
        }
        private static Dictionary<WeaponManager.Weapon, float> Weapons = new Dictionary<WeaponManager.Weapon, float>();

        private static void Electrician()
        {
            if (CheatsOn["Electrician"])
            {
                foreach (TeslaGate Zapper in UnityEngine.Object.FindObjectsOfType<TeslaGate>())
                {

                    Zapper.sizeOfKiller = new Vector3(0,0,0);

                }
            }
            else
            {

                    foreach (TeslaGate Zapper in UnityEngine.Object.FindObjectsOfType<TeslaGate>())
                    {

                      //  Zapper.sizeOfKiller = new Vector3(1.7, 6.5, 1.7);

                    }

            }
        }
        private static void Eject()
        {
            Injector.Unload();
        }

        public static string directory = Environment.ExpandEnvironmentVariables("%USERPROFILE%/Desktop/Lithium/");
        private void Start()
        {
            Memory.Init();
            foreach (KeyValuePair<string, Action> entry in Cheats)
            {
                CheatsOn[entry.Key] = false;
            }

            List<string> CheatState = File.ReadAllLines(directory + "DefaultCheatState.txt").ToList();

            foreach (string line in CheatState)
            { 
                string[] values = line.Split('=');
                CheatsOn[values[0]] = bool.Parse(values[1]); //bool.Parse(values[1]);
            }

            List<string> KeyBindings = File.ReadAllLines(directory + "KeyBindings.txt").ToList();
            foreach (string line in KeyBindings)
            {
                string[] values = line.Split('=');
                CheatKeys[values[0]] = (KeyCode)System.Enum.Parse(typeof(KeyCode), values[1]); 
            }



            PlayerMovement = PlayerManager.localPlayer.GetComponent<PlyMovementSync>();
        }
        private static void ESPDisplay(string name, Vector3 thePosition, Color colour)
        {
            var pos2d = Camera.main.WorldToScreenPoint(thePosition);
            GUI.color = colour;
            if (!(pos2d.z > 0f)) return;
            GUI.Label(new Rect(pos2d.x - 20f, Screen.height - pos2d.y - 20f, pos2d.x + 40f, Screen.height - pos2d.y + 50f), name + " [" + (int)Vector3.Distance(Camera.main.transform.position, thePosition) + "m]");
        }


        public void FixedUpdate()
        {
            if (CheatsOn["ItemsESP"]) ItemsESP();
            if (CheatsOn["PlayerESP"]) Cheats["PlayerESP"]();
        }
        public void Update()
        {
            if (CheatsOn["ZAP"]) ZAP();
            //    PlayerManager.localPlayer.GetComponent<VoicePlayerState>().Volume = 999999999;
            //  foreach (var zapper in FindObjectsOfType<TeslaGate>())
            //   {
            //       WarningMessage += " " + zapper.transform.position.ToString() + " " + zapper.sizeOfKiller;
            //  }

                //WarningMessage = string.Format(WarningMessage + "{0}" + allweapons[i].shotsPerSecond + " " + allweapons[i].unfocusedSpread + " " + allweapons[i].recoil.fovKick + " " + allweapons[i].recoil.backSpeed + " " + allweapons[i].recoil.lerpSpeed + " " + allweapons[i].recoil.shockSize + " " + allweapons[i].recoil.upSize, Environment.NewLine);

 
              //  WarningMessage = string.Format(WarningMessage + "{0}", Environment.NewLine);

            

            PlayerManager.localPlayer.GetComponent<WeaponManager>().CallCmdSyncFlash(true);
            if (CheatsOn["Noclip"]) Noclip();
            if (CheatsOn["SCPMode"]) SCPMode();
            PlayerManager.localPlayer.GetComponent<CharacterController>().enabled = (!CheatsOn["Noclip"]);
            if (!PlayerManager.localPlayer.GetComponent<PlyMovementSync>().enabled && !CheatsOn["Noclip"])
            {
                base.transform.position = realPosition;
                PlayerManager.localPlayer.GetComponent<PlyMovementSync>().enabled = true;
            }
            if (CheatsOn["AmmoMagnet"]) AmmoMagnet();
            if (CheatsOn["OpenAllDoors"]) OpenAllDoors();
            if (CheatsOn["Headless"]) Headless();
            
            WeaponsPro();
            if (CheatsOn["Electrician"]) Electrician();
            if (CheatsOn["MicroAim"]) MicroAim();
            if (CheatsOn["AntiDoor"] && PlayerManager.localPlayer) AntiDoor();
            if (CheatsOn["Stomp"]) Stomp();
            if (CheatsOn["Aimbot"]) Aimbot();
            if (Input.GetKeyUp(CheatKeys["Aimbot"])) CheatsOn["Aimbot"] = false;
            foreach (KeyValuePair<string, KeyCode> entry in CheatKeys)
            {
                if (Input.GetKeyDown(entry.Value))
                {
                    CheatsOn[entry.Key] = !CheatsOn[entry.Key];
                    Cheats[entry.Key]();
                }
            }

                PlyMovementSync movesync = PlayerManager.localPlayer.GetComponent<PlyMovementSync>();
        }

        private static string WarningMessage = "> UR GAY <";
        public void OnGUI()
        {

            var toRender = LocationsToRender.Concat(ItemsToRender).Concat(PlayersToRender);
            foreach (Tuple<string, Vector3, Color> label in toRender)
            {
                DisplayLocation(label.Item1, label.Item2, label.Item3);
            }
            if (!CheatsOn["HideMenu"])
            {
                //GUI.Box(new Rect(pos2d.x - 20f, Screen.height - pos2d.y - 20f, 100, 20), name + " [" + (int)Vector3.Distance(Camera.main.transform.position, thePosition) + "m]");
                GUI.Box(new Rect(10f, 40f, 220f, 485f), "<color=orange><b> Lithium l33t_H4x5 v4.0</b></color>");
                GUI.Label(new Rect(40f, 60f, 220f, 445f), "<color=orange><b>http://discord.gg/64Awasb</b></color>");
                GUI.Label(new Rect(65f, 80f, 220f, 445f), "<color=orange><b>press f for aimbot</b></color>");
                GUI.Label(new Rect(15f, 100f, 500f, 30f), "<color=yellow><b>──────────────────────</b></color>");
                // GUI.Label(new Rect(65f, 80f, 220f, 445f), directory);

                var size = GUI.skin.box.CalcSize(new GUIContent(WarningMessage));
                GUI.Box(new Rect((float)(Screen.width / 2 - 145), 20f, size.x, 20 + size.y), "<color=#ff7063><b><size=15>" + WarningMessage + "</size></b></color>");
                // GUI.Label(new Rect(10, 35, 500, 30), "discord.gg/64Awasb");
                if (CheatsOn["OpenAllDoors"]) GUI.Label(new Rect(Screen.width / 2f - 50, 20 + Screen.height / 2f, 500, 150), error);
                if (CheatsOn["GlobalModWarning"]) GlobalModWarning();
                else WarningMessage = " ur safe ";
                if (CheatsOn["Trace"]) Trace();
                
   
                int RectOrder = 0;
                foreach (KeyValuePair<string, bool> entry in CheatsOn)
                {
                    RectOrder++;
                    GUI.color = new Color(1f, 1f, 1f, 1f);
                    GUI.Label(new Rect(15, RectOrder * 20 + 100, 500, 30), entry.Key + " <color=#42f4d1>[" + CheatKeys[entry.Key] + "]</color>  " + (entry.Value ? "<color=#8aff62>ON</color>" : "<color=#ff7063>OFF</color>"));
                }

            }
        }
    }
}
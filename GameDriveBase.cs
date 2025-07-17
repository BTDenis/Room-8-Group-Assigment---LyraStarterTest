using gdio.common.objects;
using gdio.unreal_api;
using NUnit.Framework;

namespace LyraTests
{
    public class GameDriveBase
    {

        public ApiClient _gameDriverAPI;

        /* 
         * These variables are used primarely in CharacterMovement.cs
         */
        public String characterObjectPath               = "/*[@name = 'B_Hero_ShooterMannequin_C_0']";
        public String gameDriverCommunicationPath       = "/*[@name = 'BP_GameDriverClassPawn_C_1']";
        public String characterPlayerControllerPath     = "/*[@name = 'LyraPlayerController_0']";
        public String playerCameraManagerPath           = "//LyraPlayerCameraManager_0";

        /* 
         * These variables are used primarely in PlayerCharacterAimAtTarget.cs
         * If you want to change the Spawned Target:
         *    FullTargetToSpawnPath:
         *          Hover over the item in the Unreal Engine Editor and copy the exact "Path" from the pop-up panel.
         *          Afterwards, to this path, add the name of the object again with a "_C" suffix.
         *          Format: "PATH_TO_OBJECT"    +   "."    +   "OBJECT_NAME"    +    "_C"
         *    ActorCreatedReference:
         *          This should have the following format, similar to the FullTargetToSpawnPath:
         *          Format: "/*[@name = 'OBJECT_NAME_C_0']"
         *    TargetLevelEditorName:
         *          Same as ActorCreatedReference, but without the "/*[@name = '']" part.
         *          Format: "//OBJECT_NAME_C_0"
         */
        public String FullTargetToSpawnPath             = "/ShooterCore/Item/B_CubeHat.B_CubeHat_C";
        public String TargetCreatedReference            = "/*[@name = 'B_CubeHat_C_0']";
        public String TargetObjectInEditorName          = "B_CubeHat_C_0";

        public String UI_PistolAmmo                     = "//W_AmmoCounter_Pistol_C_0";
        public String UI_Reticle                        = "//W_Reticle_Pistol_C_0";
        public String UI_Health                         = "//W_ShooterHUDLayout_C_0";
        public String UI_OptionsMenu                    = "/*[contains(@name,'W_LyraGameMenu_C')]/Overlay_0/BackgroundDim/VerticalBox_1/ButtonsBox/*[contains(@name,'OptionsButtons')]";
        public String UI_OptionsMenu_Open               = "/*[contains(@name,'W_LyraSettingScreen_C')]";
        public String UI_OptionsMenu_Back               = "//W_BoundActionButton_C_0"; 
        public String UI_OptionsMenu_Quit               = "//QuitButton_EditorOnly";
        public String UI_OptionsMenu_Gameplay           = "//W_LyraButtonTab_C_0";
        public String UI_OptionsMenu_Video              = "/*[contains(@name,'W_LyraSettingScreen_C')]/Overlay/MenuBorder/VerticalBox_0/SizeBox_0/HeaderBorder/TabSZ/*[contains(@name,'TopSettingsTabs')]/HorizontalBox_0/TabButtonBox/*[contains(@name,'W_LyraButtonTab_C')][1]";
        public String UI_OptionsMenu_Audio              = "/*[contains(@name,'W_LyraSettingScreen_C')]/Overlay/MenuBorder/VerticalBox_0/SizeBox_0/HeaderBorder/TabSZ/*[contains(@name,'TopSettingsTabs')]/HorizontalBox_0/TabButtonBox/*[contains(@name,'W_LyraButtonTab_C')][2]";
        public String UI_OptionsMenu_MKb                = "/*[contains(@name,'W_LyraSettingScreen_C')]/Overlay/MenuBorder/VerticalBox_0/SizeBox_0/HeaderBorder/TabSZ/*[contains(@name,'TopSettingsTabs')]/HorizontalBox_0/TabButtonBox/*[contains(@name,'W_LyraButtonTab_C')][3]";
        public String UI_OptionsMenu_Gamepad            = "/*[contains(@name,'W_LyraSettingScreen_C')]/Overlay/MenuBorder/VerticalBox_0/SizeBox_0/HeaderBorder/TabSZ/*[contains(@name,'TopSettingsTabs')]/HorizontalBox_0/TabButtonBox/*[contains(@name,'W_LyraButtonTab_C')][4]";
        public String UI_OptionsMenu_ComplexBack        = "/*[contains(@name,'W_LyraSettingScreen_C')]/Overlay/MenuBorder/VerticalBox_0/Border_0/*[contains(@name,'W')]"; 
        public String UI_OptionsMenu_ComplexBack2       = "/*[contains(@name,'W_BoundActionButton_C_2')]";

        public bool hasPlayerCrouched = false;
        public bool isOptionsMenuOpen = false;

        private Vector3 basePlayerLocation;

        private bool isUnlitModeActive = true;

        [SetUp]
        public void Setup()
        {    

            _gameDriverAPI = new ApiClient();
            PrintToConsole("Connecting to Unnreal Engine...");
            _gameDriverAPI.Connect("localhost");
            PrintToConsole("Connected to Unreal Engine.");

            _gameDriverAPI.WaitForObject(characterObjectPath);

            _gameDriverAPI.CreateInputDevice("GDIO", "IMC_Default");
            basePlayerLocation = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);

            if (isUnlitModeActive)
            {
                _gameDriverAPI.KeyPress(new KeyCode[] {KeyCode.F2});
            }
        }

        [TearDown]
        public void TearDown()
        {
            _gameDriverAPI.Wait(500);
            _gameDriverAPI.FloatInputEvent("W", 0.0f);
            _gameDriverAPI.FloatInputEvent("S", 0.0f);
            _gameDriverAPI.FloatInputEvent("A", 0.0f);
            _gameDriverAPI.FloatInputEvent("D", 0.0f);
            _gameDriverAPI.FloatInputEvent("LeftControl", 0.0f);
            _gameDriverAPI.Wait(125);

            _gameDriverAPI.SetObjectPosition(characterObjectPath, basePlayerLocation);

            PrintToConsole("Reseted Player Movement and Rotation.");
            _gameDriverAPI.Wait(125);

            /*   Ensure that the player character is still there.   */
            _gameDriverAPI.WaitForObject(characterObjectPath);

            if (hasPlayerCrouched == true)
            {
                _gameDriverAPI.FloatInputEvent("LeftControl", 1.0f);
                hasPlayerCrouched = false;
            }

            _gameDriverAPI.Disconnect();
            _gameDriverAPI.Wait(500);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (_gameDriverAPI != null)
            {
                _gameDriverAPI.Disconnect();
                PrintToConsole("Disconnected from Unreal Engine.");
            }
        }

        public void PrintToConsole(String message)
        {             
            Console.WriteLine(message);
        }
        public void CreatePlayerInput(String direction, String input, float movementSpeed = 1.0f)
        {
            PrintToConsole("Moving Character: " + direction);
            _gameDriverAPI.FloatInputEvent(input, movementSpeed);
        }
    }
}
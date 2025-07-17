using gdio.common.objects;

namespace LyraTests.Tests.SmokeTests
{
    [TestFixture]
    internal class CharacterMouseInput : GameDriveBase
    {

        private String FunctionName_GetFOV = "GetPlayerFOV";

        private readonly ulong frameRate = 60;

        private String WeaponFireObject     = "/*[@name = 'B_Hero_ShooterMannequin_C_0']/*[@name = 'B_Pistol_C_0']/*[@name = 'B_WeaponFire_C_0']";
        private String WeaponImpactObject   = "/*[@name = 'B_Hero_ShooterMannequin_C_0']/*[@name = 'B_Pistol_C_0']/*[@name = 'B_WeaponImpacts_C_0']";

        [TestCase, Timeout(10000)]
        public void CharacterMouseInput_MoveCameraRandomly()
        {
            PrintToConsole("Start :: Character Camera Aim Test - Mouse Input");

            Vector3 startingCharacterRotation = _gameDriverAPI.GetObjectRotation(characterObjectPath);
            PrintToConsole("Character Start Rotation: " + startingCharacterRotation);

            PrintToConsole("Generate a random Vector3 target location for the Mouse Input.");

            // Generate a random Vector2 for mouse position (assuming screen coordinates, e.g., 0-1920 x 0-1080)
            Random rand = new Random();
            float randomX =(float)rand.NextDouble() * 1920f;
            float randomY =(float)rand.NextDouble() * 1080f;

            while (randomX == 0 || randomY == 0)
            {
                randomX = (float)rand.NextDouble() * 1920f;
                randomY = (float)rand.NextDouble() * 1080f;
            }
            Vector2 randomTarget = new Vector2(randomX, randomY);

            /* I chose the "MIDDLE" mouse button so that it won't interfere with anything*/
            _gameDriverAPI.MouseDrag(MouseButtons.MIDDLE, randomTarget, frameRate);
            _gameDriverAPI.Wait(3000);

            Vector3 endingCharacterRotation = _gameDriverAPI.GetObjectRotation(characterObjectPath);
            PrintToConsole("Character Ending Rotation: " + endingCharacterRotation);

            float movementThreshold = MathF.Sqrt(
                MathF.Pow(startingCharacterRotation.x - endingCharacterRotation.x, 2) +
                MathF.Pow(startingCharacterRotation.y - endingCharacterRotation.y, 2) +
                MathF.Pow(startingCharacterRotation.z - endingCharacterRotation.z, 2)
                );

            if (movementThreshold > 1.0f)
            {
                PrintToConsole("Character rotation has changed after mouse input.");
                Assert.Pass("PASSED: Character Camera Aim Test - Mouse Input succeeded.");
            }
            else
            {
                PrintToConsole("Character rotation has not changed after mouse input.");
                Assert.Fail("FAILED: Character Camera Aim Test - Mouse Input failed.");
            }
        }

        [TestCase, Timeout(10000)]
        public void CharacterMouseInput_MouseLeftClickEvent()
        {
            PrintToConsole("Start :: Character Camera Aim Test - Mouse Click Event");

            PrintToConsole("Trigger Mouse Click - Fire Weapon Event.");

            _gameDriverAPI.Click(MouseButtons.LEFT, new Vector2(0, 0), frameRate);
            _gameDriverAPI.Wait(500);
            bool hasWeaponFireBeenCreated = _gameDriverAPI.WaitForObject(WeaponFireObject);
            bool hasWeaponImpactBeenCreated = _gameDriverAPI.WaitForObject(WeaponImpactObject);

            if (hasWeaponFireBeenCreated && hasWeaponImpactBeenCreated)
            {
                PrintToConsole("Weapon Fire and Impact events were successfully created.");
                Assert.Pass("PASSED: Mouse Left Click Event Test succeeded.");
            }
            else
            {
                PrintToConsole("Weapon Fire or Impact events were not created.");
                Assert.Fail("FAILED: Mouse Left Click Event Test failed.");
            }
        }

        [TestCase, Timeout(10000)]
        public void CharacterMouseInput_MouseRightClickEvent() // The B_Hero_ShooterMannequin_C_0 has a Camera Setting category, with a FOV slider that starts at 80 and goes to 70 when ADS-ing. Check that.
        {
            PrintToConsole("Start :: Character Camera Aim Test - Mouse Right Click Event");

            PrintToConsole("Getting initial Player FOV.");
            float initialFOV = _gameDriverAPI.CallMethod<float>(
                gameDriverCommunicationPath,
                FunctionName_GetFOV,
                new object[] { 
            });

            PrintToConsole("Initial Player FOV: " + initialFOV);

            PrintToConsole("Trigger Mouse Right Click - Aim Down Sights Event.");
            _gameDriverAPI.Click(MouseButtons.RIGHT, new Vector2(0, 0), frameRate);
            _gameDriverAPI.Wait(10);

            float finalFOV = _gameDriverAPI.CallMethod<float>(
                gameDriverCommunicationPath,
                FunctionName_GetFOV,
                new object[] {
            });

            PrintToConsole("Final Player FOV: " + finalFOV);

            _gameDriverAPI.Wait(150);
            if (finalFOV < initialFOV)
            {
                PrintToConsole("Player FOV has changed after right click event.");
                Assert.Pass("PASSED: Character Camera Aim Test - Mouse Right Click Event succeeded.");
            }
            else
            {
                PrintToConsole("Player FOV has not changed after right click event.");
                Assert.Fail("FAILED: Character Camera Aim Test - Mouse Right Click Event failed.");
            }
        }

    }
}

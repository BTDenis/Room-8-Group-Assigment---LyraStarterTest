using gdio.common.objects;


namespace LyraTests.Tests.SmokeTests
{
    [TestFixture]
    internal class UIHud : GameDriveBase
    {
        private int waitTime = 700;

        private String FunctionName_OpenOptionsMenu = "OpenOptionsMenu";

        [TestCase, Timeout(15000)]
        public void UIHud_AssesOptionMenuOpenAndClose()
        {
            PrintToConsole("Start :: UI Hud Test - Open Pause Menu");

            OpenOptionsMenu();

            bool isOptionsMenuVisible = _gameDriverAPI.WaitForObject(UI_OptionsMenu);
            if (isOptionsMenuVisible)
            {
                PrintToConsole("Options Menu is visible.");
            }
            else
            {
                PrintToConsole("Options Menu is not visible.");
                Assert.Fail("FAILED: UI Hud Test - Open Pause Menu failed.");
            }

            bool hasOptionsMenuOpened = _gameDriverAPI.WaitForObject(UI_OptionsMenu);

            CloseOptionsMenu();

            if (hasOptionsMenuOpened)
            {
                PrintToConsole("Options Menu has been opened successfully.");
                Assert.Pass("PASSED: UI Hud Test - Open Pause Menu succeeded.");
            }
            else
            {
                PrintToConsole("Options Menu has not been opened.");
                Assert.Fail("FAILED: UI Hud Test - Open Pause Menu failed.");
            }
        }

        [TestCase, Timeout(15000)]
        public void UIHud_ClickOnOptionsMenu()
        {
            PrintToConsole("Start :: UI Hud Test - Click on Options Button");

            OpenOptionsMenu();

            bool isOptionsMenuVisible = _gameDriverAPI.WaitForObject(UI_OptionsMenu);
            if (isOptionsMenuVisible)
            {
                PrintToConsole("Options Menu is visible.");
            }
            else
            {
                PrintToConsole("Options Menu is not visible.");
                Assert.Fail("FAILED: UI Hud Test - Open Pause Menu failed.");
            }
            _gameDriverAPI.Wait(waitTime);

            PrintToConsole("Clicking on Options Button to open the Options Setings Menu.");

            bool isOptionsButtonVisible = _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu, 30);
            if (!isOptionsButtonVisible)
            {
                PrintToConsole("Options Button has not been clicked.");
                Assert.Fail("FAILED: UI Hud Test - Could not open Options Menu..");
            }
            _gameDriverAPI.Wait(waitTime);

            bool isSettingsMenuVisible = _gameDriverAPI.WaitForObject(UI_OptionsMenu_ComplexBack);
            if (!isSettingsMenuVisible)
            {
                PrintToConsole("Option Settings Menu has not been entered.");
                Assert.Fail("FAILED: UI Hud Test - Could not enter Options Settings Menu.");
            }
            _gameDriverAPI.Wait(waitTime);

            PrintToConsole(isSettingsMenuVisible.ToString());

            CloseComplexOptionsMenu();

            if (isSettingsMenuVisible)
            {
                PrintToConsole("Options Button has been clicked successfully.");
                Assert.Pass("PASSED: UI Hud Test - Click on Options Button succeeded.");
            }
            else
            {
                PrintToConsole("Options Button has not been clicked.");
                Assert.Fail("FAILED: UI Hud Test - Click on Options Button failed.");
            }
        }

        [TestCase, Timeout(15000)]
        public void UIHud_ScrollThroughOptions()
        {
            PrintToConsole("Start :: UI Hud Test - Scroll through Options");

            OpenOptionsMenu();

            _gameDriverAPI.Wait(waitTime);

            bool isOptionsMenuVisible = _gameDriverAPI.WaitForObject(UI_OptionsMenu);
            if (!isOptionsMenuVisible)
            {
                PrintToConsole("Options Menu is not visible. Cannot scroll through options.");
                Assert.Fail("FAILED: UI Hud Test - Scroll through Options failed.");
            }

            bool hasOptionsButtonBeenClicked = _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu, 30);
            if (!hasOptionsButtonBeenClicked)
            {
                PrintToConsole("Options Button has not been clicked. Cannot scroll through options.");
                Assert.Fail("FAILED: UI Hud Test - Scroll through Options failed.");
            }
            _gameDriverAPI.Wait(waitTime);

            PrintToConsole("Verifying that the Settings Menu is now visible.");

            bool isSettingsMenuVisible = _gameDriverAPI.WaitForObject(UI_OptionsMenu_Open);
            if (!isSettingsMenuVisible)
            {
                PrintToConsole("Options Settings Menu has not been opened.");
                Assert.Fail("FAILED: UI Hud Test - Scroll through Options failed.");
            }

            PrintToConsole("Scrolling through Options Menu...");

            bool hasClickedGameplayTab = _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_Gameplay, 30);
            if (hasClickedGameplayTab)
            {
                PrintToConsole("Gameplay Tab has been clicked successfully.");
            }
            else
            {
                PrintToConsole("Gameplay Tab has not been clicked.");
                Assert.Fail("FAILED: UI Hud Test - Scroll through Options failed.");
            }
            _gameDriverAPI.Wait(waitTime);

            bool hasClickedVideoTab = _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_Video, 30);  
            if (hasClickedVideoTab)
            {
                PrintToConsole("Video Tab has been clicked successfully.");
            }
            else
            {
                PrintToConsole("Video Tab has not been clicked.");
                Assert.Fail("FAILED: UI Hud Test - Scroll through Options failed.");
            }
            _gameDriverAPI.Wait(waitTime);

            bool hasClickedAudioTab = _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_Audio, 30);
            if (hasClickedAudioTab)
            {
                PrintToConsole("Audio Tab has been clicked successfully.");
            }
            else
            {
                PrintToConsole("Audio Tab has not been clicked.");
                Assert.Fail("FAILED: UI Hud Test - Scroll through Options failed.");
            }
            _gameDriverAPI.Wait(waitTime);

            bool hasClickedMkbTab = _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_MKb, 30);
            if (hasClickedMkbTab)
            {
                PrintToConsole("Mouse and Keyboard Tab has been clicked successfully.");
            }
            else
            {
                PrintToConsole("Mouse and Keyboard Tab has not been clicked.");
                Assert.Fail("FAILED: UI Hud Test - Scroll through Options failed.");
            }
            _gameDriverAPI.Wait(waitTime);

            bool hasClickedGamepadTab = _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_Gamepad, 30);
            if (hasClickedGamepadTab)
            {
                PrintToConsole("Gamepad Tab has been clicked successfully.");
            }
            else
            {
                PrintToConsole("Gamepad Tab has not been clicked.");
                Assert.Fail("FAILED: UI Hud Test - Scroll through Options failed.");
            }
            _gameDriverAPI.Wait(waitTime);

            _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_ComplexBack2, 3);

            _gameDriverAPI.Wait(waitTime);

            Assert.Pass("PASSED: UI Hud Test - Scroll through Options succeeded.");
        }

        [TestCase, Timeout(5000)]
        public void UIHud_VerifyHealthUI()
        {
            PrintToConsole("Start :: UI Hud Test - Verify Health UI");

            PrintToConsole("Checking if the Health UI is visible...");
            bool isHealthUIVisible = _gameDriverAPI.WaitForObject(UI_Health);
            if (isHealthUIVisible)
            {
                PrintToConsole("Health UI is visible.");
            }
            else
            {
                PrintToConsole("Health UI is not visible.");
                Assert.Fail("FAILED: UI Hud Test - Verify Health UI failed.");
            }

            PrintToConsole("Checking if the Reticle/Crosshair is visible...");
            bool isReticleVisible = _gameDriverAPI.WaitForObject(UI_Reticle);
            if (isReticleVisible)
            {
                PrintToConsole("Reticle/Crosshair is visible.");
            }
            else
            {
                PrintToConsole("Reticle/Crosshair is not visible.");
                Assert.Fail("FAILED: UI Hud Test - Verify Health UI failed.");
            }

            PrintToConsole("Checking if the Pistol Ammo UI is visible...");
            bool isPistolAmmoVisible = _gameDriverAPI.WaitForObject(UI_PistolAmmo);
            if (isPistolAmmoVisible)
            {
                PrintToConsole("Pistol Ammo UI is visible.");
            }
            else
            {
                PrintToConsole("Pistol Ammo UI is not visible.");
                Assert.Fail("FAILED: UI Hud Test - Verify Health UI failed.");
            }

            Assert.Pass("PASSED: UI Hud Test - Verify Health UI succeeded.");
        }

        public void OpenOptionsMenu()
        {
            if (!isOptionsMenuOpen)
            {
                PrintToConsole("Options Menu was closed. Opening Options Menu...");
                _gameDriverAPI.CallMethod(
                    gameDriverCommunicationPath,
                    FunctionName_OpenOptionsMenu,
                    new object[] { }
                );

                isOptionsMenuOpen = true;
            }
            else
            {
                //Passing without logging.
            }

            _gameDriverAPI.Wait(waitTime);
        }

        public void CloseOptionsMenu()
        {
            _gameDriverAPI.Wait(waitTime);

            if (isOptionsMenuOpen)
            {
                _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_Back, 3);
                isOptionsMenuOpen = false;
            }

        }

        public void CloseComplexOptionsMenu()
        {
            _gameDriverAPI.Wait(waitTime);

            if (isOptionsMenuOpen)
            {
                if (_gameDriverAPI.WaitForObject(UI_OptionsMenu_ComplexBack, 1000))
                {
                    _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_ComplexBack, 3);
                }
                else
                {
                    _gameDriverAPI.ClickObject(MouseButtons.LEFT, UI_OptionsMenu_ComplexBack2, 3);
                }
                isOptionsMenuOpen = false;
                _gameDriverAPI.Wait(waitTime);
            }
        }
    }
}

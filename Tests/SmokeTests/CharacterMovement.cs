using gdio.common.objects;



namespace LyraTests.Tests.SmokeTests
{
    [TestFixture]
    internal class CharacterMovement : GameDriveBase
    {
        private int waitTimeForInput        = 1000;

        /*
         * This function can take 3 parameters, but the majority of the time, it will only use the first two.
         * The third parameter, movementSpeed, is only used when the character is moving diagonally.
         * This is done so that the function can be used for both single direction movement (W, A, S, D) and diagonal movement (W+D, W+A, S+D, S+A).
        */

        [TestCase, Timeout(10000)]
        public void CharacterMovement_Forward() // TO UPDATE TO USE _gameDriverAPI.KeyPress
        {
            PrintToConsole("Start :: Character Movement Test - Forward");

            Vector3 characterStartPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character Start Position: " + characterStartPos);

            CreatePlayerInput("Forward", "W");
            PrintToConsole("Wait for " + waitTimeForInput / 1000 + " seconds for the character to move.");
            _gameDriverAPI.Wait(waitTimeForInput);

            PrintToConsole("Verifying that the character has moved.");
            Vector3 characterEndPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character End Position: " + characterEndPos);

            float movementThreshold = MathF.Sqrt(
                MathF.Pow(characterEndPos.x - characterStartPos.x, 2) +
                MathF.Pow(characterEndPos.y - characterStartPos.y, 2) +
                MathF.Pow(characterEndPos.z - characterStartPos.z, 2)
                );

            if (movementThreshold < 1.0f)
            {
                PrintToConsole("End :: Character Movement Test - Forward");
                Assert.Fail("FAILED: Forward Movement has failed.");
            }
            else
            {
                PrintToConsole("End :: Character Movement Test - Forward");
                Assert.Pass("PASSED: Forward Movement has succeeded.");
            }
        }

        [TestCase, Timeout(10000)]
        public void CharacterMovement_Backwards()
        {
            PrintToConsole("Start :: Character Movement Test - Backwards");

            Vector3 characterStartPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character Start Position: " + characterStartPos);

            CreatePlayerInput("Backwards", "S");
            PrintToConsole("Wait for " + waitTimeForInput / 1000 + " seconds for the character to move.");
            _gameDriverAPI.Wait(waitTimeForInput);

            PrintToConsole("Verifying that the character has moved.");
            Vector3 characterEndPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character End Position: " + characterEndPos);

            float movementThreshold = MathF.Sqrt(
                MathF.Pow(characterEndPos.x - characterStartPos.x, 2) +
                MathF.Pow(characterEndPos.y - characterStartPos.y, 2) +
                MathF.Pow(characterEndPos.z - characterStartPos.z, 2)
                );

            if (movementThreshold < 1.0f)
            {
                PrintToConsole("End :: Character Movement Test - Backwards");
                Assert.Fail("FAILED: Backwards Movement has failed.");
            }
            else
            {
                PrintToConsole("End :: Character Movement Test - Backwards");
                Assert.Pass("PASSED: Backwards Movement has succeeded.");
            }
        }

        [TestCase, Timeout(10000)]
        public void CharacerMovement_Left()
        {             
            PrintToConsole("Start :: Character Movement Test - Left");

            Vector3 characterStartPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character Start Position: " + characterStartPos);

            CreatePlayerInput("Left", "A");
            PrintToConsole("Wait for " + waitTimeForInput / 1000 + " seconds for the character to move."); ;
            _gameDriverAPI.Wait(waitTimeForInput);

            PrintToConsole("Verifying that the character has moved.");
            Vector3 characterEndPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character End Position: " + characterEndPos);

            float movementThreshold = MathF.Sqrt(
                MathF.Pow(characterEndPos.x - characterStartPos.x, 2) +
                MathF.Pow(characterEndPos.y - characterStartPos.y, 2) +
                MathF.Pow(characterEndPos.z - characterStartPos.z, 2)
                );

            if (movementThreshold < 1.0f)
            {
                PrintToConsole("End :: Character Movement Test - Left");
                Assert.Fail("FAILED: Left Movement has failed.");
            }
            else
            {
                PrintToConsole("End :: Character Movement Test - Left");
                Assert.Pass("PASSED: Left Movement has succeeded.");
            }
        }
        [TestCase, Timeout(10000)]
        public void CharacterMovement_Right()
        {
            PrintToConsole("Start :: Character Movement Test - Right");

            Vector3 characterStartPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character Start Position: " + characterStartPos);

            CreatePlayerInput("Right", "D");
            PrintToConsole("Wait for " + waitTimeForInput / 1000 + " seconds for the character to move.");
            _gameDriverAPI.Wait(waitTimeForInput);

            PrintToConsole("Verifying that the character has moved.");
            Vector3 characterEndPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character End Position: " + characterEndPos);

            float movementThreshold = MathF.Sqrt(
                MathF.Pow(characterEndPos.x - characterStartPos.x, 2) +
                MathF.Pow(characterEndPos.y - characterStartPos.y, 2) +
                MathF.Pow(characterEndPos.z - characterStartPos.z, 2)
                );

            if (movementThreshold < 1.0f)
            {
                PrintToConsole("End :: Character Movement Test - Right");
                Assert.Fail("FAILED: Right Movement has failed.");
            }
            else
            {
                PrintToConsole("End :: Character Movement Test - Right");
                Assert.Pass("PASSED: Right Movement has succeeded.");
            }
        }

        [TestCase, Timeout(10000)]
        public void CharacterMovement_Diagonal()
        {
            PrintToConsole("Start :: Character Movement Test - Forward + Right");

            Vector3 characterStartPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character Start Position: " + characterStartPos);

            CreatePlayerInput("Forward", "W");
            CreatePlayerInput("Right",   "D");
            PrintToConsole("Wait for " + waitTimeForInput / 1000 + " seconds for the character to move.");
            _gameDriverAPI.Wait(waitTimeForInput);

            PrintToConsole("Verifying that the character has moved.");
            Vector3 characterEndPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character End Position: " + characterEndPos);

            float movementThreshold = MathF.Sqrt(
                MathF.Pow(characterEndPos.x - characterStartPos.x, 2) +
                MathF.Pow(characterEndPos.y - characterStartPos.y, 2) +
                MathF.Pow(characterEndPos.z - characterStartPos.z, 2)
                );

            if (movementThreshold < 1.0f)
            {
                PrintToConsole("End :: Character Movement Test - Forward + Right");
                Assert.Fail("FAILED: Forward + Right Movement has failed.");
            }
            else
            {
                PrintToConsole("End :: Character Movement Test - Forward + Right");
                Assert.Pass("PASSED: Forward + Right Movement has succeeded.");
            }
        }

        [TestCase, Timeout(10000)]
        public void CharacterMovement_Jump()
        {
            PrintToConsole("Start :: Character Movement Test - Jump");

            Vector3 characterStartPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character Start Position: " + characterStartPos);

            CreatePlayerInput("Jump", "SpaceBar");
            PrintToConsole("Wait for " + waitTimeForInput / 2000 + " seconds for the character to move.");
            _gameDriverAPI.Wait(waitTimeForInput/2);

            PrintToConsole("Verifying that the character has moved.");
            Vector3 characterEndPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character End Position: " + characterEndPos);

            float movementThreshold = MathF.Sqrt(
                MathF.Pow(characterEndPos.x - characterStartPos.x, 2) +
                MathF.Pow(characterEndPos.y - characterStartPos.y, 2) +
                MathF.Pow(characterEndPos.z - characterStartPos.z, 2)
                );

            if (movementThreshold < 1.0f)
            {
                PrintToConsole("End :: Character Movement Test - Jump");
                Assert.Fail("FAILED: Jump Movement has failed.");
            }
            else
            {
                PrintToConsole("End :: Character Movement Test - Jump");
                Assert.Pass("PASSED: Jump Movement has succeeded.");
            }
        }

        [TestCase, Timeout(10000)]
        public void CharacterMovement_Crouch()
        {
            PrintToConsole("Start :: Character Movement Test - Crouch");

            Vector3 characterStartPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character Start Position: " + characterStartPos);

            CreatePlayerInput("Crouch", "LeftControl");
            PrintToConsole("Wait for " + waitTimeForInput / 1000 + " seconds for the character to move.");
            _gameDriverAPI.Wait(waitTimeForInput);

            PrintToConsole("Verifying that the character has moved.");
            Vector3 characterEndPos = _gameDriverAPI.GetObjectPosition(characterObjectPath,
                    CoordinateConversion.Local);
            PrintToConsole("Character End Position: " + characterEndPos);

            float movementThreshold = MathF.Sqrt(
                MathF.Pow(characterEndPos.x - characterStartPos.x, 2) +
                MathF.Pow(characterEndPos.y - characterStartPos.y, 2) +
                MathF.Pow(characterEndPos.z - characterStartPos.z, 2)
                );
            if (movementThreshold < 1.0f)
            {
                PrintToConsole("End :: Character Movement Test - Crouch");
                Assert.Fail("FAILED: Crouch Movement has failed.");
            }
            else
            {
                hasPlayerCrouched = true;
                PrintToConsole("End :: Character Movement Test - Crouch");
                Assert.Pass("PASSED: Crouch Movement has succeeded.");
            }
        }
    }
}

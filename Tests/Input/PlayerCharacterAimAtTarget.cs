using gdio.common.objects;



namespace LyraTests.Tests.Input
{
    [TestFixture]
    internal class PlayerCharacterAimAtTarget : GameDriveBase
    {
        private String FunctionName_SpawnTarget         = "SpawnTestActor";
        private String FunctionName_FindLookAtRotation  = "FindRotationToActor";
        private String FunctionName_RaycastToTarget     = "RaycastToTestActor";
        private String FunctionName_InjectInput         = "InjectMouseInput";
        private String FunctionName_DeleteTarget        = "DeleteTestActor";

        [TestCase, Timeout(20000)]
        public void PlayerCharacterAimAtTarget_AimAtDesignatedTarget()
        {
            PrintToConsole("Start :: Player Character Aim At Target Test");

            PrintToConsole("Creating an object of the following type " + FullTargetToSpawnPath);
            bool wasObjectCreated = _gameDriverAPI.CallMethod(
                gameDriverCommunicationPath,
                FunctionName_SpawnTarget, 
                new object[] 
                {
                    FullTargetToSpawnPath, 
                    3000.0f 
                });

            if (!wasObjectCreated)
            {
                PrintToConsole("Target Actor failed to be created. Please check that the path is correct.");
                PrintToConsole("End :: Player Character Aim At Target Test");
                Assert.Fail("FAILED: Target Actor spawn failed.");
            }

            PrintToConsole("Object created successfully. Now waiting for the object to appear in the scene.");
            _gameDriverAPI.Wait(100);
            _gameDriverAPI.WaitForObject(TargetCreatedReference);

            Vector3 TargetLocation = _gameDriverAPI.GetObjectPosition(TargetCreatedReference, CoordinateConversion.Local);
            _gameDriverAPI.WaitForObject(TargetCreatedReference);
            PrintToConsole("Object created at the following coordinates: "
                + TargetLocation
                );

            PrintToConsole("Getting the Rotator towards the created Target Actor.");  
            Vector3 LookAtRotation = _gameDriverAPI.CallMethod<Vector3>(
                gameDriverCommunicationPath,
                FunctionName_FindLookAtRotation,
                new object[]
                {
                    TargetObjectInEditorName
                });
            _gameDriverAPI.Wait(500);

            PrintToConsole("Rotator towards the Target Actor: ");
            Console.WriteLine(LookAtRotation);

            PrintToConsole("Simulating Mouse Input to aim at the Target.");
            bool hasPlayerLookedAtTarget = _gameDriverAPI.CallMethod(
                gameDriverCommunicationPath,
                FunctionName_InjectInput,
                new object[]
                {
                    LookAtRotation.z, LookAtRotation.y
                });

            _gameDriverAPI.Wait(1500);

            if (!hasPlayerLookedAtTarget)
            {
                PrintToConsole("Player failed to rotate towards the Target Actor. Please check that the path is correct.");
                PrintToConsole("End :: Player Character Aim At Target Test");
                Assert.Fail("FAILED: Player Rotate to Target Actor failed.");
            }

            PrintToConsole("Mouse input succesfully injected towards the Target Actor. Now raycasting to check if the player is aiming at the Target Actor.");   
            bool isRaycastHittingTargetActor = _gameDriverAPI.CallMethod<bool>(
                gameDriverCommunicationPath,
                FunctionName_RaycastToTarget,
                new object[]
                { 
                    TargetObjectInEditorName
                });

            _gameDriverAPI.Wait(1000);
            DestroyTargetActor();

            if (isRaycastHittingTargetActor)
            {
                PrintToConsole("Player successfully aimed at the target.");
                PrintToConsole("End :: Player Character Aim At Target Test");
                Assert.Pass("PASSED: Player Aim to Target succeeded.");
            }
            else
            {
                PrintToConsole("Player failed to aim at the target.");
                PrintToConsole("End :: Player Character Aim At Target Test");
                Assert.Fail("FAILED: Player Aim to Target failed.");
            }
        }

        public void DestroyTargetActor()
        {
            PrintToConsole("Destroying Target Actor.");
            bool isTargetActorDestroyed = _gameDriverAPI.CallMethod(
                gameDriverCommunicationPath,
                FunctionName_DeleteTarget,
                new object[] { TargetObjectInEditorName }
            );

            if (isTargetActorDestroyed)
            {
                PrintToConsole("Actor successfully destroyed.");
            }
            else
            {
                PrintToConsole("Failed to destroy Target Actor.");
            }
        }
    }
}
// Fill out your copyright notice in the Description page of Project Settings.


#include "GameDriverClassPawn.h"

#include "GameFramework/PlayerController.h"
#include "EnhancedInputSubsystems.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputLibrary.h"
#include "InputMappingContext.h"
#include "InputAction.h"
#include "DrawDebugHelpers.h"
#include "Engine/World.h"
#include "EngineUtils.h"
#include "Kismet/KismetMathLibrary.h"

// Sets default values
AGameDriverClassPawn::AGameDriverClassPawn()
{

	static ConstructorHelpers::FObjectFinder<UInputAction> LookActionFinder(TEXT("/Game/Input/Actions/IA_Look_Mouse.IA_Look_Mouse"));
	if (!LookActionFinder.Succeeded())
	{
		UE_LOG(LogTemp, Error, TEXT("Failed to load Look input action."));
		return;
	}

	PlayerSimulateLookAction = LookActionFinder.Object;
}

void AGameDriverClassPawn::SpawnTestActor(FString ActorPath, float radiusFromOriginPoint = 3000.0f)
{
	if (ActorPath.IsEmpty())
	{
		UE_LOG(LogTemp, Warning, TEXT("Actor to spawn is empty."));
		return;
	}

	UWorld* World = GetWorld();
	if (!World)
	{
		UE_LOG(LogTemp, Warning, TEXT("World is null."));
		return;
	}

	APlayerController* PlayerController = World->GetFirstPlayerController();
	if (!PlayerController)
	{
		UE_LOG(LogTemp, Warning, TEXT("PlayerController is null."));
		return;
	}

	APawn* PlayerPawn = PlayerController->GetPawn();
	if (!PlayerPawn)
	{
		UE_LOG(LogTemp, Warning, TEXT("PlayerPawn is null."));
		return;
	}

	FString FullActorPath = ActorPath;

	UClass* ActorToSpawn = Cast<UClass>(StaticLoadObject(UClass::StaticClass(), nullptr, *FullActorPath));

	if (!ActorToSpawn)
	{
		UE_LOG(LogTemp, Error, TEXT("Failed to load actor class from path: %s"), *FullActorPath);
		return;
	}

	FVector SpawnLocation = UKismetMathLibrary::RandomUnitVector() * FMath::FRandRange(radiusFromOriginPoint, MaxSpawnLevel);
	SpawnLocation.Z = FMath::FRandRange(MinSpawnLevel, MaxSpawnLevel);

	FVector DirectionToPlayerPawn = PlayerPawn->GetActorLocation() - SpawnLocation;
	FRotator SpawnRotation = FRotator::ZeroRotator;
	SpawnRotation = DirectionToPlayerPawn.Rotation();

	FActorSpawnParameters SpawnParams;
	SpawnParams.SpawnCollisionHandlingOverride = ESpawnActorCollisionHandlingMethod::AdjustIfPossibleButAlwaysSpawn;


	AActor* ActorSpawned = GetWorld()->SpawnActor<AActor>(ActorToSpawn, SpawnLocation, SpawnRotation, SpawnParams);

	if (ActorSpawned)
	{
		UE_LOG(LogTemp, Warning, TEXT("Spawned actor at location: %s"), *SpawnLocation.ToString());

		TArray<UStaticMeshComponent*> Components;
		ActorSpawned->GetComponents<UStaticMeshComponent>(Components);

		for (UStaticMeshComponent* Component : Components)
		{
			if (Component)
			{
				Component->SetCollisionEnabled(ECollisionEnabled::QueryAndPhysics);
				Component->SetCollisionResponseToAllChannels(ECollisionResponse::ECR_Block);
				Component->SetCollisionObjectType(ECC_WorldDynamic);
			}
		}

		UE_LOG(LogTemp, Warning, TEXT("Created collisions for Actor."), *ActorSpawned->GetName());

	}
	else
	{
		UE_LOG(LogTemp, Error, TEXT("Failed to spawn actor."));
	}
}

bool AGameDriverClassPawn::RaycastToTestActor(FString ActorName)
{
	bool returnValue = false;

	if (ActorName.IsEmpty())
	{
		UE_LOG(LogTemp, Warning, TEXT("ActorName is empty."));
		return returnValue;
	}

	UWorld* World = GetWorld();
	if (!World)
	{
		UE_LOG(LogTemp, Warning, TEXT("World is null."));
		return returnValue;
	}

	APlayerController* PlayerController = World->GetFirstPlayerController();
	if (!PlayerController)
	{
		UE_LOG(LogTemp, Warning, TEXT("PlayerController is null."));
		return returnValue;
	}

	APawn* Pawn = PlayerController->GetPawn();
	if (!Pawn)
	{
		UE_LOG(LogTemp, Warning, TEXT("Pawn is null."));
		return returnValue;
	}

	FVector StartRaycastLocation;
	FRotator StartRaycastRotation;

	PlayerController->GetPlayerViewPoint(StartRaycastLocation, StartRaycastRotation);

	/* Arbitrary Value to raycast forward. Can be modified in the header file.  */
	const FVector RayCastEnd = StartRaycastLocation + StartRaycastRotation.Vector() * MaxRaycastDistance;

	FCollisionQueryParams CollisionParams;
	CollisionParams.AddIgnoredActor(World->GetFirstPlayerController()->GetPawn());

	FHitResult HitResult;

	/* We would normally use this, but to avoid any kind of small offsets, we use a SphereTrace */
	//bool bHit = World->LineTraceSingleByChannel(HitResult, StartRaycastLocation, RayCastEnd, ECC_Visibility, CollisionParams);

	bool bHit = World->SweepSingleByChannel(
		HitResult,
		StartRaycastLocation,
		RayCastEnd,
		FQuat::Identity, // No rotation for the sweep
		ECC_Visibility, // Collision channel
		FCollisionShape::MakeSphere(SphereRadius), // Sphere shape for the sweep
		CollisionParams
	);

	if (bHit)
	{
		DrawDebugSphere(World, HitResult.ImpactPoint, SphereRadius, 12, FColor::Red, true, 15.0f);

		AActor* HitActor = HitResult.GetActor();
		if (!HitActor)
		{
			UE_LOG(LogTemp, Warning, TEXT("Raycast hit something that is not an actor."));
		}

		if (HitActor && HitActor->GetName() == ActorName)
		{
			returnValue = true;
			UE_LOG(LogTemp, Warning, TEXT("Raycast hit actor: %s"), *ActorName);
		}
		else
		{
			UE_LOG(LogTemp, Warning, TEXT("Raycast hit something else: %s"), *HitResult.GetActor()->GetName());
		}
	}
	else
	{
		UE_LOG(LogTemp, Warning, TEXT("Raycast did not hit anything."));
	}

	return returnValue;
}

void AGameDriverClassPawn::DeleteTestActor(FString ActorName)
{
	if (ActorName.IsEmpty())
	{
		UE_LOG(LogTemp, Warning, TEXT("ActorName is empty."));
		return;
	}

	UWorld* World = GetWorld();
	if (!World)
	{
		UE_LOG(LogTemp, Warning, TEXT("World is null."));
		return;
	}

	for (TActorIterator<AActor> It(World); It; ++It)
	{
		AActor* Actor = *It;
		if (Actor && Actor->GetName() == ActorName)
		{
			Actor->Destroy();
			UE_LOG(LogTemp, Warning, TEXT("Deleted actor: %s"), *ActorName);
			return;
		}
	}
}

void AGameDriverClassPawn::InjectMouseInput(float Pitch, float Yaw)
{
	UE_LOG(LogTemp, Warning, TEXT("InjectInput called with DeltaX: %f, DeltaY: %f"), Pitch, Yaw);

	UWorld* World = GetWorld();
	if (!World)
	{
		UE_LOG(LogTemp, Warning, TEXT("World is null."));
		return;
	}
	APlayerController* PlayerController = World->GetFirstPlayerController();
	if (!PlayerController)
	{
		UE_LOG(LogTemp, Warning, TEXT("PlayerController is null."));
		return;
	}
	
	UEnhancedInputLocalPlayerSubsystem* InputSubsystem = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PlayerController->GetLocalPlayer());

	if (!InputSubsystem)
	{
		UE_LOG(LogTemp, Warning, TEXT("InputSubsystem is null."));
		return;
	}

	UEnhancedPlayerInput* PlayerInput = InputSubsystem->GetPlayerInput();

	if (!PlayerInput)
	{
		UE_LOG(LogTemp, Warning, TEXT("PlayerInput is null."));
		return;
	}

	FRotator CurrentPlayerRotation = PlayerController->GetControlRotation();
	FRotator CurrentTargetRotation = FRotator(Pitch, Yaw, 0.f);

	FRotator RotatorDelta = (CurrentTargetRotation - CurrentPlayerRotation).GetNormalized();
	FVector2D Input(RotatorDelta.Pitch, RotatorDelta.Yaw);

	FInputActionValue InputActionValue = FInputActionValue(Input);
	InputSubsystem->InjectInputForAction(PlayerSimulateLookAction, InputActionValue, {}, {});
}

float AGameDriverClassPawn::GetPlayerFOV()
{
	UWorld* World = GetWorld();
	if (!World)
	{
		UE_LOG(LogTemp, Warning, TEXT("World is null."));
		return 0.0f;
	}

	APlayerController* PlayerController = World->GetFirstPlayerController();
	if (!PlayerController)
	{
		UE_LOG(LogTemp, Warning, TEXT("PlayerController is null."));
		return 0.0f;
	}

	APawn* Pawn = PlayerController->GetPawn();
	if (!Pawn)
	{
		UE_LOG(LogTemp, Warning, TEXT("Pawn is null."));
		return 0.0f;
	}

	float FOV = PlayerController->PlayerCameraManager->GetFOVAngle(); // Default FOV if camera manager is not available
	UE_LOG(LogTemp, Warning, TEXT("Player FOV: %f"), FOV);
	return FOV;
}

FRotator AGameDriverClassPawn::FindRotationToActor(FString ActorName)
{
	UE_LOG(LogTemp, Warning, TEXT("FindRotationToActor called with ActorName: %s"), *ActorName);

	if (ActorName.IsEmpty())
	{
		UE_LOG(LogTemp, Warning, TEXT("ActorName is empty."));
		return FRotator();
	}

	UWorld* World = GetWorld();
	if (!World)
	{
		UE_LOG(LogTemp, Warning, TEXT("World is null."));
		return FRotator();
	}

	APlayerController* PlayerController = World->GetFirstPlayerController();
	if (!PlayerController)
	{
		UE_LOG(LogTemp, Warning, TEXT("PlayerController is null."));
		return FRotator();
	}

	APawn* Pawn = PlayerController->GetPawn();
	if (!Pawn)
	{
		UE_LOG(LogTemp, Warning, TEXT("Pawn is null."));
		return FRotator();
	}

	for (TActorIterator<AActor> It(World); It; ++It)
	{
		AActor* Actor = *It;
		if (Actor && Actor->GetName() == ActorName)
		{
			FVector StartingLocation = Pawn->GetActorLocation();
			FVector TargetActorLocation = Actor->GetActorLocation();
			FRotator RotationToTarget = UKismetMathLibrary::FindLookAtRotation(StartingLocation, TargetActorLocation);

			UE_LOG(LogTemp, Warning, TEXT("Found rotation to actor: %s"), *ActorName);
			UE_LOG(LogTemp, Warning, TEXT("RotationToTarget: Pitch=%f, Yaw=%f, Roll=%f"), RotationToTarget.Pitch, RotationToTarget.Yaw, RotationToTarget.Roll);

			return RotationToTarget;
		}	
	}

	return FRotator();
}


// Called when the game starts or when spawned
void AGameDriverClassPawn::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void AGameDriverClassPawn::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

// Called to bind functionality to input
void AGameDriverClassPawn::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

}
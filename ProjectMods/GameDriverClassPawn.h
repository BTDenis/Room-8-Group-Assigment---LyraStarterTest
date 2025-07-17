// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Pawn.h"
#include "GameFramework/Actor.h"
#include "InputAction.h"
#include "GameDriverClassPawn.generated.h"

UCLASS()
class UIEXTENSION_API AGameDriverClassPawn : public APawn
{
	GENERATED_BODY()

public:
	// Sets default values for this pawn's properties
	AGameDriverClassPawn();

	/* GAMEDRIVERAPI_ExecCalls -> Helper Functions to directly interact with the world. */
	UFUNCTION(Exec,			Category = "GAMEDRIVERAPI_ExecCalls") void SpawnTestActor				(FString ActorName, float radiusFromOriginPoint);
	UFUNCTION(Exec,			Category = "GAMEDRIVERAPI_ExecCalls") FRotator FindRotationToActor		(FString ActorName);
	UFUNCTION(Exec,			Category = "GAMEDRIVERAPI_ExecCalls") void InjectMouseInput				(float Pitch, float Yaw);
	UFUNCTION(Exec,			Category = "GAMEDRIVERAPI_ExecCalls") bool RaycastToTestActor			(FString ActorName);
	UFUNCTION(Exec,			Category = "GAMEDRIVERAPI_ExecCalls") void DeleteTestActor				(FString ActorName);
	UFUNCTION(Exec,			Category = "GAMEDRIVERAPI_ExecCalls") float GetPlayerFOV				();

	/* GAMEDRIVERAPI_UI -> Helper functions to manipulate the UI. */
	UFUNCTION(Exec, BlueprintImplementableEvent, Category = "GAMEDRIVERAPI_ExecCalls") void OpenOptionsMenu();
	UFUNCTION(Exec, BlueprintImplementableEvent, Category = "GAMEDRIVERAPI_ExecCalls") void CloseOptionsMenu();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	UPROPERTY(EditDefaultsOnly, Category = "INPUT")	UInputAction* PlayerSimulateLookAction;

public:
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	// Called to bind functionality to input
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

private:

	float const MinSpawnLevel			= 250.0f;
	float const MaxSpawnLevel			= 3000.0f;
	float const MaxRaycastDistance		= 5000.0f;
	float const SphereRadius			= 25.0f;
};

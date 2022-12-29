// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "ProjectileGame/Public/Projectile.h"
#include "TurtlePlayer.generated.h"

UCLASS()
class PROJECTILEGAME_API ATurtlePlayer final : public ACharacter
{
	GENERATED_BODY()

public:
	ATurtlePlayer();

protected:
	
	//Spring arm as camera mount.
	UPROPERTY(VisibleAnywhere, BlueprintReadWrite)
	class USpringArmComponent* SpringArmComponent;
	
	UPROPERTY(VisibleAnywhere, BlueprintReadWrite)
	class UCameraComponent* CameraComponent;
	
	UPROPERTY(EditDefaultsOnly, Category = Projectile)
	TSubclassOf<AProjectile> ProjectileClass;
	
	//Forwards and backwards.
	void MoveForward(float InputAxis);

	//Left and right.
	void MoveRight(float InputAxis);

public:	
	
	UFUNCTION()
	void Fire();

	// Projectile origin offset from the camera location.
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = Gameplay)
	FVector ProjectileOffset;

	UPROPERTY(VisibleAnywhere, Category = Health)
	int Health;
	
	virtual void SetupPlayerInputComponent(class UInputComponent* PlayerInputComponent) override;

};

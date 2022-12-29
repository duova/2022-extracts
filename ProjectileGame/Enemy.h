// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Projectile.h"
#include "GameFramework/Actor.h"
#include "ProjectileGame/TurtlePlayer.h"
#include "Enemy.generated.h"

UCLASS()
class PROJECTILEGAME_API AEnemy final : public AActor
{
	GENERATED_BODY()
	
protected:
	virtual void BeginPlay() override;

	void Fire(FVector TargetLocation, bool bIsLeftBarrel);

public:
	virtual void Tick(float DeltaTime) override;

	AEnemy();

	UPROPERTY(EditDefaultsOnly, Category = Projectile)
	TSubclassOf<AProjectile> ProjectileClass;
	
	UPROPERTY(VisibleAnywhere, Category = Health)
	int Health;

	UPROPERTY(VisibleAnywhere, Category = Projectile)
	ATurtlePlayer* Player;

	ATurtlePlayer* FindPlayer() const;

	float FireTimer;
	bool BarrelSwitch;
};

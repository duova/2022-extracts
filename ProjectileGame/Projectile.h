// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Components/SphereComponent.h"
#include "GameFramework/ProjectileMovementComponent.h"
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Projectile.generated.h"

UCLASS()
class PROJECTILEGAME_API AProjectile final : public AActor
{
	GENERATED_BODY()

protected:
	virtual void BeginPlay() override;
	
public:	
	AProjectile();
	
	UPROPERTY(VisibleDefaultsOnly, Category = Projectile)
	USphereComponent* Collision;
	
	UPROPERTY(VisibleAnywhere, Category = Movement)
	UProjectileMovementComponent* ProjectileMovement;
	
	UPROPERTY(VisibleDefaultsOnly, Category = Projectile)
	UStaticMeshComponent* ProjectileMesh;
	
	UPROPERTY(VisibleDefaultsOnly, Category = Movement)
	UMaterialInstanceDynamic* ProjectileMaterial;

	mutable FVector TargetLocation;

	mutable bool Redirect;

	// Sets projectile velocity.
	void SetProjectileDirection(const FVector& Direction) const;

	virtual void Tick(float DeltaTime) override;
	
	UFUNCTION()
	void OnHit(UPrimitiveComponent* HitComponent, AActor* OtherActor, UPrimitiveComponent* OtherComponent, FVector NormalImpulse, const FHitResult& Hit);
};

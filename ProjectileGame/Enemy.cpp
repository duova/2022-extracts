// Fill out your copyright notice in the Description page of Project Settings.


#include "ProjectileGame/Public/Enemy.h"

#include "Kismet/GameplayStatics.h"
#include "Kismet/KismetMathLibrary.h"

AEnemy::AEnemy()
{
	PrimaryActorTick.bCanEverTick = true;
	
	//Initialize health.
	Health = 100;
}

void AEnemy::BeginPlay()
{
	Super::BeginPlay();

	FireTimer = 0.5f;
}

void AEnemy::Tick(float DeltaTime)
{
	//Rotate to look at turtle.
	if (Player == nullptr)
	{
		Player = FindPlayer();
		return;
	}
	SetActorRotation(UKismetMathLibrary::FindLookAtRotation(GetActorLocation(), Player->GetActorLocation()));

	FireTimer -= DeltaTime;

	if (FireTimer < 0)
	{
		Fire(Player->GetActorLocation(), BarrelSwitch);
		BarrelSwitch = !BarrelSwitch;
		FireTimer = 0.5f;
	}
}

void AEnemy::Fire(const FVector TargetLocation, const bool bIsLeftBarrel)
{
	if (ProjectileClass)
	{
		//Get firing position.
		FVector ProjectileOffset;
		if (bIsLeftBarrel)
		{
			ProjectileOffset.Set(0.0f, -1000.0f, 1000.0f);
		}
		else
		{
			ProjectileOffset.Set(0.0f, 1000.0f, 1000.0f);	
		}
		const FVector ProjectileLocation = GetActorLocation() + FTransform(GetActorRotation()).TransformVector(ProjectileOffset);

		//Get firing direction.
		FRotator ProjectileRotation = GetActorRotation();
		ProjectileRotation.Pitch -= -90;
		
		if (UWorld* World = GetWorld())
		{
			//Fill spawn parameters
			FActorSpawnParameters SpawnParams;
			SpawnParams.Owner = this;
			SpawnParams.Instigator = GetInstigator();
			//Spawn projectile.
			if (const AProjectile* Projectile = World->SpawnActor<AProjectile>(
				ProjectileClass, ProjectileLocation, ProjectileRotation, SpawnParams))
			{
				const FVector LaunchDirection = ProjectileRotation.Vector();
				Projectile->SetProjectileDirection(LaunchDirection);
				Projectile->TargetLocation = TargetLocation;
				Projectile->Redirect = true;
			}
		}
	}
}

ATurtlePlayer* AEnemy::FindPlayer() const
{
	TArray<AActor*> ActorsToFind;

	UGameplayStatics::GetAllActorsOfClass(GetWorld(), ATurtlePlayer::StaticClass(),ActorsToFind);

	for (AActor* Actor: ActorsToFind)
	{
		if (ATurtlePlayer* CastedPlayer = Cast<ATurtlePlayer>(Actor))
		{
			return CastedPlayer;
		}   
	}

	return nullptr;
}





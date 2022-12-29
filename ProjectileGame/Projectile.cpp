// Fill out your copyright notice in the Description page of Project Settings.


#include "ProjectileGame/Public/Projectile.h"

#include "VectorTypes.h"
#include "EnvironmentQuery/EnvQueryGenerator.h"
#include "Kismet/KismetMathLibrary.h"
#include "ProjectileGame/Public/Enemy.h"

// Sets default values
AProjectile::AProjectile()
{
	PrimaryActorTick.bCanEverTick = true;
	
	//Create root component.
	if(!RootComponent)
	{
		RootComponent = CreateDefaultSubobject<USceneComponent>(TEXT("ProjectileSceneComponent"));
	}

	//Create sphere collider.
	if(!Collision)
	{
		Collision = CreateDefaultSubobject<USphereComponent>(TEXT("SphereComponent"));
		Collision->InitSphereRadius(15.0f);
		RootComponent = Collision;
	}

	//Create movement component and set parameters.
	if(!ProjectileMovement)
	{
		ProjectileMovement = CreateDefaultSubobject<UProjectileMovementComponent>(TEXT("ProjectileMovementComponent"));
		ProjectileMovement->SetUpdatedComponent(Collision);
		ProjectileMovement->InitialSpeed = 4000.0f;
		ProjectileMovement->MaxSpeed = 4000.0f;
		ProjectileMovement->ProjectileGravityScale = 0.0f;
	}

	//Set projectile mesh.
	if(!ProjectileMesh)
	{
		ProjectileMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("ProjectileMeshComponent"));
		if(static ConstructorHelpers::FObjectFinder<UStaticMesh> Mesh(TEXT("'/Game/StarterContent/Shapes/Shape_Sphere.Shape_Sphere'")); Mesh.Succeeded())
		{
			ProjectileMesh->SetStaticMesh(Mesh.Object);
		}
	}

	//Set projectile material
	if (static ConstructorHelpers::FObjectFinder<UMaterial> Material(TEXT("'/Game/StarterContent/Materials/M_Metal_Brushed_Nickel.M_Metal_Brushed_Nickel'")); Material.Succeeded())
	{
		ProjectileMaterial = UMaterialInstanceDynamic::Create(Material.Object, ProjectileMesh);
	}
	ProjectileMesh->SetMaterial(0, ProjectileMaterial);
	ProjectileMesh->SetRelativeScale3D(FVector(1, 1, 1));
	ProjectileMesh->SetupAttachment(RootComponent);

	//Bind collision.
	Collision->BodyInstance.SetCollisionProfileName(TEXT("Projectile"));
	Collision->OnComponentHit.AddDynamic(this, &AProjectile::OnHit);

	//Set lifespan of projectile in seconds.
	InitialLifeSpan = 7.0f;
}

void AProjectile::BeginPlay()
{
	Super::BeginPlay();
}

void AProjectile::SetProjectileDirection(const FVector& Direction) const
{
	//Set velocity.
	ProjectileMovement->Velocity = Direction * ProjectileMovement->InitialSpeed;
}

void AProjectile::OnHit(UPrimitiveComponent* HitComponent, AActor* OtherActor, UPrimitiveComponent* OtherComponent, FVector NormalImpulse, const FHitResult& Hit)
{
	if (OtherActor != this)
	{
		AEnemy* Enemy = Cast<AEnemy>(OtherActor);
		ATurtlePlayer* TurtlePlayer = Cast<ATurtlePlayer>(OtherActor);
		if (Enemy != nullptr)
		{
			Enemy->Health -= 5;
		}
		if (TurtlePlayer != nullptr)
		{
			TurtlePlayer->Health -= 5;
		}
	}

	Destroy();
}

void AProjectile::Tick(const float DeltaTime)
{
	//Redirect to target location.
	if (Redirect)
	{
		FVector Direction = GetVelocity();
		Direction.Normalize();
		FVector TargetDirection = TargetLocation - GetActorLocation();
		TargetDirection.Normalize();
		FVector NewDirection = Direction + TargetDirection * DeltaTime * 5;
		NewDirection.Normalize();
		SetProjectileDirection(NewDirection);
	}
}




// Fill out your copyright notice in the Description page of Project Settings.

#include "TurtlePlayer.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/SpringArmComponent.h"
#include "GameFramework/CharacterMovementComponent.h"

// Sets default values
ATurtlePlayer::ATurtlePlayer()
{
	//Initialize health.
	Health = 100;
	
	SpringArmComponent = CreateDefaultSubobject<USpringArmComponent>(TEXT("SpringArm"));

	CameraComponent = CreateDefaultSubobject<UCameraComponent>(TEXT("Camera"));
	
	//Character movement component settings.
	GetCharacterMovement()->bOrientRotationToMovement = true;
	GetCharacterMovement()->bUseControllerDesiredRotation = true;
	GetCharacterMovement()->bIgnoreBaseRotation = true;
	
	//Attach spring arm to mesh.
	SpringArmComponent->SetupAttachment(GetMesh());

	//Attach camera to arm.
	CameraComponent->SetupAttachment(SpringArmComponent,USpringArmComponent::SocketName);

	//Arm follows pawn rotation.
	SpringArmComponent->bUsePawnControlRotation = true;
}

void ATurtlePlayer::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

	//Input binding.
	PlayerInputComponent->BindAxis("MoveForward", this, &ATurtlePlayer::MoveForward);
	PlayerInputComponent->BindAxis("MoveRight", this, &ATurtlePlayer::MoveRight);
	PlayerInputComponent->BindAxis("Turn", this, &APawn::AddControllerYawInput);
	PlayerInputComponent->BindAxis("LookUp", this, &APawn::AddControllerPitchInput);
	PlayerInputComponent->BindAction("Fire", IE_Pressed, this, &ATurtlePlayer::Fire);

}

void ATurtlePlayer::MoveForward(const float InputAxis)
{
	if (Controller == nullptr || InputAxis == 0.0f)
		return;
	//Get yaw.
	const FRotator Rotation = Controller->GetControlRotation();
	const FRotator YawRotation(0, Rotation.Yaw, 0);

	//Translate to direction.
	const FVector Direction = FRotationMatrix(YawRotation).GetUnitAxis(EAxis::X);

	//Add movement in direction.
	AddMovementInput(Direction, InputAxis);
}

void ATurtlePlayer::MoveRight(const float InputAxis)
{
	if (Controller == nullptr || InputAxis == 0.0f)
		return;
	//Get yaw.
	const FRotator Rotation = Controller->GetControlRotation();
	const FRotator YawRotation(0, Rotation.Yaw, 0);

	//Translate to direction.
	const FVector Direction = FRotationMatrix(YawRotation).GetUnitAxis(EAxis::Y);

	//Add movement in direction.
	AddMovementInput(Direction, InputAxis);
}

void ATurtlePlayer::Fire()
{
	if (ProjectileClass)
	{
		//Get firing position.
		FVector CameraLocation;
		FRotator CameraRotation;
		GetActorEyesViewPoint(CameraLocation, CameraRotation);
		ProjectileOffset.Set(100.0f, 0.0f, 0.0f);
		const FVector ProjectileLocation = CameraLocation + FTransform(CameraRotation).TransformVector(ProjectileOffset);

		//Get firing direction.
		FRotator ProjectileRotation = CameraRotation;
		ProjectileRotation.Pitch += 10.0f;
		
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
			}
		}
	}
}

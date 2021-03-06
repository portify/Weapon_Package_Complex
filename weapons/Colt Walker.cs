addDamageType("ColtWalker",
	'<bitmap:Add-Ons/Weapon_Package_Complex/assets/icons/ci_coltwalker> %1',
	'%2 <bitmap:Add-Ons/Weapon_Package_Complex/assets/icons/ci_coltwalker> %1',
	0.2, 1);
addDamageType("ColtWalkerHeadshot",
	'<bitmap:Add-Ons/Weapon_Package_Complex/assets/icons/ci_coltwalker><bitmap:Add-Ons/Weapon_Package_Complex/assets/icons/ci_headshot> %1',
	'%2 <bitmap:Add-Ons/Weapon_Package_Complex/assets/icons/ci_coltwalker><bitmap:Add-Ons/Weapon_Package_Complex/assets/icons/ci_headshot> %1',
	0.2, 1);

datablock AudioProfile(ColtWalkerCockSound)
{
	fileName = "Add-Ons/Weapon_Package_Complex/assets/sounds_west/coltwalker/cock.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(ColtWalkerFireSound)
{
	fileName = "Add-Ons/Weapon_Package_Complex/assets/sounds_west/coltwalker/fire.wav";
	description = AudioClose3d;
	preload = true;
};
datablock AudioProfile(ColtWalkerLeverSound)
{
	fileName = "Add-Ons/Weapon_Package_Complex/assets/sounds_west/coltwalker/lever.wav";
	description = AudioClose3d;
	preload = true;
};

new ScriptObject(WalkerInsertSFX)
{
	class = "SFXEffect";

	file[main, 1] = "Add-Ons/Weapon_Package_Complex/assets/sounds_west/coltwalker/insert.wav";
};

datablock ItemData(ColtWalkerItem)
{
	shapeFile = "Add-Ons/Weapon_Package_Complex/assets/shapes/weapons/colt_walker.dts";
	iconName = "Add-Ons/Weapon_Package_Complex/assets/icons/coltwalker";
	uiName = "Colt Walker";

	canDrop = true;
	image = ColtWalkerImage;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	shellCollisionThreshold = 2;
	shellCollisionSFX = WeaponHardImpactSFX;

	bulletType = ".357";

	itemPropsClass = "RevolverProps";
};

datablock ShapeBaseImageData(ColtWalkerImage)
{
	className = "TimeSliceRayWeapon";
	shapeFile = "Add-Ons/Weapon_Package_Complex/assets/shapes/weapons/colt_walker.dts";

	fireMuzzleVelocity = cf_muzzlevelocity_ms(340);
	fireVelInheritFactor = 0.175;
	fireGravity = cf_bulletdrop_grams(9);
	fireSpread = 0.5;
	fireHitExplosion = GunProjectile;
	fireHitOtherSFX = ComplexDefaultImpactBulletSFX;
	fireRicSFX = ComplexRicSFX;
	fireNearMissSFX = ComplexNearMissSFX;

	item = ColtWalkerItem;
	armReady = true;
	isRevolver = true;
	speedScale = 0.7;

	insertSFX = WalkerInsertSFX;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.4;
	stateAllowImageChange[0] = false;
	stateTransitionOnTimeout[0] = "HammerUp";

	stateName[2] = "Empty";
	stateTransitionOnLoaded[2] = "Ready";
	stateTransitionOnAmmo[2] = "Opening";
	stateTransitionOnTriggerDown[2] = "EmptyFire";

	stateName[3] = "EmptyFire";
	stateSound[3] = RevolverClickSound;
	stateScript[3] = "onEmptyFire";
	stateTimeoutValue[3] = 0.2;
	stateWaitForTimeout[3] = true;
	stateAllowImageChange[3] = false;
	stateTransitionOnTriggerUp[3] = "CheckChamber";

	stateName[4] = "Ready";
	stateTransitionOnNotLoaded[4] = "Empty";
	stateTransitionOnAmmo[4] = "Opening";
	stateTransitionOnTriggerDown[4] = "Fire";
	stateSequence[4] = "";

	stateName[5] = "Fire";
	stateFire[5] = true;
	stateSound[5] = ColtWalkerFireSound;
	stateScript[5] = "onFire";
	stateSequence[5] = "fire";
	stateEmitter[5] = advBigBulletFireEmitter;
	stateEmitterTime[5] = 0.1;
	stateEmitterNode[5] = "muzzleNode";
	stateTimeoutValue[5] = 0.2;
	stateWaitForTimeout[5] = true;
	stateAllowImageChange[5] = false;
	stateTransitionOnTimeout[5] = "Smoke";

	stateName[6] = "Smoke";
	stateEmitter[6] = advBigBulletSmokeEmitter;
	stateEmitterTime[6] = 0.3;
	stateEmitterNode[6] = "muzzleNode";
	stateTimeoutValue[6] = 0.3;
	stateWaitForTimeout[6] = true;
	stateAllowImageChange[6] = false;
	stateTransitionOnTimeout[6] = "HammerUp"; //Alright, delay passed, let's check if the dude released the trigger yet

	stateName[7] = "HammerUp";
	stateTransitionOnAmmo[7] = "Opening";
	stateTransitionOnTriggerDown[7] = "Cock";

	stateName[8] = "Cock";
	stateSound[8] = ColtWalkerCockSound;
	stateSequence[8] = "Clickdown";
	stateTimeoutValue[8] = 0.5;
	stateAllowImageChange[8] = false;
	stateWaitForTimeout[8] = true;
	stateTransitionOnTriggerUp[8] = "CheckChamber";

	stateName[9] = "Opening";
	stateLoadedFlag[9] = "NotLoaded"; // doesn't seem to work
	stateSequence[9] = "Openchamber";
	stateTimeoutValue[9] = 0.1;
	stateAllowImageChange[9] = false;
	stateTransitionOnTimeout[9] = "Opened";
	stateScript[9] = "onOpening";

	stateName[10] = "Opened";
	stateTransitionOnLoaded[10] = "ShellCheck";
	stateTransitionOnNoAmmo[10] = "Closing";

	stateName[11] = "Closing";
	stateSequence[11] = "Closechamber";
	stateTimeoutValue[11] = 0.1;
	stateAllowImageChange[11] = false;
	stateTransitionOnTimeout[11] = "HammerUp";
	stateScript[11] = "onClosing";

	stateName[12] = "ShellCheck";
	stateTransitionOnNoAmmo[12] = "Closing";
	stateTransitionOnNotLoaded[12] = "Opened";
	stateTransitionOnLoaded[12] = "EjectShell";

	stateName[13] = "EjectShell";
	stateScript[13] = "onEjectShell";
	stateSequence[13] = "EjectShell";
	stateTimeoutValue[13] = 0.04;
	stateTransitionOnTimeout[13] = "ShellCheck";

	// stateName[19] = "CheckTrigger";
	// stateTransitionOnTriggerUp[19] = "HammerUp";

	stateName[20] = "CheckChamber";
	stateScript[20] = "onCheckChamber";
	stateTimeoutValue[20] = 0.01;
	stateTransitionOnTimeout[20] = "Empty";
};

function ColtWalkerImage::onMount(%this, %obj, %slot)
{
	RevolverImage::onMount(%this, %obj, %slot);
}

function ColtWalkerImage::onCheckChamber(%this, %obj, %slot)
{
	RevolverImage::onCheckChamber(%this, %obj, %slot);
}

function ColtWalkerImage::onOpening(%this, %obj, %slot)
{
	RevolverImage::onOpening(%this, %obj, %slot);
}

function ColtWalkerImage::onClosing(%this, %obj, %slot)
{
	RevolverImage::onClosing(%this, %obj, %slot);
}

function ColtWalkerImage::onTrigger(%this, %obj, %slot, %trigger, %state)
{
	RevolverImage::onTrigger(%this, %obj, %slot, %trigger, %state);
}

function ColtWalkerImage::onLight(%this, %obj, %slot)
{
	%state = %obj.getImageState(%slot);
	if (%state $= "Ready" || %state $= "Empty" || %state $= "opened" || %state $= "HammerUp")
	{
		%props = %obj.getItemProps();

		%obj.setImageAmmo(%slot, !%obj.getImageAmmo(%slot));
		%obj.setImageLoaded(%slot, false);
	}
	return 1; //We kinda HAVE to do this so you don't turn on light while reloading. You still can't turn on light when in Ready state anyway.
}


function ColtWalkerImage::onEmptyFire(%this, %obj, %slot)
{
	RevolverImage::onEmptyFire(%this, %obj, %slot);
}

function ColtWalkerImage::onFire(%this, %obj, %slot)
{
	if (%obj.getState() $= "Dead")
		return;

	if(!%obj.suiciding) //If we're not firing through suicide
		Parent::onFire(%this, %obj, %slot);
	%obj.suiciding = ""; //reset the var, just in case
	%props = %obj.getItemProps();
	%props.slot[%props.currSlot] = 2;
	%props.currSlot = (%props.currSlot + 1) % 6;

	%obj.playThread(2, "shiftAway");
	%obj.schedule(100, playThread, 2, "plant");

	%obj.applyComplexKnockback(2.5);
	%obj.applyComplexScreenshake(2);

	if (isObject(%obj.client))
		%obj.client.updateDetailedGunHelp();
}

function ColtWalkerImage::onSuicide(%this, %obj, %slot)
{
	%image = %obj.getMountedImage(%slot);
	%state = %obj.getImageState(%slot);
	if(%state !$= "Ready" && %state !$= "Empty")
	{
		return 1;
	}
	%props = %obj.getItemProps();
	if(%props.slot[%props.currSlot] != 1) //Either spent or empty
	{
		%obj.setImageTrigger(%slot, 1);
		%obj.playThread(2, plant);
		serverPlay3d(RevolverClickSound, %obj.getHackPosition());
	}
	else
	{
		//What you're about to see below is probably the ugliest thing I ever coded. ~Jack Noir
		%obj.playThread(2, shiftRight);
		%obj.playThread(3, shiftLeft);
		%obj.applyComplexKnockback(5);
		%props.slot[%props.currSlot] = 2;
		serverplay3d(ColtWalkerFireSound, %obj.getMuzzlePoint(%slot));
		%obj.suiciding = 1;
		%obj.setImageTrigger(%slot, 1);
		%proj = new ScriptObject()
		{	
			class = "ProjectileRayCast";
			superClass = "TimedRayCast";

			position = %obj.getEyePoint();
			velocity = "0 0 0";

			lifetime = %this.fireLifetime;
			gravity = %this.fireGravity;

			mask = %this.fireMask $= "" ? $TypeMasks::PlayerObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType : %this.fireMask;
			exempt = "";
			sourceObject = %obj;
			sourceClient = %obj.client;
			damage = %this.directDamage;
			damageType = %this.directDamageType;
			damageRef = %this;
			hitExplosion = %this.projectile;
		};
		MissionCleanup.add(%proj);
		%obj.setDamageLevel(%obj.getDataBlock().maxDamage - 1); //Set their HP to 1 so headshot will be a guaranteed instakill
		%proj.fire();
	}
	return 1;
}


function ColtWalkerImage::onEjectShell(%this, %obj, %slot)
{
	RevolverImage::onEjectShell(%this, %obj, %slot);
}

function ColtWalkerImage::damage(%this, %obj, %col, %position, %normal)
{
	if (%col.getRegion(%position, true) $= "head")
	{
		ComplexHeadshotSFX.playFrom(%position, %col);

		%damage = 225;
		%damageType = $DamageType::ColtWalkerHeadshot;
	}
	else
	{
		ComplexFleshImpactBulletSFX.playFrom(%position, %col);

		%damage = 75;
		%damageType = $DamageType::ColtWalker;
	}

	if (!$NoCrouchDamageBonus && %col.isCrouched())
		%damage /= 2;

	%col.damage(%obj, %position, %damage, %damageType);
}

function ColtWalkerImage::getGunHelp(%this, %obj, %slot)
{
	return RevolverImage::getGunHelp(%this, %obj, %slot);
}

function ColtWalkerImage::getDetailedGunHelp(%this, %obj, %slot, %hidden)
{
	return RevolverImage::getDetailedGunHelp(%this, %obj, %slot, %hidden);
}

function ColtWalkerImage::InsertShell(%this, %obj, %slot)
{
	RevolverImage::InsertShell(%this, %obj, %slot);
}
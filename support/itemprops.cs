function Player::getItemProps(%this, %image, %slot)
{
    if (%this.currTool == -1 || !isObject(%image.item) ||
        %this.getMountedImage(%slot) != %image.getID() ||
        %image.item.getID() != %this.tool[%this.currTool])
    {
        return 0;
    }

    if (!isObject(%this.itemProps[%this.currTool]))
        %this.itemProps[%this.currTool] = %image.item.newItemProps(%this);

    return %this.itemProps[%this.currTool];
}

function ItemData::newItemProps(%this, %player)
{
    %props = new ScriptObject()
    {
        class = %this.itemPropsClass;
        superClass = "ItemProps";

        sourceItemData = %this;
        sourcePlayer = %player;
        sourceClient = %player.client;

        itemSlot = %player.currTool;
    };

    %props.onOwnerChange(%player);
    return %props;
}

function ItemProps::onOwnerChange(%this, %owner)
{
    %this.owner = %owner;
}

if (!isFunction("ItemData", "onRemove"))
    eval("function ItemData::onRemove(){}");

package ItemPropsPackage
{
    function Player::setDataBlock(%this, %data)
    {
        %maxTools = %this.getDataBlock().maxTools;

        for (%i = %data.maxTools; %i < %maxTools; %i++)
        {
            if (isObject(%this.itemProps[%i]))
                %this.itemProps[%i].delete();
        }

        Parent::setDataBlock(%this, %data);
    }

    function Player::mountImage(%this, %image, %slot, %y, %z)
    {
        Parent::mountImage(%this, %image, %slot, %y, %z);

        if (%this.getMountedImage(%slot).item.itemPropsAlways)
            %this.getItemProps(%this.getMountedImage(%slot), %slot);
    }

    function Armor::onRemove(%this, %player)
    {
        Parent::onRemove(%this, %player);

        for (%i = 0; %i < %this.maxTools; %i++)
        {
            if (isObject(%player.itemProps[%i]))
                %player.itemProps[%i].delete();
        }
    }

    function ItemData::onAdd(%this, %obj)
    {
        Parent::onAdd(%this, %obj);

        if (isObject($DroppedItemProps))
        {
            %obj.itemProps = $DroppedItemProps;
            $DroppedItemProps.onOwnerChange(%obj);
            $DroppedItemProps = "";
        }
    }

    function ItemData::onRemove(%this, %obj)
    {
        Parent::onRemove(%this, %obj);

        if (isObject(%obj.itemProps))
            %obj.itemProps.delete();
    }

    function serverCmdDropTool(%client, %index)
    {
        %player = %client.player;

        if (isObject(%player.tool[%index]) && isObject(%player.itemProps[%index]))
            $DroppedItemProps = %player.itemProps[%index];

        Parent::serverCmdDropTool(%client, %index);

        if (!isObject(%player.tool[%index]) && isObject(%player.itemProps[%index]))
        {
            if (isObject($DroppedItemProps))
                $DroppedItemProps.delete();
            else
                $DroppedItemProps = "";
        }
    }

    function Armor::onCollision(%this, %obj, %col, %velocity, %speed)
    {
        if (%obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1 &&
            %col.getClassName() $= "Item" && isObject(%col.itemProps))
        {
            if (%col.canPickup == 0)
                return;

            %client = %obj.client;
            %data = %col.getDataBlock();

            if (!isObject(%client))
                return;

            %miniGame = %client.miniGame;

            if (isObject(%miniGame) && %miniGame.WeaponDamage == 1)
            {
                if (getSimTime() - %client.lastF8Time < 5000)
                    return;
            }

            for (%i = 0; %i < %this.maxTools; %i++)
            {
                if (!isObject(%obj.tool[%i]))
                    break;

                if (%obj.tool[%i] == %data)
                    return;
            }

            if (%i == %this.maxTools)
                return;

            if (miniGameCanUse(%obj, %col) == 0)
            {
                if (isObject(%col.spawnBrick))
                    %ownerName = %col.spawnBrick.getGroup().name;

                if ($lastError == $LastError::MiniGameDifferent)
                {
                    if (isObject(%client.miniGame))
                        %msg = "This item is not part of the mini-game.";
                    else
                        %msg = "This item is part of a mini-game.";
                }
                else if ($lastError == $LastError::MiniGameNotYours)
                    %msg = "You do not own this item.";
                else if ($lastError == $LastError::NotInMiniGame)
                    %msg = "This item is not part of the mini-game.";
                else
                    %msg = %ownerName @ " does not trust you enough to use this item.";

                commandToClient(%client, 'CenterPrint', %msg, 1);
                return;
            }

            %obj.tool[%i] = %data;
            %obj.itemProps[%i] = %col.itemProps;

            // improve this later
            %col.itemProps.itemSlot = %i;
            %col.itemProps.onOwnerChange(%obj);
            %col.itemProps = "";

            messageClient(%client, 'MsgItemPickup', '', %i, %data);

            if (%col.isStatic())
                %col.Respawn();
            else
                %col.delete();

            return;
        }

        Parent::onCollision(%this, %obj, %col, %velocity, %speed);
    }
};

activatePackage("ItemPropsPackage");

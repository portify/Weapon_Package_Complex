function ShapeBaseImageData::onTrigger(%this, %obj, %slot, %trigger, %state)
{
    return 0;
}

function ShapeBaseImageData::onLight(%this, %obj, %slot)
{
    return 0;
}

package ImageTriggerPackage
{
    function serverCmdLight(%client)
    {
        %player = %client.player;

        if (isObject(%player) && !isObject(%player.light))
        {
            for (%i = 0; %i < 4; %i++)
            {
                %image = %player.getMountedImage(%i);

                if (isObject(%image) && %image.onLight(%player, %i))
                    %stop = 1;
            }
        }

        if (!%stop)
            Parent::serverCmdLight(%client);
    }

    function Armor::onTrigger(%this, %obj, %slot, %state)
    {
        for (%i = 0; %i < 4; %i++)
        {
            %image = %obj.getMountedImage(%i);

            if (isObject(%image) && %image.onTrigger(%obj, %i, %slot, %state))
                %stop = 1;
        }

        if (!%stop)
            Parent::onTrigger(%this, %obj, %slot, %state);
    }
};

activatePackage("ImageTriggerPackage");

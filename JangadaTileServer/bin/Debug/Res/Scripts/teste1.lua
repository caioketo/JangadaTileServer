function Interval(creature)
	if (creature ~= nil) then
		return creature.Name
	end
	return scriptManager:Path()
end

function Cast(creature)
	scriptManager:AddTask(1000, Interval, creature)
    return 1
end
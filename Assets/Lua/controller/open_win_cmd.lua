--[[
	open window command
--]]
local OpenWindowCommand = class("OpenWindowCommand", ICommand)

function OpenWindowCommand:execute(notification)
	local name = notification._body.name
	local mediator = facade:retrieveMediator(name)
	if mediator ~= nil then
		guiMgr:ShowWindow(name, nil)
		if mediator.init then
			if notification._body.data == nil then notification._body.data = {} end
			mediator:init(unpack(notification._body.data))
		end
	else
		print("OpenWindowCommand: error: mediator is nil, name =", notification._body.name)
	end
end

return OpenWindowCommand
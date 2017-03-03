--[[
	tip window
--]]
local TipCommand = class("TipCommand", ICommand)

function TipCommand:ctor()
end

function TipCommand:execute(notification)
	local mediator = facade:retrieveMediator("PanelTip")
	if mediator ~= nil then
		mediator:show_tip(unpack(notification._body.data))
	else
		print("OpenWindowCommand: error: mediator is nil, name =", notification._body.name)
	end
end

return TipCommand
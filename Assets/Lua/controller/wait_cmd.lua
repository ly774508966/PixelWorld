--[[
	Wait window
--]]
local WaitCommand = class("WaitCommand", ICommand)

function WaitCommand:ctor()
end

function WaitCommand:execute(notification)
	local mediator = facade:retrieveMediator("PanelWait")
	if notification._body.name == "show" then
		print("... show wait ...")
		mediator:show()
	elseif notification._body.name == "hide" then 
		print("... hide wait ...")
		mediator:hide()
	end
end

return WaitCommand
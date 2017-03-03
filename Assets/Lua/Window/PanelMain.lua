local gameObject
local transform

PanelMain = {}
local this = PanelMain
this._name = 'PanelMain'

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelMain.Awake(obj)
	gameObject = obj
	transform = obj.transform
end

function PanelMain.OnDestroy()
	gameObject = nil
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelMain:listNotificationInterests()
	return {}
end
function PanelMain:handleNotification(notification)
end

return PanelMain
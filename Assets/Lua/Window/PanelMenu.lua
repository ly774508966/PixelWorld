local gameObject
local transform

PanelMenu = {}
local this = PanelMenu
this._name = "PanelMenu"

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelMenu.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()


    --facade:sendNotification(WAIT, {name="show"})
end
function PanelMenu.OnDestroy()
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelMenu:listNotificationInterests()
	return {}
end
function PanelMenu:handleNotification(notification)
end

--初始化面板--
function PanelMenu.InitPanel()
	local btnAlert = transform:FindChild("Button Alert").gameObject
	local btnBag = transform:FindChild("Button Bag").gameObject
	local btnEquip = transform:FindChild("Button Equip").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btnAlert, this.OnBtnAlert)
	window:AddClick(btnBag, this.OnBtnBag)
	window:AddClick(btnEquip, this.OnBtnEquip)
end

--单击事件--

function PanelMenu.OnBtnAlert(go)
	print('OnBtnAlert')
    facade:sendNotification(WAIT, {name="show"})
end

function PanelMenu.OnBtnBag(go)
	print('OnBtnBag')
    facade:sendNotification(OPEN_WINDOW, {name="PanelBag"})
end

function PanelMenu.OnBtnEquip(go)
	print('OnBtnEquip')
    facade:sendNotification(OPEN_WINDOW, {name="PanelEquip"})
end

return PanelMenu
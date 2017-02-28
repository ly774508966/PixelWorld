local gameObject
local transform

PanelEquip = {}
local this = PanelEquip
this._name = "PanelEquip"


-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelEquip.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
end

function PanelEquip.OnDestroy()
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelEquip:listNotificationInterests()
	return {}
end
function PanelEquip:handleNotification(notification)
end

--初始化面板--
function PanelEquip.InitPanel()
	local btn_close = transform:FindChild("Button Close").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btn_close, this.OnBtnClose)
end


-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
--单击事件
function PanelEquip.OnBtnClose(go)
	print('OnBtnClose')
	guiMgr:HideWindow(gameObject)
end

return PanelEquip
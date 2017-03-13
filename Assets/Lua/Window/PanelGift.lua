local gameObject
local transform

PanelGift = {}
local this = PanelGift
this._name = "PanelGift"


-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelGift.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
end

function PanelGift.OnDestroy()
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelGift:listNotificationInterests()
	return {}
end
function PanelGift:handleNotification(notification)
end

--初始化面板--
function PanelGift.InitPanel()
	local btn_close = transform:FindChild("Button Close").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btn_close, this.OnBtnClose)
end


-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
--单击事件
function PanelGift.OnBtnClose(go)
	print('OnBtnClose')
	guiMgr:HideWindow(gameObject)
end

return PanelGift
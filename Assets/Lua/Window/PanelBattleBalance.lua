local gameObject
local transform

PanelBattleBalance = {}
local this = PanelBattleBalance
this._name = "PanelBattleBalance"


-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelBattleBalance.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
end

function PanelBattleBalance.OnDestroy()
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelBattleBalance:listNotificationInterests()
	return {}
end
function PanelBattleBalance:handleNotification(notification)
end

--初始化面板--
function PanelBattleBalance.InitPanel()
	local btn_close = transform:FindChild("Button OK").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btn_close, this.OnBtnClose)
end


-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
--单击事件
function PanelBattleBalance.OnBtnClose(go)
	print('OnBtnClose')
	guiMgr:HideWindow(gameObject)

    sceneMgr:GotoScene(SceneID.Main)
end

return PanelBattleBalance
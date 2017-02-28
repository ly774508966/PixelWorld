local gameObject
local transform

PanelLogin = {}
local this = PanelLogin
this._name = "PanelLogin"

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelLogin.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
end
function PanelLogin.OnDestroy()
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelLogin:listNotificationInterests()
	return {}
end
function PanelLogin:handleNotification(notification)
end


--初始化面板--
function PanelLogin.InitPanel()
	local btn_login = transform:FindChild("Button Login").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btn_login, this.OnBtnLogin)
end


-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
-- login
function PanelLogin.OnBtnLogin(go)
	print('OnBtnLogin')
	Network.login('AdamWu', '123456')
end

return PanelLogin
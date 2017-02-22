local gameObject
local transform

PanelLogin = {}
local this = PanelLogin


--启动事件--
function PanelLogin.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
	print("Awake--->>")
end


--初始化面板--
function PanelLogin.InitPanel()
	local btn_login = transform:FindChild("Button Login").gameObject

	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(btn_login, this.OnBtnLogin)
end


--单击事件--
function PanelLogin.OnDestroy()
	print("OnDestroy---->>>")
end

function PanelLogin.OnBtnLogin(go)
	print('OnBtnLogin')
	Network.login('AdamWu', '123456')
end
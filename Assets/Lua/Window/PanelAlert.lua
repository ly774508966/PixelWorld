local gameObject
local transform

PanelAlert = {}
local this = PanelAlert


--启动事件--
function PanelAlert.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
	print("Awake--->>")
end

function PanelAlert.setTitleMsg(title, msg)
	this.title = title
	this.msg = msg
end

--初始化面板--
function PanelAlert.InitPanel()
	this.btn_ok = transform:FindChild("Button OK").gameObject
	this.btn_cancel = transform:FindChild("Button Cancel").gameObject

	local text_title = transform:FindChild("Text Title"):GetComponent('Text')
	local text_msg = transform:FindChild("Text Msg"):GetComponent("Text")
	text_title.text = this.title
	text_msg.text = this.msg


	window = transform:GetComponent('LuaBehaviour')

	window:AddClick(this.btn_ok, this.OnBtnOK)
	window:AddClick(this.btn_cancel, this.OnBtnCancel)
end


--单击事件--
function PanelAlert.OnDestroy()
	print("OnDestroy---->>>")
end

function PanelAlert.OnBtnOK(go)
	print('OnBtnOK')
	guiMgr:HideWindow(gameObject)
end

function PanelAlert.OnBtnCancel(go)
	print('OnBtnCancel')
	guiMgr:HideWindow(gameObject)
end
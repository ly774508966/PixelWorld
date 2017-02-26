local gameObject
local transform

PanelItemDetail = {}
local this = PanelItemDetail
this._name = 'PanelItemDetail'


-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelItemDetail.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
	print("Awake--->>")
end
function PanelItemDetail.OnDestroy()
	print("OnDestroy---->>>")
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelItemDetail:listNotificationInterests()
	return {BAG_SELL_OK}
end
function PanelItemDetail:handleNotification(notification)
	if notification._name == BAG_SELL_OK then
		local data = notification._body
		print (data)
		self:OnSellOK(data)
	end
end


--初始化面板--
function PanelItemDetail.InitPanel()
	local btn_close = transform:FindChild("Button Close").gameObject
	local btn_ok = transform:FindChild("Button OK").gameObject
	local btn_sell = transform:FindChild("Button Sell").gameObject

	window = transform:GetComponent('LuaBehaviour')
	window:AddClick(btn_close, this.OnBtnClose)
	window:AddClick(btn_ok, this.OnBtnClose)
	window:AddClick(btn_sell, this.OnBtnSell)
end

function PanelItemDetail:OnSellOK(id)
	print("OnSellOK", id)

	guiMgr:HideWindow(gameObject)
end

-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
-- close
function PanelItemDetail.OnBtnClose(go)
	print('OnBtnClose')
	guiMgr:HideWindow(gameObject)
end
-- sell
function PanelItemDetail.OnBtnSell(go)
	print('OnBtnSell')
	Network.sell(1)
end

return PanelItemDetail
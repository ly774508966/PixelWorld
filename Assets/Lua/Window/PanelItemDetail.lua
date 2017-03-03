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
end
function PanelItemDetail.OnDestroy()
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
		this.OnSellOK(data)
	end
end
function PanelItemDetail:init( ... )
	self.itemid = ...

	this.InitPanel()
end

--初始化面板--
function PanelItemDetail.InitPanel()
	local btn_close = transform:Find("Button Close").gameObject
	local btn_ok = transform:Find("Button OK").gameObject
	local btn_sell = transform:Find("Button Sell").gameObject

	window = transform:GetComponent('LuaBehaviour')
	window:AddClick(btn_close, this.OnBtnClose)
	window:AddClick(btn_ok, this.OnBtnClose)
	window:AddClick(btn_sell, this.OnBtnSell)

	local cfg = CFG.items[tostring(this.itemid)]

	local img_face = transform:Find("Image Face"):GetComponent("Image")
	local img_q = transform:Find("Image Quality"):GetComponent("Image")
	local text_title = transform:Find("Text Title"):GetComponent("Text")
	local text_desc = transform:Find("Text Msg"):GetComponent("Text")
    img_face.sprite = resMgr:LoadSprite('Icon/'..tostring(cfg.icon))
	img_q.sprite = resMgr:LoadPackSprite('Sprite/Public/item_q_'..tostring(cfg.quality))
	text_title.text = cfg.name
	text_desc.text = cfg.desc
end

function PanelItemDetail.OnSellOK(id)
	print("OnSellOK", id)

	guiMgr:HideWindow(gameObject)
end

-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
-- close
function PanelItemDetail.OnBtnClose(go)
	guiMgr:HideWindow(gameObject)
end
-- sell
function PanelItemDetail.OnBtnSell(go)
	print('OnBtnSell', this.itemid)
	Network.sell(this.itemid)
end

return PanelItemDetail
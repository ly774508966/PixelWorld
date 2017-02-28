local GameObject = UnityEngine.GameObject
local gameObject
local transform

PanelBag = {}
local this = PanelBag
this._name = "PanelBag"


-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelBag.Awake(obj)
	gameObject = obj
	transform = obj.transform

	this.InitPanel()
end

function PanelBag.OnDestroy()
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelBag:listNotificationInterests()
	return {}
end
function PanelBag:handleNotification(notification)
end

--初始化面板--
function PanelBag.InitPanel()
	local btn_close = transform:FindChild("Button Close").gameObject
	local content = transform:Find('Scroll View/Viewport/Content')

	window = transform:GetComponent('LuaBehaviour')
	window:AddClick(btn_close, this.OnBtnClose)


	local prefab = resMgr:LoadAsset('UI/Widget/BagItem')
	local count = 100
	local itemHeight = 100
	for i = 0, count-1 do
		local go = GameObject.Instantiate(prefab)
		go.name = tostring(i)
		go.transform:SetParent(content)
		go.transform.localScale = Vector3.one
		local rt = go:GetComponent('RectTransform')
		rt.anchorMax = Vector2.New(0.5, 1)
		rt.anchorMin = Vector2.New(0.5, 1)
		rt.anchoredPosition = Vector2.New((i%5)*itemHeight-200, -math.floor(i/5)*itemHeight-itemHeight/2)
		go:AddComponent(typeof(UnityEngine.UI.Button))
        window:AddClick(go, this.OnBtnItem)

	    local label = go.transform:FindChild('Text')
	    label:GetComponent('Text').text = tostring(i)
	end
	content:GetComponent('RectTransform').sizeDelta = Vector2.New(0, math.floor(count/5)*100)
end


-- --------------------------------------------------------------------
--	click event
-----------------------------------------------------------------------
function PanelBag.OnBtnItem(go)
	print('OnItemClick', go.name)
	PanelItemDetail.itemid = tonumber(go.name)
	guiMgr:ShowWindow('PanelItemDetail', nil)
end

function PanelBag.OnBtnClose(go)
	print('OnBtnClose')
	guiMgr:HideWindow(gameObject)
end


return PanelBag

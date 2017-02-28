-- 背包数据
local BagProxy = class("BagProxy", IProxy)
local TAG = "BagProxy"

function BagProxy:ctor()
	self._proxyName = "BagProxy"
end

-- 解析
function BagProxy:parse(data)
	print(TAG, "parseData")

	self._items = {}
	
	
	if data then
		for _, v in ipairs(data) do
			if tonumber(v["num"]) > 0 then
				self._items[v["id"]] = {num=tonumber(v["num"])}
			end
		end
	end
	print(inspect(self._items))
end


--  获取所有物品
function BagProxy:getItems()
	return self._items
end

function BagProxy:getItem(itemid)
	local itemid = tonumber(itemid)
	return self._items[itemid]
end

function BagProxy:getItemNum(itemid)
	local itemid = tonumber(itemid)
	if self._items[itemid] then
		return self._items[itemid]["num"]
	else
		return 0
	end
end

function BagProxy:addItem(itemid, num)
	print(TAG, "addItem", itemid, num)
	local itemid = tonumber(itemid)
	local num = tonumber(num)
	
	if not self._items[itemid] then 
		self._items[itemid] = {num=0} 
	end

	self._items[itemid]["num"] = self._items[itemid]["num"] + num
end

function BagProxy:delItem(itemid, num)
	print(TAG, "delItem", itemid, num)
	local itemid = tonumber(itemid)
	local num = tonumber(num)
	
	if self._items[itemid] then 
		self._items[itemid]["num"] = self._items[itemid]["num"] - num
		if self._items[itemid]["num"] < 0 then
			self._items[itemid]["num"] = 0
		end
	end
end

function BagProxy:addItems(data)
	print(TAG, "addItems", #data)
	if data then
		for _, v in ipairs(data) do
			if type(v) == "table" then
				self:addItem(v["id"], v["num"])
			end
		end
	end
end

function BagProxy:delItems(data)
	print(TAG, "delItems", #data)
	if data then
		for _, v in ipairs(data) do
			if type(v) == "table" then
				self:delItem(v["id"], v["num"])
			end
		end
	end
end

return BagProxy
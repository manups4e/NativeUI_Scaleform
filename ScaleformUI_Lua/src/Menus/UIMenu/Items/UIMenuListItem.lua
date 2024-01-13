UIMenuListItem = setmetatable({}, UIMenuListItem)
UIMenuListItem.__index = UIMenuListItem
UIMenuListItem.__call = function() return "UIMenuItem", "UIMenuListItem" end

---@class UIMenuListItem : UIMenuItem
---@field public Base UIMenuItem
---@field public AddPanel fun(self:UIMenuListItem, item:UIMenuStatisticsPanel|UIMenuPercentagePanel|UIMenuColorPanel|UIMenuGridPanel):nil

---New
---@param Text string
---@param Items table
---@param Index number
---@param Description string
function UIMenuListItem.New(Text, Items, Index, Description, color, highlightColor, textColor, highlightedTextColor)
    if type(Items) ~= "table" then Items = {} end
    if Index == 0 then Index = 1 end
    local _UIMenuListItem = {
        Base = UIMenuItem.New(Text or "", Description or "", color or SColor.HUD_Panel_light, highlightColor or SColor.HUD_White, textColor or SColor.HUD_White, highlightedTextColor or SColor.HUD_Black),
        Items = Items,
        _Index = tonumber(Index) or 1,
        Panels = {},
        SidePanel = nil,
        ItemId = 1,
        OnListChanged = function(menu, item, newindex)
        end,
        OnListSelected = function(menu, item, newindex)
        end,
    }
    return setmetatable(_UIMenuListItem, UIMenuListItem)
end

function UIMenuListItem:ItemData(data)
    if data == nil then
        return self.Base._itemData
    else
        self.Base._itemData = data
    end
end

---SetParentMenu
---@param Menu table
function UIMenuListItem:SetParentMenu(Menu)
    if Menu ~= nil and Menu() == "UIMenu" then
        self.Base.ParentMenu = Menu
    else
        return self.Base.ParentMenu
    end
end

function UIMenuListItem:LabelFont(fontTable)
    if fontTable == nil then
        return self.Base:LabelFont()
    else
        self.Base:LabelFont(fontTable)
    end
end

-- not supported on Lobby and Pause menu yet
function UIMenuListItem:RightLabelFont(fontTable)
    if fontTable == nil then
        return self.Base:RightLabelFont()
    else
        self.Base:RightLabelFont(fontTable)
    end
end

function UIMenuListItem:AddSidePanel(sidePanel)
    if sidePanel() == "UIMissionDetailsPanel" then
        sidePanel:SetParentItem(self)
        self.SidePanel = sidePanel
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("ADD_SIDE_PANEL_TO_ITEM",
                self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)), 0,
                sidePanel.PanelSide, sidePanel.TitleType,
                sidePanel.Title,
                sidePanel.TitleColor, sidePanel.TextureDict, sidePanel.TextureName)
        end
    elseif sidePanel() == "UIVehicleColorPickerPanel" then
        sidePanel:SetParentItem(self)
        self.SidePanel = sidePanel
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("ADD_SIDE_PANEL_TO_ITEM",
                IndexOf(self.Base.ParentMenu.Items, self), 1, sidePanel.PanelSide, sidePanel.TitleType, sidePanel.Title,
                sidePanel.TitleColor)
        end
    end
end

---Selected
---@param bool boolean
function UIMenuListItem:Selected(bool)
    if bool ~= nil then
        self.Base:Selected(ToBool(bool), self)
    else
        return self.Base._Selected
    end
end

---Hovered
---@param bool boolean
function UIMenuListItem:Hovered(bool)
    if bool ~= nil then
        self.Base._Hovered = ToBool(bool)
    else
        return self.Base._Hovered
    end
end

---Enabled
---@param bool boolean
function UIMenuListItem:Enabled(bool)
    if bool ~= nil then
        self.Base:Enabled(bool, self)
    else
        return self.Base._Enabled
    end
end

---Description
---@param str string
function UIMenuListItem:Description(str)
    if tostring(str) and str ~= nil then
        self.Base:Description(str, self)
    else
        return self.Base._Description
    end
end

function UIMenuListItem:BlinkDescription(bool)
    if bool ~= nil then
        self.Base:BlinkDescription(bool, self)
    else
        return self.Base:BlinkDescription()
    end
end

---Text
---@param Text string
function UIMenuListItem:Label(Text)
    if tostring(Text) and Text ~= nil then
        self.Base:Label(tostring(Text), self)
    else
        return self.Base:Label()
    end
end

function UIMenuListItem:MainColor(color)
    if (color) then
        self.Base._mainColor = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS",
                self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor)
        end
    else
        return self.Base._mainColor
    end
end

function UIMenuListItem:TextColor(color)
    if (color) then
        self.Base._textColor = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS",
                self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor)
        end
    else
        return self.Base._textColor
    end
end

function UIMenuListItem:HighlightColor(color)
    if (color) then
        self.Base._highlightColor = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS",
                self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor)
        end
    else
        return self.Base._highlightColor
    end
end

function UIMenuListItem:HighlightedTextColor(color)
    if color then
        self.Base._highlightedTextColor = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS",
                self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor)
        end
    else
        return self.Base._highlightedTextColor
    end
end

---Index
---@param Index number
function UIMenuListItem:Index(Index)
    if tonumber(Index) then
        if Index > #self.Items then
            self._Index = 1
        elseif Index < 1 then
            self._Index = #self.Items
        else
            self._Index = Index
        end
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("SET_ITEM_VALUE",
                self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self._Index - 1)
        end
    else
        return self._Index
    end
end

---ItemToIndex
---@param Item table
function UIMenuListItem:ItemToIndex(Item)
    for i = 1, #self.Items do
        if type(Item) == type(self.Items[i]) and Item == self.Items[i] then
            return i
        elseif type(self.Items[i]) == "table" and (type(Item) == type(self.Items[i].Name) or type(Item) == type(self.Items[i].Value)) and (Item == self.Items[i].Name or Item == self.Items[i].Value) then
            return i
        end
    end
end

---IndexToItem
---@param Index number
function UIMenuListItem:IndexToItem(Index)
    if tonumber(Index) then
        if tonumber(Index) == 0 then Index = 1 end
        if self.Items[tonumber(Index)] then
            return self.Items[tonumber(Index)]
        end
    end
end

---LeftBadge
function UIMenuListItem:LeftBadge(Badge)
    if tonumber(Badge) then
        self.Base:LeftBadge(Badge, self)
    else
        return self.Base:LeftBadge()
    end
end

---RightBadge
function UIMenuListItem:RightBadge()
    error("This item does not support badges")
end

---RightLabel
function UIMenuListItem:RightLabel()
    error("This item does not support a right label")
end

---AddPanel
---@param panel UIMenuStatisticsPanel|UIMenuPercentagePanel|UIMenuColorPanel|UIMenuGridPanel
function UIMenuListItem:AddPanel(panel)
    if panel() == "UIMenuPanel" then
        self.Panels[#self.Panels + 1] = panel
        ---@diagnostic disable-next-line: param-type-mismatch
        panel:SetParentItem(self)
    end
end

---RemovePanelAt
---@param Index table
function UIMenuListItem:RemovePanelAt(Index)
    if tonumber(Index) then
        if self.Panels[Index] then
            table.remove(self.Panels, tonumber(Index))
        end
    end
end

---FindPanelIndex
---@param Panel table
function UIMenuListItem:FindPanelIndex(Panel)
    if Panel() == "UIMenuPanel" then
        for Index = 1, #self.Panels do
            if self.Panels[Index] == Panel then
                return Index
            end
        end
    end
    return nil
end

---FindPanelItem
function UIMenuListItem:FindPanelItem()
    for Index = #self.Items, 1, -1 do
        if self.Items[Index].Panel then
            return Index
        end
    end
    return nil
end

function UIMenuListItem:ChangeList(list)
    if type(list) ~= "table" then return end
    self.Items = {}
    self.Items = list
    if self.Base.ParentMenu:Visible() and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
        ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_LISTITEM_LIST",
            self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
            table.concat(self.Items, ","), self._Index - 1)
    end
end

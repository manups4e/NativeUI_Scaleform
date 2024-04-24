UIMenuStatsItem = setmetatable({}, UIMenuStatsItem)
UIMenuStatsItem.__index = UIMenuStatsItem
UIMenuStatsItem.__call = function() return "UIMenuItem", "UIMenuStatsItem" end

---@class UIMenuStatsItem : UIMenuItem
---@field public Base UIMenuItem

---New
---@param Text string
---@param Description string
---@param Index number|0
---@param barColor SColor
---@param type number|0
---@param mainColor SColor
---@param highlightColor SColor
---@param textColor SColor
---@param highlightedTextColor SColor
function UIMenuStatsItem.New(Text, Description, Index, barColor, type, mainColor, highlightColor, textColor, highlightedTextColor)
    local _UIMenuStatsItem = {
        Base = UIMenuItem.New(Text or "", Description or "", SColor.HUD_Panel_light, highlightColor or SColor.HUD_White, textColor or SColor.HUD_White, highlightedTextColor or SColor.HUD_Black),
        _Index = Index or 0,
        Panels = {},
        SidePanel = nil,
        _Color = barColor or SColor.HUD_Freemode,
        _Type = type or 0,
        ItemId = 5,
        OnStatsChanged = function(menu, item, newindex)
        end,
        OnStatsSelected = function(menu, item, newindex)
        end,
    }
    return setmetatable(_UIMenuStatsItem, UIMenuStatsItem)
end

function UIMenuStatsItem:ItemData(data)
    if data == nil then
        return self.Base._itemData
    else
        self.Base._itemData = data
    end
end

---SetParentMenu
---@param Menu table
function UIMenuStatsItem:SetParentMenu(Menu)
    if Menu() == "UIMenu" then
        self.Base.ParentMenu = Menu
    else
        return self.Base.ParentMenu
    end
end

function UIMenuStatsItem:AddSidePanel(sidePanel)
    if sidePanel() == "UIMissionDetailsPanel" then
        sidePanel:SetParentItem(self)
        self.SidePanel = sidePanel
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("ADD_SIDE_PANEL_TO_ITEM",
                self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)), 0, sidePanel.PanelSide, sidePanel.TitleType,
                sidePanel.Title,
                sidePanel.TitleColor, sidePanel.TextureDict, sidePanel.TextureName)
        end
    elseif sidePanel() == "UIVehicleColorPickerPanel" then
        sidePanel:SetParentItem(self)
        self.SidePanel = sidePanel
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("ADD_SIDE_PANEL_TO_ITEM",
                IndexOf(self.Base.ParentMenu.Items, self), 1, sidePanel.PanelSide, sidePanel.TitleType, sidePanel.Title,
                sidePanel.TitleColor)
        end
    end
end

---Selected
---@param bool number
function UIMenuStatsItem:Selected(bool)
    if bool ~= nil then
        self.Base:Selected(ToBool(bool), self)
    else
        return self.Base._Selected
    end
end

---Hovered
---@param bool boolean
function UIMenuStatsItem:Hovered(bool)
    if bool ~= nil then
        self.Base._Hovered = ToBool(bool)
    else
        return self.Base._Hovered
    end
end

---Enabled
---@param bool boolean
function UIMenuStatsItem:Enabled(bool)
    if bool ~= nil then
        self.Base:Enabled(bool, self)
    else
        return self.Base._Enabled
    end
end

---Description
---@param str string
function UIMenuStatsItem:Description(str)
    if tostring(str) and str ~= nil then
        self.Base:Description(tostring(str), self)
    else
        return self.Base._Description
    end
end

---Text
---@param Text string
function UIMenuStatsItem:Label(Text)
    if tostring(Text) and Text ~= nil then
        self.Base:Label(tostring(Text), self)
    else
        return self.Base:Label()
    end
end

function UIMenuStatsItem:MainColor(color)
    if color then
        self.Base._mainColor = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS", self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor)
        end
    else
        return self.Base._mainColor
    end
end

function UIMenuStatsItem:TextColor(color)
    if color then
        self.Base._textColor = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS", self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor)
        end
    else
        return self.Base._textColor
    end
end

function UIMenuStatsItem:HighlightColor(color)
    if color then
        self.Base._highlightColor = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS", self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor)
        end
    else
        return self.Base._highlightColor
    end
end

function UIMenuStatsItem:HighlightedTextColor(color)
    if color then
        self.Base._highlightedTextColor = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS", self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor)
        end
    else
        return self.Base._highlightedTextColor
    end
end

function UIMenuStatsItem:SliderColor(color)
    if color then
        self._Color = color
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_COLORS", self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)),
                self.Base._mainColor, self.Base._highlightColor, self.Base._textColor, self.Base._highlightedTextColor,
                self._Color)
        end
    else
        return self._Color
    end
end

function UIMenuStatsItem:BlinkDescription(bool)
    if bool ~= nil then
        self.Base:BlinkDescription(bool, self)
    else
        return self.Base:BlinkDescription()
    end
end

---Index
---@param Index table
function UIMenuStatsItem:Index(Index)
    if tonumber(Index) then
        if Index > 100 then
            self._Index = 100
        elseif Index < 0 then
            self._Index = 0
        else
            self._Index = Index
        end
        self.OnStatsChanged(self._Index)
        if self.Base.ParentMenu ~= nil and self.Base.ParentMenu:Visible() and self.Base.ParentMenu.Pagination:IsItemVisible(IndexOf(self.Base.ParentMenu.Items, self)) then
            ScaleformUI.Scaleforms._ui:CallFunction("SET_ITEM_VALUE",
                self.Base.ParentMenu.Pagination:GetScaleformIndex(IndexOf(self.Base.ParentMenu.Items, self)), self._Index)
        end
    else
        return self._Index
    end
end

---LeftBadge
function UIMenuStatsItem:LeftBadge()
    error("This item does not support badges")
end

---RightBadge
function UIMenuStatsItem:RightBadge()
    error("This item does not support badges")
end

---RightLabel
function UIMenuStatsItem:RightLabel()
    error("This item does not support a right label")
end

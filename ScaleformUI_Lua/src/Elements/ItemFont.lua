ItemFont = setmetatable({}, ItemFont)

---@comment Creates a new ItemFont
---@param fontName string
---@param fontId? number
---@return table
function ItemFont.New(fontName, fontId)
    if not fontId then fontId = 0 end
    local font = {
        FontName = fontName,
        FontID = fontId
    }
    return setmetatable(font, ItemFont)
end

---@comment Registers a font gfx and creates a new ItemFont instance
---@param gfxName string
---@param fontName string
---@return table
function ItemFont.RegisterFont(gfxName, fontName)
    RegisterFontFile(gfxName)
    local font = {
        FontName = fontName,
        FontID = RegisterFontId(fontName)
    }
    return setmetatable(font, ItemFont)
end
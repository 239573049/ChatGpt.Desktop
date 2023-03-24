function setValue(key, value) {
    localStorage.setItem(key, value)
}

function getValue(key) {
    return localStorage.getItem(key)
}

function removeValue(key) {
    localStorage.removeItem(key)
}

function clear() {
    localStorage.clear()
}

export {
    setValue,
    getValue,
    removeValue,
    clear
}
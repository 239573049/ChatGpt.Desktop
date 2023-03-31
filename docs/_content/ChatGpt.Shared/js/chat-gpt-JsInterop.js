function addEventListener(id) {
    var obj = document.getElementById(id);
    obj.addEventListener('scroll', function () {
        var scrollTop =  obj.scrollTop;
        console.log(obj,scrollTop);
    });
}

function ElementsByTagName(tag) {
    var obj = document.getElementsByTagName('html');
    obj.addEventListener('scroll', function () {
        var scrollTop = obj.scrollTop;
        console.log(obj, scrollTop);
    });
}

function scrollToBottom() {
    var element = document.documentElement;
    var bottom = element.scrollHeight - element.clientHeight;
    element.scrollTop = bottom;
}

function setClipboard(value) {
    if (window.clipboardData && window.clipboardData.setData) {
        // For IE
        window.clipboardData.setData('Text', value);
    } else if (document.queryCommandSupported && document.queryCommandSupported('copy')) {
        // For other browsers that support copy command
        var textarea = document.createElement('textarea');
        textarea.textContent = value;
        textarea.style.position = 'fixed';
        document.body.appendChild(textarea);
        textarea.select();
        try {
            document.execCommand('copy');
        } catch (ex) {
            console.warn('Copy to clipboard failed.', ex);
        } finally {
            document.body.removeChild(textarea);
        }
    } else {
        console.warn('Copy to clipboard not supported.');
    }
}

export {
    addEventListener,
    setClipboard,
    scrollToBottom,
    ElementsByTagName
}
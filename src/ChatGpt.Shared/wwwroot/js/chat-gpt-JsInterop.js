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
    const clipboard = navigator.clipboard;
    // ���ı����Ƶ���������
    clipboard.writeText(value).then(() => {
        console.log('Text copied to clipboard');
    }).catch((error) => {
        console.error('Failed to copy text: ', error);
    });
}

export {
    addEventListener,
    setClipboard,
    scrollToBottom,
    ElementsByTagName
}
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

export {
    addEventListener,
    ElementsByTagName
}
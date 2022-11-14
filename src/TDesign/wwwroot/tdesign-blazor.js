﻿window.hash = function (id) {
    var el = document.getElementById(id);
    var topSize = el.offsetTop - el.offsetHeight;

    el.parentNode.scrollTo({
        top: topSize,
        left: 0,
        behavior: 'smooth'
    })
}


window.anchorOnScroll = function (dotNetHelper) {
    var anchors = document.getElementsByClassName("t-anchor");

    for (var i = 0; i < anchors.length; i++) {

        document.getElementById(anchors[i].getAttribute("container").replace("#", "")).onscroll = function (e) {
            console.warn(e.srcElement.scrollTop)
            var s = dotNetHelper.invokeMethodAsync('OnScrollAnchorChangeAsync', e.srcElement.scrollTop);
            console.log(s)
        }

    }
}


window.getOffsetTop = function (id) {
    var el = document.getElementById(id);
    return el.offsetTop;
}

window.getOffsetHeight = function (id) {
    var el = document.getElementById(id);
    return el.offsetHeight;
}
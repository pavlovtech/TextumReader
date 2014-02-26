function autoResizeDiv() {
    //if (document.getElementById('content').style.height < window.innerHeight)
    //    document.getElementById('content').style.height = window.innerHeight - 300 + 'px';
}

$(document).ready(function() {
    window.onresize = autoResizeDiv;
    autoResizeDiv();
});

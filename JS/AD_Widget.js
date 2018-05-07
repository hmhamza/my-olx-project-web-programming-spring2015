
function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function Close_AD() {

    var node = document.getElementById("AD_Widget");

    if (node != null) {
        node.parentNode.removeChild(node);
    }
    
}

function Refresh_AD() {
    var R = getRandomInt(1, 7);
    var AD = "A" + R + ".jpg";
    
    var html = "<span>";
    html += "<img src='/Images/" + AD + "' style='width: 520px; height: 120px; margin: auto; ' />";
    html += "</span>";
    html += "<span>";

    html += "<img src='/Images/close.png'  style='width: 25px; height: 25px; margin: -85px 0 0 0 ; ' onclick='Close_AD()'  />";
    html += "</span>";

    document.getElementById("AD_Widget").innerHTML = html;

}

setInterval(Refresh_AD, 2000);
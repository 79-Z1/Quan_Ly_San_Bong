$(async function () {
    $('#logout-btn').on('click',async (e) => {
        e.preventDefault();
        await postData('/TaiKhoansUser/Logout', {})
        window.location.replace(`http://localhost:44381/TaiKhoansUser/Login`);
    })
});

async function postData(url, data) {
    return $.ajax({
        type: "post",
        cache: false,
        url: url,
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        traditional: true,
        success: function (result) {
            return result;
        },
        error: function (err) {
            console.log(err);
        },
    });
}

function getCookie(cookieName) {
    var name = cookieName + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieArray = decodedCookie.split(';');

    for (var i = 0; i < cookieArray.length; i++) {
        var cookie = cookieArray[i].trim();
        if (cookie.indexOf(name) === 0) {
            return cookie.substring(name.length, cookie.length);
        }
    }

    return "";
}
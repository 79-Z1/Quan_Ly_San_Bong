$(async function () {
    let gioDat = $('#gioDat').val();
    let maTK = getCookie("MaTK");

    $('#gioKetThuc').text(parseInt($('#gioBatDau').text()) + parseInt($('#gioDat').val()) + ":00");

    // Kết nối tới Hub
    let datSanHub = $.connection.datSan;
    console.log(datSanHub);

    // Xử lý sự kiện nhận tin nhắn từ server
    datSanHub.client.sendData = (message) => {
        console.log(message);
    }

    $('#gioDat').on('change', async function () {
        if ($(this).val() > gioDat) {
            $('#gioKetThuc').text(parseInt($('#gioBatDau').text()) + parseInt($(this).val()) + ":00");

            const data = {
                maTK: maTK,
                maSan: $('#maSan').text(),
                gioBatDau: parseInt($('#gioBatDau').text()),
                gioKetThuc: parseInt($('#gioBatDau').text()) + parseInt($(this).val())
            }
            const res = await postData('/DatSans/TinhTien', data);
            $('#thanhTien').text(res.tongTien);
        } else {
            $('#gioKetThuc').text(parseInt($('#gioBatDau').text()) + parseInt($(this).val()) + ":00");

            const data = {
                maTK: maTK,
                maSan: $('#maSan').text(),
                gioBatDau: parseInt($('#gioBatDau').text()),
                gioKetThuc: parseInt($('#gioBatDau').text()) + parseInt($(this).val())
            }
            const res = await postData('/DatSans/TinhTien', data);
            $('#thanhTien').text(res.tongTien);
        }

        gioDat = $(this).val();
    });

    // Khởi tạo kết nối SignalR
    $.connection.hub.start().done(function () {
        console.log("SignalR connected.");
        $('#datSan-btn').on('click', async () => {
            const data = {
                MaTK: maTK,
                MaSan: $('#maSan').text(),
                MaCTS: $('#maCTS').text(),
                GioBatDau: parseInt($('#gioBatDau').text()),
                GioKetThuc: parseInt($('#gioKetThuc').text()),
                NgayDenSan: new Date($('#ngayDat').text())
            }
            await postData('/DatSans/Create', data)
            // Gửi tin nhắn tới server
            await datSanHub.server.sendData({
                maSan: $('#maSan').text(),
                maCTS: $('#maCTS').text(),
                gioBatDau: parseInt($('#gioBatDau').text()),
                gioKetThuc: parseInt($('#gioKetThuc').text()),
                ngayDenSan: new Date($('#ngayDat').text())
            });
            window.location.replace(`http://localhost:44381/DatSans/LichSu/${maTK}`);
        })
    });


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
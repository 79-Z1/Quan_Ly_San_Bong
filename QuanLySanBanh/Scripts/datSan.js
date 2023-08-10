$(function () {
    // Kết nối tới Hub
    var datSanHub = $.connection.datSan;
    console.log(datSanHub);

    // Xử lý sự kiện nhận tin nhắn từ server
    datSanHub.client.sendData = (message) => {
        console.log(message);
        window.location.reload();
    }
    $.connection.hub.start().done(function () {
        console.log("SignalR connected.");
    });
});
// Khi modal được mở
$('#staticBackdrop').on('show.bs.modal', function () {
    // Gọi hàm BangGia để lấy danh sách giá sân
    $.ajax({
        url: '/DatSans/BangGia', // Đường dẫn tới action BangGia trong controller DatSansController
        method: 'GET',
        success: function (data) {
            // Xóa danh sách giá sân hiện tại
            $('#priceList').empty();

            // Duyệt qua danh sách giá sân và thêm vào modal
            data.forEach(function (price) {
                var listItem = $('<li>').text('Giờ: ' + price.Gio + ', Giá: ' + price.GiaTheoGio);
                $('#priceList').append(listItem);
            });
        },
        error: function (xhr, status, error) {
            console.log(error); // Xử lý lỗi nếu có
        }
    });
});




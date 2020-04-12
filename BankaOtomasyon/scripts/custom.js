$(function () {
    $("#tblDepartmanlar").DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
        }
    });
    $("#TblKullanicilar").on("click", ".BtnKullaniciSil", function () {
        if (confirm("Kullanıcıyı silmek istediğinize emin misiniz ? ")) {
            var id = $(this).data("id");
            var btn = $(this);
            $.ajax({
                type: "GET",
                url: location.origin + "/Otomasyon/Sil/" + id,
                success: function () {
                    btn.parent().parent().remove();
                },
                error: function (error) {
                    var sonuc = error;
                    var sonuc = error;
                }
            });
        }
    });
});
function CheckDateTypeIsValid(dateElement) {
    var value = $(dateElement).val();
    if (value == '') {
        $(dateElement).valid("false");
    }
    else {
        $(dateElement).valid();
    }
}
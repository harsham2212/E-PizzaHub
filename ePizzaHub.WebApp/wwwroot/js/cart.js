function AddToCart(itemId, name, unitPrice, quantity) {
    $.ajax({
        url: '/Cart/AddToCart/' + itemId + "/" + unitPrice + "/" + quantity,
        type: 'GET',
        success: function (response) {
            if (response.status === 'success') {
                var counter = response.count;
                $("#cartCounter").text(counter);

                $("#toastCart > .toast-body").text(name + " added to cart successfully!");
                $("#toastCart").toast('show');
                setTimeout(function () {
                    $("#toastCart").toast('hide');
                }, 4000);
            }
        }
    });
}

function UpdateQuantity(id, currentQuantity, quantity) {
    if ((currentQuantity >= 1 && quantity > 0) || (currentQuantity > 1 && quantity == -1)) {
        $.ajax({
            url: '/Cart/UpdateQuantity/' + id + "/" + quantity,
            type: 'GET',
            success: function (response) {
                if (response > 0) {
                    location.reload();
                }
            }
        });
    }
}

function DeleteItem(itemId) {
    $.ajax({
        url: '/Cart/DeleteItem/' + itemId,
        type: 'GET',
        success: function (response) {
            if (response > 0) {
                location.reload();
            }
        }
    });
}

$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: '/Cart/GetCartCount',
        success: function (response) {
            $("#cartCounter").text(response);
        }
    });
});
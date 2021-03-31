var items = document.querySelectorAll(".Seat");
var link = document.querySelector(".linkBuy");
var strDefault = link.getAttribute('href');


items.forEach(function (item) {
    item.addEventListener("click", function () {
        if (!this.classList.contains('active'))
            this.classList.add('active');
        else this.classList.remove('active');
        push();
    });
});
function push() {
    
    link.href = strDefault;
    var itemsActive = document.querySelectorAll(".Seat.active");
    
    console.log(link);
    var str = "";
    itemsActive.forEach(function (item) {
        str += "&SeatId=" + item.getAttribute('id');
    });
    //str = link.getAttribute('href') + str;
    link.href += str;

 
};
const corousel_items = document.querySelectorAll(".corousel_item");
console.log(corousel_items);
var max_count = 4;
var idx = 1;
var head_id = 0;
var tail_id;
corousel_items.forEach(function (item) {
    if (idx > max_count) {
        item.style.display = 'none'
    }
    else {
        tail_id = idx;
    }
    idx++;
});
tail_id--;
function next() {
    console.log(head_id, tail_id);

    if (tail_id < corousel_items.length-1) {
        head_id++;
        tail_id++;
    }
    
    corousel_items.forEach(function (item, i) {
        if (i >= head_id && i <= tail_id) {
            item.style.display = 'inline-block'

        }
        else {
            item.style.display = 'none'
        }
        
    });
}
function previous() {
    if (head_id > 0) {
        head_id--;
        tail_id--;
    }

    corousel_items.forEach(function (item, i) {
        if (i >= head_id && i <= tail_id) {
            item.style.display = 'inline-block'

        }
        else {
            item.style.display = 'none'
        }

    });
}